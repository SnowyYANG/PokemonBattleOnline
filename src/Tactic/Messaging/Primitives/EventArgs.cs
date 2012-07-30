using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace LightStudio.Tactic.Messaging.Primitive
{
  public class MessageEventArgs : EventArgs
  {
    public IMessage Message
    { get; private set; }

    public MessageEventArgs(IMessage message)
    {
      this.Message = message;
    }
  }

  public class IdMessageEventArgs : EventArgs
  {
    public int MessagerId
    { get; private set; }

    public IMessage Message
    { get; private set; }

    public IdMessageEventArgs(int messagerId, IMessage message)
    {
      this.MessagerId = messagerId;
      this.Message = message;
    }
  }

  public class MessageExceptionEventArgs : EventArgs
  {
    public MessageException Exception
    { get; private set; }

    public MessageExceptionEventArgs(MessageException exception)
    {
      this.Exception = exception;
    }
  }

  public class MessagerEventArgs : EventArgs
  {
    public IMessager Messager
    { get; private set; }

    public MessagerEventArgs(IMessager messager)
    {
      this.Messager = messager;
    }
  }

  public class SessionCreatedEventArgs : EventArgs
  {
    public MessageSession Session
    { get; private set; }

    public SessionCreatedEventArgs(MessageSession session)
    {
      this.Session = session;
    }
  }

  public class SessionEndedEventArgs : EventArgs
  {
    public int SessionId
    { get; private set; }

    public SessionEndedEventArgs(int sessionId)
    {
      this.SessionId = sessionId;
    }
  }
}
