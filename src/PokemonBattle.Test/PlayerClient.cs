using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Data;
using LightStudio.PokemonBattle.Game;

namespace LightStudio.PokemonBattle.Test
{
  interface ITestClient : IDisposable
  {
    event Action<InputRequest> RequireInput;
    GameOutward GameOutward { get; }
    SimGame SimGame { get; }
    void Struggle();
    void Move(int index);
    void Pokemon(int index);
  }
  class PlayerClient : ITestClient, IGameOutwardEvents
  {
    private static readonly string[] TEAMS = { "P1", "P2" };
    private static readonly Dictionary<int, string> PLAYERS;
    static PlayerClient()
    {
      PLAYERS = new Dictionary<int, string>(2);
      PLAYERS.Add(1, "P1");
      PLAYERS.Add(2, "P2");
    }

    public event Action GameEnd = delegate { };
    public readonly int PlayerId;
    public readonly int TeamId;
    private readonly Host Host;
    private readonly IPokemonData[] Pokemons;
    private readonly bool ShowLog;
    private readonly Queue<int> Ids;
    private readonly Dispatcher Dispatcher;

    public PlayerClient(Host host, int playerId, int teamId, IPokemonData[] pokemons, bool showLog, IEnumerable<int> ids)
    {
      Host = host;
      PlayerId = playerId;
      TeamId = teamId;
      Pokemons = pokemons;
      ShowLog = showLog;
      Ids = new Queue<int>(ids);
      Dispatcher = new Dispatcher("P" + playerId.ToString(), true);
      _InformReportUpdateDelegate = new Action<ReportFragment>(_InformReportUpdate);
    }

    public GameOutward GameOutward
    { get; private set; }
    public SimGame SimGame
    { get; private set; }

    private Delegate _InformReportUpdateDelegate;
    private bool needInput;
    public void InformReportUpdate(ReportFragment fragment)
    {
      Dispatcher.BeginInvoke(_InformReportUpdateDelegate, fragment);
    }
    private void _InformReportUpdate(ReportFragment fragment)
    {
      if (GameOutward == null)
      {
        GameOutward = new GameOutward(Host.Settings, PLAYERS, TEAMS);
        GameOutward.LeapTurn += delegate { };
        GameOutward.AddListner(this);
        SimGame = new SimGame(GameOutward, PlayerId, TeamId, Pokemons, Ids.Dequeue);
      }
      GameOutward.Update(fragment);
      SimGame.Update(fragment);
      if (GameOutward.Teams[0].Normal + GameOutward.Teams[0].Abnormal == 0 || GameOutward.Teams[1].Normal + GameOutward.Teams[1].Abnormal == 0) GameEnd();
      else
        if (needInput)
        {
          needInput = false;
          if (_requireInput != null)
          {
            request.Init(SimGame);
            request.InputFinished += (i) => Host.Input(this, i);
            _requireInput(request);
          }
        }
    }
    public void InformRequireInput(InputRequest request)
    {
      this.request = request;
      needInput = true;
    }

    #region ITestClient
    private InputRequest request;
    private Action<InputRequest> _requireInput;
    event Action<InputRequest> ITestClient.RequireInput
    {
      add { _requireInput += value; }
      remove { _requireInput -= value; }
    }
    void ITestClient.Struggle()
    {
      request.Fight();
    }
    void ITestClient.Move(int index)
    {
      request.Move(SimGame.OnboardPokemons[0].Moves[index]);
    }
    void ITestClient.Pokemon(int index)
    {
      if (request.IsSendout) request.Pokemon(SimGame.Player.GetPokemon(index), 0);
      else request.Pokemon(SimGame.Player.GetPokemon(index));
    }
    #endregion

    #region IGameOutwardEvents
    void AddText(IText t)
    {
      if (t.Contents == null) AIController.WriteBattleReport(t.Text);
      else foreach (var child in t.Contents) AddText(child);
    }
    void IGameOutwardEvents.GameLogAppend(IText t)
    {
      if (ShowLog) AddText(t);
    }
    void IGameOutwardEvents.TurnEnd()
    {
    }
    void IGameOutwardEvents.GameEnd()
    {
    }
    #endregion

    public void Dispose()
    {
      Dispatcher.Dispose();
    }
  }
}
