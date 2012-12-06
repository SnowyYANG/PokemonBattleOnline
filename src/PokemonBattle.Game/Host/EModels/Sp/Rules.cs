using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Data;

namespace LightStudio.PokemonBattle.Game.Host.Sp
{
  public static class Rules
  {
    public static bool CanAddState(PokemonProxy pm, AttachedState state, PokemonProxy by, bool showFail)
    {
      if (state != AttachedState.SLP || !pm.Controller.GameSettings.SleepRule || pm.Pokemon.TeamId == by.Pokemon.TeamId) goto TRUE;
      var p = pm.Tile.Field.GetCondition<PokemonProxy>("RULE_SLP");
      if (p == null || p.State != PokemonState.SLP) goto PREPARE;
      pm.AddReportPm("RULE_SLP");
      return false;
    PREPARE:
      pm.Tile.Field.SetCondition("RULE_SLP", pm);
    TRUE:
      return true;
    }
  }
}
