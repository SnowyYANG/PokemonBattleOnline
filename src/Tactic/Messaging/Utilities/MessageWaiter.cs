using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics.Contracts;
namespace LightStudio.Tactic.Messaging
{
  internal class MessageWaiter : DisposableObject
    {
        private ManualResetEvent waiter;

        public IMessager Messager
        { get; private set; }

        public IMessage Message
        { get; private set; }

        public string Header
        { get; private set; }

        public MessageWaiter(IMessager messager)
            : this(messager, string.Empty)
        { }

        public MessageWaiter(IMessager messager, string header)
        {
            Contract.Requires(messager != null);

            this.waiter = new ManualResetEvent(false);
            this.Messager = messager;
            this.Header = header;
            messager.Received += (sender, e) => OnReceive(e.Message);
        }

        private void OnReceive(IMessage message)
        {
            if (string.IsNullOrEmpty(Header) || message.Header == Header)
            {
                Message = message;
                SetWaiter();
            }
        }

        public bool Wait()
        {
            try
            {
                waiter.WaitOne();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private void SetWaiter()
        {
            try
            {
                waiter.Set();
            }
            catch (Exception)
            { }
        }

        protected override void DisposeManagedResources()
        {
            SetWaiter();
            waiter.Dispose();
        }
    }
}
