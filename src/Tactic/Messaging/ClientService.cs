using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using LightStudio.Tactic.Messaging.Lobby;
using LightStudio.Tactic.Serialization;

namespace LightStudio.Tactic.Messaging
{
  public abstract class ClientService<T> : IDisposable where T : IBytable, new()
  {
    protected readonly Client<T> Client;
    
    public ClientService(Client<T> client, params byte[] receiveMessageHeaders)
    {
      Client = client;
      client.RegisterService(this, receiveMessageHeaders);
    }

    protected internal abstract void ReadMessage(User<T> user, byte header, BinaryReader reader);
    protected void SendMessage(byte header, params int[] receivers)
    {
      SendMessage(header, null, receivers);
    }
    protected void SendMessage(byte header, Action<BinaryWriter> writer, params int[] receivers)
    {
      Client.SendMessage(header, writer, receivers);
    }

    public virtual void Dispose()
    {
    }
  }
}
