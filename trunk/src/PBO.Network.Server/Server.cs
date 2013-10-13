using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;

namespace PokemonBattleOnline.Network
{
  public class Server : IDisposable
  {
    internal readonly TcpServer Network;
    private readonly LoginServer Login;
    public readonly ServerState State;
    private readonly ConcurrentDictionary<int, ServerUser> users;

    internal Server(int port)
    {
      Network = new TcpServer(port);
      Login = new LoginServer(Network, this);
      State = new ServerState(this);
      users = new ConcurrentDictionary<int, ServerUser>();
    }

    public void Start()
    {
      Network.IsListening = true;
    }
    internal void AddUser(ServerUser user)
    {
      lock (State.StateLocker)
      {
        if (users.TryAdd(user.Network.Id, user))
        {
          State.Users.Add(user.User);
          Send(Commands.UserChanged.AddUser(user.Network.Id, user.User.Name, user.User.Avatar));
        }
#if DEBUG
        else System.Diagnostics.Debugger.Break();
#endif
      }
    }
    internal void RemoveUser(ServerUser user)
    {
      lock (State.StateLocker)
      {
        ServerUser r;
        if (!users.TryRemove(user.Network.Id, out r))
#if DEBUG
          System.Diagnostics.Debugger.Break()
#endif
          ;
        State.Users.Remove(user.User);
        Send(Commands.UserChanged.RemoveUser(user.Network.Id));
      }
      Login.RemoveName(user.User.Name);
    }
    internal void Send(S2C s2c)
    {
      foreach (var u in users.Values) u.Send(s2c);
    }

    public void Dispose()
    {
      Network.Dispose();
      Login.Dispose();
    }
  }
}
