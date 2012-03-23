using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics.Contracts;

namespace LightStudio.Tactic.Messaging
{
  public class MessageServer : MessageCenter<MessageSession>, IMessageServer
  {

    #region Variables And Properties

    private IdGenerator idGenerator;
 
    /// <summary>
    /// better to be protected AND internal
    /// </summary>
    internal IAcceptor Acceptor
    { get; private set; }

    #endregion

    internal MessageServer(IAcceptor acceptor)
      : this(acceptor, 0)
    { }

    /// <param name="idBase">the first session id will be idBase + 1, and so on</param>
    internal MessageServer(IAcceptor acceptor, int idBase)
    {
      Contract.Requires(acceptor != null);

      this.idGenerator = new IdGenerator(idBase);
      this.Acceptor = acceptor;
      this.Acceptor.Accepted +=
          (sender, e) => OnSessionCreated(CreateSession(idGenerator.NextId(), e.Messager));
      this.Acceptor.UnhandledException +=
          (sender, e) => OnUnhandledException(e.Exception);
    }

    /// <summary>
    /// override this method to use custom MessageSession
    /// </summary>
    protected virtual MessageSession CreateSession(int id, IMessager messager)
    {
      return new MessageSession(id, messager);
    }

    protected override void DisposeManagedResources()
    {
      Acceptor.Dispose();
      base.DisposeManagedResources();
    }

    #region IMessageServer

    public ICollection<MessageSession> Sessions
    {
      get
      {
        return Messagers.Values;
      }
    }

    public void Start()
    {
      Acceptor.Start();
    }

    public void Stop()
    {
      Acceptor.Stop();
    }

    public void EndSession(int sessionId)
    {
      if (IdDisposed)
        return;

      var session = GetMessager(sessionId);
      if (session != null)
        session.End();
    }

    public void EndAllSessions()
    {
      if (IdDisposed)
        return;

      foreach (var session in Sessions)
      {
        session.End();
      }
    }

    public event EventHandler<SessionCreatedEventArgs> SessionCreated = delegate { };
    protected virtual void OnSessionCreated(MessageSession session)
    {
      session.Ended += (sender, e) => OnSessionEnded(session.Id);
      AddMessager(session.Id, session);

      SessionCreated(this, new SessionCreatedEventArgs(session));
    }

    public event EventHandler<SessionEndedEventArgs> SessionEnded = delegate { };
    protected virtual void OnSessionEnded(int sessionId)
    {
      RemoveMessager(sessionId);

      SessionEnded(this, new SessionEndedEventArgs(sessionId));
    }

    #endregion
  }
}
