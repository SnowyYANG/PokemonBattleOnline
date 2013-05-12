using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.Tactic.Logging;

namespace LightStudio.Tactic.Messaging.Primitive
{
  internal abstract class Connector : DisposableObject, IConnector
    {

        public abstract bool IsConnected
        { get; }

        public abstract IMessager Messager
        { get; }

        #region Events

        public event EventHandler Connected = delegate { };
        protected virtual void OnConnected()
        {
            if (IdDisposed)
                return;

            Connected(this, EventArgs.Empty);
        }

        /// <summary>
        /// a safe version of OnConnected which ignores exceptions thrown by handlers
        /// </summary>
        protected void OnSafeConencted()
        {
            try
            {
                OnConnected();
            }
            catch (Exception)
            { }
        }

        public event EventHandler Disconnected = delegate { };
        protected virtual void OnDisconnected()
        {
            if (IdDisposed)
                return;

            Disconnected(this, EventArgs.Empty);
        }

        /// <summary>
        /// a safe version of OnDisconnected which ignores exceptions thrown by handlers
        /// </summary>
        protected void OnSafeDisconnected()
        {
            try
            {
                OnDisconnected();
            }
            catch (Exception)
            { }
        }

        public event EventHandler<MessageExceptionEventArgs> ConnectFailed = delegate { };
        protected virtual void OnConnectFailed(MessageException exception)
        {
            if (IdDisposed)
                return;

            LoggerFacade.LogException(exception);
            ConnectFailed(this, new MessageExceptionEventArgs(exception));
        }

        /// <summary>
        /// a safe version of OnConnectFailed which ignores exceptions thrown by handlers
        /// </summary>
        protected void OnSafeConnectFailed(MessageException exception)
        {
            try
            {
                OnConnectFailed(exception);
            }
            catch (Exception)
            { }
        }

        /// <summary>
        /// this method wraps the specified message and exception in a MessageException, 
        /// and calls OnConnectFailed(MessageException) method
        /// </summary>
        protected void OnConnectFailed(string message, Exception exception)
        {
            OnConnectFailed(new MessageException(message, exception));
        }

        /// <summary>
        /// a safe version of OnConnectFailed which ignores exceptions thrown by handlers
        /// </summary>
        protected void OnSafeConnectFailed(string message, Exception exception)
        {
            try
            {
                OnConnectFailed(message, exception);
            }
            catch (Exception)
            { }
        }

        #endregion

        public abstract void Connect();

        public abstract void Disconnect();
    }
}
