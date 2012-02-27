using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Interactive;

namespace LightStudio.PokemonBattle.Game
{
  internal class Controller : IController
  {
    event Action IController.PokemonWithdrawing
    {
      add { SwitchController.PokemonWithdrawing += value; }
      remove { SwitchController.PokemonWithdrawing -= value; }
    }
    event Action IController.PokemonSendout
    {
      add { SwitchController.PokemonSendout += value; }
      remove { SwitchController.PokemonSendout -= value; }
    }

    public readonly TurnBuilder TurnBuilder;
    public readonly GameContext Game;
    public readonly Board Board;
    internal readonly SwitchController SwitchController;
    internal readonly InputController InputController;
    
    private Random random;

    public Controller(GameContext game)
    {
      Game = game;
      TurnBuilder = new TurnBuilder(game);
      SwitchController = new SwitchController(this);
      InputController = new InputController(this);
    }

    public List<IPokemonProxy> OnboardPokemons
    { get { return Board.Pokemons; } }

    #region Service
    public int GetRandomInt(int min, int max)
    {
      return random.Next(min, max);
    }
    #endregion

    #region Input
    internal ActionInputFailure InputSwitch(PokemonProxy withdraw, Pokemon sendout)
    {
      return InputController.Switch(withdraw, sendout);
    }
    internal ActionInputFailure InputSendout(Pokemon sendout, Position position)
    {
      return InputController.Sendout(sendout, position);
    }
    internal ActionInputFailure InputSelectMove(MoveProxy move, Position position)
    {
      return InputController.SelectMove(move, position);
    }
    internal ActionInputFailure InputStruggle(PokemonProxy pm)
    {
      return InputController.Struggle(pm);
    }
    #endregion

    #region Sort
    private int ComparePokemon(IPokemonProxy _a, IPokemonProxy _b)
    {
      PokemonProxy a = _a as PokemonProxy;
      PokemonProxy b = _b as PokemonProxy;
      if (a.Action == PokemonAction.WillSwitch && b.Action == PokemonAction.WillSwitch) return a.Speed - b.Speed;
      if (a.Action == PokemonAction.WillSwitch) return 1;
      if (b.Action == PokemonAction.WillSwitch) return -1;

      if (a.SelectedMove.Priority != b.SelectedMove.Priority)
        return a.SelectedMove.Priority - b.SelectedMove.Priority;

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
      int n = OnboardPokemons.Count;
      for (int i = 0; i < n - 1; i++)
      {
        int j;
        j = GetRandomInt(i, n - 1);
        IPokemonProxy temp = OnboardPokemons[i];
        OnboardPokemons[i] = OnboardPokemons[j];
        OnboardPokemons[j] = temp;
      }
      OnboardPokemons.Sort(ComparePokemon);
    }
    #endregion

    #region Switch or Sendout
    public bool CanSendout(Pokemon pm, Position position)
    {
      return SwitchController.CanSendout(pm, position);
    }
    public bool Withdraw(IPokemonProxy pm)
    {
      return SwitchController.Withdraw(pm as PokemonProxy);
    }
    public bool Sendout(Pokemon pm, Position position)
    {
      return SwitchController.CanSendout(pm, position);
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
