using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;
using System.Threading;

namespace PokemonBattleOnline.Network
{
  internal class LoginServer : IDisposable
  {
    private readonly Server Server;
    private readonly ConcurrentDictionary<int, LoginUser> Users;
    private readonly Dictionary<string, object> Names;
    private readonly object UserLocker;
    private volatile bool isDisposed;

    public LoginServer(TcpServer network, Server server)
    {
      network.NewComingUser += OnNewUser;
      Server = server;
      Names = new Dictionary<string, object>();
      Users = new ConcurrentDictionary<int, LoginUser>();
      UserLocker = new object();
    }

    private TcpServer Network
    { get { return Server.Network; } }
    
    private void OnNewUser(TcpUser user)
    {
      if (isDisposed || !Users.TryAdd(user.Id, new LoginUser(user, this))) user.Dispose();
    }

    internal void RemoveName(string name)
    {
      lock (UserLocker)
      {
        Names.Remove(name);
      }
    }
    public bool RegisterName(LoginUser user, string name)
    {
      lock (UserLocker)
      {
        if (Names.ContainsKey(name)) return false;
        Names.Add(name, user);
        return true;
      }
    }
    public void BadLogin(LoginUser user)
    {
      LoginUser u;
      if (Users.TryRemove(user.Network.Id, out u))
      {
        if (user.Name != null) RemoveName(user.Name);
        u.Dispose();
      }
    }
    public void LoginComplete(LoginUser user)
    {
      LoginUser u;
      Users.TryRemove(user.Network.Id, out u);
      if (user == u) Server.AddUser(new ServerUser(user, Server));
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
