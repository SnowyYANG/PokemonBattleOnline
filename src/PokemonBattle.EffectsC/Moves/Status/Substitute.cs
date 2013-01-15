using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokemonBattleOnline.Game.Host.Effects.Moves.Status
{
  class Substitute : StatusMoveE
  {
    public Substitute(int id)
      : base(id)
    {
    }
    
    protected override bool NotFail(AtkContext atk)
    {
      return atk.Attacker.Hp > 1 && atk.Attacker.Hp > atk.Attacker.Pokemon.Hp.Origin >> 2;
    }
    protected override void Act(AtkContext atk)
    {
      var aer = atk.Attacker;
      if (aer.OnboardPokemon.HasCondition("Substitute")) aer.AddReportPm("HasSubstitute");
      else
      {
        int hp = aer.Pokemon.Hp.Origin >> 2;
        aer.OnboardPokemon.SetCondition("Substitute", hp);
        aer.Pokemon.SetHp(aer.Hp - hp);
        aer.Controller.ReportBuilder.Add(GameEvents.Substitute.EnSubstitute(aer));
      }
    }
  }
}
