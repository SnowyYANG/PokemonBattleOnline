using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace LightStudio.Tactic.Messaging.Primitive
{
    public class MessageSession : Messager
    {
        #region Properties

        public int Id
        { get; private set; }

        public DateTimeOffset CreatedTime
        { get; private set; }

        public IMessager Messager
        { get; private set; }

        #endregion

        #region Events

        public event EventHandler Ended = delegate { };
        protected virtual void OnEnded()
        {
            Ended(this, EventArgs.Empty);
        }

        #endregion

        public MessageSession(int id, IMessager messager)
        {
            Contract.Requires(messager != null);

            this.Id = id;
            this.CreatedTime = DateTimeOffset.Now;
            this.Messager = messager;
            this.Messager.Received += (sender, e) => OnReceive(e.Message);
            this.Messager.UnhandledException += (sender, e) => OnUnhandledException(e.Exception);
        }

        public override void Send(IMessage message)
        {
            if (IdDisposed)
                return;

            Messager.Send(message);
        }

        public override void StartReceive()
        {
            if (IdDisposed)
                return;

            Messager.StartReceive();
        }

        protected override void OnUnhandledException(MessageException exception)
        {
            if (IdDisposed)
                return;

            base.OnUnhandledException(exception);
            End();
        }

        public void End()
        {
            if (IdDisposed)
                return;

            OnEnded();
            Dispose();
        }

        protected override void DisposeManagedResources()
        {
            Messager.Dispose();
            base.DisposeManagedResources();
        }
    }
}
