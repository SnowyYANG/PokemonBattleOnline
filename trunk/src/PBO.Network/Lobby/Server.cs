using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;
using PokemonBattleOnline.Network.Lobby;
using PokemonBattleOnline.Network.Room;

namespace PokemonBattleOnline.Network
{
  public class Server : IDisposable
  {
    public event Action<User> UsersUpdate;
    
    internal readonly TcpServer Network;
    internal readonly object UserLocker;
    internal readonly object StateLocker;
    private readonly LoginServer LoginServer;
    private readonly Dictionary<string, ServerUser> Users;
    private readonly ConcurrentDictionary<int, ServerUser> users;

    internal Server(TcpServer network)
    {
      Network = network;
      UserLocker = new object();
      StateLocker = new object();
      LoginServer = new LoginServer(network, this);
      Users = new Dictionary<string, ServerUser>();
      users = new ConcurrentDictionary<int, ServerUser>();
    }

    private string _welcome;
    public string Welcome
    {
      get { return _welcome; }
      set
      {
        if (_welcome != value)
        {
          _welcome = value;
          //通知客户端
        }
      }
    }

    internal ClientInitInfo GetClientInitInfo(int user)
    {
      lock (StateLocker)
      {
        return new ClientInitInfo(this, user);
      }
    }

    internal bool HasUser(string name)
    {
      lock(UserLocker)
      {
        return Users.ContainsKey(name);
      }
    }
    internal void AddUser(ServerUser user) //处于UserLocker中
    {
      Users.Add(user.User.Name, user);
      users[user.Network.Id] = user;
      UsersUpdate(user.User);
    }
    internal void RemoveUser(ServerUser user)
    {
      lock (UserLocker) //其实我觉得不lock也行...
      {
        Users.Remove(user.User.Name);
        ServerUser u;
        users.TryRemove(user.Network.Id, out u);
        user.User.State = UserState.Quited;
        UsersUpdate(user.User);
      }
    }

    internal void SendP2PPack(ServerUser serverUser, byte[] arraySegment, params int[] to)
    {
      throw new NotImplementedException();
    }
    internal void PublicChat(ServerUser serverUser, string p)
    {
      throw new NotImplementedException();
    }

    public void Start()
    {
      Network.IsListening = true;
    }

    public void Dispose()
    {
      Network.Dispose();
      LoginServer.Dispose();
    }
  }
}
