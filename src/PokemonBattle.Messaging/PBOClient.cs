using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using LightStudio.Tactic.Messaging;

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
              Lobby = null;
              Challenge = null;
              Spectate = null;
            }
            _client = value;
            if (_client != null)
            {
              Lobby = new Lobby(_client);
              var battle = new BattleClient(_client);
              Challenge = new ChallengeManager(battle);
              Spectate = new SpectateManager(battle);
              if (NewClientPrepared != null) NewClientPrepared();
            }
          }
      }
    }
    public static Lobby Lobby { get; private set; }
    public static ChallengeManager Challenge { get; private set; }
    public static SpectateManager Spectate { get; private set; }
  }
}
