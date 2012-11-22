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
#if DEBUG
    private Random randomSeeds;
#endif

    internal Controller(GameContext game)
    {
      Game = game;
      pokemons = new Dictionary<int, PokemonProxy>();
      foreach (Team t in game.Teams)
        foreach (Pokemon p in t.Pokemons.Values) pokemons.Add(p.Id, new PokemonProxy(this, p));

      ReportBuilder = new ReportBuilder(this);
      SwitchController = new SwitchController(this);
      InputController = new InputController(this);
      TurnController = new TurnController(this);
#if DEBUG
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
    public List<PokemonProxy> OnboardPokemons
    { get { return TurnController.OnboardPokemons; } }
    /// <summary>
    /// sorted by speed
    /// </summary>
    public IEnumerable<Tile> Tiles
    { get { return TurnController.Tiles; } }
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
          Abilities.WeatherChanged(this);
        }
      }
    }
    /// <summary>
    /// sorted by speed
    /// </summary>
    public IEnumerable<PokemonProxy> GetOnboardPokemons(int teamId)
    {
      return Tiles.Where((t) => t.Pokemon != null && t.Pokemon.Pokemon.TeamId == teamId).Select((t)=>t.Pokemon);
    }
    #endregion

    #region Turn Loop
    public bool CanContinue
    { 
      get
      {
        if (InputController.NeedInput) return false;
        if (Game.CheckGameEnd())
        {
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
    public bool Sendout(Tile position, bool debut = true, string log = null)
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
