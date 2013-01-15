using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Is = PokemonBattleOnline.Game.Host.Sp.Items;

namespace PokemonBattleOnline.Game.Host.Effects.Moves.Attack
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
      if (aer.CanLostItem && aer.CanUseItem && !Is.Gem(aer.Pokemon.Item.Id))
      {
        base.Execute(atk);
        if (atk.Fail) aer.ConsumeItem();
        aer.Controller.ReportBuilder.Add(new GameEvents.RemoveItem(null, aer));
      }
      else atk.FailAll();
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
