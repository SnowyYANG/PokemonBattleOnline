using System;
using System.Net;
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
    private readonly Dictionary<int, ServerUser> Users;
    private readonly IdsPool RoomIds;
    private readonly Dictionary<int, RoomHost> Rooms;
    public List<IPAddress> Banlist;

    internal Server(int port)
    {
      Locker = new object();
      Network = new TcpServer(port);
      Login = new LoginServer(Network, this);
      Users = new Dictionary<int, ServerUser>();
      RoomIds = new IdsPool();
      Rooms = new Dictionary<int, RoomHost>();
      Banlist = Network.Banlist;
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
        user.Network.Sender.Send(GetCII(user.Network.Id).ToPack());
        Send(Commands.UserS2C.AddUser(user.Network.Id, user.User.Name, user.User.Avatar));
        Users.Add(user.Network.Id, user);
        Console.Write(DateTime.Now.ToString("(hh:mm:ss) "));
        Console.WriteLine(user.User.Name + " has entered the lobby.");
      }
    }

    public void ListUsers()
    {
        foreach (ServerUser user in Users.Values)
        {
            Console.WriteLine(user.User.Name);
        }
    }
    public void BanIp(IPAddress ip)
    {
        Banlist.Add(ip);
        foreach (KeyValuePair<int, ServerUser> ea in Users)
            if (ea.Value.Network.EndPoint.Address == ip)
            {
                Users.Remove(ea.Key);
                RemoveUser(ea.Value);
            }
    }
    public void UnbanIp(IPAddress ip)
    {
        Banlist.Remove(ip);
    }
    private ClientInitInfo GetCII(int user)
    { 
      //already in lock
      var lus = new List<User>();
      var rs = new Room[Rooms.Count];
      foreach (var u in Users.Values)
        if (u.Room == null) lus.Add(u.User);
      int i = 0;
      foreach (var r in Rooms.Values) rs[i++] = r.Room;
      return new ClientInitInfo(user, lus.ToArray(), rs);
    }


    internal void RemoveUser(ServerUser user)
    {
      lock (Locker)
      {
        if (user.Room != null) user.Room.RemoveUser(user);
        Users.Remove(user.Network.Id);
        Send(Commands.UserS2C.RemoveUser(user.Network.Id));
      }
      Login.RemoveName(user.User.Name);
      Console.Write(DateTime.Now.ToString("(hh:mm:ss) "));
      Console.WriteLine(user.User.Name + " has left the lobby.");
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
      Send(Commands.RoomS2C.NewRoom(id, settings));
      return rc;
    }
    internal void RemoveRoom(RoomHost rc)
    {
      var room = rc.Room;
      if (Rooms.Remove(room.Id))
      {
        rc.Dispose();
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
