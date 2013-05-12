using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightStudio.PokemonBattle.Game.Host.Effects.Moves.Status
{
  class SetAbility : StatusMoveE
  {
    private readonly int Ability;
    
    public SetAbility(int id, int abilty)
      : base(id)
    {
      Ability = abilty;
    }
    protected override void Act(AtkContext atk)
    {
      var former = atk.Target.Defender.Ability.Id;
      if (former == Sp.Abilities.TRUANT || former == Sp.Abilities.MULTITYPE) Fail(atk.Target);
      else atk.Target.Defender.ChangeAbility(Ability, "SetAbility");
    }
  }
}
