using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Data;
using LightStudio.PokemonBattle.Game.GameEvents;

namespace LightStudio.PokemonBattle.Game.Host.Sp
{
  public static partial class Triggers
  {
    public static void KOed(DefContext def)
    {
      var der = def.Defender;
      if (der.Hp == 0)
      {
        var aer = def.AtkContext.Attacker;
        if (der.OnboardPokemon.HasCondition("DestinyBond"))
        {
          der.AddReportPm("DestinyBond"); //战报顺序已测
          aer.Pokemon.SetHp(0);
          aer.CheckFaint();
        }
        if (der.OnboardPokemon.HasCondition("Grudge"))
        {
          int formerPP = def.AtkContext.MoveProxy.PP;
          def.AtkContext.MoveProxy.PP = 0;
          aer.Controller.ReportBuilder.Add(new PPChange("Grudge", def.AtkContext.MoveProxy, formerPP));
        }
        if (aer.CanChangeLv7D(aer, StatType.Atk, 1, false) != 0 && aer.RaiseAbility(Abilities.MOXIE)) aer.ChangeLv7D(aer, false, 1);
      }
    }
  }
}
