using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Game.Host.Sp;

namespace LightStudio.PokemonBattle.Game.Host
{
  public class Controller
  {
    internal event Action<ReportFragment, IDictionary<int, InputRequest>> ReportUpdated;

    public readonly ReportBuilder ReportBuilder;
    internal readonly GameContext Game;
    private readonly SwitchController SwitchController;
    private readonly InputController InputController;
    private readonly TurnController TurnController;
    private readonly Dictionary<int, PokemonProxy> pokemons;
    
    private Random random;
#if ETV
    private Random randomSeeds;
#endif

    internal Controller(GameContext game)
    {
      Game = game;
      pokemons = new Dictionary<int, PokemonProxy>();
      foreach (Team t in game.Teams)
        foreach(var pl in t.Players)
          foreach (Pokemon p in pl.Pokemons) pokemons.Add(p.Id, new PokemonProxy(this, p));

      ReportBuilder = new ReportBuilder(this);
      SwitchController = new SwitchController(this);
      InputController = new InputController(this);
      TurnController = new TurnController(this);
#if ETV || DEBUG
      randomSeeds = new Random(1);
      random = new Random(randomSeeds.Next());
#else
      random = new Random();
#endif
    }

    public IGameSettings GameSettings
    { get { return Game.Settings; } }
    public Board Board
    { get { return Game.Board; } }
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
      return Game.Teams[tile.Team].GetPlayer(Game.Settings.Mode.GetPlayerIndex(tile.X));
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
      get { return Abilities.IgnoreWeather(this) ? Weather.Normal : Board.Weather; }
      set
      {
        if (Board.Weather != value)
        {
          Board.RemoveCondition("Weather");
          Board.Weather = value;
          ReportBuilder.Add(new GameEvents.WeatherChange(value));
          if (!Abilities.IgnoreWeather(this)) Abilities.WeatherChanged(this);
        }
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
    private bool gameEnd;
    public bool CanContinue
    { 
      get
      {
        if (InputController.NeedInput || gameEnd) return false;
        if (Game.CheckGameEnd())
        {
          gameEnd = true;
          ReportBuilder.NewFragment();
          ReportUpdated(ReportBuilder.GetFragment(), null);
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
      if (!InputController.NeedInput) TurnController.StartGameLoop();
    }
    #endregion

    #region Input
    internal bool CheckInputSucceed(Player player)
    {
      return InputController.CheckInputSucceed(player);
    }
    public void PauseForSendoutInput(Tile tile) //逃生按钮、追击死亡
    {
      InputController.PauseForSendoutInput(tile);
      PauseForInput();
    }
    internal void PauseForTurnInput()
    {
      InputController.PauseForTurnInput();
      PauseForInput();
    }
    internal void PauseForEndTurnInput()
    {
      InputController.PauseForEndTurnInput();
      PauseForInput();
    }
    private void PauseForInput()
    {
      if (InputController.NeedInput)
      {
#if DEBUG
        random = new Random(randomSeeds.Next());
#else
        random = new Random();
#endif
        ReportBuilder.NewFragment();
        ReportUpdated(ReportBuilder.GetFragment(), InputController.InputRequirements);
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
  }
}
