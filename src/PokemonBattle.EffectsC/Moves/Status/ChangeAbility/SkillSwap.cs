using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Abs = LightStudio.PokemonBattle.Game.Host.Sp.Abilities;

namespace LightStudio.PokemonBattle.Game.Host.Effects.Moves.Status
{
  class SkillSwap : StatusMoveE
  {
    public SkillSwap(int id)
      : base(id)
    {
    }
    protected override void Act(AtkContext atk)
    {
      int a = atk.Attacker.OnboardPokemon.Ability;
      int d = atk.Target.Defender.OnboardPokemon.Ability;
      if
        (
        a == d ||
        a == Abs.WONDER_GUARD || a == Abs.ILLUSION || a == Abs.MULTITYPE ||
        d == Abs.WONDER_GUARD || d == Abs.ILLUSION || d == Abs.MULTITYPE
        )
        FailAll(atk);
      else
      {
        var aer = atk.Attacker;
        var der = atk.Target.Defender;
        aer.Controller.ReportBuilder.Add("SkillSwap");
        if (aer.Pokemon.TeamId != der.Pokemon.TeamId)
        {
          aer.AddReportPm("SkillSwapDetail", d);
          der.AddReportPm("SkillSwapDetail", a);
        }
        aer.Ability.Detach(aer);
        der.Ability.Detach(der);
        aer.OnboardPokemon.Ability = d;
        der.OnboardPokemon.Ability = a;
        aer.Ability.Attach(aer);
        der.Ability.Attach(der);
      }
    }
  }
}
