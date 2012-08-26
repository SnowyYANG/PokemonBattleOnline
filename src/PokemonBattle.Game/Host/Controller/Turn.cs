using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightStudio.PokemonBattle.Game.Host
{
  internal class TurnController : ControllerComponent
  {
    private readonly Comparer comparer;
    private readonly Tile[] tiles;

    public TurnController(Controller controller)
      : base(controller)
    {
      comparer = new Comparer(Game.Board);
      Tiles = tiles = Board.Tiles.ToArray(); //this is a copy
      OnboardPokemons = new List<PokemonProxy>();
    }

    public List<PokemonProxy> OnboardPokemons
    { get; private set; }
    public IEnumerable<Tile> Tiles
    { get; private set; }

    private void SortOnboardPokemons()
    {
      for (int i = 0; i < OnboardPokemons.Count - 1; i++)
      {
        int j;
        j = Controller.GetRandomInt(i, OnboardPokemons.Count - 1);
        PokemonProxy temp = OnboardPokemons[i];
        OnboardPokemons[i] = OnboardPokemons[j];
        OnboardPokemons[j] = temp;
      }
      OnboardPokemons = new List<PokemonProxy>(OnboardPokemons.OrderBy((pm) => pm, comparer));
      foreach (var pm in OnboardPokemons)
        if (pm.UsingItem) Sp.Items.RaiseItem(pm);
    }
    private void SortTiles()
    {
      for (int i = 0; i < tiles.Length - 1; i++)
      {
        int j;
        j = Controller.GetRandomInt(i, tiles.Length - 1);
        Tile temp = tiles[i];
        tiles[i] = tiles[j];
        tiles[j] = temp;
      }
      Tiles = tiles.OrderBy((pm) => pm, comparer).ToArray();
    }

    internal void BeginTurn()
    {
      bool needInput = false;
      foreach (PokemonProxy p in OnboardPokemons)
        needInput |= p.CheckNeedInput();
      if (needInput) Controller.PauseForTurnInput(Prepare);
      else Prepare();
    }
    private void Prepare()
    {
      ReportBuilder.NewTurn();
      SortOnboardPokemons();
      foreach (PokemonProxy p in OnboardPokemons) p.AttachBehaviors();
      Switch();
      Pre_UseMove();
      ActMove();
    }
    public void Switch()
    {
    LOOP:
      PokemonProxy p = OnboardPokemons.FirstOrDefault((pm) => pm.Action == PokemonAction.WillSwitch);
      if (p == null) return;
      p.Switch();
      goto LOOP;  
    }
    private void Pre_UseMove()
    {
      foreach (PokemonProxy p in OnboardPokemons)
        p.PreMove();
    }
    public void ActMove() //蜻蜓返的inputFinished
    {
    LOOP:
      PokemonProxy p = OnboardPokemons.FirstOrDefault((pm) => pm.CanActMove);
      if (p == null)
      {
        EndTurnEffects();
        return;
      }
      else
      {
        p.ActMove();
        if (Controller.CanContinue) goto LOOP;
      }
    }
    private void EndTurnEffects()
    {
      if (Controller.TurnNumber == 0)
      {
        EndTurnSendout();
        NextTurn();
      }
      else
      {
        SortTiles();
        EffectsService.EndTurn.Execute(Controller);
        if (Controller.CanContinue) EndTurnCheckForInput();
      }
    }
    private void EndTurnCheckForInput()
    {
      foreach (Tile t in Tiles)
        if (t.Pokemon == null && Controller.CanSendout(t))
        {
          Controller.PauseForEndTurnInput(EndTurnSendout);
          return;
        }
      NextTurn();
    }
    private void EndTurnSendout()
    {
      foreach(Tile t in Tiles)
        if (t.WillSendoutPokemonIndex != Tile.NOPM_INDEX)
          Controller.Sendout(t, false);
      SortTiles();
      foreach (Tile t in Tiles)
        if (t.Pokemon != null) t.Pokemon.Debut();
      if (ReportBuilder.TurnNumber != 0) EndTurnCheckForInput();
    }
    private void NextTurn()
    {
      Sp.Abilities.SlowStart(Controller);
      Board.ClearAllElementsTurnConditions();
      BeginTurn();
    }
  }
}
