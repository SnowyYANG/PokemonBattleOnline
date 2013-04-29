using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokemonBattleOnline.Game.Host.Effects.Items
{
  class LeppaBerry : ItemE
  {
    public LeppaBerry(int id)
      : base(id)
    {
    }
    public override void Attach(PokemonProxy pm)
    {
      foreach (var m in pm.Moves)
        if (m.PP == 0)
        {
          m.PP += 10;
          pm.ConsumeItem();
          pm.Controller.ReportBuilder.Add(new GameEvents.PPChange("ItemPPRecover", m, 0) { Item = true });
          return;
        }
    }
  }
}
