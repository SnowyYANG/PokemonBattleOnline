using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonBattleOnline.Game.Host.Sp;
using Is = PokemonBattleOnline.Game.Host.Sp.Items;

namespace PokemonBattleOnline.Game.Host.Effects.Moves.Attack
{
  class EatDefenderBerry : RemoveItem
  {
    public EatDefenderBerry(int id)
      : base(id, true, null)
    {
    }
    protected override void ImplementEffect(DefContext def)
    {
      var i = Sp.Items.BerryNumber(def.Defender.Item.Id);
      if (i != 0) def.SetCondition("EatenBerry", i);
    }
    protected override void PostEffect(DefContext def)
    {
      var b = def.GetCondition<int>("EatenBerry");
      if (b != 0)
      {
        var i = Is.BerryNumberToItemId(b);
        var aer = def.AtkContext.Attacker;
        def.AtkContext.Attacker.AddReportPm("EatDefenderBerry", i);
        Is.RaiseItemByMove(aer, i, aer);
      }
    }
  }
}
