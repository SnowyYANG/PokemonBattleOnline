using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Interactive;
using LightStudio.PokemonBattle.Game.Sp;

namespace LightStudio.PokemonBattle.Game
{
  public class Controller
  {
    public event Action<PokemonProxy> PokemonWithdrawing //追击
    {
      add { SwitchController.PokemonWithdrawing += value; }
      remove { SwitchController.PokemonWithdrawing -= value; }
    }
    internal event Action<ReportFragment, int[]> ReportUpdated;

    public readonly ReportBuilder ReportBuilder;
    internal readonly GameContext Game;
    private readonly SwitchController SwitchController;
    private readonly InputController InputController;
    private readonly TurnController TurnController;
    
    private Random random;
    private Action inputFinished;

    internal Controller(GameContext game)
    {
      Game = game;
      ReportBuilder = new ReportBuilder(game);
      SwitchController = new SwitchController(this);
      InputController = new InputController(this);
      TurnController = new TurnController(this);
      random = new Random();
    }

    public Board Board
    { get { return Game.Board; } }
    public List<PokemonProxy> OnboardPokemons
    { get { return TurnController.OnboardPokemons; } }
    public IEnumerable<Tile> Tiles
    { get { return TurnController.Tiles; } }
    public int TurnNumber
    { get { return ReportBuilder.TurnNumber; } }

    #region Access
    internal Player GetPlayer(Tile tile)
    {
      return Game.Teams[tile.Team].GetPlayer(Game.Settings.Mode.GetPlayerIndex(tile.X));
    }
    public int GetRandomInt(int min, int max)
    {
      return random.Next(min, max + 1);
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
    public Weather GetAvailableWeather()
    {
      return Abilities.HaveCloudNine(this) ? Weather.Invalid : Board.Weather;
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
        random = new Random();
        ReportBuilder.NewFragment();
        if (ReportUpdated != null) ReportUpdated(ReportBuilder.GetFragment(), InputController.Players.ToArray());
        this.inputFinished = inputFinished;
      }
      else
        inputFinished();
    }
    internal InputResult InputSwitch(PokemonProxy withdraw, int sendoutIndex)
    {
      return InputController.Switch(withdraw, sendoutIndex);
    }
    internal InputResult InputSendout(Tile position, int sendoutIndex)
    {
      return InputController.Sendout(position, sendoutIndex);
    }
    internal InputResult InputSelectMove(MoveProxy move, Tile position)
    {
      return InputController.SelectMove(move, position);
    }
    internal InputResult InputStruggle(PokemonProxy pm)
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
