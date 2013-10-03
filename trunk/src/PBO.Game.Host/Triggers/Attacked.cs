using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonBattleOnline.Game;

namespace PokemonBattleOnline.Game.Host.Triggers
{
  internal static class Attacked
  {
    public static void Execute(DefContext def)
    {
      var der = def.Defender;
      var atk = def.AtkContext;
      var aer = atk.Attacker;
      var touch = atk.Move.Flags.NeedTouch;
      var realHurt = def.Damage != 0;

      if (aer.Ability == As.POISON_TOUCH && touch && der.Controller.RandomHappen(30) && der.CanAddState(aer, AttachedState.PSN, false))
      {
        aer.RaiseAbility();
        der.AddState(aer, AttachedState.PSN, false);
      }
      switch (def.Defender.Ability) //此时破格不能无视
      {
        case As.ILLUSION:
          ATs.DeIllusion(def.Defender);
          break;
        case As.STATIC:
          if (touch) AddState(def, AttachedState.PAR);
          break;
        case As.POISON_POINT:
          if (touch) AddState(def, AttachedState.PSN);
          break;
        case As.FLAME_BODY:
          if (touch) AddState(def, AttachedState.BRN);
          break;
        case As.CUTE_CHARM:
          if (touch) AddState(def, AttachedState.Attract);
          break;
        case As.ROUGH_SKIN:
        case As.IRON_BARBS:
          if (touch) RoughSkin(def);
          break;
        case As.EFFECT_SPORE:
          if (touch && realHurt) EffectSpore(def);
          break;
        case As.ANGER_POINT:
          if (def.IsCt && der.OnboardPokemon.Lv5D.Atk != 6)
          {
            der.RaiseAbility();
            der.ChangeLv7D(der, StatType.Atk, 12, false, "AngerPoint");
          }
          break;
        case As.AFTERMATH:
          if (touch && der.Hp == 0 && aer.CanEffectHurt && !aer.Controller.Board.Pokemons.Any((p) => p.Ability == As.DAMP))
          {
            der.RaiseAbility();
            aer.EffectHurtByOneNth(4, "Hurt", 0, 0);
          }
          break;
        case As.PICKPOCKET:
          if (touch) Pickpocket(def);
          break;
        case As.CURSED_BODY:
          if (atk.Controller.RandomHappen(30) && aer.CanAddState(der, AttachedState.Disable, false))
          {
            der.RaiseAbility();
            aer.AddState(der, AttachedState.Disable, false, 4);
          }
          break;
        case As.WEAK_ARMOR:
          if (atk.Move.Category == MoveCategory.Physical && !(der.CanChangeLv7D(der, StatType.Speed, 1, false) == 0 && der.CanChangeLv7D(der, StatType.Def, -1, false) == 0))
          {
            der.RaiseAbility();
            der.ChangeLv7D(der, false, 0, -1, 0, 0, 1);
          }
          break;
        case As.MUMMY:
          if (touch && aer.Ability != As.MULTITYPE && aer.Ability != As.MUMMY)
          {
            der.RaiseAbility();
            var fa = aer.OnboardPokemon.Ability;
            aer.ChangeAbility(As.MUMMY);
            aer.AddReportPm("SetAbility", As.MUMMY, fa);
          }
          break;
        case As.JUSTIFIED:
          if (atk.Move.Type == BattleType.Dark && der.CanChangeLv7D(der, StatType.Atk, 1, false) != 0)
          {
            der.RaiseAbility();
            der.ChangeLv7D(der, StatType.Atk, 1, false);
          }
          break;
        case As.RATTLED:
          Rattled(def);
          break;
      }
      switch (def.Defender.Item)
      {
        case Is.STICKY_BARB: //65
          if (touch && aer.Pokemon.Item == null && der.Controller.RandomHappen(10))
          {
            der.RemoveItem();
            aer.SetItem(Is.STICKY_BARB);
          }
          break;
        case Is.ROCKY_HELMET: //104
          if (touch) aer.EffectHurtByOneNth(6, "RockyHelmet", 0, 0);
          break;
        case Is.AIR_BALLOON: //105
          ITs.AirBalloon(def);
          break;
        case Is.ABSORB_BULB: //109
          AttackedUpItem(def, BattleType.Water, StatType.SpAtk);
          break;
        case Is.CELL_BATTERY: //110
          AttackedUpItem(def, BattleType.Electric, StatType.Atk);
          break;
        case Is.ENIGMA_BERRY: //188
          if (def.EffectRevise > 0) der.HpRecoverByOneNth(4, false, "ItemRecover", 188, true);
          break;
        case Is.JABOCA_BERRY: //191
          ReHurtBerry(def, MoveCategory.Physical);
          break;
        case Is.ROWAP_BERRY: //192
          ReHurtBerry(def, MoveCategory.Special);
          break;
      }
      if (der.OnboardPokemon.HasCondition("Rage")) der.ChangeLv7D(der, StatType.Atk, 1, false, "Rage");
    }
    private static void AddState(DefContext def, AttachedState state)
    {
      if (def.AtkContext.Attacker.CanAddState(def.Defender, state, false) && def.AtkContext.Controller.RandomHappen(30))
      {
        def.Defender.RaiseAbility();
        def.AtkContext.Attacker.AddState(def.Defender, state, false, 0);
      }
    }
    private static void RoughSkin(DefContext def)
    {
      if (def.AtkContext.Attacker.CanEffectHurt)
      {
        def.Defender.RaiseAbility();
        def.AtkContext.Attacker.EffectHurtByOneNth(8, "Hurt", 0, 0);
      }
    }
    private static void EffectSpore(DefContext d)
    {
      var a = d.AtkContext;
      if (a.Controller.RandomHappen(10))
      {
        var i = a.Controller.GetRandomInt(0, 2);
        var state = i == 0 ? AttachedState.PAR : i == 1 ? AttachedState.SLP : AttachedState.PSN;
        if (a.Attacker.CanAddState(d.Defender, state, false))
        {
          d.Defender.RaiseAbility();
          a.Attacker.AddState(d.Defender, state, false);
        }
      }
    }
    private static void Pickpocket(DefContext d)
    {
      var der = d.Defender;
      var aer = d.AtkContext.Attacker;
      if (der.Pokemon.Item == null && ITs.CanLostItem(aer))
      {
        var i = aer.Pokemon.Item.Id;
        aer.RemoveItem();
        der.RaiseAbility();
        der.SetItem(i);
        der.AddReportPm("Pickpocket", i);
      }
    }
    private static void Rattled(DefContext d)
    {
      var type = d.AtkContext.Move.Type;
      var der = d.Defender;
      if ((type == BattleType.Dark || type == BattleType.Ghost || type == BattleType.Bug) && der.CanChangeLv7D(der, StatType.Speed, 1, false) != 0)
      {
        der.RaiseAbility();
        der.ChangeLv7D(der, StatType.Speed, 1, false);
      }
    }
    private static void ReHurtBerry(DefContext def, MoveCategory category)
    {
      var aer = def.AtkContext.Attacker;
      if (def.AtkContext.Move.Category == category && aer.CanEffectHurt)
      {
        int hp = aer.Pokemon.Hp.Origin >> 3;
        if (hp == 0) hp = 1;
        aer.Pokemon.SetHp(aer.Hp - hp);
        aer.AddReportPm("ReHurtItem", def.Defender, def.Defender.Pokemon.Item.Id);
        aer.Controller.ReportBuilder.ShowHp(aer);
      }
    }
    private static void AttackedUpItem(DefContext def, BattleType type, StatType stat)
    {
      if (def.AtkContext.Type == type) ITs.ChangeLv5D(def.Defender, stat, 1);
    }
  }
}
