using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.ComponentModel;
using LightStudio.Tactic;
using LightStudio.PokemonBattle.Data;
using LightStudio.PokemonBattle.Interactive;
using LightStudio.PokemonBattle.Game;

namespace LightStudio.PokemonBattle.Room
{
  public class Host : IHost, INotifyPropertyChanged, IDisposable
  {
    public event PropertyChangedEventHandler PropertyChanged = delegate { };
    public event Action Closed = delegate { };
    private readonly Dispatcher dispatcher;
    private readonly HashSet<int> users;
    private readonly ObservableCollection<int> players;
    private readonly ObservableCollection<int> spectators;
    private readonly IGame game;
    private RoomState state;
    
    public Host(GameSettings settings)
    {
      dispatcher = new Dispatcher(true);
      users = new HashSet<int>();
      players = new ObservableCollection<int>();
      spectators = new ObservableCollection<int>();
      Players = new ReadOnlyObservableCollection<int>(players);
      Spectators = new ReadOnlyObservableCollection<int>(spectators);
      game = GameFactory.CreateGame(settings);
      game.ReportUpdated += InformReportUpdate;
      game.InputSucceed += InformInputSucceed;
    }

    public ReadOnlyObservableCollection<int> Spectators
    { get; private set; }
    public ReadOnlyObservableCollection<int> Players
    { get; private set; }
    public RoomState State
    {
      get { return state; }
      private set
      {
        if (state != value)
        {
          state = value;
          OnPropertyChanged(STATE);
        }
      }
    }
    public bool CanStartGame
    { get { return game.Prepared; } }

    #region Commands
    void IHost.ExecuteCommand(IHostCommand command, int senderId)
    {
      dispatcher.Invoke((Action<IHost, int>)(command.Execute), this, senderId);
    }
    void IRoomManager.Chat(int userId, MessageTarget target, int targetId, string content)
    {
    }
    void IRoomManager.JoinGame(int userId, PokemonCustomInfo[] pokemons, int teamId)
    {
      bool canStartGame = CanStartGame;
      if (game.SetPlayer(teamId, userId, pokemons))
      {
        players.Add(userId);
        users.Add(userId);
        InformEnterSucceed(userId);
        if (CanStartGame != canStartGame) OnPropertyChanged(CAN_START_GAME);
      }
      else
        InformEnterFailed("debug.failed", userId);
    }
    void IRoomManager.SpectateGame(int userId)
    {
      spectators.Add(userId);
      users.Add(userId);
      InformEnterSucceed(userId);
    }
    void IRoomManager.Quit(int userId)
    {
      if (users.Contains(userId))
      {
        users.Remove(userId);
        InformUserQuit(userId);
        if (players.Contains(userId))
        {
          InformGameStop();
        }
        else if (spectators.Contains(userId))
        {
          spectators.Remove(userId);
        }
      }
    }
    void IGameManager.RequestTie(int userId)
    {
      if (State == RoomState.GameStarted) ;
    }
    void IGameManager.RejectTie(int userId)
    {
      if (State == RoomState.GameStarted) ;
    }
    void IGameManager.AcceptTie(int userId)
    {
      if (State == RoomState.GameStarted) ;
    }
    void IGameManager.Input(int userId, ActionInput action)
    {
      if (State == RoomState.GameStarted)
      {
        if (!game.InputAction(userId, action))
          InformInputFail(userId);
      }
    }
    #endregion

    #region Inform
    internal event Action<IUserInformation, int[]> SendInformation;
    void OnSendInformation(IUserInformation info, params int[] userIds)
    {
      if (userIds.Length < 1) SendInformation(info, users.ToArray());
      else SendInformation(info, userIds);
    }
    void InformChat(int userId, MessageTarget target, string content)
    {
    }
    void InformUserSpectateGame(int userId)
    {
    }
    void InformUserJoinGame(int userId, int teamId)
    {
    }
    void InformUserQuit(int userId)
    {
      OnSendInformation(new UserQuitInfo(userId));
    }
    void InformUserKicked(int userId)
    {
      OnSendInformation(new UserKickedInfo(userId));
    }
    void InformEnterFailed(string message, int userId)
    {
      OnSendInformation(new EnterFailedInfo(message), userId);
    }
    void InformEnterSucceed(int userId)
    {
      if (players.Contains(userId))
      {
        Game.Player p = game.GetPlayer(userId);
        List<int> ids = new List<int>();
        foreach (Pokemon pm in p.Pokemons)
        {
          ids.Add(pm.Id);
          foreach (Move m in pm.Moves)
            if (m != null) ids.Add(m.Id);
          ids.Add(pm.StruggleId);
          ids.Add(pm.SwitchId);
        }
        OnSendInformation(new EnterSucceedInfo(game.Settings, players.ToArray(), spectators.ToArray(), ids.ToArray()), userId);
      }
      else OnSendInformation(new EnterSucceedInfo(game.Settings, players.ToArray(), spectators.ToArray()), userId);
    }
    void InformTimeUp(IList<int> breakers)
    {
      OnSendInformation(new TimeUpInfo(breakers));
    }
    void InformTimeElapsed(int remainingSeconds)
    {
    }
    /// <summary>
    /// 正常结束 至少有一方精灵全部倒下
    /// </summary>
    void InformGameResult(int team0, int team1)
    {
      State = RoomState.GameEnd;
      OnSendInformation(GameResultInfo.GameResult(team0, team1));
    }
    /// <summary>
    /// 议和，不经过Game
    /// </summary>
    void InformGameTie()
    {
      State = RoomState.GameEnd;
      OnSendInformation(GameResultInfo.GameTie());
    }
    /// <summary>
    /// 有玩家违规，中途退出游戏或超时，不经过Game
    /// </summary>
    void InformGameStop()
    {
      State = RoomState.GameEnd;
      OnSendInformation(GameResultInfo.GameStop());
    }
    void InformReportUpdate(ReportFragment fragment, int[] playersNeedInput)
    {
      HashSet<int> watchOnly = new HashSet<int>(users);
      watchOnly.RemoveWhere((id) => playersNeedInput.Contains(id));
      if (watchOnly.Count > 0) OnSendInformation(new ReportUpdateInfo(fragment, true), watchOnly.ToArray());
      foreach(int id in playersNeedInput)
        OnSendInformation(new ReportUpdateInfo(fragment, false), id);
    }
    void InformAdditionalInfo(PokemonAdditionalInfo info)
    {
      OnSendInformation(new PmAddionalInfo(info), info.GetReceiversId());
    }

    void InformRequestTie()
    {
    }
    void InformTieRejected()
    {
    }

    void InformInputFail(int userId)
    {
      OnSendInformation(new InputFailInfo(), userId);
    }
    void InformInputSucceed(Game.Player player)
    {
      OnSendInformation(new InputSucceedInfo(), player.Id);
    }
    #endregion

    public void Kick(int targetId)
    {
    }
    public void StartGame()
    {
      if (game.Start()) State = RoomState.GameStarted;
    }
    public void CloseRoom()
    {
      Closed();
    }

    #region PropertyChanged
    public const string CAN_START_GAME = "CanStartGame";
    public const string STATE = "State";
    private void OnPropertyChanged(string propertyName)
    {
      PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
    }
    #endregion

    public void Dispose()
    {
      PropertyChanged = delegate { };
      SendInformation = delegate { };
      dispatcher.Dispose();
    }
  }
}
