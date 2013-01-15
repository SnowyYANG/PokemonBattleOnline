using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;
using System.Threading;

namespace PokemonBattleOnline.Tactic.Network
{
  internal class LoginServer<TSE, TUE>
  {
    private readonly Server<TSE, TUE> Server;
    private readonly ConcurrentDictionary<int, LoginUser<TUE>> Users;
    private readonly ConcurrentDictionary<string, LoginUser<TUE>> NamedUsers;

    public LoginServer(INetworkServer network, Server<TSE, TUE> server)
    {
      Server = server;
      network.NewUser += OnNewUser;
    }

    private INetworkServer Network
    { get { return Server.Network; } }
    private object UserLocker
    { get { return Server.UserLocker; } }

    private void OnNewUser(INetworkUser user)
    {
      
    }

    internal bool RegisterUserName(string name, LoginUser<TUE> user)
    {
      lock (UserLocker)
      {
        return !Server.HasUser(name) && NamedUsers.TryAdd(name, user);
      }
    }
    internal void LoginComplete(LoginUser<TUE> user)
    {
      lock (UserLocker)
      {
        LoginUser<TUE> u;
        NamedUsers.TryRemove(user.Name, out u);
        Server.AddUser(user);
      }
    }
  }
}
