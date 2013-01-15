using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonBattleOnline.Data;

namespace PokemonBattleOnline.Game.Host.Effects.Moves.Attack
{
  class TriAttack : AttackMoveE
  {
    public TriAttack(int id)
      : base(id)
    {
    }
    protected override void ImplementEffect(DefContext def)
    {
      if (def.RandomHappen(20))
      {
        int i = def.Defender.Controller.GetRandomInt(0, 2);
        AttachedState s = i == 0 ? AttachedState.PAR : i == 1 ? AttachedState.BRN : AttachedState.FRZ;
        def.Defender.AddState(def.AtkContext.Attacker, s, false);
      }
    }
  }
}
