using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokemonBattleOnline.Game.Host.Triggers
{
  internal static class StateAdded
  {
    public static void Execute(PokemonProxy pm)
    {
      switch (pm.Item)
      {
        case Is.MENTAL_HERB:
          MentalHerb(pm);
          break;
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
          if (pm.State == PokemonState.PSN || pm.State == PokemonState.BadlyPSN)
          {
            var s = pm.State.ToString();
            pm.DeAbnormalState();
            pm.RaiseItem("ItemDePSN", true);
          }
          break;
        case Is.PERSIM_BERRY:
          if (pm.OnboardPokemon.RemoveCondition("Confuse")) pm.RaiseItem("ItemDeConfuse", true);
          break;
        case Is.LUM_BERRY:
          if (pm.State != PokemonState.Normal)
          {
            var s = pm.State.ToString();
            pm.DeAbnormalState();
            pm.RaiseItem("ItemDe" + s, true);
          }
          break;
      }
    }

    private static void DeStateBerry(PokemonProxy pm, PokemonState state)
    {
      if (pm.State == state)
      {
        pm.DeAbnormalState();
        pm.RaiseItem("ItemDe" + state.ToString(), true);
      }
    }

    private static bool MentalHerb(PokemonProxy pm, string condition)
    {
      if (pm.OnboardPokemon.RemoveCondition(condition))
      {
        pm.AddReportPm("De" + condition);
        return true;
      }
      return false;
    }
    private static void MentalHerb(PokemonProxy pm)
    {
      var a = pm.OnboardPokemon.RemoveCondition("Attract");
      if (a) pm.AddReportPm("ItemDeAttract", pm.Pokemon.Item.Id);
      if (a | MentalHerb(pm, "Encore") | MentalHerb(pm, "Taunt") | MentalHerb(pm, "Torment") | MentalHerb(pm, "Disable")) pm.ConsumeItem();
    }
  }
}
