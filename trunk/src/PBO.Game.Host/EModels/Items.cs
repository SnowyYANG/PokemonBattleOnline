using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonBattleOnline.Game;
using PokemonBattleOnline.Game.Host.Triggers;

namespace PokemonBattleOnline.Game.Host
{
  internal static class ITs
  {
    public static void RaiseItem(this PokemonProxy pm, string key)
    {
      var item = pm.Pokemon.Item;
      if (item != null)
      {
        if (item.Type == ItemType.Normal) pm.AddReportPm(key, item.Id);
        else
        {
          pm.ConsumeItem();
          pm.Controller.ReportBuilder.Add(new GameEvents.RemoveItem(key, pm, item.Id));
        }
      }
    }

    public static void ChangeLv5D(PokemonProxy pm, StatType stat, int change)
    {
      change = pm.CanChangeLv7D(pm, stat, change, false);
      if (change == 0) return;
      var i = pm.Pokemon.Item.Id;
      string log;
      switch (change)
      {
        case 1:
          log = "Item7DUp1";
          break;
        case 2:
          log = "Item7DUp2";
          break;
        case -1:
          log = "7DDown1";
          break;
        case -2:
          log = "7DDown2";
          break;
        default:
          log = change > 0 ? "Item7DUp3" : "7DDown3";
          break;
      }
      pm.OnboardPokemon.ChangeLv7D(stat, change);
      pm.ConsumeItem();
      pm.Controller.ReportBuilder.Add(new GameEvents.RemoveItem(log, pm, stat, change > 0 ? i : 0));
    }

    public static bool ChoiceItem(int item)
    {
      return item == Is.CHOICE_BAND || item == Is.CHOICE_SCARF || item == Is.CHOICE_SPECS;
    }

    /// <summary>
    /// pm.Item should not be null
    /// </summary>
    /// <param name="pm"></param>
    /// <returns></returns>
    public static bool NeverLostItem(Pokemon pm)
    {
      var i = pm.Item.Id;
      return
        i == Is.RSVP_MAIL ||
        pm.Form.Species.Number == 487 && i == Is.GRISEOUS_ORB || //giratina
        PlatedArceus(pm) ||
        pm.Form.Species.Number == 649 && Is.DOUSE_DRIVE <= i && i <= Is.CHILL_DRIVE; //genesect
    }
    public static bool CanLostItem(PokemonProxy pm)
    {
      Item i = pm.Pokemon.Item;
      return !
        (
        i == null ||
        NeverLostItem(pm.Pokemon) ||
        pm.Ability == As.STICKY_HOLD
        );
    }
    public static bool CanUseItem(PokemonProxy pm)
    { return !(pm.OnboardPokemon.HasCondition("Embargo") || pm.Controller.Board.HasCondition("MagicRoom") || pm.Ability == As.KLUTZ); }
    public static bool PlatedArceus(Pokemon pm)
    {
      return pm.Item != null && pm.Form.Species.Number == 493 && Is.FLAME_PLATE <= pm.Item.Id && pm.Item.Id <= Is.IRON_PLATE;
    }
    public static bool Berry(int id)
    {
      return BerryNumber(id) != 0;
    }
    public static int BerryNumber(int id)
    {
      return Is.CHERI_BERRY <= id && id <= Is.ROWAP_BERRY ? id - Is.CHERI_BERRY + 1 : 0;
    }
    public static int BerryNumberToItemId(int number)
    {
      return 0 < number && number <= 64 ? Is.CHERI_BERRY - 1 + number : 0;
    }
    public static bool Gem(int id)
    {
      return Is.FIRE_GEM <= id && id <= Is.NORMAL_GEM;
    }
    public static StatType GetTaste(int berry)
    {
      switch (berry)
      {
        case 11:
          return StatType.Atk;
        case 12:
          return StatType.SpAtk;
        case 13:
          return StatType.Speed;
        case 14:
          return StatType.SpDef;
        case 15:
          return StatType.Def;
      }
      return StatType.Invalid;
    }
    public static StatType GetTaste(Item item)
    {
      return GetTaste(BerryNumber(item.Id));
    }
    public static void RaiseItemByMove(PokemonProxy pm, int id, PokemonProxy by)
    {
      var op = pm.OnboardPokemon;
      switch (id)
      {
        case Is.WHITE_HERB: //5
          {
            bool raise = false;
            var lvs = (Simple6D)op.Lv5D;
            if (lvs.Atk < 0) { lvs.Atk = 0; raise = true; }
            if (lvs.Def < 0) { lvs.Def = 0; raise = true; }
            if (lvs.SpAtk < 0) { lvs.SpAtk = 0; raise = true; }
            if (lvs.SpDef < 0) { lvs.SpDef = 0; raise = true; }
            if (lvs.Speed < 0) { lvs.Speed = 0; raise = true; }
            if (op.AccuracyLv < 0) { op.AccuracyLv = 0; raise = true; }
            if (op.EvasionLv < 0) { op.EvasionLv = 0; raise = true; }
            if (raise) pm.AddReportPm("WhiteHerb");
          }
          break;
        case Is.MENTAL_HERB:
          if (op.RemoveCondition("Attract")) pm.AddReportPm("ItemDeAttract", 8);
          break;
        case Is.KINGS_ROCK:
          op.AddTurnCondition("Flinch");
          break;
        case Is.LIGHT_BALL:
          pm.AddState(by, AttachedState.PAR, false);
          break;
        case Is.POISON_BARB:
          pm.AddState(by, AttachedState.PSN, false);
          break;
        case Is.TOXIC_ORB:
          pm.AddState(by, AttachedState.PSN, false, 15);//战报待验证
          break;
        case Is.FLAME_ORB:
          pm.AddState(by, AttachedState.BRN, false);//战报
          break;
        case Is.RAZOR_FANG:
          op.AddTurnCondition("Flinch");
          break;
        case Is.CHERI_BERRY:
          if (pm.State == PokemonState.PAR) pm.DeAbnormalState();//战报
          break;
        case Is.CHESTO_BERRY:
          if (pm.State == PokemonState.PAR) pm.DeAbnormalState();
          break;
        case Is.PECHA_BERRY:
          if (pm.State == PokemonState.PAR) pm.DeAbnormalState();
          break;
        case Is.RAWST_BERRY:
          if (pm.State == PokemonState.BRN) pm.DeAbnormalState();
          break;
        case Is.ASPEAR_BERRY:
          if (pm.State == PokemonState.FRZ) pm.DeAbnormalState();
          break;
        case Is.LEPPA_BERRY:
          foreach (var m in pm.Moves)
            if (m.PP == 0)
            {
              m.PP += 10;
              pm.Controller.ReportBuilder.Add(new GameEvents.SetPP("ItemPPRecover", m) { Arg2 = 134 });
              return;
            }
          foreach (var m in pm.Moves)
            if (m.PP != m.Move.PP.Origin)
            {
              m.PP += 10;
              pm.Controller.ReportBuilder.Add(new GameEvents.SetPP("ItemPPRecover", m) { Arg2 = 134 });
              return;
            }
          break;
        case Is.ORAN_BERRY:
          pm.HpRecover(10, false, "ItemHpRecover", 135);
          break;
        case Is.PERSIM_BERRY:
          if (op.RemoveCondition("Confuse")) pm.AddReportPm("DeConfuse");
          break;
        case Is.LUM_BERRY:
          if (pm.State != PokemonState.Normal) pm.DeAbnormalState();
          break;
        case Is.SITRUS_BERRY:
          pm.HpRecoverByOneNth(3, false, "ItemHpRecover", 138);
          break;
        case Is.FIGY_BERRY:
        case Is.WIKI_BERRY:
        case Is.MAGO_BERRY:
        case Is.AGUAV_BERRY:
        case Is.IAPAPA_BERRY:
          pm.HpRecoverByOneNth(8, false, "ItemRecover", id);
          if (pm.Pokemon.Nature.DislikeTaste(GetTaste(BerryNumber(id)))) pm.AddState(pm, AttachedState.Confuse, false);
          break;
        case Is.LIECHI_BERRY:
          pm.ChangeLv7D(by, StatType.Atk, 1, false);
          break;
        case Is.GANLON_BERRY:
          pm.ChangeLv7D(by, StatType.Def, 1, false);
          break;
        case Is.SALAC_BERRY:
          pm.ChangeLv7D(by, StatType.Speed, 1, false);
          break;
        case Is.PETAYA_BERRY:
          pm.ChangeLv7D(by, StatType.SpAtk, 1, false);
          break;
        case Is.APICOT_BERRY:
          pm.ChangeLv7D(by, StatType.SpDef, 1, false);
          break;
        case Is.LANSAT_BERRY:
          if (pm.OnboardPokemon.AddCondition("FocusEnergy")) pm.AddReportPm("ItemEnFocusEnergy", Is.LANSAT_BERRY);
          break;
        case Is.STARF_BERRY:
          {
            var ss = new List<StatType>();
            if (pm.CanChangeLv7D(by, StatType.Atk, 2, false) != 0) ss.Add(StatType.Atk);
            if (pm.CanChangeLv7D(by, StatType.Def, 2, false) != 0) ss.Add(StatType.Def);
            if (pm.CanChangeLv7D(by, StatType.SpAtk, 2, false) != 0) ss.Add(StatType.SpAtk);
            if (pm.CanChangeLv7D(by, StatType.SpDef, 2, false) != 0) ss.Add(StatType.SpDef);
            if (pm.CanChangeLv7D(by, StatType.Speed, 2, false) != 0) ss.Add(StatType.Speed);
            var n = ss.Count;
            if (n != 0) pm.ChangeLv7D(by, ss[pm.Controller.GetRandomInt(0, n - 1)], 2, false);
          }
          break;
        case Is.MICLE_BERRY:
          if (pm.OnboardPokemon.AddCondition("MicleBerry")) pm.AddReportPm("MicleBerry", Is.MICLE_BERRY);
          break;
      }
    }

    public static void WhiteHerb(PokemonProxy pm)
    {
      if (pm.Item == Is.WHITE_HERB)
      {
        Simple6D lvs = (Simple6D)pm.OnboardPokemon.Lv5D;
        bool raise = false;
        if (lvs.Atk < 0) { lvs.Atk = 0; raise = true; }
        if (lvs.Def < 0) { lvs.Def = 0; raise = true; }
        if (lvs.SpAtk < 0) { lvs.SpAtk = 0; raise = true; }
        if (lvs.SpDef < 0) { lvs.SpDef = 0; raise = true; }
        if (lvs.Speed < 0) { lvs.Speed = 0; raise = true; }
        if (pm.OnboardPokemon.AccuracyLv < 0) { pm.OnboardPokemon.AccuracyLv = 0; raise = true; }
        if (pm.OnboardPokemon.EvasionLv < 0) { pm.OnboardPokemon.EvasionLv = 0; raise = true; }
        if (raise) RaiseItem(pm, "WhiteHerb");
      }
    }
    public static bool AirBalloon(PokemonProxy pm) //气球的提示信息不是Attach而是Debut，是唯一会Debut的道具
    {
      if (pm.Item == Is.AIR_BALLOON) //batonpass embargo
      {
        pm.AddReportPm("EnBalloon");
        return true;
      }
      return false;
    }
    public static void AirBalloon(DefContext def)
    {
      def.Defender.RemoveItem();
      def.Defender.AddReportPm("DeBalloon");
    }
    public static void AttackPostEffect(AtkContext atk)
    {
      var aer = atk.Attacker;
      var c = aer.Controller;
      if (!atk.IgnoreSwitchItem)
      {
        bool e = true, r = MoveE.CanForceSwitch(aer, true);
        foreach (var d in atk.Targets.Where((d) => !d.HitSubstitute && d.Defender.Tile != null).OrderBy((d) => d.Defender.Speed).ToArray())
        {
          var der = d.Defender;
          var i = der.Item;
          if (e && i == Is.EJECT_BUTTON)
          {
            atk.SetCondition("EjectButton", der.Tile);
            der.ConsumeItem();
            c.ReportBuilder.Add(new GameEvents.RemoveItem(null, der));
            c.Withdraw(der, "EjectButton");
            if (r == false) break;
            e = false;
          }
          else if (r && i == Is.RED_CARD)
          {
            der.ConsumeItem();
            c.ReportBuilder.Add(new GameEvents.RemoveItem("RedCard", der, aer.Id));
            MoveE.ForceSwitchImplement(aer, null);
            if (e == false) return;
            r = false;
          }
        }
      }
      if (aer.Item == Is.SHELL_BELL)
      {
        if (atk.TotalDamage != 0)
          aer.HpRecoverByOneNth(atk.TotalDamage >> 3, false, "ItemRecover", SHELL_BELL);
      }
      else if (aer.Item == Is.LIFE_ORB)
      {
        aer.EffectHurtByOneNth(10, "LifeOrb");
        aer.CheckFaint();
      }
    }
    public static bool CanAttackFlinch(DefContext def)
    {
      int item = def.AtkContext.Attacker.Item;
      return (item == Is.KINGS_ROCK || item == Is.RAZOR_FANG) && def.Defender.Controller.RandomHappen(10);
    }
    public static bool PowerHerb(PokemonProxy pm)
    {
      if (pm.Item == Is.POWER_HERB)
      {
        RaiseItem(pm, "PowerHerb");
        pm.OnboardPokemon.CoordY = CoordY.Plate;
        return true;
      }
      return false;
    }
    public static double FloatStone(PokemonProxy pm)
    {
      if (pm.Item == Is.FLOAT_STONE) return 0.5d;
      return 1d;
    }
    public static void CheckGem(AtkContext atk)
    {
      var i = atk.Attacker.Item;
      if (Gem(i) && BattleTypeHelper.GetItemType(i, 112, false) == atk.Type)
      {
        atk.SetTurnCondition("Gem");
        atk.Controller.ReportBuilder.Add(new GameEvents.RemoveItem("Gem", atk.Attacker, i, atk.Move.Id));
        atk.Attacker.ConsumeItem();
      }
    }
    public static void DestinyKnot(PokemonProxy pm, PokemonProxy by)
    {
      if (pm.Item == Is.DESTINY_KNOT) by.AddState(pm, AttachedState.Attract, false, 0, "ItemEnAttract", Is.DESTINY_KNOT);
    }
  }
}
