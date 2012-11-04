using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightStudio.PokemonBattle.Game.Host.Effects.Moves.Attack
{
  class Chatter : AttackMoveE
  {
    public Chatter(int id)
      : base(id)
    {
    }
    protected override void ImplementEffect(DefContext def)
    {
      //我不认为天之恩惠全力攻击会有效，这也没法测
      if (def.AtkContext.Attacker.Pokemon.Form.Type.Number == 441 && def.Defender.Controller.RandomHappen(30))
        def.Defender.AddState(def.AtkContext.Attacker, Data.AttachedState.Confuse, false);
    }
  }
}
