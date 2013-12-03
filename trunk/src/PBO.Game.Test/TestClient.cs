using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonBattleOnline.Game;

namespace PokemonBattleOnline.Test
{
  class TestClient : IGameOutwardEvents, IDisposable
  {
    private static readonly string[,] PLAYERS = { {"P00", "P01" }, {"P10", "P11"} };

    public event Action GameEnd = delegate { };
    public readonly int PlayerId;
    public readonly int TeamId;
    private readonly Host Host;
    private readonly IPokemonData[] Pokemons;
    private readonly bool ShowLog;
    private readonly Dispatcher Dispatcher;

    public TestClient(Host host, int playerId, int teamId, IPokemonData[] pokemons, bool showLog, IEnumerable<int> ids)
    {
      Host = host;
      PlayerId = playerId;
      TeamId = teamId;
      Pokemons = pokemons;
      ShowLog = showLog;
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
        GameOutward = new GameOutward(Host.Settings, PLAYERS);
        GameOutward.AddListner(this);
        SimGame = new SimGame(Host.Settings, new SimPlayer(TeamId, 0, Pokemons), null);
      }
      GameOutward.Update(fragment.Events);
      SimGame.Update(fragment);
      if (GameOutward.Board.Teams[0].AliveCount == 0 || GameOutward.Board.Teams[1].AliveCount == 0) GameEnd();
      else
        if (needInput)
        {
          needInput = false;
          if (RequireInput != null)
          {
            request.Init(SimGame);
            request.InputFinished += (i) => Host.Input(this, i);
            RequireInput(request);
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
    public Action<InputRequest> RequireInput;
    public void Struggle()
    {
      request.Fight();
    }
    public void Move(int index, bool mega)
    {
      request.Move(SimGame.OnboardPokemons[0].Moves[index], mega);
    }
    public void Pokemon(int index)
    {
      if (request.IsSendOut) request.Pokemon(SimGame.Player.GetPokemon(index), 0);
      else request.Pokemon(SimGame.Player.GetPokemon(index));
    }
    #endregion

    #region IGameOutwardEvents
    void IGameOutwardEvents.GameLogAppend(string t, LogStyle style)
    {
      if (ShowLog) AIController.WriteBattleReport(t);
    }
    void IGameOutwardEvents.TurnEnd()
    {
    }
    #endregion

    public void Dispose()
    {
      Dispatcher.Dispose();
    }
  }
}
