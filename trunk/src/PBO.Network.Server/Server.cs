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
    private readonly Dictionary<int, ServerUser> Users;
    private readonly IdsPool RoomIds;
    private readonly Dictionary<int, RoomController> Rooms;
    internal readonly object Locker;

    internal Server(int port)
    {
      Network = new TcpServer(port);
      Login = new LoginServer(Network, this);
      State = new ServerState(this);
      Controller = new ServerController(this);
      Users = new Dictionary<int, ServerUser>();
      RoomIds = new IdsPool();
      Rooms = new Dictionary<int, RoomController>();
    }

    public void Start()
    {
      Network.IsListening = true;
    }
    /// <summary>
    /// non-lock
    /// </summary>
    internal ServerUser GetUser(int id)
    {
      return Users.ValueOrDefault(id);
    }
    internal void AddUser(ServerUser user)
    {
      lock (Locker)
      {
        user.Network.Sender.Send(State.GetClientInitInfo(user.Network.Id).ToPack());
        Send(Commands.UserS2C.AddUser(user.Network.Id, user.User.Name, user.User.Avatar));
        Users.Add(user.Network.Id, user);
        State.UserList.Add(user.User);
      }
    }
    internal void RemoveUser(ServerUser user)
    {
      lock (Locker)
      {
        Users.Remove(user.Network.Id);
        State.UserList.Remove(user.User);
        Send(Commands.UserS2C.RemoveUser(user.Network.Id));
      }
      Login.RemoveName(user.User.Name);
    }
    internal RoomController GetRoom(int id)
    {
      return Rooms.ValueOrDefault(id);
    }
    internal RoomController AddRoom(GameSettings settings)
    {
      var id = RoomIds.GetId();
      var rc = new RoomController(this, id, settings);
      Rooms.Add(id, rc);
      State.RoomList.Add(rc.Room);
      Send(Commands.RoomS2C.NewRoom(id, settings));
      return rc;
    }
    internal void RemoveRoom(RoomController rc)
    {
      var room = rc.Room;
      if (Rooms.Remove(room.Id))
      {
        rc.Dispose();
        State.RoomList.Remove(room);
        room.RemoveUsers();
        Send(Commands.RoomS2C.RemoveRoom(room.Id));
      }
    }

    internal void Send(IS2C s2c)
    {
      foreach (var u in Users.Values) u.Send(s2c);
    }

    public void Dispose()
    {
      Network.Dispose();
      Login.Dispose();
    }
  }
}
