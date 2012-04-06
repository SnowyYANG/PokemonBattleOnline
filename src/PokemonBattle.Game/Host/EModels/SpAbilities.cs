using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Data;
using LightStudio.PokemonBattle.Interactive.GameEvents;

namespace LightStudio.PokemonBattle.Game
{
  internal static class SpAbilities
  {
    #region ids
    const int EARLY_BIRD = 27;
    const int ILLUSION = 56;
    const int MOLD_BREAKER = 79;
    const int NO_GUARD = 85;
    const int STALL = 131;
    const int STEADFAST = 133;
    const int TERAVOLT = 148;
    const int TURBOBLAZE = 154;
    const int UNAWARE = 155;
    #endregion

    public static void RaiseAbility(this PokemonProxy pm)
    {
      pm.Controller.ReportBuilder.Add(new AbilityEvent(pm));
    }

    #region IsXXX
    public static bool IgnoreDefenderAbility(this IAbilityE ability)
    {
      return ability.Id == MOLD_BREAKER || ability.Id == TURBOBLAZE || ability.Id == TERAVOLT;
    }
    public static bool NoGuard(this IAbilityE ability)
    {
      return ability.Id == NO_GUARD;
    }
    public static bool Unaware(this IAbilityE ability)
    {
      return ability.Id == UNAWARE;
    }
    public static bool Stall(this IAbilityE ability)
    {
      return ability.Id == STALL;
    }
    public static bool EarlyBird(this IAbilityE ability)
    {
      return ability.Id == EARLY_BIRD;
    }
    public static bool Illusion(this IAbilityE ability)
    {
      return ability.Id == ILLUSION;
    }
    #endregion

    public static void CheckSteadfast(PokemonProxy pm)
    {
      if (pm.Ability.Id == STEADFAST)
      {
        pm.RaiseAbility();
        pm.ChangeLv7D(StatType.Speed, 1);
      }
    }
  }
}
