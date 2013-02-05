using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonBattleOnline.Data;

namespace PokemonBattleOnline.Game.Host.Sp
{
  public static class Items
  {
    #region ids
    public const int LEFTOVERS = 18;
    public const int TOXIC_ORB = 49;
    public const int FLAME_ORB = 50;
    public const int BLACK_SLUDGE = 58;
    public const int STICKY_BARB = 65;
    private const int LIFE_ORB = 47;
    #endregion

    public static void RaiseItem(this PokemonProxy pm, string key = null)
    {
      EffectsService.GetItem(pm.Pokemon.Item).Raise(pm, key);
    }

    #region isXXX
    public static bool LightClay(this ItemE item)
    {
      return item.Id == 46;
    }
    public static bool GripClaw(this ItemE item)
    {
      return item.Id == 63;
    }
    public static bool ShedShell(this ItemE item)
    {
      return item.Id == 72;
    }
    public static bool BigRoot(this ItemE item)
    {
      return item.Id == 73;
    }
    public static bool AirBalloon(this ItemE item)
    {
      return item.Id == 105;
    }
    public static bool RingTarget(this ItemE item)
    {
      return item.Id == 107;
    }
    public static bool BindingBand(this ItemE item)
    {
      return item.Id == 108;
    }
    public static bool IronBall(this ItemE item)
    {
      return item.Id == 55;
    }
    public static bool ChoiceItem(this ItemE item)
    {
      return item.Id == 9 || item.Id == 64 || item.Id == 74;
    }
    #endregion

    public static bool CantLostItem(Pokemon pm)
    {
      return
        pm.Item != null &&
        (pm.Item.Id == 193 ||
        pm.Form.Type.Number == 487 && pm.Item.Id == 1 || //giratina
        PlatedArceus(pm) ||
        pm.Form.Type.Number == 649 && pm.Item.Id > 97 && pm.Item.Id < 102); //genesect
    }
    public static bool PlatedArceus(Pokemon pm)
    {
      return pm.Form.Type.Number == 493 && pm.Item.Id > 74 && pm.Item.Id < 91;
    }
    public static bool Berry(int id)
    {
      return BerryNumber(id) != 0;
    }
    public static int BerryNumber(int id)
    {
      return 129 <= id && id <= 192 ? id - 128 : 0;
    }
    public static int BerryNumberToItemId(int number)
    {
      return 0 < number && number <= 64 ? 128 + number : 0;
    }
    public static bool Gem(int id)
    {
      return 111 < id && id < 129;
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
        case 5: //white herb
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
        case 8:
          if (op.RemoveCondition("Attract")) pm.AddReportPm("ItemDeAttract", 8);
          break;
        case 10:
          op.AddTurnCondition("Flinch");
          break;
        case 19:
          pm.AddState(by, AttachedState.PAR, false);
          break;
        case 28:
          pm.AddState(by, AttachedState.PSN, false);
          break;
        case 49:
          pm.AddState(by, AttachedState.PSN, false, 15);//战报待验证
          break;
        case 50:
          pm.AddState(by, AttachedState.BRN, false);//战报
          break;
        case 97:
          op.AddTurnCondition("Flinch");
          break;
        case 129:
          if (pm.State == PokemonState.PAR) pm.DeAbnormalState();//战报
          break;
        case 130:
          if (pm.State == PokemonState.PAR) pm.DeAbnormalState();
          break;
        case 131:
          if (pm.State == PokemonState.PAR) pm.DeAbnormalState();
          break;
        case 132:
          if (pm.State == PokemonState.BRN) pm.DeAbnormalState();
          break;
        case 133:
          if (pm.State == PokemonState.FRZ) pm.DeAbnormalState();
          break;
        case 134:
          foreach (var m in pm.Moves)
            if (m.PP == 0)
            {
              m.PP += 10;
              pm.Controller.ReportBuilder.Add(new GameEvents.PPChange("ItemPPRecover", m, 0));
              return;
            }
          foreach (var m in pm.Moves)
            if (m.PP != m.Move.PP.Origin)
            {
              m.PP += 10;
              pm.Controller.ReportBuilder.Add(new GameEvents.PPChange("ItemPPRecover", m, 0));
              return;
            }
          break;
        case 135:
          pm.HpRecover(10, false, "ItemHpRecover", 135);
          break;
        case 136:
          if (op.RemoveCondition("Confuse")) pm.AddReportPm("DeConfuse");
          break;
        case 137:
          if (pm.State != PokemonState.Normal) pm.DeAbnormalState();
          break;
        case 138:
          pm.HpRecoverByOneNth(3, false, "ItemHpRecover", 138);
          break;
        case 139:
        case 140:
        case 141:
        case 142:
        case 143:
          pm.HpRecoverByOneNth(8, false, "ItemRecover", id);
          if (pm.Pokemon.Nature.DislikeTaste(GetTaste(BerryNumber(id)))) pm.AddState(pm, AttachedState.Confuse, false);
          break;
        case 181:
          pm.ChangeLv7D(by, StatType.Atk, 1, false);
          break;
        case 182:
          pm.ChangeLv7D(by, StatType.Def, 1, false);
          break;
        case 183:
          pm.ChangeLv7D(by, StatType.Speed, 1, false);
          break;
        case 184:
          pm.ChangeLv7D(by, StatType.SpAtk, 1, false);
          break;
        case 185:
          pm.ChangeLv7D(by, StatType.SpDef, 1, false);
          break;
        case 186:
          //会心果，号称进入蓄气状态
          break;
        case 187:
          {
            var ss = new List<StatType>();
            if (pm.CanChangeLv7D(by, StatType.Atk, 2, false) != 0) ss.Add(StatType.Atk);
            if (pm.CanChangeLv7D(by, StatType.Def, 2, false) != 0) ss.Add(StatType.Def);
            if (pm.CanChangeLv7D(by, StatType.SpAtk, 2, false) != 0) ss.Add(StatType.SpAtk);
            if (pm.CanChangeLv7D(by, StatType.SpDef, 2, false) != 0) ss.Add(StatType.SpDef);
            if (pm.CanChangeLv7D(by, StatType.Speed, 2, false) != 0) ss.Add(StatType.Speed);
            if (pm.CanChangeLv7D(by, StatType.Accuracy, 2, false) != 0) ss.Add(StatType.Accuracy);
            if (pm.CanChangeLv7D(by, StatType.Evasion, 2, false) != 0) ss.Add(StatType.Evasion);
            var n = ss.Count;
            if (n != 0) pm.ChangeLv7D(by, ss[pm.Controller.GetRandomInt(0, n - 1)], 2, false);
          }
          break;
        case 189:
          //神秘果，某种意义上提高命中
          //测试，投掷后啄食或投掷后咬1/4血
          break;
      }
    }

    #region internal
    internal static void WhiteHerb(PokemonProxy pm)
    {
      if (pm.Item.Id == 5)
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
    internal static Modifier WideLens(AtkContext atk)
    {
      return (Modifier)(atk.Attacker.Item.Id == 42 ? 0x1199 : 0x1000);
    }
    internal static Modifier ZoomLens(DefContext target)
    {
      return (Modifier)(target.AtkContext.Attacker.Item.Id == 53 && target.AtkContext.Attacker.LastMoveTurn == target.Defender.LastMoveTurn ? 0x1333 : 0x1000);
    }
    internal static Modifier AccuracyModifier(DefContext def)
    {
      const int BRIGHT_POWDER = 4, LAX_INCENSE = 37;
      int i = def.Defender.Item.Id;
      ushort r = 0x1000;
      if (i == BRIGHT_POWDER) r = 0xE66;
      else if (i == LAX_INCENSE) r = 0xF34;
      return r;
    }
    internal static Modifier DamageFinalModifier(DefContext target)
    {
      //If the user is holding the item Metronome. If n is the number of time the current move was used successfully and successively, the value of the modifier is 0x1000+n*0x333 if n≤4 and 0x2000 otherwise.
      const int METRONOME = 54;
      //If the user is holding an expert belt and the move was super effective.
      const int EXPERT_BELT = 45;
      //If the user is holding a Life Orb.

      PokemonProxy aer = target.AtkContext.Attacker;
      int item = aer.Item.Id;
      ushort r = 0x1000;
      switch (item)
      {
        case EXPERT_BELT:
          if (target.EffectRevise > 0) r = 0x1333;
          break;
        case LIFE_ORB:
          r = 0x14cc;
          break;
        case METRONOME:
          var c = aer.OnboardPokemon.GetCondition("LastMove");
          if (c != null && target.AtkContext.Move == c.Move)
          {
            if (c.Int < 5) r += (ushort)(0x333 * c.Int);
            else r = 0x2000;
          }
          break;
      }
      return r;
    }
    internal static bool AirBalloon(PokemonProxy pm) //气球的提示信息不是Attach而是Debut，是唯一会Debut的道具
    {
      if (pm.Item.Id == 105)
      {
        pm.AddReportPm("EnBalloon");
        return true;
      }
      return false;
    }
    internal static void AttackPostEffect(AtkContext atk)
    {
      const int SHELL_BELL = 35, EJECT_BUTTON = 111, RED_CARD = 106;
      var aer = atk.Attacker;
      var c = aer.Controller;
      if (!atk.IgnoreSwitchItem)
      {
        bool e = true, r = MoveE.CanForceSwitch(aer, true);
        foreach (var d in atk.Targets.Where((d) => d.Defender.Tile != null).OrderBy((d) => d.Defender.Speed).ToArray())
        {
          var der = d.Defender;
          var i = der.Item.Id;
          if (e && i == EJECT_BUTTON)
          {
            atk.SetCondition("EjectButton", der.Tile);
            der.ConsumeItem();
            c.ReportBuilder.Add(new GameEvents.RemoveItem(null, der));
            c.Withdraw(der, "EjectButton");
            if (r == false) break;
            e = false;
          }
          else if (r && i == RED_CARD)
          {
            der.ConsumeItem();
            c.ReportBuilder.Add(new GameEvents.RemoveItem("RedCard", der, aer.Id));
            MoveE.ForceSwitchImplement(aer, null);
            if (e == false) return;
            r = false;
          }
        }
      }
      if (aer.Item.Id == SHELL_BELL)
      {
        if (atk.TotalDamage != 0)
          aer.HpRecoverByOneNth(atk.TotalDamage >> 3, false, "ItemRecover", SHELL_BELL);
      }
      else if (aer.Item.Id == LIFE_ORB)
      {
        aer.EffectHurtByOneNth(10, "LifeOrb");
        aer.CheckFaint();
      }
    }
    internal static bool CanAttackFlinch(DefContext def)
    {
      const int KINGS_ROCK = 10, RAZOR_FANG = 97;
      int item = def.AtkContext.Attacker.Item.Id;
      return (item == KINGS_ROCK || item == RAZOR_FANG) && def.Defender.Controller.RandomHappen(10);
    }
    internal static bool PowerHerb(PokemonProxy pm)
    {
      if (pm.Item.Id == 48)
      {
        RaiseItem(pm, "PowerHerb");
        pm.OnboardPokemon.CoordY = CoordY.Plate;
        return true;
      }
      return false;
    }
    internal static double FloatStone(PokemonProxy pm)
    {
      if (pm.Item.Id == 103) return 0.5d;
      return 1d;
    }
    internal static bool MicleBerry(AtkContext atk)
    {
      PokemonProxy pm = atk.Attacker;
      if (pm.Item.Id == 189 && Abilities.Gluttony(pm))
      {
        RaiseItem(pm, "MicleBerry");
        return true;
      }
      return false;
    }
    internal static void CheckGem(AtkContext atk)
    {
      var i = atk.Attacker.Item.Id;
      if (Gem(i) && BattleTypeHelper.GetItemType(i, 112, false) == atk.Type)
      {
        atk.SetTurnCondition("Gem");
        atk.Controller.ReportBuilder.Add(new GameEvents.RemoveItem("Gem", atk.Attacker, i, atk.Move.Id));
        atk.Attacker.ConsumeItem();
      }
    }
    #endregion
  }
}
