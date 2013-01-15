using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using PokemonBattleOnline.Tactic.Network;

namespace PokemonBattleOnline.Messaging
{
  public sealed class PBOClient : IDisposable
  {
    #region static
    public static event EventHandler Disconnected;
    public static event EventHandler BadPack;
    private static readonly object Locker;

    static PBOClient()
    {
      Locker = new object();
    }

    private static PBOClient _current;
    public static PBOClient Current
    {
      get
      {
        lock (Locker)
        {
          return _current;
        }
      }
      private set
      {
        lock (Locker)
        {
          _current = value;
        }
      }
    }
    #endregion

    private readonly Client<E, UE> Client;

    private PBOClient(Client<E, UE> client)
    {
      Client = client;
    }

    public User<UE> User
    { get { return Client.User; } }

    /// <summary>
    /// currently simply do nothing
    /// </summary>
    internal void RegisterRoom()
    {
    }
    internal void DeregisterRoom()
    {
    }

    internal void RegisterRoomUser(int userId)
    {
    }

    public void Dispose()
    {
      Client.Dispose();
      Current = null;
    }
  }
}
