using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using PokemonBattleOnline.Tactic.Network;
using PokemonBattleOnline.Network.Lobby;
using PokemonBattleOnline.Network.Room;
using System.Threading;

namespace PokemonBattleOnline.Network
{
  public class Client : IPackReceivedListener, IDisposable
  {
    private const int KEEP_ALIVE = 30000;

    private static void OnKeepAlive(object state)
    {
      ((INetworkClient)state).SendEmpty();
    }

    public event Action<string> AdminChat;
    public event Action<string, User> PublicChat;
    public event Action<string, User> PrivateChat;
    protected readonly INetworkClient Network;
    private readonly Timer KeepAlive;
    
    internal Client(INetworkClient network, ClientInitInfo init)
    {
      Network = network;
      network.Listener = this;
      network.Disconnect += OnDisconnect;
      KeepAlive = new Timer(OnKeepAlive, network, KEEP_ALIVE, KEEP_ALIVE);
    }

    public ObservableCollection<User> Users
    { get; private set; }
    public User User
    { get; private set; }

    public User GetUser(int id)
    {
      throw new NotImplementedException();
    }
    public User GetUser(string name)
    {
      throw new NotImplementedException();
    }

    protected virtual void OnDisconnect()
    {
      Dispose();
    }
    void IPackReceivedListener.OnPackReceived(byte[] pack)
    {
      var h = pack[0];
      switch (h)
      {
        case 0:
          Serializer.DeserializeFromJson<ClientCommand>(pack, 1).Execute(this);
          break;
        case 1:
          {
            var u = pack.ToUInt16(1);
            if (u != null)
            {
              var user = GetUser(u.Value);
              if (user != null) Serializer.DeserializeFromJson<P2PCommand>(pack, 3).Execute(this, user);
            }
          }
          break;
        case 2:
          {
            var u = pack.ToUInt16(1);
            if (u != null)
            {
              var user = GetUser(u.Value);
              if (user != null) UIDispatcher.BeginInvoke(PublicChat, pack.ToUnicodeString(3), user);
            }
          }
          break;
        default:
#if DEBUG
          System.Diagnostics.Debugger.Break();
#endif
          //close
          break;
      }
    }
    protected void SendUserCommand(UserCommand command)
    {
      using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
      {
        ms.WriteByte(0);
        Serializer.SerializeToJson(command, ms);
        Network.Send(ms.ToArray());
      }
    }
    protected void SendP2PCommand(P2PCommand command, params int[] to)
    {
      using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
      {
        ms.WriteByte(1);
        ms.WriteByte((byte)to.Length);
        foreach (var t in to)
        {
          var i = ((ushort)to[0]).ToPack();
          ms.Write(i, 0, i.Length);
        }
        Serializer.SerializeToJson(command, ms);
        Network.Send(ms.ToArray());
      }
    }
    protected void SendPublicChat(string chat)
    {
      var pack = chat.ToPack(1);
      pack[0] = 10;
      Network.Send(pack);
    }

    private void SendKeepAlive()
    {
      Network.SendEmpty();
    }
    #region clientcommand
    internal void AddUser(int id, string name, ushort avatar)
    {
      UIDispatcher.Invoke((Action<User>)Users.Add, new User(id, name, avatar));
    }
    internal void RemoveUser(int id)
    {
      foreach (var u in Users)
        if (u.Id == id)
        {
          UIDispatcher.Invoke((Func<User, bool>)Users.Remove, u);
          break;
        }
    }
    internal void OnAdminChat(string chat)
    {
      UIDispatcher.BeginInvoke(AdminChat, chat);
    }
    internal void OnPrivateChat(User from, string chat)
    {
      UIDispatcher.BeginInvoke(PrivateChat, chat, from);
    }
    #endregion

    public void Dispose()
    {
      Network.Dispose();
    }
  }
}
