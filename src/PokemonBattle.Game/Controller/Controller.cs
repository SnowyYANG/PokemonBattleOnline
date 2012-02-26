using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Interactive;

namespace LightStudio.PokemonBattle.Game
{
  internal class Controller : IController
  {
    private readonly TurnBuilder turnBuilder;
    private readonly ActionInput[] actions;
    Random random;

    public Controller(GameContext game)
    {
      Game = game;
      turnBuilder = new TurnBuilder(game);
      actions = new ActionInput[game.Settings.PlayersPerTeam * game.Settings.TeamCount];
    }

    public GameContext Game
    { get; private set; }
    public Board Board
    { get; private set; }
    public List<PokemonProxy> ActivePokemons
    { get; private set; }

    #region Service
    public int GetRandomInt(int min, int max)
    {
      return random.Next(min, max);
    }
    #endregion

    #region Input
    public bool Switch(PokemonProxy withdraw, Pokemon sendout)
    {
      if (withdraw.Action == Action.Standby && withdraw.Owner == sendout.Owner && sendout.Hp.Value > 0)
      {
        withdraw.Action = Action.Switch;
        withdraw.SwitchPokemon = sendout;
        return true;
      }
      return false;
    }
    public bool SelectMove(PokemonProxy pm, Move move, Position position = null)
    {
    }
    #endregion

    #region Sort
    private int ComparePokemon(PokemonProxy a, PokemonProxy b)
    {
      if (a.Action == Action.Switch && b.Action == Action.Switch) return a.Speed - b.Speed;
      if (a.Action == Action.Switch) return 1;
      if (b.Action == Action.Switch) return -1;

      if (a.SelectMove.Type.Priority != b.SelectMove.Type.Priority)
        return a.SelectMove.Type.Priority - b.SelectMove.Type.Priority;

      #warning unfinished Items
      //if (a.Item != b.Item)//1=先制爪/先制果发动 0=无道具 -1=后攻尾/满腹香炉发动
      //  return (a.Item - b.Item);

      bool aIsStall = a.HasWorkingAbility(AbilityIds.STALL);
      bool bIsStall = b.HasWorkingAbility(AbilityIds.STALL);
      if (aIsStall && !bIsStall) return -1;
      if (!aIsStall && bIsStall) return 1;

      if (Board.Conditions["TrickRoom"] != null) return b.Speed - a.Speed;
      else return a.Speed - b.Speed;
    }
    private void SortActivePokemons()
    {
      int n = ActivePokemons.Count;
      for (int i = 0; i < n - 1; i++)
      {
        int j;
        j = GetRandomInt(i, n - 1);
        PokemonProxy temp = ActivePokemons[i];
        ActivePokemons[i] = ActivePokemons[j];
        ActivePokemons[j] = temp;
      }
      ActivePokemons.Sort(ComparePokemon);
    }
    #endregion

    #region Switch
    public void Withdraw(int pmId)
    {
      throw new NotImplementedException();
    }
    public void Sendout(int pmId)
    {
      throw new NotImplementedException();
      //int i = player.GetPokemonIndex(withdraw.Id);
      //int j = player.GetPokemonIndex(sendout.Id);
      //if (j >= Game.Settings.XBound)
      //{
      //  Pokemon temp = player.Pokemons[i];
      //  player.Pokemons[i] = player.Pokemons[j];
      //  player.Pokemons[j] = temp;
      //}
    }
    public void Sendout(Player player, int pmIndex)
    {
      throw new NotImplementedException();
    }
    #endregion
  }
}
