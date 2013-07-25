using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightStudio.PokemonBattle.Game.Host.Triggers
{
  internal static class StateAdded
  {
    public static void Execute(PokemonProxy pm)
    {
      switch (pm.Item)
      {
        case Is.CHERI_BERRY:
          DeStateBerry(pm, PokemonState.PAR);
          break;
        case Is.CHESTO_BERRY:
          DeStateBerry(pm, PokemonState.SLP);
          break;
        case Is.RAWST_BERRY:
          DeStateBerry(pm, PokemonState.BRN);
          break;
        case Is.ASPEAR_BERRY:
          DeStateBerry(pm, PokemonState.FRZ);
          break;
        case Is.PECHA_BERRY:
          if (pm.State == PokemonState.PSN || pm.State == PokemonState.BadlyPSN) pm.DeAbnormalState(true);
          break;
        case Is.PERSIM_BERRY:
          if (pm.OnboardPokemon.RemoveCondition("Confuse"))
          {
            pm.ConsumeItem();
            pm.RaiseItem("ItemDeConfuse");
          }
          break;
        case Is.LUM_BERRY:
          if (pm.State != PokemonState.Normal) pm.DeAbnormalState(true);
          break;
      }
    }

    private static void DeStateBerry(PokemonProxy pm, PokemonState state)
    {
      if (pm.State == state) pm.DeAbnormalState(true);
    }

    private static bool Act(OnboardPokemon pm, string condition)
    {
      return pm.RemoveCondition(condition);
    }
    private static void Act(PokemonProxy pm)
    {
      var op = pm.OnboardPokemon;
      bool i = Act(op, "Attract");
      bool e = Act(op, "Encore");
      bool ta = Act(op, "Taunt");
      bool to = Act(op, "Torment");
      bool d = Act(op, "Disable");
      if (i || e || ta || to || d)
      {
        pm.ConsumeItem();
        pm.Controller.ReportBuilder.Add(new GameEvents.MentalHerb(pm, i, e, ta, to, d));
      }
    }
  }
}
