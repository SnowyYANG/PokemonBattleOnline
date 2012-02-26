using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace LightStudio.Tactic.Messaging
{
  public class MessageClient : Messager, IMessageClient
    {

        #region Properties

        public bool IsConnected
        { get; private set; }

        /// <summary>
        /// better to be protected AND internal
        /// </summary>
        internal IConnector Connector
        { get; private set; }

        protected IMessager Messager
        { get; private set; }

        #endregion

        #region Events

        public event EventHandler Connected = delegate { };
        protected void OnConnected()
        {
            Messager = Connector.Messager;
            Messager.Received += (sender, e) => OnReceive(e.Message);
            Messager.UnhandledException += (sender, e) => OnUnhandledException(e.Exception);
            IsConnected = true;

            Connected(this, EventArgs.Empty);
        }

        public event EventHandler Disconnected = delegate { };
        protected void OnDisconnected()
        {
            IsConnected = false;
            Messager = null;

            Disconnected(this, EventArgs.Empty);
        }

        public event EventHandler<MessageExceptionEventArgs> ConnectFailed = delegate { };
        protected virtual void OnConnectFailed(MessageException exception)
        {
            ConnectFailed(this, new MessageExceptionEventArgs(exception));
        }

        #endregion

        internal MessageClient(IConnector connector)
        {
            Contract.Requires(connector != null);

            this.Connector = connector;
            this.Connector.Connected += (sender, e) => OnConnected();
            this.Connector.ConnectFailed += (sender, e) => OnConnectFailed(e.Exception);
            this.Connector.Disconnected += (sender, e) => OnDisconnected();
        }

        public void Connect()
        {
            Connector.Connect();
        }

        public void Disconnect()
        {
            Connector.Disconnect();
        }

        public override void Send(IMessage message)
        {
            var deliverer = Messager;//for thread-safe
            if (deliverer != null)
                deliverer.Send(message);
        }

        public override void StartReceive()
        {
            var messager = Messager;//for thread-safe
            if (messager != null)
                messager.StartReceive();
        }

        protected override void OnUnhandledException(MessageException exception)
        {
            if (IdDisposed)
                return;

            base.OnUnhandledException(exception);
            Disconnect();
        }

        protected override void DisposeManagedResources()
        {
            Connector.Dispose();
            if (Messager != null)
            {
                Messager.Dispose();
                Messager = null;
            }

            base.DisposeManagedResources();
        }
    }
}
