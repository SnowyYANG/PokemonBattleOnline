using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.IO.Compression;
using PokemonBattleOnline.Network.Lobby;
using PokemonBattleOnline.Network.Room;

namespace PokemonBattleOnline.Network
{
  internal interface IClientEventsListener
  {
    void Disconnected();
    void UsersUpdate(User user);
    void AdminChat(string chat);
    void PublicChat(string chat, User from);
    void PrivateChat(string chat, User from);
  }
  public class Client : IPackReceivedListener, IDisposable
  {
    private static void OnKeepAlive(object state)
    {
      ((TcpClient)state).Send(PackHelper.EMPTYPACK);
    }

    protected readonly TcpClient Network;
    private readonly ConcurrentDictionary<int, User> Users;
    private readonly Timer KeepAlive;
    
    internal Client(TcpClient network, ClientInitInfo init)
    {
      Network = network;
      network.Listener = this;
      network.Disconnect += OnDisconnect;
      KeepAlive = new Timer(OnKeepAlive, network, PBOMarks.TIMEOUT, PBOMarks.TIMEOUT);
    }

    private User _user;
    public User User
    { get { return _user; } }
    internal IClientEventsListener Listener
    { get; set; }

    public User GetUser(int id)
    {
      User r;
      return Users.TryGetValue(id, out r) ? r : null;
    }

    protected virtual void OnDisconnect()
    {
      if (!isDisposed)
      {
        Listener.Disconnected();
        Dispose();
      }
    }
    void IPackReceivedListener.OnPackReceived(byte[] pack)
    {
      var h = pack[0];
      switch (h)
      {
        case PackHeaders.CS_COMMAND:
          Serializer.DeserializeFromCompressedJson<ClientCommand>(pack, 1).Execute(this);
          break;
        case PackHeaders.P2P_COMMAND:
          {
            var u = pack.ToUInt16(1);
            if (u != null)
            {
              var user = GetUser(u.Value);
              if (user != null) Serializer.DeserializeFromCompressedJson<P2PCommand>(pack, 3).Execute(this, user);
            }
          }
          break;
        case PackHeaders.PUBLIC_CHAT:
          {
            var u = pack.ToUInt16(1);
            if (u != null)
            {
              var user = GetUser(u.Value);
              if (user != null) OnPublicChat(pack.ToUnicodeString(3), user);
            }
          }
          break;
        default:
          Dispose();
          break;
      }
    }
    public void SendUserCommand(UserCommand command)
    {
      using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
      {
        ms.WriteByte(0);
        Serializer.SerializeToCompressedJson(command, ms);
        Network.Send(ms.ToArray());
      }
    }
    public void SendP2PCommand(P2PCommand command, int to)
    {
      using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
      {
        ms.WriteByte(PackHeaders.P2P_COMMAND);
        ms.Write(((ushort)to).ToPack(), 0, 2);
        Serializer.SerializeToCompressedJson(command, ms);
        Network.Send(ms.ToArray());
      }
    }

    private void SendChat(byte header, string chat)
    {
      var pack = chat.ToPack(1);
      pack[0] = header;
      Network.Send(pack);
    }
    public void SendPublicChat(string chat)
    {
      SendChat(PackHeaders.PUBLIC_CHAT, chat);
    }
    public void SendRoomChat(string chat)
    {
      SendChat(PackHeaders.ROOM_CHAT, chat);
    }

    private void SendKeepAlive()
    {
      Network.SendEmpty();
    }
    #region clientcommand
    internal void AddUser(User user)
    {
      Users[user.Id] = user;
      Listener.UsersUpdate(user);
    }
    internal void RemoveUser(int id)
    {
      User u;
      if (Users.TryRemove(id, out u))
      {
        u.State = UserState.Quited;
        Listener.UsersUpdate(u);
      }
    }
    internal void OnAdminChat(string chat)
    {
      Listener.AdminChat(chat);
    }
    internal void OnPrivateChat(string chat, User from)
    {
      Listener.PrivateChat(chat, from);
    }
    internal void OnPublicChat(string chat, User from)
    {
      Listener.PublicChat(chat, from);
    }
    #endregion

    private volatile bool isDisposed;
    public void Dispose()
    {
      if (!isDisposed)
      {
        isDisposed = true;
        Network.Dispose();
      }
    }
  }
}
