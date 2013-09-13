using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonBattleOnline.Game;

namespace PokemonBattleOnline.Game.Host.Triggers
{
  internal static class CanAddState
  {
    public static bool Execute(PokemonProxy pm, PokemonProxy by, AttachedState state, bool showFail)
    {
      switch (pm.Ability)
      {
        case As.LIMBER: //7
          return CantAddState(pm, by, state, showFail, AttachedState.PAR);
        case As.INSOMNIA: //15
        case As.VITAL_SPIRIT: //72
          return CantAddState(pm, by, state, showFail, AttachedState.SLP);
        case As.IMMUNITY: //17
          return CantAddState(pm, by, state, showFail, AttachedState.PSN);
        case As.OWN_TEMPO: //20
          return CantAddState(pm, by, state, showFail, AttachedState.Confuse);
        case As.MAGMA_ARMOR: //40
          return CantAddState(pm, by, state, showFail, AttachedState.FRZ);
        case As.WATER_VEIL: //41
          return CantAddState(pm, by, state, showFail, AttachedState.BRN);
        case As.OBLIVIOUS: //12
          return CantAddState(pm, by, state, showFail, AttachedState.Attract, "NoEffect");
        case As.LEAF_GUARD: //102
          return LeafGuard(pm, by, state, showFail);
      }
      return true;
    }

    private static bool CantAddState(PokemonProxy pm, PokemonProxy by, AttachedState state, bool showFail, AttachedState s0)
    {
      if (state == s0)
      {
        if (showFail)
        {
          pm.RaiseAbility();
          pm.AddReportPm("Cant" + s0.ToString());
        }
        return false;
      }
      return true;
    }

    private static bool CantAddState(PokemonProxy pm, PokemonProxy by, AttachedState state, bool showFail, AttachedState s0, string log)
    {
      if (state == s0)
      {
        if (showFail)
        {
          pm.RaiseAbility();
          pm.AddReportPm(log);
        }
        return false;
      }
      return true;
    }

    private static bool LeafGuard(PokemonProxy pm, PokemonProxy by, AttachedState state, bool showFail)
    {
      if (pm.Controller.Weather == Weather.IntenseSunlight && (state == AttachedState.PAR || state == AttachedState.SLP || state == AttachedState.FRZ || state == AttachedState.BRN || state == AttachedState.PSN))
      {
        if (showFail)
        {
          pm.RaiseAbility();
          pm.AddReportPm("Cant" + state.ToString());
        }
        return false;
      }
      return true;
    }
  }
}
