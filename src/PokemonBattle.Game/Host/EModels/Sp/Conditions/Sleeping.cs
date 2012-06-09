using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightStudio.PokemonBattle.Game.Sp.Conditions
{
  public class Sleeping : PmCondition
  {
    public Sleeping(PokemonProxy pm, int count)
      : base("Sleeping", pm, count == 0 ? pm.Controller.GetRandomInt(2, 4) : count)
    {
    }
    
    public override bool CanExecute()
    {
      int count = pm.OnboardPokemon.GetCondition<int>("Sleeping");
      count--;
      if (pm.Ability.EarlyBird()) count--;
      pm.OnboardPokemon.SetCondition("Sleeping", count);
      if (count <= 0) pm.State = PokemonState.Normal; //auto Remove
      else
      {
        AddReport(new Interactive.GameEvents.PositionChange("Sleeping", pm));
        if (!pm.SelectedMove.AvailableEvenSleeping())//梦话打鼾
          return false;
      }
      return true;
    }
  }
}
