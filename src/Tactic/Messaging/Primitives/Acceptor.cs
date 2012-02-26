using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.Tactic.Logging;

namespace LightStudio.Tactic.Messaging
{
    internal abstract class Acceptor : DisposableObject, IAcceptor
    {
        public abstract bool Accepting
        { get; }

        #region Events

        public event EventHandler<MessagerEventArgs> Accepted = delegate { };
        protected virtual void OnAccepted(IMessager messager)
        {
            if (IdDisposed)
                return;

            LoggerFacade.LogDebug("Acceptor : Accepted a new messager");

            Accepted(this, new MessagerEventArgs(messager));
        }

        /// <summary>
        /// a safe version of OnAccepted which ignores exceptions thrown by handlers
        /// </summary>
        protected void OnSafeAccepted(IMessager messager)
        {
            try
            {
                OnAccepted(messager);
            }
            catch (Exception)
            { }
        }

        public event EventHandler<MessageExceptionEventArgs> UnhandledException = delegate { };
        protected virtual void OnUnhandledException(MessageException exception)
        {
            if (IdDisposed)
                return;

            LoggerFacade.LogException(exception, Priority.Medium);
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

        /// <summary>
        /// this method will wraps the specified message and exception in a MessageException, 
        /// and calls OnUnhandledException(MessageException) method
        /// </summary>
        protected void OnException(string message, Exception exception)
        {
            OnUnhandledException(new MessageException(message, exception));
        }

        /// <summary>
        /// a safe version of OnUnhandledException which ignores exceptions thrown by handlers
        /// </summary>
        protected void OnSafeException(string message, Exception exception)
        {
            try
            {
                OnException(message, exception);
            }
            catch (Exception)
            { }
        }

        #endregion

        public abstract void Start();

        public abstract void Stop();
    }
}
