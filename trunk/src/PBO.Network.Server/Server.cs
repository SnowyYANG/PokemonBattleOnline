using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokemonBattleOnline.Network
{
  public class Server : IDisposable
  {
    internal readonly object Locker;
    internal readonly TcpServer Network;
    private readonly LoginServer Login;
    public readonly ServerState State;
    private readonly Dictionary<int, ServerUser> Users;
    private readonly IdsPool RoomIds;
    private readonly Dictionary<int, RoomHost> Rooms;

    internal Server(int port)
    {
      Locker = new object();
      Network = new TcpServer(port);
      Login = new LoginServer(Network, this);
      State = new ServerState();
      Users = new Dictionary<int, ServerUser>();
      RoomIds = new IdsPool();
      Rooms = new Dictionary<int, RoomHost>();
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
        if (user.Room != null) user.Room.RemoveUser(user);
        Users.Remove(user.Network.Id);
        State.UserList.Remove(user.User);
        Send(Commands.UserS2C.RemoveUser(user.Network.Id));
      }
      Login.RemoveName(user.User.Name);
    }
    internal RoomHost GetRoom(int id)
    {
      return Rooms.ValueOrDefault(id);
    }
    internal RoomHost AddRoom(string name, GameSettings settings)
    {
      var id = RoomIds.GetId();
      var rc = new RoomHost(this, id, name, settings);
      Rooms.Add(id, rc);
      State.RoomList.Add(rc.Room);
      Send(Commands.RoomS2C.NewRoom(id, settings));
      return rc;
    }
    internal void RemoveRoom(RoomHost rc)
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
