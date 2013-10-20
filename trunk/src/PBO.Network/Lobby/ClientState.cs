using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokemonBattleOnline.Network
{
  public class ClientState
  {
    private readonly Client Client;
    private readonly Dictionary<int, User> users;
    private readonly Dictionary<int, Room> rooms;

    internal ClientState(Client client, LoginClient login, ClientInitInfo cii)
    {
      Client = client;
      users = new Dictionary<int, User>();
      rooms = new Dictionary<int, Room>();
      _user = new User(cii.User, login.Name, login.Avatar);
      _users = new ObservableList<User>(cii.LobbyUsers);
      _rooms = new ObservableList<Room>(cii.Rooms);
      
      foreach (var u in cii.LobbyUsers) users.Add(u.Id, u);
      foreach (var r in cii.Rooms)
      {
        rooms.Add(r.Id, r);
        foreach (var u in r.Players) AddUser(u);
        foreach (var u in r.Spectators) AddUser(u);
      }

      users.Add(_user.Id, _user);
      _users.Add(_user);
    }

    private readonly User _user;
    public User User
    { get { return _user; } }
    private readonly ObservableList<User> _users;
    public IEnumerable<User> Users
    { get { return _users; } }
    private readonly ObservableList<Room> _rooms;
    public IEnumerable<Room> Rooms
    { get { return _rooms; } }

    public User GetUser(int id)
    {
      return users.ValueOrDefault(id);
    }
    public Room GetRoom(int id)
    {
      return rooms.ValueOrDefault(id);
    }

    internal bool RemoveUser(int id)
    {
      if (users.Remove(id))
      {
        foreach (var u in Users)
          if (u.Id == id)
          {
            _users.Remove(u);
            break;
          }
        return true;
      }
      return false;
    }
    internal void AddUser(User u)
    {
      users[u.Id] = u;
      _users.Add(u);
    }

    internal void AddRoom(Room room)
    {
      rooms.Add(room.Id, room);
      _rooms.Add(room);
    }
    internal void RemoveRoom(int id)
    {
      if (rooms.Remove(id))
        foreach(var r in _rooms)
          if (r.Id == id)
          {
            r.RemoveUsers();
            _rooms.Remove(r);
            break;
          }
    }
  }
}
