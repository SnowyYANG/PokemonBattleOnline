using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Data;

namespace LightStudio.PokemonBattle.Game.Host.Effects.Moves.Status
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
      var item = atk.Controller.Board[aer.Pokemon.TeamId].GetCondition<Item>("UsedItem" + aer.Id);
      if (item == null) FailAll(atk);
      else aer.ChangeItem(item.Id, "Recycle");
    }
  }
}
