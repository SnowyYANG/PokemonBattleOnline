using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics.Contracts;
using LightStudio.Tactic.Logging;

namespace LightStudio.Tactic.Messaging.Primitive
{
  public abstract class Messager : DisposableObject, IMessager
  {
    #region Events

    public event EventHandler<MessageEventArgs> Received = delegate { };
    protected virtual void OnReceive(IMessage message)
    {
      if (IdDisposed) return;
      Received(this, new MessageEventArgs(message));
    }

    /// <summary>
    /// a safe version of OnReceive which ignores exceptions thrown by handlers
    /// </summary>
    protected void OnSafeReceive(IMessage message)
    {
      try
      {
        OnReceive(message);
      }
      catch (Exception)
      { }
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

    /// <summary>
    /// this method wraps the specified message and exception in a MessageException, 
    /// and calls OnUnhandledException method
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

    public abstract void StartReceive();

    public abstract void Send(IMessage message);
  }
}
