using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace PokemonBattleOnline.Game.Host.Effects.Items
{
  class StickyBarb : ItemE
  {
    public StickyBarb(int id)
      : base(id)
    {
    }
    public override void Attacked(DefContext def)
    {
      var der = def.Defender;
      var aer = def.AtkContext.Attacker;
      if (aer.Pokemon.Item == null && def.AtkContext.Move.Flags.NeedTouch && der.Controller.RandomHappen(10))
      {
        der.RemoveItem();
        aer.ChangeItem(65, null, aer);
      }
    }
  }
}
