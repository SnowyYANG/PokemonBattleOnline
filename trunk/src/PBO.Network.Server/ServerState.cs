using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokemonBattleOnline.Network
{
  public class ServerState
  {
    public ServerState()
    {
      UserList = new ObservableList<User>();
      RoomList = new ObservableList<Room>();
    }

    internal readonly ObservableList<User> UserList;
    public IEnumerable<User> Users
    { get { return UserList; } }
    internal readonly ObservableList<Room> RoomList;
    /// <summary>
    /// for binding only
    /// </summary>
    public IEnumerable<Room> Rooms
    { get { return RoomList; } }

    internal ClientInitInfo GetClientInitInfo(int user)
    {
      return new ClientInitInfo(user, UserList, RoomList);
    }
  }
}
