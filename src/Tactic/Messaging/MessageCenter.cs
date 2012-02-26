using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using LightStudio.Tactic.Logging;

namespace LightStudio.Tactic.Messaging
{
  public class MessageCenter<T> : DisposableObject, IMessageCenter
        where T : IMessager
    {
        protected ConcurrentDictionary<int, T> Messagers
        { get; private set; }

        public MessageCenter()
        {
            Messagers = new ConcurrentDictionary<int, T>();
        }

        protected T GetMessager(int messagerId)
        {
            if (IdDisposed)
                return default(T);

            return Messagers.ValueOrDefault(messagerId);
        }

        protected bool AddMessager(int messagerId, T messager)
        {
            if (IdDisposed)
                return false;

            if (Messagers.TryAdd(messagerId, messager))
            {
                messager.Received += (sender, e) => OnReceive(messagerId, e.Message);
                messager.StartReceive();
                return true;
            }
            return false;
        }

        protected bool RemoveMessager(int messagerId)
        {
            if (IdDisposed)
                return false;

            T messager;
            if (Messagers.TryRemove(messagerId, out messager))
            {
                messager.Dispose();
                return true;
            }
            return false;
        }

        public void Broadcast(IMessage message)
        {
            if (IdDisposed)
                return;

            foreach (var messager in Messagers.Values)
            {
                messager.Send(message);
            }
        }

        public bool Send(int messagerId, IMessage message)
        {
            if (IdDisposed)
                return false;

            var messager = GetMessager(messagerId);
            if (messager != null)
            {
                messager.Send(message);
                return true;
            }
            return false;
        }

        protected override void DisposeManagedResources()
        {
            var messagers = Messagers.Values;
            Messagers.Clear();
            foreach (var messager in messagers)
            {
                messager.Dispose();
            }
        }

        #region Events

        public event EventHandler<IdMessageEventArgs> Received = delegate { };
        protected virtual void OnReceive(int messagerId, IMessage message)
        {
            if (IdDisposed)
                return;

            Received(this, new IdMessageEventArgs(messagerId, message));
        }

        public event EventHandler<MessageExceptionEventArgs> UnhandledException = delegate { };
        protected virtual void OnUnhandledException(MessageException exception)
        {
            if (IdDisposed)
                return;

            LoggerFacade.LogException(exception);
            UnhandledException(this, new MessageExceptionEventArgs(exception));
        }

        /// <summary>
        /// a safe version of OnUnhandledException which ignores exceptions thrown by handlers
        /// </summary>
        protected void OnSafeUnhandledException(MessageException exception)
        {
            try
            {
                OnUnhandledException(exception);
            }
            catch (Exception)
            { }
        }

        #endregion
    }
}
