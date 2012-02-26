using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using System.Threading;
using System.Threading.Tasks;

namespace LightStudio.Tactic.Messaging
{
    public class Dispatcher : DisposableObject
    {
        private SyncList<Work> workList;
        private ManualResetEvent addWorkEvent;
        private object addWorkLock;
        private Thread thread;

        public Dispatcher()
            : this(false)
        { }

        public Dispatcher(bool autoStart)
        {
            workList = new SyncList<Work>();
            addWorkEvent = new ManualResetEvent(false);
            addWorkLock = new object();
            if (autoStart)
                Start();
        }

        public void Start()
        {
            if (thread == null)
            {
                thread = new Thread(Process);
                thread.Start();
            }
        }

        private void Process()
        {
            while (!IdDisposed)
            {
                try
                {
                    addWorkEvent.WaitOne();
                }
                catch (Exception)
                {
                    return;
                }

                if (IdDisposed)
                    return;

                IList<Work> works;
                lock (addWorkLock)
                {
                    works = workList.DeItems();
                    try
                    {
                        addWorkEvent.Reset();
                    }
                    catch (Exception)
                    { }
                }
                foreach (var work in works)
                {
                    work.DoWork();
                }
            }
        }

        private void AddWork(Work work)
        {
            if (IdDisposed)
                return;

            workList.Add(work);
            SetAddWorkEvent();
        }

        public void Invoke(Action action)
        {
            Invoke(action as Delegate);
        }

        public void Invoke(Delegate method)
        {
            var work = new Work(method, true);
            Wait(work);
        }

        public void Invoke(Delegate method, params object[] args)
        {
            var work = new Work(method, true, args);
            Wait(work);
        }

        private void Wait(Work work)
        {
            if (IdDisposed)
                return;

            AddWork(work);
            work.Wait();
            work.Dispose();
        }

        public void BeginInvoke(Action action)
        {
            BeginInvoke(action as Delegate);
        }

        public void BeginInvoke(Delegate method)
        {
            AddWork(new Work(method));
        }

        public void BeginInvoke(Delegate method, params object[] args)
        {
            AddWork(new Work(method, args));
        }

        private void SetAddWorkEvent()
        {
            try
            {
                lock (addWorkLock)
                {
                    addWorkEvent.Set();
                }
            }
            catch (Exception)
            { }
        }

        protected override void DisposeManagedResources()
        {
            SetAddWorkEvent();
            addWorkEvent.Dispose();
            DisposeWorks(workList.DeItems());
        }

        private static void DisposeWorks(IEnumerable<Work> works)
        {
            foreach (var work in works)
            {
                work.Dispose();
            }
        }

        private class Work : DisposableObject 
        {
            public Delegate Method
            { get; private set; }

            public ManualResetEvent CompleteWaiter
            { get; private set; }

            public object[] Arguments
            { get; private set; }

            public bool IsSynchronized
            { get; private set; }

            public Work(Delegate method, bool isSynchronized)
            {
                this.Method = method;
                this.IsSynchronized = isSynchronized;
                if (isSynchronized)
                    CompleteWaiter = new ManualResetEvent(false);
            }

            public Work(Delegate method, bool isSynchronized, params object[] args)
                : this(method, isSynchronized)
            {
                this.Arguments = args;
            }

            public Work(Delegate method)
                : this(method, false)
            { }

            public Work(Delegate method, params object[] args)
                : this(method, false, args)
            { }

            /// <summary>
            /// exceptions are ignored
            /// </summary>
            public void DoWork()
            {
                try
                {
                    Method.DynamicInvoke(Arguments);
                }
                catch (Exception e)
                { }
                SetComplete();
            }

            public void Wait()
            {
                if (IsSynchronized)
                {
                    try
                    {
                        CompleteWaiter.WaitOne();
                    }
                    catch (Exception)
                    { }
                }
            }

            private void SetComplete()
            {
                if (IsSynchronized)
                {
                    try
                    {
                        CompleteWaiter.Set();
                    }
                    catch (Exception)
                    { }
                }
            }

            protected override void DisposeManagedResources()
            {
                if (IsSynchronized)
                {
                    SetComplete();
                    CompleteWaiter.Dispose();
                }
            }
        }
    }
}
