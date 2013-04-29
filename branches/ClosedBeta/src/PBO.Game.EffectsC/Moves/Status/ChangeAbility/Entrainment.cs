using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Abs = PokemonBattleOnline.Game.Host.Sp.Abilities;

namespace PokemonBattleOnline.Game.Host.Effects.Moves.Status
{
  class Entrainment : StatusMoveE
  {
    public Entrainment(int id)
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
        a == Abs.FORECAST || a == Abs.ILLUSION || a == Abs.ZEN_MODE || a == Abs.FLOWER_GIFT ||
        d == Abs.TRUANT || d == Abs.MULTITYPE
        )
        atk.FailAll();
      else atk.Target.Defender.ChangeAbility(a, "SetAbility");
    }
  }
}
