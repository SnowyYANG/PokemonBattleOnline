using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Data;

namespace LightStudio.PokemonBattle.Game.Sp.Conditions
{
  internal class FlashFire
  {
    public static void ReviseDamage1(AtkContext atk)
    {
      if (atk.Type == BattleType.Fire && atk.Attacker.OnboardPokemon.HasCondition("FlashFire"))
        atk.DamageRevise1.Enqueue(1.5);
    }
  }
}
