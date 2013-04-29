using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokemonBattleOnline.Game.Host.Effects.Moves.Attack
{
  class Thief : AttackMoveE
  {
    public Thief(int id)
      : base(id)
    {
    }
    public override void InitAtkContext(AtkContext atk)
    {
      if (atk.Attacker.Pokemon.Item == null) atk.SetCondition("Thief");
    }
    protected override void ImplementEffect(DefContext def)
    {
      var aer = def.AtkContext.Attacker;
      if (def.AtkContext.HasCondition("Thief") && def.Defender.CanLostItem)
      {
        var i = def.Defender.Pokemon.Item.Id;
        def.Defender.RemoveItem();
        aer.ChangeItem(i, "Thief", def.Defender, false); //先铁棘再果子
      }
    }
  }
}
