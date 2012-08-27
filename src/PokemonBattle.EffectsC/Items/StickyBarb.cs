using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace LightStudio.PokemonBattle.Game.Host.Effects.Items
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
      if (aer.Pokemon.Item == null && def.AtkContext.Move.AdvancedFlags.NeedTouch && der.Controller.RandomHappen(10))
      {
        der.Pokemon.Item = null;
        aer.ChangeItem(65, null, aer);
      }
    }
  }
}
