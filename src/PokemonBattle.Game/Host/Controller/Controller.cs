﻿using System;
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
    private Action inputFinished;
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

    public Board Board
    { get { return Game.Board; } }
    /// <summary>
    /// sorted by speed
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
    public Tile GetTile(int team, int x)
    {
      return Game.Board[team, x];
    }
    public Weather Weather
    {
      get { return Abilities.IgnoreWeather(this) ? Weather.Normal : Board.Weather; }
      set
      {
        if (Board.Weather != value)
        {
          Board.Weather = value;
          ReportBuilder.Add(new GameEvents.WeatherChange(value));
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
    internal void StartGameLoop()
    {
      ReportBuilder.NewFragment();
      TurnController.BeginTurn();
    }
    internal void TryContinueGameLoop()
    {
      if (inputFinished != null && !InputController.NeedInput) inputFinished();
    }
    public void ActMove() //蜻蜓返、脱离按钮
    {
      TurnController.ActMove();
    }
    public void Switch()//追击死亡专用后门
    {
      TurnController.Switch();
    }
    #endregion

    #region Input
    internal bool CheckInputSucceed(Player player)
    {
      return InputController.CheckInputSucceed(player);
    }
    internal void PauseForSendoutInput(Action inputFinished, Tile tile) //逃生按钮、追击死亡
    {
      InputController.PauseForSendoutInput(tile);
      PauseForInput(inputFinished);
    }
    internal void PauseForTurnInput(Action inputFinished)
    {
      InputController.PauseForTurnInput();
      PauseForInput(inputFinished);
    }
    internal void PauseForEndTurnInput(Action inputFinished)
    {
      InputController.PauseForEndTurnInput();
      PauseForInput(inputFinished);
    }
    private void PauseForInput(Action inputFinished)
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
        this.inputFinished = inputFinished;
      }
      else inputFinished();
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
    public bool Withdraw(PokemonProxy pm, bool canPursuit = true)
    {
      return SwitchController.Withdraw(pm, canPursuit);
    }
    public bool Sendout(Tile position, bool debut = true)
    {
      return SwitchController.Sendout(position, debut);
    }
    #endregion
  }
}
