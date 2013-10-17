using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokemonBattleOnline.Network
{
  public class ServerState
  {
    private readonly Server Server;
    internal readonly object StateLocker;

    public ServerState(Server server)
    {
      StateLocker = new object();
      _users = new ObservableList<User>();
      _rooms = new ObservableList<Room>();
    }

    private readonly ObservableList<User> _users;
    public ObservableList<User> Users
    { get { return _users; } }
    private readonly ObservableList<Room> _rooms;
    public ObservableList<Room> Rooms
    { get { return _rooms; } }

    internal ClientInitInfo GetClientInitInfo(int user)
    {
      lock (StateLocker)
      {
        return new ClientInitInfo(user, _users, _rooms);
      }
    }
  }
}
