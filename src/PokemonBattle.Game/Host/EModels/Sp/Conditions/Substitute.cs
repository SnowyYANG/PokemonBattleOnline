using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightStudio.PokemonBattle.Game.Sp.Conditions
{
  static class Substitute
  {
    public static bool Hurt(DefContext def)
    {
      int hp = def.Defender.OnboardPokemon.GetCondition<int>("Substitute");
      if (hp > 0)
      {
        Controller c = def.Defender.Controller;
        if (def.Damage > hp) def.Damage = hp;
        hp -= def.Damage;
        def.Defender.AddReportPm("HurtSubstitute");
        if (def.EffectRevise > 1) c.ReportBuilder.Add("SuperHurt_s");
        else if (def.EffectRevise < 1) c.ReportBuilder.Add("WeakHurt_s");
        if (def.IsCt) c.ReportBuilder.Add("CT_s");
        if (hp == 0)
        {
          c.ReportBuilder.Add(Interactive.GameEvents.Substitute.DeSubstitute(def.Defender));
          def.Defender.OnboardPokemon.RemoveCondition("Substitute");
        }
        else def.Defender.OnboardPokemon.SetCondition("Substitute", hp);
        def.HitSubstitute = true;
        return true;
      }
      return false;
    }
  }
}
