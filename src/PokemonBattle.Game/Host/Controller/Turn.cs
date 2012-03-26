using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightStudio.PokemonBattle.Game
{
  internal class TurnController : ControllerComponent
  {
    private readonly Comparer comparer;
    private readonly Tile[] tiles;

    public TurnController(Controller controller)
      : base(controller)
    {
      comparer = new Comparer(Game.Board);
      tiles = new Tile[Board.TeamCount * Board.XBound];
      OnboardPokemons = new List<PokemonProxy>();
      int k = -1;
      for (int i = 0; i < Board.TeamCount; i++)
        for (int j = 0; j < Board.XBound; j++)
          tiles[++k] = Board[i, j];
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
        j = GetRandomInt(i, OnboardPokemons.Count - 1);
        PokemonProxy temp = OnboardPokemons[i];
        OnboardPokemons[i] = OnboardPokemons[j];
        OnboardPokemons[j] = temp;
      }
      OnboardPokemons = new List<PokemonProxy>(OnboardPokemons.OrderBy((pm) => pm, comparer));
    }
    private void SortTiles()
    {
      for (int i = 0; i < tiles.Length - 1; i++)
      {
        int j;
        j = GetRandomInt(i, tiles.Length - 1);
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
      ReportBuilder.AddNewTurn();
      SortOnboardPokemons();
      foreach (PokemonProxy p in OnboardPokemons)
        p.AttachBehaviors();
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
        goto LOOP;
      }
    }
    private void EndTurnEffects()
    {
      SortTiles();
      //TODO:
      //

      EndTurnCheckForInput();
    }
    private void EndTurnCheckForInput()
    {
      bool startNextTurn = true;
      foreach(Tile t in Tiles)
        if (t.Pokemon == null && Controller.CanSendout(t))
        {
          startNextTurn = false;
          if (ReportBuilder.TurnNumber == 0)
            t.WillSendoutPokemonIndex = GameSettings.Mode.GetPokemonIndex(t.X);
          else
          {
            Controller.PauseForEndTurnInput(EndTurnSendout);
            return;
          }
        }
      if (startNextTurn) NextTurn();
      else EndTurnSendout();//第0回合专用
    }
    private void EndTurnSendout()
    {
      foreach(Tile t in Tiles)
        if (t.WillSendoutPokemonIndex != Tile.NOPM_INDEX)
          Controller.Sendout(t, false);
      SortTiles();
      foreach (Tile t in Tiles)
        if (t.Pokemon != null) t.Pokemon.Debut();
      EndTurnCheckForInput();
    }
    private void NextTurn()
    {
      //缓慢启动与其他状态
      BeginTurn();
    }
  }
}
