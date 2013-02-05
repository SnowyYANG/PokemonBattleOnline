using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokemonBattleOnline.Game.Host.Effects.Moves.Attack
{
  class Chatter : AttackMoveE
  {
    public Chatter(int id)
      : base(id)
    {
    }
    public override void Execute(AtkContext atk)
    {
      if (atk.Attacker.Pokemon.Chatter != null) atk.Controller.ReportBuilder.Add("Chatter", atk.Attacker.Id);
      base.Execute(atk);
    }
    protected override void ImplementEffect(DefContext def)
    {
      var chatter = def.AtkContext.Attacker.Pokemon.Chatter;
      if (chatter != null && Math.Abs(chatter.GetHashCode()) % 3 != 1 && def.RandomHappen(10))
        def.Defender.AddState(def.AtkContext.Attacker, Data.AttachedState.Confuse, false);
    }
  }
}
