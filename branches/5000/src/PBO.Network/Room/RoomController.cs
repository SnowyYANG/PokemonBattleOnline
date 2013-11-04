﻿using System;
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
      UIDispatcher.Invoke(GameStop, reason, player);
    }
    public static event Action<User[]> TimeReminder;
    internal static void OnTimeReminder(User[] waitForWhom)
    {
      UIDispatcher.Invoke(TimeReminder, new object[] { waitForWhom });
    }
    public static event Action<KeyValuePair<User, int>[]> TimeUp;
    internal static void OnTimeUp(KeyValuePair<User, int>[] remainingTime)
    {
      UIDispatcher.Invoke(TimeUp, new object[] { remainingTime });
    }
    public static event Action Entered;
    internal static void OnEntered()
    {
      UIDispatcher.Invoke(Entered);
    }
    public static event Action Quited;
    public static event Action GameInited;

    internal readonly Client _Client;
    
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
          _game.GameEnd += Game_GameEnd;
          OnPropertyChanged("Game");
          UIDispatcher.Invoke(GameInited);
        }
      }
    }

    private void Game_GameEnd()
    {
      OnGameStop(GameStopReason.GameEnd, null);
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
      InputRequest = null;
      _prepare00 = false;
      _prepare01 = false;
      _prepare10 = false;
      _prepare11 = false;
    }

    internal void OnQuited()
    {
      Reset();
      UIDispatcher.BeginInvoke(Quited);
    }

    internal PokemonData[] Partner;
    internal void GameStart(ReportFragment gameUpdateS2C)
    {
      Dictionary<int, string> ps = new Dictionary<int, string>();
      var mi = Room.Settings.Mode.PlayersPerTeam();
      string[, ] players = new string[2, mi];
      for (int t = 0; t < 2; ++t)
        for (int i = 0; i < mi; ++i) players[t, i] = Room[t, i].Name;
      if (User.Seat != Seat.Spectator) PlayerController = new PlayerController(this, Self, Partner);
      Game = new GameOutward(Room.Settings, players);
      Game.Start(gameUpdateS2C);
    }

    internal InputRequest InputRequest;
  }
}