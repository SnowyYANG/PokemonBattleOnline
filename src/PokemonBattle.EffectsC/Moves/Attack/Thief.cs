using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightStudio.PokemonBattle.Game.Host.Effects.Moves.Attack
{
  class Thief : AttackMoveE
  {
    public Thief(int id)
      : base(id)
    {
    }
    protected override void Act(AtkContext atk)
    {
      atk.Attachment = atk.Attacker.Pokemon.Item == null;
      base.Act(atk);
    }
    protected override void ImplementEffect(DefContext def)
    {
      var aer = def.AtkContext.Attacker;
      if (def.AtkContext.Attachment && def.Defender.CanLostItem)
      {
        var i = def.Defender.Pokemon.Item.Id;
        def.Defender.RemoveItem();
        aer.ChangeItem(i, "Thief", def.Defender, false); //先铁棘再果子
      }
      base.ImplementEffect(def);
    }
  }
}
