using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokemonBattleOnline.Game.Host
{
  internal class Controller : IDisposable
  {
    public event Action<ReportFragment, IDictionary<int, InputRequest>> GameUpdated;

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

    internal Controller(IGameSettings gameSettings, Team[] teams)
    {
      Teams = teams;
      GameSettings = gameSettings;
      Board = new Board(GameSettings);
      pokemons = new Dictionary<int, PokemonProxy>();
      foreach (Team t in teams)
        foreach(var pl in t.Players)
          foreach (Pokemon p in pl.Pokemons) pokemons.Add(p.Id, new PokemonProxy(this, p));

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
      Timer = new GoTimer(teams[0].Players.Concat(teams[1].Players).Select((p) => p.Id));
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
        if (SingleSendout != null)
        {
          Sendout(SingleSendout);
          ReportBuilder.AddHorizontalLine();
          SingleSendout = null;
        }
        TurnController.StartGameLoop();
      }
    }
    #endregion

    #region Input
    internal bool CheckInputSucceed(Player player)
    {
      return InputController.CheckInputSucceed(player);
    }
    private Tile SingleSendout;
    public void PauseForSendoutInput(Tile tile) //逃生按钮、追击死亡
    {
      if (InputController.PauseForSendoutInput(tile))
      {
        SingleSendout = tile;
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
        GameUpdated(ReportBuilder.GetFragment(), InputController.InputRequirements);
        Timer.Resume(InputController.InputRequirements.Keys);
      }
    }
    internal bool InputSendout(Tile tile, int sendoutIndex)
    {
      return InputController.Sendout(tile, sendoutIndex);
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

    #region Switch or Sendout
    public bool CanSendout(Tile tile)
    {
      return SwitchController.CanSendout(tile);
    }
    public bool CanSendout(Pokemon pokemon)
    {
      return SwitchController.CanSendout(pokemon);
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
    internal void GameStartSendout(IEnumerable<Tile> tiles)
    {
      SwitchController.GameStartSendout(tiles);
    }
    public bool Sendout(Tile position, bool debut = true, string log = "Sendout1")
    {
      if (SwitchController.Sendout(position, debut, log))
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
