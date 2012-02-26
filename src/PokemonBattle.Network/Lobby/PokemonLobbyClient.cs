using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using LightStudio.Tactic.Messaging;
using LightStudio.Tactic.Messaging.Lobby;
using LightStudio.Tactic.Serialization;
using LightStudio.PokemonBattle.Data;
using LightStudio.PokemonBattle.Game;
//using LightStudio.PokemonBattle.Room;

namespace LightStudio.PokemonBattle.Messaging
{
  public class PokemonLobbyClient : LobbyClient
  {
    public const byte CHAT = 1;
    public const byte CHALLENGE = 2;
    public const byte GAME_MESSAGE = 3;
    public const byte ACCEPT_CHALLENGE = 4;
    public const byte REFUSE_CHALLENGE = 5;
    public const byte CANCEL_CHALLENGE = 6;

    public event Action<Room.IUserController> EnterSucceed
    {
      add { if (battleClient != null) battleClient.EnterSucceed += value; }
      remove { if (battleClient != null) battleClient.EnterSucceed -= value; }
    }
    private object roomLock; //make sure one room one time
    private IBattleClient battleClient;
    private IBattleHost battleHost;
    private PokemonCustomInfo[] challengingPms;
    private GameSettings currentSettings; //被挑战的临时游戏设置与此变量无关

    public PokemonLobbyClient(IPAddress serverAddress, int serverPort)
      : base(new TcpMessageClient(serverAddress, serverPort))
    {
      roomLock = new object();
      LobbyService.Register(this);
      EnterSucceed += (user) =>
        {
          user.Quited += () =>
            {
              lock (roomLock)
              {
                battleClient = null;
                battleHost = null;
                challengingPms = null;
                if (User.State == UserState.Battling || User.State == UserState.Watching)
                  ChangeState(UserState.Normal);
              };
            };
          switch (user.Role)
          {
            case Room.UserRole.Player:
              User.State = UserState.Aggressive;
              break;
            case Room.UserRole.Spectator:
              User.State = UserState.Watching;
              break;
          }
        };
    }

    protected override void OnMessageReceived(int senderId, string content)
    {
      base.OnMessageReceived(senderId, content);
      if (string.IsNullOrEmpty(content)) return;
      ReadMessage(content, (header, reader) =>
        {
          switch (header)
          {
            case CHAT:
              OnChatMessageReceived(GetUser(senderId), reader.ReadString());
              break;
            case CHALLENGE:
              OnChallenged(senderId, GameSettings.ReadFromMessage(reader));
              break;
            case CANCEL_CHALLENGE:
              OnChallengeCanceled(senderId);
              break;
            case ACCEPT_CHALLENGE:
              OnChallengeAccepted(senderId);
              break;
            case REFUSE_CHALLENGE:
              OnChallengeRefused(senderId);
              break;
            case GAME_MESSAGE:
              HandleGameMessage(senderId, reader);
              break;
          }
        });
    }
    protected override void DisposeManagedResources()
    {
      base.DisposeManagedResources();
      if (battleClient != null)
        battleClient.Dispose();
    }

    #region GameMessage
    private void HandleGameMessage(int senderId, BinaryReader reader)
    {
      if (battleHost != null)
      {
        string header = reader.ReadString();
        string content = reader.ReadString();
        var message = new TextMessage(header, content);
        battleHost.OnReceived(senderId, message);
      }
      else if (battleClient != null && senderId == battleClient.HostId)
      {
        string header = reader.ReadString();
        string content = reader.ReadString();
        var message = new TextMessage(header, content);
        battleClient.OnReceived(message);
      }
    }
    #endregion

    #region Challenge
    public event Action<User, GameSettings> Challenged = delegate { };
    private void OnChallenged(int userId, GameSettings settings)
    {
      Challenged(GetUser(userId), settings);
    }
    public bool Challenge(int target, PokemonCustomInfo[] pokemons, GameSettings settings)
    {
      User u = GetUser(target);
      if (u != null && u.State != UserState.Battling && pokemons != null && pokemons.Length > 0) //it's impossible for a client to get UserState.Invalid
        lock (roomLock)
        {
          if (challengingPms == null)
          {
            SendMessage(CHALLENGE, writer => settings.WriteToMessage(writer), target);
            challengingPms = pokemons;
            currentSettings = settings;
            return true;
          }
        }
      return false;
    }
    #endregion

    #region CancelChallenge
    /// <summary>
    /// remember to determine user!=null
    /// </summary>
    public event Action<User> ChallengeCanceled = delegate { };
    private void OnChallengeCanceled(int userId)
    {
      ChallengeCanceled(GetUser(userId));
    }
    public void CancelChallenge(int target)
    {
      lock (roomLock)
      {
        if (challengingPms != null)
        {
          SendMessage(CANCEL_CHALLENGE, target);
          challengingPms = null;
        }
      }
    }
    #endregion

    #region RefuseChallenge
    public event Action<User> ChallengeRefused = delegate { };
    private void OnChallengeRefused(int userId)
    {
      ChallengeRefused(GetUser(userId));
    }
    public void RefuseChallenge(int challenger)
    {
      SendMessage(REFUSE_CHALLENGE, challenger);
    }
    #endregion

    #region AcceptChallenge & StartGame
    public event Action<User> ChallengeAccepted = delegate { };
    private void OnChallengeAccepted(int userId)
    {
      lock (roomLock)
      {
        ChallengeAccepted(GetUser(userId));
        SingleHost s = new SingleHost(User.Id, currentSettings);///////////
        battleHost = s;
        battleClient = s;
        StartGame(challengingPms);
        challengingPms = null;
      }
    }
    public bool AcceptChallenge(int challenger, PokemonCustomInfo[] pokemons)
    {
      if (pokemons != null && pokemons.Length > 0)
      lock (roomLock)
        if (battleClient == null)
        {
          SendMessage(ACCEPT_CHALLENGE, challenger);
          //假如对方房间还没设好怎么办...测试了似乎没问题？
          battleClient = new SingleClient(challenger, currentSettings);
          StartGame(pokemons);
          challengingPms = null;
          return true;
        }
      return false;
    }
    private void StartGame(PokemonCustomInfo[] pokemonsTeam)
    {
      //roomLock is already locked
      battleClient.MessageSent += (sender, e) =>
        SendMessage(GAME_MESSAGE, writer =>
        {
          writer.Write(e.Message.Header);
          writer.Write(e.Message.Content);
        }, e.Receivers);
      battleClient.Start(pokemonsTeam);
    }
    #endregion

    #region Chat
    public event EventHandler<ChatMessageReceivedEventArgs> ChatMessageReceived = delegate { };
    private void OnChatMessageReceived(User userInfo, string content)
    {
      ChatMessageReceived(this, new ChatMessageReceivedEventArgs(userInfo, content));
    }
    public void Chat(string message, params int[] receivers)
    {
      SendMessage(CHAT, reader => reader.Write(message), receivers);
    }
    #endregion
  }
}
