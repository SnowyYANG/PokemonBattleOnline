using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace PokemonBattleOnline
{
  public class Dispatcher : DisposableObject
  {
    private readonly Thread thread;
    private readonly ConcurrentQueue<Work> works;
    private readonly AutoResetEvent addWorkEvent;
    private readonly object addWorkLocker;

    public Dispatcher(string name, bool autoStart)
    {
      thread = new Thread(Process) { Name = name };
      works = new ConcurrentQueue<Work>();
      addWorkLocker = new object();
      addWorkEvent = new AutoResetEvent(false);
      if (autoStart) Start();
    }

    public void Start()
    {
      thread.Start();
    }

    private void Process()
    {
    LOOP:
      try
      {
        Work work;
        while (!IsDisposed)
        {
          addWorkEvent.WaitOne();
          while (works.TryDequeue(out work)) work.DoWork();
          lock (addWorkLocker)
          {
            if (works.IsEmpty) addWorkEvent.Reset();
          }
        }
      }
      catch
      {
        goto LOOP;
      }
    }

    private void AddWork(Work work)
    {
      if (!IsDisposed)
      {
        works.Enqueue(work);
        lock (addWorkLocker)
        {
          addWorkEvent.Set();
        }
      }
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
      if (IsDisposed) return;

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

    protected override void DisposeManagedResources()
    {
      try
      {
        addWorkEvent.Set();
        addWorkEvent.Dispose();
      }
      catch
      {
      }
      Work work;
      while (works.TryDequeue(out work)) work.Dispose();
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

      public Work(Delegate method, bool isSynchronized, params object[] args)
      {
        this.Method = method;
        this.Arguments = args;
        this.IsSynchronized = isSynchronized;
        if (isSynchronized)
          CompleteWaiter = new ManualResetEvent(false);
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
        {
#if DEBUG
          System.Diagnostics.Debugger.Break();
#endif
        }
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
