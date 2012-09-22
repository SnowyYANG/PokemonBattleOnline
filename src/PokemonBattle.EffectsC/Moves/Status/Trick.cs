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
      var der = atk.Target.Defender;
      if ((der.Pokemon.Item == null && atk.Attacker.Item == null) || Is.CantLostItem(atk.Attacker.Pokemon) || Is.CantLostItem(der.Pokemon)) FailAll(atk);
      else
      {
        atk.Attacker.AddReportPm("Trick");
        var di = der.Pokemon.Item;
        var ai = atk.Attacker.Pokemon.Item;
        if (ai != null) der.ChangeItem(ai.Id, "GetItem", null, false);
        if (di != null) atk.Attacker.ChangeItem(di.Id, "GetItem", null, false);
      }
    }
  }
}
