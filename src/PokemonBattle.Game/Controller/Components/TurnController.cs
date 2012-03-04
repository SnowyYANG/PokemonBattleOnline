using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightStudio.PokemonBattle.Game
{
  internal class TurnController : ControllerComponent
  {
    private readonly Comparer comparer;
    private readonly List<Tile> tiles;

    public TurnController(Controller controller)
      : base(controller)
    {
      comparer = new Comparer(Game.Board);
      tiles = new List<Tile>(Board.TeamCount * Board.XBound);
      for (int i = 0; i < Board.TeamCount; i++)
        for (int j = 0; j < Board.XBound; j++) tiles.Add(Board[i, j]);
    }

    public List<PokemonProxy> OnboardPokemons
    { get { throw new NotImplementedException(); } }
    public IEnumerable<Tile> Tiles
    { get; private set; }

    private void SortOnboardPokemons()
    {
      int n = OnboardPokemons.Count;
      for (int i = 0; i < n - 1; i++)
      {
        int j;
        j = GetRandomInt(i, n - 1);
        PokemonProxy temp = OnboardPokemons[i];
        OnboardPokemons[i] = OnboardPokemons[j];
        OnboardPokemons[j] = temp;
      }
      //OnboardPokemons.Sort(ComparePokemon);
    }
    private void SortTiles()
    {
      for (int i = 0; i < tiles.Count - 1; i++)
      {
        int j;
        j = GetRandomInt(i, tiles.Count - 1);
        Tile temp = tiles[i];
        tiles[i] = tiles[j];
        tiles[j] = temp;
      }
    }

    public void BeginTurn()
    {
      ReportBuilder.AddNewTurn();
      foreach (PokemonProxy p in OnboardPokemons)
        p.CheckNeedInput();
      Controller.ContinueAfterInput(Prepare);
    }
    private void Prepare()
    {
      SortOnboardPokemons();
      foreach (PokemonProxy p in OnboardPokemons)
        p.Prepare();
      Action();
    }
    public void Action()
    {
      foreach(PokemonProxy p in OnboardPokemons)
        if (p.Action == PokemonAction.WillMove || p.Action == PokemonAction.WillSwitch)
        {
          p.Act();
        }
      EndTurnEffects();
    }
    private void EndTurnEffects()
    {
    }
    public void EndTurnSwitch()
    {
      foreach(Tile t in Tiles)
        if (t.Pokemon == null && Controller.CanSendout(t))
        {
          if (ReportBuilder.TurnNumber == 0)
          {
            switch (GameSettings.Mode)
            {
              case Data.GameMode.Single:
#warning 想像一下并非如此，再输入的过程中精灵就已经交换了，估计Sendout是一触即发的，WillSendoutPokemon也不需要吧
                t.WillSendoutPokemon = t.ResponsiblePlayer.Pokemons[0];
                break;
            }
          }
          else Controller.ContinueAfterInput(EndTurnSwitch);
          break;
        }
      foreach(Tile t in Tiles)
        if (t.WillSendoutPokemon != null)
        {
          Controller.Sendout(t);
        }
    }
    public void NextTurn()
    {
      //缓慢启动与其他状态
      BeginTurn();
    }
  }
}
