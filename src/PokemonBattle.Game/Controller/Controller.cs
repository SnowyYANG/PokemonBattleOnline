using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Interactive;

namespace LightStudio.PokemonBattle.Game
{
  public class Controller
  {
    public event Action PokemonWithdrawing
    {
      add { SwitchController.PokemonWithdrawing += value; }
      remove { SwitchController.PokemonWithdrawing -= value; }
    }
    internal event Action<int[]> RequireInput
    {
      add { InputController.RequireInput += value; }
      remove { InputController.RequireInput -= value; }
    }
    internal event Action<ReportFragment> ReportUpdated;

    public readonly ReportBuilder ReportBuilder;
    internal readonly GameContext Game;
    private readonly SwitchController SwitchController;
    private readonly InputController InputController;
    private readonly TurnController TurnController;
    
    private Random random;

    internal Controller(GameContext game)
    {
      Game = game;
      ReportBuilder = new ReportBuilder(game);
      SwitchController = new SwitchController(this);
      InputController = new InputController(this);
      TurnController = new TurnController(this);
    }

    public List<PokemonProxy> OnboardPokemons
    { get { return TurnController.OnboardPokemons; } }
    public IEnumerable<Tile> Tiles
    { get { return TurnController.Tiles; } }

    #region Access
    public int GetRandomInt(int min, int max)
    {
      return random.Next(min, max);
    }
    public Tile GetTile(int team, int x)
    {
      return Game.Board[team, x];
    }
    #endregion

    #region Turn Loop
    internal void StartGameLoop()
    {
      TurnController.BeginTurn();
    }
    #endregion

    #region Input
    internal void ContinueAfterInput(Action inputFinished)
    {
      random = new Random();
      ReportUpdated(ReportBuilder.GetFragment());
      InputController.ContinueAfterInput(inputFinished);
    }
    internal bool InputSwitch(PokemonProxy withdraw, Pokemon sendout)
    {
      return InputController.Switch(withdraw, sendout);
    }
    internal bool InputSendout(Pokemon sendout, Tile position)
    {
      return InputController.Sendout(sendout, position);
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
    public bool CanWithdraw(PokemonProxy pm)
    {
      return SwitchController.CanWithdraw(pm);
    }
    public bool CanSendout(Pokemon pm, Tile tile)
    {
      return SwitchController.CanSendout(pm, tile);
    }
    public bool Withdraw(PokemonProxy pm)
    {
      return SwitchController.Withdraw(pm);
    }
    public bool Sendout(Tile position)
    {
      return SwitchController.Sendout(position);
    }
    #endregion

    public bool HasAvailableAbility(int abilityId)
    {
      foreach (PokemonProxy pm in OnboardPokemons)
        if (pm.HasWorkingAbility(abilityId)) return true;
      return false;
    }
    public bool HasAvailableAbility(int teamId, int abilityId)
    {
      foreach (PokemonProxy pm in OnboardPokemons)
        if (pm.Position.Team == teamId && pm.HasWorkingAbility(abilityId)) return true;
      return false;
    }  
  }
}
