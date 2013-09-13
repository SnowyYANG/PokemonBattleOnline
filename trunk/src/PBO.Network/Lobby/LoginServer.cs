using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;
using System.Threading;

namespace PokemonBattleOnline.Network.Lobby
{
  internal class LoginServer : IDisposable
  {
    private readonly Server Server;
    private readonly ConcurrentDictionary<int, LoginUser> Users;
    private readonly Dictionary<string, LoginUser> NamedUsers;
    private volatile bool isDisposed;

    public LoginServer(TcpServer network, Server server)
    {
      network.NewComingUser += OnNewUser;
      Server = server;
      NamedUsers = new Dictionary<string, LoginUser>();
      Users = new ConcurrentDictionary<int, LoginUser>();
    }

    private TcpServer Network
    { get { return Server.Network; } }
    private object UserLocker
    { get { return Server.UserLocker; } }
    
    private void OnNewUser(TcpUser user)
    {
      if (isDisposed || !Users.TryAdd(user.Id, new LoginUser(user, this))) user.Dispose();
    }

    public ClientInitInfo GetClientInitInfo(int id)
    {
      return  Server.GetClientInitInfo(id);
    }

    public bool RegisterUserName(LoginUser user, string name)
    {
      lock (UserLocker)
      {
        if (Server.HasUser(name) || NamedUsers.ContainsKey(name)) return false;
        NamedUsers.Add(name, user);
        return true;
      }
    }
    public void BadLogin(LoginUser user)
    {
      LoginUser u;
      if (Users.TryRemove(user.Network.Id, out u))
      {
        if (user.Name != null)
        {
          lock (UserLocker)
          {
            NamedUsers.Remove(user.Name);
          }
        }
        u.Dispose();
      }
    }
    public void LoginComplete(LoginUser user)
    {
      LoginUser u;
      Users.TryRemove(user.Network.Id, out u);
      if (user == u)
      {
        lock (UserLocker)
        {
          NamedUsers.Remove(user.Name);
          Server.AddUser(new ServerUser(user, Server));
        }
      }
      else u.Dispose();
    }

    public void Dispose()
    {
      //never dispose Network, only server should dispose Network
      isDisposed = true;
      foreach (var u in Users.Values) u.Dispose();
    }
  }
}
