using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Game.Host.Sp;
using Is = LightStudio.PokemonBattle.Game.Host.Sp.Items;

namespace LightStudio.PokemonBattle.Game.Host.Effects.Moves.Status
{
  class Trick : StatusMoveE
  {
    public Trick(int id)
      : base(id)
    {
    }
    protected override void Act(AtkContext atk)
    {
      var aer = atk.Attacker;
      var der = atk.Target.Defender;
      var di = der.Pokemon.Item;
      var ai = aer.Pokemon.Item;
      if ((di == null && ai == null) || ai == di || Is.CantLostItem(aer.Pokemon) || Is.CantLostItem(der.Pokemon)) FailAll(atk);
      else
      {
        aer.AddReportPm("Trick");
        if (ai != null)
        {
          aer.RemoveItem();
          der.ChangeItem(ai.Id, "GetItem", aer, false);
        }
        if (di != null)
        {
          der.RemoveItem();
          aer.ChangeItem(di.Id, "GetItem", der, false);
        }
      }
    }
  }
}
