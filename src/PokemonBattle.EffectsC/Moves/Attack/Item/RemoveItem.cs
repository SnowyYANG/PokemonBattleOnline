using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokemonBattleOnline.Game.Host.Effects.Moves.Attack
{
  class RemoveItem : AttackMoveE
  {
    bool Berry;
    string Log;
    
    public RemoveItem(int id, bool berryOnly, string log)
      : base(id)
    {
      Berry = berryOnly;
      Log = log;
    }
    protected override void ImplementEffect(DefContext def)
    {
      var der = def.Defender;
      if (der.CanLostItem)
      {
        var i = der.Pokemon.Item.Id;
        if (!Berry || Sp.Items.BerryNumber(i) != 0)
        {
          der.RemoveItem();
          if (Log != null) der.Controller.ReportBuilder.Add(new GameEvents.RemoveItem(Log, der, i, Berry ? null : (ValueType)def.AtkContext.Attacker.Id));
        }
      }
    }
  }
}
