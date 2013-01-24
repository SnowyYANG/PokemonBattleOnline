using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using PokemonBattleOnline.Tactic.Network;

namespace PokemonBattleOnline.Network.Lobby
{
  public class Client : ClientBase, IDisposable
  {
    public event Action<string> AdminChat;
    public event Action<string, User> PublicChat;
    public event Action<string, User> PrivateChat;
    
    static Client()
    {
      //register
    }
    
    internal Client(INetworkClient network, ClientInitInfo init)
      : base(network)
    {
    }

    public ObservableCollection<User> Users
    { get; private set; }
    public User User
    { get; private set; }

    protected virtual void OnPackReceived(byte header, byte[] content)
    {
      //header >= 128
      //user define
    }
    protected sealed override void OnPackReceived(byte[] pack)
    {
      var h = pack.GetHeader();
      var c = pack.GetContent();
      switch (h)
      {
        case 0:
          Serializer.DeserializeFromJson<ClientCommand>(c).Execute(this);
          break;
        case 1:
          {
            var u = c.SubArray(0, 2).ToInt16();
            if (u != null)
            {
              var user = GetUser(u.Value);
              if (user != null) Serializer.DeserializeFromJson<P2PCommand>(c.SubArray(2)).Execute(this, user);
            }
          }
          break;
        case 2:
          {
            var u = c.SubArray(0, 2).ToInt16();
            if (u != null)
            {
              var user = GetUser(u.Value);
              if (user != null) UIDispatcher.BeginInvoke(PublicChat, Encoding.Unicode.GetString(c.SubArray(2)), user);
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
    private void Send(byte header, byte[] pack)
    {
      Network.Send(header, pack);
    }
    protected void SendUserCommand(UserCommand command)
    {
      Network.Send(0, Serializer.SerializeToJson(command));
    }
    protected void SendP2PCommand(P2PCommand command, params int[] to)
    {
      var c = Serializer.SerializeToJson(command);
      using (System.IO.MemoryStream ms = new System.IO.MemoryStream(c.Length + 8))
      {
        ms.WriteByte((byte)to.Length);
        foreach (var t in to)
        {
          var i = ((ushort)to[0]).ToPack();
          ms.Write(i, 0, i.Length);
        }
        ms.Write(c, 0, c.Length);
        Network.Send(1, ms.ToArray());
      }
    }
    protected void SendPublicChat(string chat)
    {
      Network.Send(10, Encoding.Unicode.GetBytes(chat));
    }

    public User GetUser(int id)
    {
      throw new NotImplementedException();
    }
    public User GetUser(string name)
    {
      throw new NotImplementedException();
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
