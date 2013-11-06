using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokemonBattleOnline.Game.Host
{
  internal class Controller : IDisposable
  {
    public event Action<ReportFragment, InputRequest[,]> GameUpdated;

    public readonly ReportBuilder ReportBuilder;
    public readonly IGameSettings GameSettings;
    public readonly Team[] Teams;
    public readonly Board Board;
    public readonly GoTimer Timer;
    private readonly SwitchController SwitchController;
    private readonly InputController InputController;
    private readonly TurnController TurnController;
    private readonly Dictionary<int, PokemonProxy> pokemons;
    
    private Random random;
#if ETV
    private Random randomSeeds;
#endif

    internal Controller(IGameSettings settings, IPokemonData[,][] pms)
    {
      GameSettings = settings;
      Board = new Board(GameSettings);
      pokemons = new Dictionary<int, PokemonProxy>();

      Teams = new Team[settings.Mode.TeamCount()];
      for (int t = 0; t < Teams.Length; ++t)
      {
        var players = new Player[settings.Mode.PlayersPerTeam()];
        for (int p = 0; p < settings.Mode.PlayersPerTeam(); ++p)
        {
          var pl = new Player(this, t, p, pms[t, p]);
          players[p] = pl;
          foreach (Pokemon pm in pl.Pokemons) pokemons.Add(pm.Id, new PokemonProxy(this, pm));
        }
        Teams[t] = new Team(t, players, settings);
      }

      ReportBuilder = new ReportBuilder(this);
      SwitchController = new SwitchController(this);
      InputController = new InputController(this);
      TurnController = new TurnController(this);
#if ETV
      randomSeeds = new Random(1);
      random = new Random(randomSeeds.Next());
#else
      random = new Random();
#endif
      Timer = new GoTimer(Teams[0].Players.Concat(Teams[1].Players).ToArray());
      Timer.Start();
    }

    /// <summary>
    /// sorted by action speed
    /// </summary>
    public List<PokemonProxy> ActingPokemons
    { get { return TurnController.ActingPokemons; } }
    /// <summary>
    /// sorted by speed
    /// </summary>
    public IEnumerable<Tile> Tiles
    { get { return TurnController.Tiles; } }
    public IEnumerable<PokemonProxy> OnboardPokemons
    { get { return TurnController.Pokemons; } }
    public int TurnNumber
    { get { return ReportBuilder.TurnNumber; } }

    #region Access
    internal Player GetPlayer(Tile tile)
    {
      return Teams[tile.Team].GetPlayer(GameSettings.Mode.GetPlayerIndex(tile.X));
    }
    internal PokemonProxy GetPokemon(Pokemon pokemon)
    {
      return pokemons[pokemon.Id];
    }
    public int GetRandomInt(int min, int max)
    {
      return min == max ? min : random.Next(min, max + 1);
    }
    public bool RandomHappen(int percentage)
    {
      return random.Next(100) < percentage;
    }
    public bool OneNth(int n)
    {
      return random.Next(n) == 0;
    }
    public Weather Weather
    {
      get { return ATs.IgnoreWeather(this) ? Weather.Normal : Board.Weather; }
      set
      {
        Board.Weather = value;
        ReportBuilder.ShowWeather(this);
        if (!ATs.IgnoreWeather(this)) ATs.WeatherChanged(this);
      }
    }
    /// <summary>
    /// sorted by speed
    /// </summary>
    public IEnumerable<PokemonProxy> GetOnboardPokemons(int teamId)
    {
      return OnboardPokemons.Where((p) => p.Pokemon.TeamId == teamId);
    }
    #endregion

    #region Turn Loop
    private bool _isGameEnd;
    public bool IsGameEnd
    {
      get
      {
        if (_isGameEnd) return true;
        _isGameEnd = Teams.Any((t) => t.Players.All((p) => p.PmsAlive == 0));
        return _isGameEnd;
      }
    }
    public bool CanContinue
    { 
      get
      {
        if (InputController.NeedInput || _isGameEnd) return false;
        if (IsGameEnd)
        {
          ReportBuilder.NewFragment();
          GameUpdated(ReportBuilder.GetFragment(), null);
          return false;
        }
        return true;
      }
    }
    internal void StartGameLoop()
    {
      ReportBuilder.NewFragment();
      TurnController.StartGameLoop();
    }
    internal void TryContinueGameLoop()
    {
      if (!InputController.NeedInput)
      {
        ReportBuilder.TimeTick();
        if (SingleSendOut != null)
        {
          SendOut(SingleSendOut);
          ReportBuilder.AddHorizontalLine();
          SingleSendOut = null;
        }
        TurnController.StartGameLoop();
      }
    }
    #endregion

    #region Input
    internal bool CheckInputSucceed(int teamId, int teamIndex)
    {
      return InputController.CheckInputSucceed(Teams[teamId].GetPlayer(teamIndex));
    }
    private Tile SingleSendOut;
    public void PauseForSendOutInput(Tile tile) //逃生按钮、追击死亡
    {
      if (InputController.PauseForSendOutInput(tile))
      {
        SingleSendOut = tile;
        PauseForInput();
      }
    }
    internal void PauseForTurnInput()
    {
      if (InputController.PauseForTurnInput()) PauseForInput();
    }
    internal void PauseForEndTurnInput()
    {
      if (InputController.PauseForEndTurnInput()) PauseForInput();
    }
    private void PauseForInput()
    {
      if (InputController.NeedInput)
      {
#if ETV
        random = new Random(randomSeeds.Next());
#else
        random = new Random();
#endif
        ReportBuilder.NewFragment();
        var r = InputController.InputRequirements;
        GameUpdated(ReportBuilder.GetFragment(), r);
        var players = new List<Player>();
        if (r[0, 0] != null) players.Add(Teams[0].GetPlayer(0));
        if (r[0, 1] != null) players.Add(Teams[0].GetPlayer(1));
        if (r[1, 0] != null) players.Add(Teams[1].GetPlayer(0));
        if (r[1, 1] != null) players.Add(Teams[1].GetPlayer(1));
        Timer.Resume(players);
      }
    }
    internal bool InputSendOut(Tile tile, int sendoutIndex)
    {
      return InputController.SendOut(tile, sendoutIndex);
    }
    internal bool InputSelectMove(MoveProxy move, Tile position)
    {
      return InputController.SelectMove(move, position);
    }
    internal bool InputStruggle(PokemonProxy pm)
    {
      return InputController.Struggle(pm);
    }
    #endregion

    #region switch or send out
    public bool CanSendOut(Tile tile)
    {
      return SwitchController.CanSendOut(tile);
    }
    public bool CanSendOut(Pokemon pokemon)
    {
      return SwitchController.CanSendOut(pokemon);
    }
    public bool CanWithdraw(PokemonProxy pm)
    {
      return SwitchController.CanWithdraw(pm);
    }
    public bool Withdraw(PokemonProxy pm, string log, bool canPursuit = true)
    {
      if (SwitchController.Withdraw(pm, log, canPursuit))
      {
        Board.RefreshPokemons();
        return true;
      }
      return false;
    }
    internal void GameStartSendOut(IEnumerable<Tile> tiles)
    {
      SwitchController.GameStartSendOut(tiles);
    }
    public bool SendOut(Tile position, bool debut = true, string log = "rf_SendOut1")
    {
      if (SwitchController.SendOut(position, debut, log))
      {
        Board.RefreshPokemons();
        return true;
      }
      return false;
    }
    #endregion

    public void Dispose()
    {
      Timer.Dispose();
    }
  }
}
