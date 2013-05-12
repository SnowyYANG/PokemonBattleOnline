using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Data;
using Is = LightStudio.PokemonBattle.Game.Host.Sp.Items;

namespace LightStudio.PokemonBattle.Game.Host.Effects.Moves.Attack
{
  class NaturalGift : AttackMoveE
  {
    public NaturalGift(int id)
      : base(id)
    {
    }
    public override void Execute(AtkContext atk)
    {
      var aer = atk.Attacker;
      if (aer.CanLostItem && aer.CanUseItem && Is.Berry(aer.Pokemon.Item.Id))
      {
        base.Execute(atk);
        if (atk.Fail) aer.ConsumeItem();
        aer.Controller.ReportBuilder.Add(new GameEvents.RemoveItem(null, aer));
      }
      else atk.FailAll();
    }
    protected override void CalculateType(AtkContext atk)
    {
      var i = Is.BerryNumber(atk.Attacker.Pokemon.Item.Id);
      atk.Type = i < 36 ? BattleTypeHelper.GetItemType(i, 1) : i < 53 ? BattleTypeHelper.GetItemType(i, 36, false) : BattleTypeHelper.GetItemType(i, 50);
    }
    protected override void CalculateBasePower(DefContext def)
    {
      var i = Is.BerryNumber(def.AtkContext.Attacker.Pokemon.Item.Id);
      def.BasePower = i < 17 ? 60 : i < 33 ? 70 : i < 36 ? 80 : i < 53 ? 60 : 80;
    }
    protected override void ImplementEffect(DefContext def)
    {
      def.AtkContext.Attacker.ConsumeItem();
    }
  }
}
