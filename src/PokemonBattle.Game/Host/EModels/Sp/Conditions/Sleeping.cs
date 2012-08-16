using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightStudio.PokemonBattle.Game.Host.Sp.Conditions
{
  public class Sleeping : PmCondition
  {
    public Sleeping(PokemonProxy pm, int count)
      : base("Sleeping", pm, count == 0 ? pm.Controller.GetRandomInt(2, 4) : count)
    {
    }
    
    public override bool CanExecute()
    {
      int count = Pm.OnboardPokemon.GetCondition<int>("Sleeping");
      count--;
      if (Pm.Ability.EarlyBird()) count--;
      Pm.OnboardPokemon.SetCondition("Sleeping", count);
      if (count <= 0) Pm.State = PokemonState.Normal; //auto Remove
      else
      {
        AddResetYReport("Sleeping");
        if (!Pm.SelectedMove.AvailableEvenSleeping())//梦话打鼾
          return false;
      }
      return true;
    }
  }
}
