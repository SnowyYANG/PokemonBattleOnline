using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokemonBattleOnline.Network
{
  public class ClientState
  {
    private readonly Client Client;
    private readonly Dictionary<int, User> Users;

    internal ClientState(Client client, LoginClient login, ClientInitInfo cii)
    {
      Client = client;
      Users = new Dictionary<int, User>();
      _user = new User(cii.User, login.Name, login.Avatar);
    }

    private readonly User _user;
    public User User
    { get { return _user; } }

    public User GetUser(int id)
    {
      return Users.ValueOrDefault(id);
    }
    internal void AddUser(User user)
    {
      Users[user.Id] = user;
      Client.Listener.UpdateUser(user);
    }
    internal void RemoveUser(int id)
    {
      if (Users.Remove(id)) Client.Listener.RemoveUser(id);
    }
  }
}
