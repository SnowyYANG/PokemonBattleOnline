using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;

namespace PokemonBattleOnline.Tactic.Network
{
  public class Server<TE, TUE>
  {
    static Server()
    {
      //register
    }
    
    internal readonly INetworkServer Network;
    internal readonly object UserLocker;
    public readonly Dictionary<string, ServerUser<TE, TUE>> Users;

    internal Server(INetworkServer network)
    {
      Network = network;
    }

    internal bool HasUser(string name)
    {
      lock(UserLocker)
      {
        return Users.ContainsKey(name);
      }
    }
    internal void AddUser(LoginUser<TUE> user) //处于UserLocker中
    {
      Users.Add(user.Name, new ServerUser<TE, TUE>(user, this));
    }
    private void UserRemoved(User<TUE> user)
    {
    }
    internal void RemoveUser(string name, RemoveUserReason reason)
    {
    }
    public void RemoveUser(string name)
    {
    }

    internal void SendP2PPack(ServerUser<TE, TUE> serverUser, byte[] arraySegment, params int[] to)
    {
      throw new NotImplementedException();
    }
    internal void PublicChat(ServerUser<TE, TUE> serverUser, string p)
    {
      throw new NotImplementedException();
    }
  }
}
