using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace LightStudio.Tactic.Messaging
{
  [ContractClass(typeof(IMessageServerContract))]
  public interface IMessageServer : IMessageCenter
  {
    ICollection<MessageSession> Sessions { get; }
    void Start();
    void Stop();
    void EndSession(int sessionId);
    void EndAllSessions();

    event EventHandler<SessionCreatedEventArgs> SessionCreated;
    event EventHandler<SessionEndedEventArgs> SessionEnded;
  }
}
