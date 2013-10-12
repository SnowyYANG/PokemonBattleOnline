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
      _users = new ObservableList<User>();
      _rooms = new ObservableList<Room>();
    }

    private readonly User _user;
    public User User
    { get { return _user; } }
    private readonly ObservableList<User> _users;
    public ObservableList<User> Users
    { get { return _users; } }
    private readonly ObservableList<Room> _rooms;
    public ObservableList<Room> Rooms
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
            Users.Remove(u);
            break;
          }
        return true;
      }
      return false;
    }
    internal void AddUser(User u)
    {
      u.Client = Client;
      users[u.Id] = u;
      Users.Add(u);
    }
  }
}
