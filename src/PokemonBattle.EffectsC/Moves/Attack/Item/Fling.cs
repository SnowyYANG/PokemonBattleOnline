using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Is = LightStudio.PokemonBattle.Game.Host.Sp.Items;

namespace LightStudio.PokemonBattle.Game.Host.Effects.Moves.Attack
{
  class Fling : AttackMoveE
  {
    public Fling(int id)
      : base(id)
    {
    }

    public override void Execute(AtkContext atk)
    {
      var aer = atk.Attacker;
      if (aer.CanLostItem)
      {
        var i = aer.Item.Id;
        if (i != 0 && !Is.Gem(i))
        {
          base.Execute(atk);
          if (atk.FailAll) aer.RemoveItem();
          return;
        }
      }
      FailAll(atk);
    }
    protected override void Act(AtkContext atk)
    {
      atk.Attacker.AddReportPm("Fling", atk.Attacker.Pokemon.Item.Id);
      base.Act(atk);
    }
    protected override void CalculateBasePower(DefContext def)
    {
      def.BasePower = def.AtkContext.Attacker.Pokemon.Item.FlingPower;
    }
    protected override void ImplementEffect(DefContext def)
    {
      var aer = def.AtkContext.Attacker;
      var i = aer.Pokemon.Item.Id;
      aer.ConsumeItem();
      Is.RaiseItemByMove(def.Defender, i, aer);
    }
  }
}
