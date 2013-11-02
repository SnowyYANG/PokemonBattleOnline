using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonBattleOnline.Game;
using PokemonBattleOnline.Network.Commands;

namespace PokemonBattleOnline.Network
{
  public class RoomController : ObservableObject
  {
    public static event Action<string, User> RoomChat;
    internal static void OnRoomChat(string chat, User user)
    {
      UIDispatcher.Invoke(RoomChat, chat, user);
    }
    public static event Action<GameStopReason, User> GameStop;
    internal static void OnGameStop(GameStopReason reason, User player)
    {
      UIDispatcher.Invoke(GameStop, player);
    }
    public static event Action<User[]> TimeReminder;
    internal static void OnTimeReminder(User[] waitForWhom)
    {
      UIDispatcher.Invoke(TimeReminder, waitForWhom);
    }
    public static event Action<KeyValuePair<User, int>[]> TimeUp;
    internal static void OnTimeUp(KeyValuePair<User, int>[] remainingTime)
    {
      UIDispatcher.Invoke(TimeUp, remainingTime);
    }
    public static event Action Entered;
    internal static void OnEntered()
    {
      UIDispatcher.Invoke(Entered);
    }
    public static event Action Quited;
    public static event Action GameStarted;

    internal readonly Client _Client;
    internal RequireInputS2C InputRequest;
    
    internal RoomController(Client client)
    {
      _Client = client;
    }

    public ClientController Client
    { get { return _Client.Controller; } }
    public User User
    { get { return _Client.Controller.User; } }
    public Room Room
    { get { return _Client.Controller.User.Room; } }
    private GameOutward _game;
    public GameOutward Game
    {
      get { return _game; }
      private set
      {
        if (_game != value)
        {
          _game = value;
          OnPropertyChanged("Game");
          UIDispatcher.Invoke(GameStarted);
        }
      }
    }
    private PlayerController _playerController;
    public PlayerController PlayerController
    {
      get { return _playerController; }
      private set
      {
        if (_playerController != value)
        {
          _playerController = value;
          OnPropertyChanged("PlayerController");
        }
      }
    }
    private bool _prepare00;
    public bool Prepare00
    {
      get { return _prepare00; }
      internal set
      {
        if (_prepare00 != value)
        {
          _prepare00 = value;
          OnPropertyChanged("Prepare00");
        }
      }
    }
    private bool _prepare01;
    public bool Prepare01
    {
      get { return _prepare01; }
      internal set
      {
        if (_prepare01 != value)
        {
          _prepare01 = value;
          OnPropertyChanged("Prepare01");
        }
      }
    }
    private bool _prepare10;
    public bool Prepare10
    {
      get { return _prepare10; }
      internal set
      {
        if (_prepare10 != value)
        {
          _prepare10 = value;
          OnPropertyChanged("Prepare10");
        }
      }
    }
    private bool _prepare11;
    public bool Prepare11
    {
      get { return _prepare11; }
      internal set
      {
        if (_prepare11 != value)
        {
          _prepare11 = value;
          OnPropertyChanged("Prepare11");
        }
      }
    }
    
    public void ChangeSeat(Seat seat)
    {
      //too complex logic
      throw new NotImplementedException();
    }
    public void Chat(string chat)
    {
      _Client.Send(ChatC2S.RoomChat(chat));
    }
    public void Quit()
    {
      _Client.Send(SetSeatC2S.LeaveRoom());
    }

    private PokemonData[] Self;
    public void GamePrepare(PokemonData[] team)
    {
      if (User.Seat != Seat.Spectator && !Room.Battling)
      {
        Self = team;
        _Client.Send(PrepareC2S.Prepare(team));
      }
    }
    public void GameUnPrepare()
    {
      if (User.Seat != Seat.Spectator && !Room.Battling) _Client.Send(PrepareC2S.UnPrepare());
    }

    internal void Reset()
    {
      _game = null;
      _playerController = null;
      _prepare00 = false;
      _prepare01 = false;
      _prepare10 = false;
      _prepare11 = false;
      UIDispatcher.BeginInvoke(Quited);
    }

    internal PokemonData[] Partner;
    internal void GameStart(ReportFragment gameUpdateS2C)
    {
      Dictionary<int, string> ps = new Dictionary<int, string>();
      string[, ] players = new string[2, 2];
      players[0, 0] = Room[0, 0].Name;
      players[0, 1] = Room[0, 1].Name;
      players[1, 0] = Room[1, 0].Name;
      players[1, 1] = Room[1, 1].Name;
      Game = new GameOutward(Room.Settings, players);
      if (User.Seat != Seat.Spectator) PlayerController = new PlayerController(this, Self, Partner);
      Game.Start(gameUpdateS2C);
    }
  }
}
