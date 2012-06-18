using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightStudio.PokemonBattle.Game.Sp.Conditions
{
  static class Substitute
  {
    private static int Generic(DefContext def)
    {
      int hp = def.Defender.OnboardPokemon.GetCondition<int>("Substitute");
      if (hp > 0) def.HitSubstitute = true;
      return hp;
    }
    private static void Disappear(PokemonProxy pm)
    {
      pm.Controller.ReportBuilder.Add(Interactive.GameEvents.Substitute.DeSubstitute(pm));
      pm.OnboardPokemon.RemoveCondition("Substitute");
    }
    public static bool Hurt(DefContext def)
    {
      int hp = Generic(def);
      if (hp > 0)
      {
        Controller c = def.Defender.Controller;
        if (def.Damage > hp) def.Damage = hp;
        hp -= def.Damage;
        def.Defender.AddReportPm("HurtSubstitute");
        if (def.EffectRevise > 0) c.ReportBuilder.Add("SuperHurt_s");
        else if (def.EffectRevise < 0) c.ReportBuilder.Add("WeakHurt_s");
        if (def.IsCt) c.ReportBuilder.Add("CT_s");
        if (hp == 0) Disappear(def.Defender);
        else def.Defender.OnboardPokemon.SetCondition("Substitute", hp);
        return true;
      }
      return false;
    }
    public static bool OHKO(DefContext def)
    {
      int hp = Generic(def);
      if (hp > 0)
      {
        def.Damage = hp;
        Disappear(def.Defender);
        return true;
      }
      return false;
    }
  }
}
