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
    }

    internal ClientInitInfo GetClientInitInfo(int user)
    {
      lock (StateLocker)
      {
        return new ClientInitInfo(user);
      }
    }
    internal void AddUser(ServerUser user)
    {
      //Users.Add(user.User.Name, user);
      //users[user.Network.Id] = user;
      //UsersUpdate(user.User);
    }
    internal void RemoveUser(ServerUser user)
    {
      //Users.Remove(user.User.Name);
      //ServerUser u;
      //users.TryRemove(user.Network.Id, out u);
      //user.User.State = UserState.Quited;
      //UsersUpdate(user.User);
      //users.Remove(user.User.Name);
    }
  }
}
