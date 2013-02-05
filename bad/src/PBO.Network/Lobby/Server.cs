using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;
using PokemonBattleOnline.Tactic.Network;
using PokemonBattleOnline.Network.Lobby;
using PokemonBattleOnline.Network.Room;

namespace PokemonBattleOnline.Network
{
  public class Server : IDisposable
  {
    static Server()
    {
      //register
    }
    
    internal readonly INetworkServer Network;
    internal readonly object UserLocker;
    public readonly Dictionary<string, ServerUser> Users;

    internal Server(INetworkServer network)
    {
      Network = network;
      UserLocker = new object();
    }

    internal bool HasUser(string name)
    {
      lock(UserLocker)
      {
        return Users.ContainsKey(name);
      }
    }
    internal void AddUser(LoginUser user) //处于UserLocker中
    {
      Users.Add(user.Name, new ServerUser(user, this));
    }
    private void UserRemoved(User user)
    {
    }
    public void RemoveUser(string name)
    {
    }
    internal void RemoveUser(ServerUser serverUser)
    {
      throw new NotImplementedException();
    }

    internal void SendP2PPack(ServerUser serverUser, byte[] arraySegment, params int[] to)
    {
      throw new NotImplementedException();
    }
    internal void PublicChat(ServerUser serverUser, string p)
    {
      throw new NotImplementedException();
    }

    public void Dispose()
    {
      Network.Dispose();
    }
  }
}
