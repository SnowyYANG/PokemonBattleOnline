using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonBattleOnline.Data;

namespace PokemonBattleOnline.Game.Host.Effects.Moves.Status
{
  class Recycle : StatusMoveE
  {
    public Recycle(int id)
      : base(id)
    {
    }
    protected override bool NotFail(AtkContext atk)
    {
      return atk.Attacker.Pokemon.Item == null;
    }
    protected override void Act(AtkContext atk)
    {
      var aer = atk.Attacker;
      var item = aer.Tile.Field.GetCondition<Item>("UsedItem" + aer.Id);
      if (item == null) atk.FailAll();
      else
      {
        aer.ChangeItem(item.Id, "Recycle");
        aer.Tile.Field.RemoveCondition("UsedItem" + aer.Id);
      }
    }
  }
}
