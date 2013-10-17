using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokemonBattleOnline.Network
{
  public class Server : IDisposable
  {
    internal readonly TcpServer Network;
    private readonly LoginServer Login;
    public readonly ServerState State;
    internal readonly ServerController Controller;
    private readonly Dictionary<int, ServerUser> users;

    internal Server(int port)
    {
      Network = new TcpServer(port);
      Login = new LoginServer(Network, this);
      State = new ServerState(this);
      Controller = new ServerController(this);
      users = new Dictionary<int, ServerUser>();
    }

    public void Start()
    {
      Network.IsListening = true;
    }
    internal void AddUser(ServerUser user)
    {
      lock (State.StateLocker)
      {
        user.Network.Sender.Send(State.GetClientInitInfo(user.Network.Id).ToPack());
        SendAll(Commands.UserChanged.AddUser(user.Network.Id, user.User.Name, user.User.Avatar));
        users.Add(user.Network.Id, user);
        State.Users.Add(user.User);
      }
    }
    internal void RemoveUser(ServerUser user)
    {
      lock (State.StateLocker)
      {
        users.Remove(user.Network.Id);
        State.Users.Remove(user.User);
        SendAll(Commands.UserChanged.RemoveUser(user.Network.Id));
      }
      Login.RemoveName(user.User.Name);
    }
    internal void SendAll(S2C s2c)
    {
      foreach (var u in users.Values) u.Send(s2c);
    }
    internal void SendRoom(S2C s2c)
    {
    }
    internal void Send(S2C s2c)
    {
    }

    public void Dispose()
    {
      Network.Dispose();
      Login.Dispose();
    }
  }
}
