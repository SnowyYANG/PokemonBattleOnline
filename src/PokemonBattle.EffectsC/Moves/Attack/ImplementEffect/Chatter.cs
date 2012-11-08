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
    public override void Execute(AtkContext atk)
    {
      if (atk.Attacker.Pokemon.Chatter != null) atk.Controller.ReportBuilder.Add("Chatter", atk.Attacker.Pokemon.Chatter);
      base.Execute(atk);
    }
    protected override void ImplementEffect(DefContext def)
    {
      var chatter = def.AtkContext.Attacker.Pokemon.Chatter;
      //我不认为天之恩惠全力攻击会有效，这也没法测
      if (chatter != null && def.Defender.Controller.RandomHappen(Math.Abs(chatter.GetHashCode()) % 30))
        def.Defender.AddState(def.AtkContext.Attacker, Data.AttachedState.Confuse, false);
    }
  }
}
