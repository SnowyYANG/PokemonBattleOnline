using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokemonBattleOnline.Game.Host.Effects.Moves.Status
{
  class Autotomize : StatusMoveE
  {
    public Autotomize(int id)
      : base(id)
    {
    }
    protected override void Act(AtkContext atk)
    {
      base.Act(atk);
      if (!atk.Fail && atk.Attacker.OnboardPokemon.Weight > 0.1)
      {
        atk.Attacker.OnboardPokemon.Weight -= 100;
        atk.Attacker.AddReportPm("Autotomize");
      }
    }
  }
}
