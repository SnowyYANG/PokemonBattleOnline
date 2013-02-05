using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Abs = PokemonBattleOnline.Game.Host.Sp.Abilities;

namespace PokemonBattleOnline.Game.Host.Effects.Moves.Status
{
  class RolePlay : StatusMoveE
  {
    public RolePlay(int id)
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
        a == Abs.ILLUSION || a == Abs.MULTITYPE ||
        d == Abs.WONDER_GUARD || d == Abs.FORECAST || d == Abs.MULTITYPE || d == Abs.ILLUSION || d == Abs.ZEN_MODE
        )
        atk.FailAll();
      else atk.Attacker.ChangeAbility(d, "SetAbility");
    }
  }
}
