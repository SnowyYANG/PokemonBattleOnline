using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightStudio.PokemonBattle.Game.Host.Effects.Moves.Attack
{
  class KnockOff : AttackMoveE
  {
    public KnockOff(int id)
      : base(id)
    {
    }
    protected override void ImplementEffect(DefContext def)
    {
      var der = def.Defender;
      if (der.CanLostItem)
      {
        var i = der.Pokemon.Item.Id;
        der.RemoveItem();
        der.Controller.ReportBuilder.Add(new GameEvents.RemoveItem("KnockOff", der, i, def.AtkContext.Attacker));
      }
    }
  }
}
