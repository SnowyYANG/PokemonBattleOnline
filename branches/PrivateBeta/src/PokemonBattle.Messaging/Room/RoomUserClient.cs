﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using LightStudio.Tactic.Logging;
using LightStudio.Tactic.Messaging;
using LightStudio.PokemonBattle.Data;
using LightStudio.PokemonBattle.Game;

namespace LightStudio.PokemonBattle.Messaging.Room
{
  internal abstract class RoomUserClient : IRoomUser, IRoom, IDisposable
  {
    public readonly int HostId;

    public event Action<string> EnterFailed = delegate { };
    public event Action EnterSucceed;
    public event Action GameEnd;
    public event Action Quited;
    private ObservableCollection<Player> players;
    private ObservableCollection<int> spectators;
    private GameOutward game;
    private Dispatcher gameEventsDispatcher;
    protected IRoomEventsListener Listener
    { get; private set; }

    protected RoomUserClient(int hostId)
    {
      HostId = hostId;
      players = new ObservableCollection<Player>();
      Players = new ReadOnlyObservableCollection<Player>(players);
      spectators = new ObservableCollection<int>();
      Spectators = new ReadOnlyObservableCollection<int>(spectators);
      gameEventsDispatcher = new Dispatcher("RoomUserClient", true);
    }

    public ReadOnlyObservableCollection<Player> Players { get; private set; }
    public ReadOnlyObservableCollection<int> Spectators { get; private set; }
    public GameInitSettings Settings { get; private set; }
    public GameOutward Game
    { get { return game; } }
    public RoomState RoomState { get; private set; }
    public abstract Tactic.Messaging.UserState State { get; }
    public abstract IPlayerController PlayerController { get; }

    private void EndGame()
    {
      RoomState = Room.RoomState.GameEnd;
      GameEnd();
    }

    void IRoom.AddListener(IRoomEventsListener listener)
    {
      if (this.Listener == null) this.Listener = listener;
      else throw new Exception("IRoom.AddListener");
    }

    #region Information
    void IRoomUser.ExecuteInformation(IUserInformation info)
    {
      UIDispatcher.Invoke((Action<IRoomUser>)(info.Execute), this);
    }
    void IRoomInformer.InformUserSpectateGame(int userId)
    {
      spectators.Add(userId);
    }
    void IRoomInformer.InformUserJoinGame(int userId, int teamId)
    {
      players.Add(new Player(userId, teamId));
    }
    private void RemoveUser(int userId)
    {
      if (!spectators.Remove(userId))
      {
        foreach(Player p in players)
          if (p.Id == userId)
          {
            players.Remove(p);
            break;
          }
      }
    }
    void IRoomInformer.InformUserQuit(int userId)
    {
      RemoveUser(userId);
    }
    void IRoomInformer.InformUserKicked(int userId)
    {
      RemoveUser(userId);
    }
    void IRoomInformer.InformEnterFailed(string message)
    {
      EnterFailed(message);
    }
    void IRoomInformer.InformEnterSucceed(GameInitSettings settings, Player[] players, int[] spectators)
    {
      InformEnterSucceed(settings, players, spectators);
      EnterSucceed();
    }
    void IGameInformer.InformGameStop(GameStopReason reason, int player)
    {
      EndGame();
      Listener.GameStop(reason, player);
    }
    void IGameInformer.InformGameTie()
    {
      EndGame();
      Listener.GameTie();
    }
    void IGameInformer.InformTimeUp(IEnumerable<KeyValuePair<int, int>> remainingTime)
    {
      EndGame();
      Listener.TimeUp(remainingTime);
    }
    void IGameInformer.InformWaitingForInput(int[] players)
    {
      Listener.TimeReminder(players);
    }

    protected virtual void InformEnterSucceed(GameInitSettings settings, Player[] players, int[] spectators)
    {
      Settings = settings;
      this.players = new ObservableCollection<Player>(players);
      this.spectators = new ObservableCollection<int>(spectators);
      Players = new ReadOnlyObservableCollection<Player>(this.players);
      Spectators = new ReadOnlyObservableCollection<int>(this.spectators);
    }
    protected virtual void InformReportUpdate(ReportFragment fragment)
    {
      if (game == null)
      {
        RoomState = RoomState.GameStarted;
        Dictionary<int, string> ps = new Dictionary<int,string>();
        string[] teams = new string[2];
        foreach(Player p in players)
        {
          string name = p.GetName();
          ps.Add(p.Id, name);
          if (teams[p.Team] == null) teams[p.Team] = name;
        }
        game = new GameOutward(Settings, ps, teams);
        game.GameEnd += EndGame;
        Listener.GameStart();
      }
      game.Update(fragment);
    }
    void IGameInformer.InformReportUpdate(ReportFragment fragment)
    {
      gameEventsDispatcher.BeginInvoke((Action<ReportFragment>)InformReportUpdate, fragment);
    }

    #region Player Only
    protected abstract void InformPlayerInfo(int teamIndex, IPokemonData[] parner);
    void IGameInformer.InformPlayerInfo(int teamIndex, IPokemonData[] parner)
    {
      InformPlayerInfo(teamIndex, parner);
    }
    protected abstract void InformRequireInput(InputRequest pminfo, int spentTime);
    void IGameInformer.InformRequireInput(InputRequest pminfo, int spentTime)
    {
      InformRequireInput(pminfo, spentTime);
    }
    protected abstract void InformRequestTie();
    void IGameInformer.InformRequestTie()
    {
      InformRequestTie();
    }
    protected abstract void InformTieRejected();
    void IGameInformer.InformTieRejected()
    {
      InformTieRejected();
    }
    #endregion
    #endregion

    #region Command
    protected Action<IHostCommand> sendCommand;
    public event Action<IHostCommand> SendCommand
    { add { sendCommand += value; } remove { } }
    public abstract void EnterRoom();
    private bool isQuited = false;
    public void Quit()
    {
      lock(this)
        if (!isQuited)
        {
          isQuited = true;
          sendCommand(new QuitCommand());
          Quited();
        }
    }
    #endregion

    public void Dispose()
    {
      Quit();
      EnterFailed = delegate { };
      EnterSucceed = delegate { };
      gameEventsDispatcher.Dispose();
    }
  }
}
