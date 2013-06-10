using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using LightStudio.Tactic.Messaging;
using LightStudio.PokemonBattle.Messaging.Room;

namespace LightStudio.PokemonBattle.Messaging
{
  public static class PBOClient
  {
    public static event Action NewClientPrepared;

    private static object locker = new object();
    
    public static void Prepare4Login(IPAddress serverAddress, int serverPort)
    {
      Client = Client.NewTcpClient(serverAddress, serverPort);
    }

    private static Client _client;
    public static Client Client
    {
      get
      {
        lock (locker)
          return _client;
      }
      private set
      {
        lock(locker)
          if (_client != value)
          {
            if (_client != null)
            {
              if (_client.IsLogined) _client.Logout();
              _client.Dispose();
              Lobby.Dispose();
              Battle.Dispose();
              Challenge.Dispose();
              Spectate.Dispose();
            }
            _client = value;
            if (_client != null)
            {
              Lobby = new Lobby(_client);
              Battle = new BattleClient(_client);
              Challenge = new ChallengeManager(_client, Battle);
              Spectate = new SpectateManager(Battle);
              if (NewClientPrepared != null) NewClientPrepared();
            }
          }
      }
    }
    public static BattleClient Battle { get; private set; }
    public static Lobby Lobby { get; private set; }
    public static ChallengeManager Challenge { get; private set; }
    public static SpectateManager Spectate { get; private set; }

    public static string GetName(int player)
    {
      //thread safe?
      lock(locker)
        if (Client != null)
        {
          var u = Client.GetUser(player);
          if (u != null) return u.Name;
        }
        return "#" + player.ToString();
    }
    public static string GetName(this Player player)
    {
      return GetName(player.Id);
    }
    public static void Dispose()
    {
      Client = null;
    }
  }
}
