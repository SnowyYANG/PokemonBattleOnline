using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonBattleOnline.Game;

namespace PokemonBattleOnline.Game.Host.Triggers
{
  internal static class CalculateDamages
  {
    public static void Execute(AtkContext atk)
    {
      var move = atk.Move;
      var def = atk.Target;
      var der = def.Defender;
      var aer = atk.Attacker;

      bool ls = false, r = false, feint = false;
      switch (move.Id)
      {
        case Ms.BRICK_BREAK: //280
          if (der.Pokemon.TeamId != aer.Pokemon.TeamId)
          {
            ls = der.Tile.Field.RemoveCondition("LightScreen");
            r = der.Tile.Field.RemoveCondition("Reflect");
          }
          break;
        case Ms.FEINT: //364
          feint = der.OnboardPokemon.RemoveCondition("Protect") | (der.Pokemon.TeamId != atk.Attacker.Pokemon.TeamId && (der.Tile.Field.RemoveCondition("QuickGuard") | der.Tile.Field.RemoveCondition("WideGuard")));
          break;
      }
      switch (atk.Move.Id)
      {
        case Ms.COUNTER:
          Counter(atk, "PhysicalDamage", 0x2000);
          break;
        case Ms.MIRROR_COAT:
          Counter(atk, "SpecialDamage", 0x2000);
          break;
        case Ms.METAL_BURST:
          Counter(atk, "Damage", 0x1800);
          break;
        case Ms.SONICBOOM: //49
          def.Damage = 20;
          break;
        case Ms.DRAGON_RAGE: //82
          def.Damage = 40;
          break;
        case Ms.SEISMIC_TOSS: //69
        case Ms.NIGHT_SHADE: //101
          def.Damage = aer.Pokemon.Lv;
          break;
        case Ms.BIDE: //117
          def.Damage = atk.GetCondition("Bide").Damage << 1;
          break;
        case Ms.PSYWAVE: //149
          def.Damage = der.Controller.GetRandomInt(50, 150) * aer.Pokemon.Lv / 100;
          if (def.Damage == 0) def.Damage = 1;
          break;
        case Ms.SUPER_FANG: //162
          def.Damage = der.Hp >> 1;
          if (def.Damage == 0) def.Damage = 1;
          break;
        case Ms.ENDEAVOR: //283
          def.Damage = der.Hp - aer.Hp;
          if (def.Damage < 0) def.Damage = 0;
          break;
        case Ms.FINAL_GAMBIT: //515
          def.Damage = atk.Attacker.Hp;
          break;
        default:
          if (move.Class != MoveInnerClass.OHKO)
          {
            ITs.CheckGem(atk);
            atk.CTLv = move.CtLv;
            if (atk.CTLv < 5)
            {
              atk.CTLv += STs.CtLvRevise(aer);
              if (atk.CTLv > 4) atk.CTLv = 4;
            }
            foreach (DefContext d in atk.Targets) CalculateDamage(d);
          }
          break;
      }
      switch (atk.Move.Id)
      {
        case Ms.FALSE_SWIPE:
          if (def.Damage >= def.Defender.Hp) def.Damage = def.Defender.Hp - 1;
          break;
        case Ms.BEAT_UP:
          {
            BattleType a = def.AtkContext.Type;
            def.EffectRevise = a == BattleType.Ground && der.Item == Is.IRON_BALL && der.OnboardPokemon.HasType(BattleType.Flying) ? 0 : a.EffectRevise(der.OnboardPokemon.Types);
          }
          break;
        case Ms.SELFDESTRUCT: //120
        case Ms.EXPLOSION: //153
        case Ms.FINAL_GAMBIT: //515
          atk.Attacker.Faint();
          break;
        case Ms.BRICK_BREAK: //280
          if (ls) atk.Controller.ReportBuilder.ShowLog("DeLightScreen", der.Tile.Field.Team);
          if (r) atk.Controller.ReportBuilder.ShowLog("DeReflect", der.Tile.Field.Team);
          break;
        case Ms.FEINT: //364
          if (feint) der.AddReportPm("Feint");
          break;
      }
    }
    private static void CalculateEffectRevise(DefContext def)
    {
      BattleType a = def.AtkContext.Type;
      OnboardPokemon der = def.Defender.OnboardPokemon;
      def.EffectRevise = a == BattleType.Ground && def.Defender.Item == Is.IRON_BALL && der.HasType(BattleType.Flying) ? 0 : a.EffectRevise(der.Types);
    }
    private static readonly int[] LV_CT = { 16, 8, 4, 3, 2, 0 };
    private static void CalculateDamage(DefContext def)
    {
      var atk = def.AtkContext;
      var aer = atk.Attacker;
      var c = aer.Controller;
      var move = atk.Move;

      if (!(def.Defender.Tile.Field.HasCondition("LuckyChant") || ATs.CannotBeCted(def.Ability)))
        if (move.CtLv > 5) def.IsCt = true;
        else def.IsCt = c.OneNth(LV_CT[atk.CTLv]);

      def.Damage = aer.Pokemon.Lv * 2 / 5 + 2;
      {
        MoveBasePower.Execute(def);
        def.Damage *= def.BasePower * PowerModifier.Execute(def);
      }
      {
        int a;
        {
          OnboardPokemon p;
          if (move.Id == Ms.FOUL_PLAY) p = def.Defender.OnboardPokemon;
          else p = atk.Attacker.OnboardPokemon;
          StatType st = move.Category == MoveCategory.Physical ? StatType.Atk : StatType.SpAtk;
          a = p.FiveD.GetStat(st);
          if (def.Ability != As.UNAWARE)
          {
            int atkLv = p.Lv5D.GetStat(st);
            if (!(def.IsCt && atkLv < 0)) a = OnboardPokemon.Get5D(a, atkLv);
          }
        }
        a *= AModifier.Hustle(atk);
        def.Damage *= a * AModifier.Execute(def);
      }
      {
        StatType st = move.Category == MoveCategory.Physical || move.UsePhysicalDef() ? StatType.Def : StatType.SpDef;
        int d = def.Defender.OnboardPokemon.FiveD.GetStat(st);
        int defLv;
        if (aer.Ability == As.UNAWARE || move.IgnoreDefenderLv7D()) defLv = 0;
        else
        {
          defLv = def.Defender.OnboardPokemon.Lv5D.GetStat(st);
          if (!(def.IsCt && defLv > 0)) d = OnboardPokemon.Get5D(d, defLv);
        }
        d *= DModifier.Sandstorm(def);
        def.Damage /= d * DModifier.Execute(def);
      }
      def.Damage /= 50;
      def.Damage += 2;
      //1.Apply the multi-target modifier
      if (atk.MultiTargets) def.ModifyDamage(0xC00);
      //2.Apply the weather modifier
      {
        Weather w = c.Weather;
        BattleType type = atk.Type;
        if (w == Weather.IntenseSunlight)
        {
          if (type == BattleType.Water) def.ModifyDamage(0x800);
          else if (type == BattleType.Fire) def.ModifyDamage(0x1800);
        }
        else if (w == Weather.HeavyRain)
        {
          if (type == BattleType.Water) def.ModifyDamage(0x1800);
          else if (type == BattleType.Fire) def.ModifyDamage(0x800);
        }
      }
      //3.In case of a critical hit, double the value
      if (def.IsCt) def.Damage <<= 1;
      //4.Alter with a random factor
      def.Damage *= aer.Controller.GetRandomInt(85, 100);
      def.Damage /= 100;
      //5.Apply STAB modifier
      if (atk.Attacker.OnboardPokemon.HasType(atk.Type))
        def.ModifyDamage((ushort)(atk.Attacker.Ability == As.ADAPTABILITY ? 0x2000 : 0x1800));
      //6.Alter with type effectiveness
      CalculateEffectRevise(def);
      if (def.EffectRevise > 0) def.Damage <<= def.EffectRevise;
      else if (def.EffectRevise < 0) def.Damage >>= -def.EffectRevise;
      //7.Alter with user's burn
      if (move.Category == MoveCategory.Physical && aer.State == PokemonState.BRN && aer.Ability != As.GUTS) def.Damage >>= 1;
      //8.Make sure damage is at least 1
      if (def.Damage < 1) def.Damage = 1;
      //9.Apply the final modifier
      def.Damage *= DamageModifier.Execute(def);
    }

    private static void Counter(AtkContext atk, string condition, Modifier modifier)
    {
      ITs.CheckGem(atk);
      atk.Target.Damage = atk.Attacker.OnboardPokemon.GetCondition(condition).Damage;
      atk.Target.ModifyDamage(modifier);
    }

  }
}
