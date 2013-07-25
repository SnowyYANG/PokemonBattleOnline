using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Data;

namespace LightStudio.PokemonBattle.Game.Host.Triggers
{
  internal static class MoveAct
  {
    public static void Execute(AtkContext atk)
    {
      var aer = atk.Attacker;
      var move = atk.Move;

      #region pre
      switch (move.Id)
      {
        case Ms.FLING:
          aer.AddReportPm("Fling", aer.Pokemon.Item.Id);
          break;
        case Ms.UPROAR:
          if (atk.GetCondition("MultiTurn").Turn == 3)
          {
            atk.Attacker.AddReportPm("EnUproar");
            foreach (var p in atk.Controller.Board.Pokemons)
              if (p.State == PokemonState.SLP) p.DeAbnormalState();
          }
          break;
        case Ms.CHATTER:
          if (aer.Pokemon.Chatter != null) aer.AddReportPm("Chatter");
          break;
      }
      #endregion
      #region main
      switch (move.Id)
      {
        case Ms.MIST: //54
          AddTeamCondition(atk, "Mist");
          break;
        case Ms.LIGHT_SCREEN: //113
          AddTeamCondition(atk, "LightScreen", aer.Item == Is.LIGHT_CLAY ? 8 : 5);
          break;
        case Ms.REFLECT: //115
          AddTeamCondition(atk, "Reflect", aer.Item == Is.LIGHT_CLAY ? 8 : 5);
          break;
        case Ms.SAFEGUARD: //219
          AddTeamCondition(atk, "Safeguard");
          break;
        case Ms.TAILWIND: //366
          AddTeamCondition(atk, "Tailwind", 4);
          break;
        case Ms.LUCKY_CHANT: //381
          AddTeamCondition(atk, "LuckyChant");
          break;
        case Ms.GROWTH: //74
          {
            var c = aer.Controller.Weather == Weather.IntenseSunlight ? 2 : 1;
            aer.ChangeLv7D(aer, StatType.Atk, c, true);
            aer.ChangeLv7D(aer, StatType.SpAtk, c, true);
          }
          break;
        case Ms.MIMIC: //102
          Mimic(atk);
          break;
        case Ms.FOCUS_ENERGY: //116
          AddCondition(atk, "FocusEnergy");
          break;
        case Ms.IMPRISON: //286
          AddCondition(atk, "Imprison");
          break;
        case Ms.AQUA_RING: //392
          AddCondition(atk, "AquaRing");
          break;
        case Ms.HAZE: //114
          foreach(var pm in atk.Controller.Board.Pokemons) pm.OnboardPokemon.SetLv7D(0, 0, 0, 0, 0, 0, 0);
          aer.Controller.ReportBuilder.Add("Haze");
          break;
        case Ms.BIDE: //117
          {
            var turn = atk.GetCondition("MultiTurn").Turn;
            if (turn == 1)
            {
              atk.Attacker.AddReportPm("DeBide");
              StatusMove(atk);
            }
            else if (turn == 2) atk.Attacker.AddReportPm("Bide");
          }
          break;
        case Ms.TRANSFORM: //144
          if (aer.CanTransform(atk.Target.Defender)) aer.Transform(atk.Target.Defender);
          else atk.FailAll();
          break;
        case Ms.SPLASH: //150
          aer.Controller.ReportBuilder.Add("Splash");
          break;
        case Ms.REST: //156
          Rest(atk);
          break;
        case Ms.CONVERSION: //160
          Conversion(atk);
          break;
        case Ms.SUBSTITUTE: //164
          Substitute(atk);
          break;
        case Ms.TRIPLE_KICK: //167
          TripleKick(atk);
          break;
        case Ms.SPIDER_WEB: //169
        case Ms.MEAN_LOOK: //212
        case Ms.BLOCK: //335
          CantSelectWithdraw(atk);
          break;
        case Ms.MIND_READER: //170
        case Ms.LOCKON: //199
          LockOn(atk);
          break;
        case Ms.CURSE: //174
          Curse(atk);
          break;
        case Ms.CONVERSION_2: //176
          Conversion2(atk);
          break;
        case Ms.SPITE: //180
          Spite(atk);
          break;
        case Ms.PROTECT: //182
        case Ms.DETECT: //197
          SelfProtect(atk, "Protect");
          break;
        case Ms.ENDURE: //203
          SelfProtect(atk, "Endure");
          break;
        case Ms.BELLY_DRUM: //187
          BellyDrum(atk);
          break;
        case Ms.SPIKES: //191
          EntryHazards(atk, "EnSpikes");
          break;
        case Ms.TOXIC_SPIKES: //390
          EntryHazards(atk, "EnToxicSpikes");
          break;
        case Ms.STEALTH_ROCK: //446
          EntryHazards(atk, "EnStealthRock");
          break;
        case Ms.DESTINY_BOND: //194
          KOedCondition(atk, "DestinyBond");
          break;
        case Ms.GRUDGE: //288
          KOedCondition(atk, "Grudge");
          break;
        case Ms.SANDSTORM: //201
          WeatherMove(atk, Weather.Sandstorm, 60);
          break;
        case Ms.RAIN_DANCE: //240
          WeatherMove(atk, Weather.HeavyRain, 62);
          break;
        case Ms.SUNNY_DAY: //241
          WeatherMove(atk, Weather.IntenseSunlight, 61);
          break;
        case Ms.HAIL: //258
          WeatherMove(atk, Weather.Hailstorm, 59);
          break;
        case Ms.HEAL_BELL: //215
          HealBell(atk, "HealBell");
          break;
        case Ms.AROMATHERAPY: //312
          HealBell(atk, "Aromatherapy");
          break;
        case Ms.PRESENT: //217
          if (atk.GetCondition<int>("Present") == 0) atk.Target.Defender.HpRecoverByOneNth(4, true);
          else AttackMove(atk);
          break;
        case Ms.PAIN_SPLIT: //220
          PainSplit(atk);
          break;
        case Ms.BATON_PASS: //226
          BatonPass(atk);
          break;
        case Ms.ENCORE: //227
          Encore(atk);
          break;
        case Ms.MORNING_SUN: //234
        case Ms.SYNTHESIS: //235
        case Ms.MOONLIGHT: //236
          Moonlight(atk);
          break;
        case Ms.PSYCH_UP: //244
          {
            var lv5d = atk.Target.Defender.OnboardPokemon.Lv5D;
            aer.OnboardPokemon.SetLv7D(lv5d.Atk, lv5d.SpAtk, lv5d.Def, lv5d.SpDef, lv5d.Speed, atk.Target.Defender.OnboardPokemon.AccuracyLv, atk.Target.Defender.OnboardPokemon.EvasionLv);
            aer.AddReportPm("PsychUp", atk.Target.Defender);
          }
          break;
        case Ms.FUTURE_SIGHT: //248
        case Ms.DOOM_DESIRE: //353
          FSDD(atk);
          break;
        case Ms.STOCKPILE://255
          StockPile(atk);
          break;
        case Ms.SWALLOW: //256
          {
            var i = aer.OnboardPokemon.GetCondition<int>("Stockpile");
            if (i == 0) atk.FailAll();
            else aer.HpRecoverByOneNth(8 >> i, true);
          }
          break;
        case Ms.MEMENTO: //262
          aer.Faint();
          atk.Target.Defender.ChangeLv7D(aer, true, -2, 0, -2);
          break;
        case Ms.TAUNT: //269
          {
            var der = atk.Target.Defender;
            if (der.OnboardPokemon.AddCondition("Taunt", 3)) der.AddReportPm("EnTaunt");
            else atk.FailAll();
          }
          break;
        case Ms.TRICK: //271
        case Ms.SWITCHEROO: //415
          Trick(atk);
          break;
        case Ms.ROLE_PLAY: //272
          RolePlay(atk);
          break;
        case Ms.WISH: //273
          if (!aer.Tile.AddCondition("Wish", new Condition() { Turn = aer.Controller.TurnNumber + 1, Int = aer.Pokemon.Hp.Origin >> 1 })) atk.FailAll();
          break;
        case Ms.MAGIC_COAT: //277
          aer.OnboardPokemon.SetTurnCondition("MagicCoat");
          aer.AddReportPm("EnMagicCoat");
          break;
        case Ms.RECYCLE: //278
          Recycle(atk);
          break;
        case Ms.SKILL_SWAP: //285
          SkillSwap(atk);
          break;
        case Ms.REFRESH: //287
          if (aer.State == PokemonState.Normal) atk.FailAll();
          else aer.DeAbnormalState();
          break;
        case Ms.CAMOUFLAGE: //293
          Camouflage(atk);
          break;
        case Ms.MUD_SPORT: //300
          Sport(atk, "MudSport");
          break;
        case Ms.WATER_SPORT: //346
          Sport(atk, "WaterSport");
          break;
        case Ms.GRAVITY: //356
          Gravity(atk);
          break;
        case Ms.HEALING_WISH: //361
          HealingWish(atk, "HealingWish");
          break;
        case Ms.LUNAR_DANCE: //461
          HealingWish(atk, "LunarDance");
          break;
        case Ms.ACUPRESSURE: //367
          {
            StatType[] ss = ALL_STATS.Where((s) => atk.Target.Defender.CanChangeLv7D(aer, s, 2, false) != 0).ToArray();
            if (ss.Length == 0) atk.FailAll();
            else atk.Target.Defender.ChangeLv7D(aer, ss[aer.Controller.GetRandomInt(0, ss.Length - 1)], 2, true);
          }
          break;
        case Ms.PSYCHO_SHIFT: //375
          PsychoShift(atk);
          break;
        case Ms.GASTRO_ACID: //380
          GastroAcid(atk);
          break;
        case Ms.POWER_TRICK: //379
          PowerTrick(atk);
          break;
        case Ms.POWER_SWAP: //384
          SwapLv7D(atk, "PowerSwap", POWER_STATS);
          break;
        case Ms.GUARD_SWAP: //385
          SwapLv7D(atk, "GuardSwap", GUARD_STATS);
          break;
        case Ms.HEART_SWAP: //391
          SwapLv7D(atk, "HeartSwap", ALL_STATS);
          break;
        case Ms.WORRY_SEED: //388
          SetAbility(atk, As.INSOMNIA);
          break;
        case Ms.SIMPLE_BEAM: //493
          SetAbility(atk, As.SIMPLE);
          break;
        case Ms.MAGNET_RISE: //393
          if (!aer.OnboardPokemon.HasCondition("Ingrain") && aer.OnboardPokemon.AddCondition("MagnetRise", aer.Controller.TurnNumber + 5)) aer.AddReportPm("EnMagnetRise");
          else atk.FailAll();
          break;
        case Ms.DEFOG: //432
          Defog(atk);
          break;
        case Ms.TRICK_ROOM: //443
          Room(atk, "TrickRoom");
          break;
        case Ms.MAGIC_ROOM: //478
          Room(atk, "MagicRoom");
          break;
        case Ms.WIDE_GUARD: //469
          TeamProtect(atk, "WideGuard");
          break;
        case Ms.QUICK_GUARD: //501
          TeamProtect(atk, "QuickGuard");
          break;
        case Ms.GUARD_SPLIT: //470
          Split5D(atk, "GuardSplit", GUARD_STATS);
          break;
        case Ms.POWER_SPLIT: //471
          Split5D(atk, "PowerSplit", POWER_STATS);
          break;
        case Ms.WONDER_ROOM: //472
          WonderRoom(atk);
          break;
        case Ms.TELEKINESIS: //477
          Telekinesis(atk);
          break;
        case Ms.SOAK: //487
          Soak(atk);
          break;
        case Ms.ENTRAINMENT: //494
          Entrainment(atk);
          break;
        case Ms.SHELL_SMASH: //504
          aer.ChangeLv7D(aer, true, 2, -1, 2, -1, 2);
          break;
        case Ms.REFLECT_TYPE: //513
          ReflectType(atk);
          break;
        case Ms.BESTOW: //516
          Bestow(atk);
          break;
        default:
          if (move.Category == MoveCategory.Status) StatusMove(atk);
          else AttackMove(atk);
          break;
      }
      #endregion
      #region post
      switch (move.Id)
      {
        case Ms.MINIMIZE: //107
          aer.OnboardPokemon.SetCondition("Minimize");
          break;
        case Ms.DEFENSE_CURL: //111
          aer.OnboardPokemon.SetCondition("DefenseCurl");
          break;
        case Ms.CHARGE: //268
          aer.OnboardPokemon.SetCondition("Charge", atk.Controller.TurnNumber + 1);
          aer.AddReportPm("Charge");
          break;
        case Ms.ROOST: //355
          aer.OnboardPokemon.SetTurnCondition("Roost");
          break;
        case Ms.AUTOTOMIZE: //475
          if (!atk.Fail && atk.Attacker.OnboardPokemon.Weight > 0.1)
          {
            aer.OnboardPokemon.Weight -= 100;
            aer.AddReportPm("Autotomize");
          }
          break;
      }
      #endregion
    }

    private static readonly int[] TIMES25 = new int[8] { 2, 2, 2, 3, 3, 3, 4, 5 };
    private static void AttackMove(AtkContext atk)
    {
      var aer = atk.Attacker;
      var move = atk.Move;

      //生成攻击次数
      int times = move.MinTimes == move.MaxTimes || atk.Attacker.Ability == As.SKILL_LINK ? move.MaxTimes : TIMES25[atk.Controller.GetRandomInt(0, 7)];

      int atkTeam = aer.Pokemon.TeamId;
      int hits = 0;
      do
      {
        hits++;
        MoveCalculateDamages.Execute(atk);
        Implement(atk.Targets.Where((d) => d.Defender.Pokemon.TeamId == atkTeam));
        Implement(atk.Targets.Where((d) => d.Defender.Pokemon.TeamId != atkTeam));
      }
      while (hits < times && atk.Target.Defender.Hp != 0 && aer.Hp != 0 && aer.State != PokemonState.FRZ && aer.State != PokemonState.SLP);

      if (move.MinTimes != 0)
      {
        if (atk.Target.EffectRevise > 0) aer.Controller.ReportBuilder.Add("SuperHurt0");
        else if (atk.Target.EffectRevise < 0) aer.Controller.ReportBuilder.Add("WeakHurt0");
        aer.Controller.ReportBuilder.Add("Hits", hits);
      }
      if (atk.Type == BattleType.Fire)
        foreach (DefContext d in atk.Targets)
          if (d.Defender.State == PokemonState.FRZ) d.Defender.DeAbnormalState();

      FinalEffect(atk);
    }
    private static void DamagePercentage(DefContext def, int percentage)
    {
      var aer = def.AtkContext.Attacker;
      var ability = aer.Ability;
      int v = def.Damage * percentage / 100;
      if (percentage > 0)
      {
        if (aer.Item == Is.BIG_ROOT) v = (int)(v * 1.3);
        if (ability != As.MAGIC_GUARD && def.Defender.RaiseAbility(As.LIQUID_OOZE)) aer.EffectHurt(v);
        else aer.HpRecover(v, false);
      }
      else if (ability != As.ROCK_HEAD) aer.EffectHurt(-v, "ReHurt");
    }
    private static void Implement(IEnumerable<DefContext> defs)
    {
      DefContext def = defs.FirstOrDefault();
      if (def == null) return;
      var move = def.AtkContext.Move;
      if (move.Class == MoveInnerClass.OHKO)
      {
        if (!Sp.Conditions.Substitute.OHKO(def))
        {
          def.Defender.Controller.ReportBuilder.Add("OHKO");
          def.Damage = def.Defender.Hp;
          var e = new GameEvents.MoveHurt();
          def.Defender.Controller.ReportBuilder.Add(e);
          def.Defender.MoveHurt(def);
          e.SetHurt(defs, true);
          PassiveEffect(def);
        }
      }
      else
      {
        AtkContext atk = def.AtkContext;
        PokemonProxy a = atk.Attacker;
        bool allSub = true;
        if (!move.Flags.IgnoreSubstitute)
          foreach (DefContext d in defs) allSub &= Sp.Conditions.Substitute.Hurt(d);
        if (!allSub)
        {
          foreach (DefContext d in defs)
            if (d.RemoveCondition("Antiberry")) d.Defender.RaiseItem("Antiberry");
          var e = new GameEvents.MoveHurt();
          a.Controller.ReportBuilder.Add(e);
          foreach (DefContext d in defs)
          {
            d.Defender.MoveHurt(d);
            atk.TotalDamage += d.Damage;
          }
          e.SetHurt(defs, atk.Move.MinTimes == 0);
        }

        if (move.HurtPercentage > 0) DamagePercentage(def, move.HurtPercentage);
        if (move.Class == MoveInnerClass.AttackWithSelfLv7DChange && atk.RandomHappen(move.Lv7DChanges.First().Probability)) a.ChangeLv7D(atk.Attacker, move);

        foreach (DefContext d in defs)
          if (!d.HitSubstitute)
          {
            MoveImplementEffect.Execute(d);
            PassiveEffect(d);
            if (a.Hp != 0 && d.Defender.Hp != 0) MovePostEffect.Execute(d);
          }

        if (a.Hp > 0)
        {
          if (move.HurtPercentage < 0) DamagePercentage(def, move.HurtPercentage);
          else if (move.MaxHpPercentage < 0) //拼命专用
          {
            var change = a.Pokemon.Hp.Origin * move.MaxHpPercentage / 100;
            a.Pokemon.SetHp(a.Hp + (change == 0 ? -1 : change));
            a.OnboardPokemon.SetTurnCondition("Assurance");
            a.Controller.ReportBuilder.Add(new GameEvents.HpChange(a, "ReHurt"));
          }
          a.CheckFaint();
        }
      }// OHKO else
    }
    private static void PassiveEffect(DefContext def)
    {
      var der = def.Defender;
      Attacked.Execute(def);
      def.AtkContext.Attacker.CheckFaint();
      var op = der.OnboardPokemon; //the state before withdraw
      if (der.CheckFaint()) STs.KOed(def, op);
      else if (def.AtkContext.Move.MaxTimes > 1) HpChanged.Execute(der);
    }
    private static void FinalEffect(AtkContext atk)
    {
      if (!(atk.Move.HasProbabilitiedAdditonalEffects() && atk.Attacker.Ability == As.SHEER_FORCE))
      {
        foreach (DefContext d in atk.Targets) As.ColorChange(d);
        Is.AttackPostEffect(atk);
      }
    }

    private static void StatusMove(AtkContext atk)
    {
      bool notAllFail = false;
      var move = atk.Move;
      switch (move.Class)
      {
        case MoveInnerClass.AddState:
          foreach (var d in atk.Targets) notAllFail |= d.Defender.AddState(d);
          if (atk.Move.Attachment.State == AttachedState.PerishSong)
            if (notAllFail) atk.Controller.ReportBuilder.Add("EnPerishSong");
            else atk.FailAll();
          break;
        case MoveInnerClass.Lv7DChange:
          foreach (var d in atk.Targets) notAllFail |= d.Defender.ChangeLv7D(d);
          atk.Fail = !notAllFail;
          break;
        case MoveInnerClass.HpRecover:
          foreach (var d in atk.Targets)
            d.Defender.HpRecover(d.Defender.Pokemon.Hp.Origin * atk.Move.MaxHpPercentage / 100, true);
          break;
        case MoveInnerClass.ConfusionWithLv7DChange:
          atk.Target.Defender.AddState(atk.Target);
          atk.Target.Defender.ChangeLv7D(atk.Target);
          break;
        case MoveInnerClass.ForceToSwitch:
          int aLv = atk.Attacker.Pokemon.Lv, dLv = atk.Target.Defender.Pokemon.Lv;
          if ((aLv < dLv && (aLv + dLv) * atk.Controller.GetRandomInt(0, 255) < dLv >> 2) || !MoveE.ForceSwitch(atk.Target)) atk.FailAll();
          break;
      }
    }

    private static void Curse(AtkContext atk)
    {
      var aer = atk.Attacker;
      if (aer.OnboardPokemon.HasType(BattleType.Ghost))
      {
        if (atk.Target.Defender.OnboardPokemon.AddCondition("Curse"))
        {
          aer.Pokemon.SetHp(aer.Hp - (aer.Pokemon.Hp.Origin >> 1));
          atk.Controller.ReportBuilder.Add(new GameEvents.HpChange(aer, "EnCurse", atk.Target.Defender.Id));
          aer.CheckFaint();
        }
        else atk.FailAll();
      }
      else aer.ChangeLv7D(aer, true, 1, 1, 0, 0, -1);
    }
    private static void StockPile(AtkContext atk)
    {
      var aer = atk.Attacker;
      int i = aer.OnboardPokemon.GetCondition<int>("Stockpile") + 1;
      aer.OnboardPokemon.SetCondition("Stockpile", i);
      aer.AddReportPm("EnStockpile", i);
      aer.ChangeLv7D(aer, false, 0, 1, 0, 1);
    }
    private static void CantSelectWithdraw(AtkContext atk)
    {
      if (atk.Target.Defender.OnboardPokemon.AddCondition("CantSelectWithdraw", atk.Attacker)) atk.Target.Defender.AddReportPm("CantSelectWithdraw");
      else atk.FailAll();
    }
    private static void AddCondition(AtkContext atk, string condition)
    {
      if (atk.Target.Defender.OnboardPokemon.AddCondition(condition)) atk.Target.Defender.AddReportPm("En" + condition);
      else atk.FailAll();
    }
    private static void Telekinesis(AtkContext atk)
    {
      var der = atk.Target.Defender.OnboardPokemon;
      if (der.Form.Type.Number == 50 || der.Form.Type.Number == 51) atk.Target.Defender.AddReportPm("NoEffect");
      else if (der.AddCondition("Telekinesis", atk.Controller.TurnNumber + 2)) atk.Target.Defender.AddReportPm("EnTelekinesis");
      else atk.FailAll();
    }
    private static void WeatherMove(AtkContext atk, Weather weather, int item)
    {
      if (atk.Controller.Board.Weather == weather) atk.FailAll();
      else
      {
        atk.Controller.Weather = weather;
        atk.Controller.Board.SetCondition("Weather", atk.Controller.TurnNumber + atk.Attacker.Item == item ? 7 : 4);
      }
    }
    private static void Sport(AtkContext atk, string condition)
    {
      if (atk.Attacker.OnboardPokemon.AddCondition(condition)) atk.Controller.ReportBuilder.Add(condition);
      else atk.FailAll();
    }
    private static void Room(AtkContext atk, string condition)
    {
      var c = atk.Controller;
      if (c.Board.AddCondition(condition, c.TurnNumber + 4)) c.ReportBuilder.Add("En" + condition, atk.Attacker);
      else
      {
        c.Board.RemoveCondition(condition);
        c.ReportBuilder.Add("De" + condition);
      }
    }
    private static void AddTeamCondition(AtkContext atk, string condition, int turn = 5)
    {
      var team = atk.Attacker.Pokemon.TeamId;
      if (atk.Controller.Board[team].AddCondition(condition, atk.Controller.TurnNumber + turn - 1)) atk.Controller.ReportBuilder.Add("En" + condition, team);
      else atk.FailAll();
    }
    private static void EntryHazards(AtkContext atk, string log)
    {
      var team = 1 - atk.Attacker.Pokemon.TeamId;
      if (atk.Controller.Board[team].EnEntryHazards(atk.Move)) atk.Controller.ReportBuilder.Add(log, team);
      else atk.FailAll();
    }
    private static void LockOn(AtkContext atk)
    {
     var der = atk.Target.Defender;
     var c = new Condition() { By = atk.Attacker, Turn = atk.Controller.TurnNumber + 1 };
     if (der.OnboardPokemon.AddCondition("NoGuard", c)) atk.Attacker.AddReportPm("LockOn", der);
     else atk.FailAll();
    }
    private static void BellyDrum(AtkContext atk)
    {
      var aer = atk.Attacker;
      if (aer.OnboardPokemon.Lv5D.Atk != 6 && aer.Hp > aer.Pokemon.Hp.Origin >> 1)
      {
        aer.Pokemon.SetHp(aer.Hp - (aer.Pokemon.Hp.Origin >> 1));
        aer.OnboardPokemon.ChangeLv7D(StatType.Atk, 12);
        aer.Controller.ReportBuilder.Add(new GameEvents.HpChange(aer, "BellyDrum", 0, 0));
      }
      else atk.FailAll();
    }
    private static void KOedCondition(AtkContext atk, string condition)
    {
      atk.Attacker.OnboardPokemon.SetCondition(condition);
      atk.Attacker.AddReportPm("En" + condition);
    }
    private static void Camouflage(AtkContext atk)
    {
      var aer = atk.Attacker;
      var t = aer.Controller.GameSettings.Terrain.GetBattleType();
      if (aer.OnboardPokemon.Type1 == t && aer.OnboardPokemon.Type2 == BattleType.Invalid) atk.FailAll();
      else
      {
        aer.OnboardPokemon.Type1 = t;
        aer.OnboardPokemon.Type2 = BattleType.Invalid;
        aer.AddReportPm("TypeChange", t);
      }
    }
    private static void HealingWish(AtkContext atk, string condition)
    {
      var aer = atk.Attacker;
      if (aer.Pokemon.Owner.PmsAlive > aer.Controller.GameSettings.Mode.OnboardPokemonsPerPlayer())
      {
        aer.Tile.SetTurnCondition(condition);
        aer.Faint();
      }
      else atk.FailAll();
    }
    private static void PsychoShift(AtkContext atk)
    {
      var aer = atk.Attacker;
      if (aer.State == PokemonState.Normal) atk.FailAll();
      else
      {
        AttachedState a;
        switch(aer.State)
        {
          case PokemonState.BadlyPSN:
            atk.Target.Defender.AddState(aer, AttachedState.PSN, true, 15);
            return;
          case PokemonState.BRN:
            a = AttachedState.BRN;
            break;
          case PokemonState.FRZ:
            a = AttachedState.FRZ;
            break;
          case PokemonState.PAR:
            a = AttachedState.PAR;
            break;
          case PokemonState.PSN:
            a = AttachedState.PSN;
            break;
          case PokemonState.SLP:
            a = AttachedState.SLP;
            break;
          default:
            return;
        }
        atk.Target.Defender.AddState(aer, a, true);
        atk.Attacker.DeAbnormalState();
      }
    }
    private static void PowerTrick(AtkContext atk)
    {
      var s = atk.Attacker.OnboardPokemon.FiveD;
      var a = s.Atk;
      s.Atk = s.Def;
      s.Def = a;
      atk.Attacker.AddReportPm("PowerTrick");
    }
    private static void GastroAcid(AtkContext atk)
    {
      var der = atk.Target.Defender;
      if (der.Ability != As.MULTITYPE && der.OnboardPokemon.AddCondition("GastroAcid"))
      {
        der.AddReportPm("EnGastroAcid");
        AbilityDetach.Execute(der);
      }
      else atk.FailAll();
    }
    private static readonly IEnumerable<StatType> POWER_STATS = new StatType[] { StatType.Atk, StatType.SpAtk };
    private static readonly IEnumerable<StatType> GUARD_STATS = new StatType[] { StatType.Def, StatType.SpDef };
    private static readonly IEnumerable<StatType> ALL_STATS = new StatType[] { StatType.Atk, StatType.Def, StatType.SpAtk, StatType.SpDef, StatType.Speed, StatType.Accuracy, StatType.Evasion };
    private static void SwapLv7D(AtkContext atk, string log, IEnumerable<StatType> stats)
    {
      var aer = atk.Attacker;
      var der = atk.Target.Defender;
      foreach(var s in stats)
      {
        var t = der.OnboardPokemon.GetLv7D(s);
        der.OnboardPokemon.SetLv7D(s, aer.OnboardPokemon.GetLv7D(s));
        aer.OnboardPokemon.SetLv7D(s, t);
      }
      aer.Controller.ReportBuilder.Add(log, aer, der);
    }
    private static void Split5D(AtkContext atk, string log, IEnumerable<StatType> stats)
    {
      var aer = atk.Attacker;
      var der = atk.Target.Defender;
      foreach (var s in stats)
      {
        var v = (aer.OnboardPokemon.FiveD.GetStat(s) + der.OnboardPokemon.FiveD.GetStat(s)) >> 1;
        aer.OnboardPokemon.FiveD.SetStat(s, v);
        der.OnboardPokemon.FiveD.SetStat(s, v);
      }
      aer.Controller.ReportBuilder.Add(log, aer, der);
    }
    private static void Defog(AtkContext atk)
    {
      var r = atk.Controller.ReportBuilder;
      atk.Target.Defender.ChangeLv7D(atk.Attacker, StatType.Evasion, -1, true);
      var t = atk.Target.Defender.Pokemon.TeamId;
      var f = atk.Controller.Board[t];
      f.DeEntryHazards(r);
      if (f.RemoveCondition("Reflect")) r.Add("DeReflect", t);
      if (f.RemoveCondition("LightScreen")) r.Add("DeLightScreen", t);
      if (f.RemoveCondition("Mist")) r.Add("DeMist", t);
      if (f.RemoveCondition("Safeguard")) r.Add("DeSafeguard", t);
    }
    private static void Soak(AtkContext atk)
    {
      var der = atk.Target.Defender;
      if (Is.PlatedArceus(der.Pokemon) || (der.OnboardPokemon.Type1 == BattleType.Water && der.OnboardPokemon.Type2 == BattleType.Invalid)) atk.FailAll();
      else
      {
        der.OnboardPokemon.Type1 = BattleType.Water;
        der.OnboardPokemon.Type2 = BattleType.Invalid;
        der.AddReportPm("TypeChange", BattleType.Water);
      }
    }
    private static void ReflectType(AtkContext atk)
    {
      var aer = atk.Attacker.OnboardPokemon;
      var t1 = atk.Target.Defender.OnboardPokemon.Type1;
      var t2 = atk.Target.Defender.OnboardPokemon.Type2;
      if (aer.Type1 == t1 && aer.Type2 == t2) atk.FailAll();
      else
      {
        aer.Type1 = t1;
        aer.Type2 = t2;
        atk.Attacker.AddReportPm("ReflectType", atk.Target.Defender);
      }
    }
    private static void Bestow(AtkContext atk)
    {
      var aer = atk.Attacker;
      if (atk.Target.Defender.Pokemon.Item == null)
      {
        var i = aer.Pokemon.Item.Id;
        aer.Pokemon.Item = null;
        atk.Target.Defender.SetItem(i, "Bestow", aer);
      }
      else atk.FailAll();
    }
    private static void SelfProtect(AtkContext atk, string condition)
    {
      atk.Attacker.OnboardPokemon.SetTurnCondition(condition);
      atk.Attacker.AddReportPm("En" + condition);
    }
    private static void TeamProtect(AtkContext atk, string condition)
    {
      var team = atk.Attacker.Pokemon.TeamId;
      atk.Controller.Board[team].SetTurnCondition(condition);
      atk.Controller.ReportBuilder.Add("En" + condition, team);
    }
    private static void Moonlight(AtkContext atk)
    {
      var aer = atk.Attacker;
      if (aer.CanHpRecover(true))
      {
        var hp = aer.Pokemon.Hp.Origin;
        var w = aer.Controller.Weather;
        if (w == Weather.IntenseSunlight) hp = hp * 2 / 3;
        else if (w == Weather.Normal) hp >>= 1;
        else hp >>= 2;
        aer.HpRecover(hp);
      }
      else atk.Fail = true;
    }
    private static void Substitute(AtkContext atk)
    {
      var aer = atk.Attacker;
      if (atk.Attacker.Hp > 1 && atk.Attacker.Hp > atk.Attacker.Pokemon.Hp.Origin >> 2)
      {
        if (aer.OnboardPokemon.HasCondition("Substitute")) aer.AddReportPm("HasSubstitute");
        else
        {
          int hp = aer.Pokemon.Hp.Origin >> 2;
          aer.OnboardPokemon.SetCondition("Substitute", hp);
          aer.Pokemon.SetHp(aer.Hp - hp);
          aer.Controller.ReportBuilder.Add(GameEvents.Substitute.EnSubstitute(aer));
        }
      }
      else atk.FailAll();
    }
    private static void Trick(AtkContext atk)
    {
      var aer = atk.Attacker;
      var der = atk.Target.Defender;
      var ai = aer.Pokemon.Item;
      var di = der.Pokemon.Item;
      if ((di == null && ai == null) || Is.CantLostItem(aer.Pokemon) || Is.CantLostItem(der.Pokemon)) atk.FailAll();
      else
      {
        aer.AddReportPm("Trick");
        if (ai != null) aer.RemoveItem();
        if (di != null) der.RemoveItem();
        if (ai != null) der.SetItem(ai.Id, "GetItem", aer, false);
        if (di != null) aer.SetItem(di.Id, "GetItem", der, false);
      }
    }
    private static void WonderRoom(AtkContext atk)
    {
      var c = atk.Controller;
      foreach (var pm in c.OnboardPokemons)
      {
        var stats = pm.OnboardPokemon.FiveD;
        var d = stats.Def;
        stats.Def = stats.SpDef;
        stats.SpDef = d;
      }
      if (c.Board.AddCondition("WonderRoom", c.TurnNumber + 4)) c.ReportBuilder.Add("EnWonderRoom");
      else
      {
        c.Board.RemoveCondition("WonderRoom");
        c.ReportBuilder.Add("DeWonderRoom");
      }
    }
    private static void Rest(AtkContext atk)
    {
      var aer = atk.Attacker;
      if (aer.Hp == aer.Pokemon.Hp.Origin) atk.FailAll();
      else if (CanAddState.Execute(aer, aer, AttachedState.SLP, true))
      {
        aer.Controller.ReportBuilder.Add(new RestGameEvent(aer));
        aer.Pokemon.SetHp(aer.Pokemon.Hp.Origin);
        aer.Pokemon.State = PokemonState.SLP;
        aer.OnboardPokemon.SetCondition("SLP", 3);
        aer.Tile.Field.SetCondition("Rest" + aer.Id);
        StateAdded.Execute(aer);
      }
    }
    private static void Recycle(AtkContext atk)
    {
      var aer = atk.Attacker;
      if (aer.Pokemon.Item == null)
      {
        var item = aer.Tile.Field.GetCondition<Item>("UsedItem" + aer.Id);
        if (item == null) atk.FailAll();
        else
        {
          aer.SetItem(item.Id, "Recycle");
          aer.Tile.Field.RemoveCondition("UsedItem" + aer.Id);
        }
      }
      else atk.FailAll();
    }
    private static void PainSplit(AtkContext atk)
    {
      int hp = (atk.Attacker.Hp + atk.Target.Defender.Hp) >> 1;
      atk.Attacker.Pokemon.SetHp(hp);
      atk.Target.Defender.Pokemon.SetHp(hp);
      atk.Controller.ReportBuilder.Add(new PainSplitEvent(atk));
    }
    private static void HealBell(AtkContext atk, string log)
    {
      var aer = atk.Attacker;
      aer.Controller.ReportBuilder.Add(new HealBellEvent(aer, log));
      foreach (var pm in aer.Tile.Field.Pokemons)
        if (pm.State != PokemonState.Normal) pm.Pokemon.State = PokemonState.Normal;
      foreach (var pm in aer.Pokemon.Owner.Pokemons)
        if (pm.Hp.Value > 0 && pm.State != PokemonState.Normal) pm.State = PokemonState.Normal;
    }
    private static void Gravity(AtkContext atk)
    {
      var c = atk.Controller;
      if (c.Board.AddCondition("Gravity", c.TurnNumber + 4))
      {
        c.ReportBuilder.Add("EnGravity");
        foreach (var pm in c.Board.Pokemons)
        {
          if (pm.OnboardPokemon.CoordY == CoordY.Air)
          {
            pm.OnboardPokemon.CoordY = CoordY.Plate;
            pm.CancelMove();
            pm.OnboardPokemon.RemoveCondition("SkyDrop");
            c.ReportBuilder.Add(GameEvents.PositionChange.Reset("Gravity", pm));
          }
          else if (!HasEffect.IsGroundAffectable(pm, true, false, false)) pm.AddReportPm("Gravity");
        }
      }
      else atk.FailAll();
    }
    private static void Conversion(AtkContext atk)
    {
      var aer = atk.Attacker;
      var type1 = aer.OnboardPokemon.Type1;
      var type2 = aer.OnboardPokemon.Type2;
      var ms = (from m in aer.Moves
                where !(m.Type.Id == Ms.CONVERSION || m.Type.Type == type1 || m.Type.Type == type2)
                select m.Type.Type).ToArray();
      if (ms.Length == 0) atk.FailAll();
      else
      {
        var type = ms[aer.Controller.GetRandomInt(0, ms.Length - 1)];
        aer.OnboardPokemon.Type1 = type;
        aer.OnboardPokemon.Type2 = Data.BattleType.Invalid;
        aer.AddReportPm("TypeChange", type);
      }
    }
    private static void BatonPass(AtkContext atk)
    {
      var aer = atk.Attacker;
      var t = aer.Tile;
      var o = aer.OnboardPokemon;
      if (aer.Controller.Withdraw(aer, "SelfWithdraw", false))
      {
        t.SetCondition("BatonPass", o);
        aer.Controller.PauseForSendoutInput(t);
      }
      else atk.FailAll();
    }
    private static void SkillSwap(AtkContext atk)
    {
      int a = atk.Attacker.OnboardPokemon.Ability;
      int d = atk.Target.Defender.OnboardPokemon.Ability;
      if
        (
        a == d ||
        a == As.WONDER_GUARD || a == As.ILLUSION || a == As.MULTITYPE ||
        d == As.WONDER_GUARD || d == As.ILLUSION || d == As.MULTITYPE
        )
        atk.FailAll();
      else
      {
        var aer = atk.Attacker;
        var der = atk.Target.Defender;
        aer.Controller.ReportBuilder.Add("SkillSwap");
        if (aer.Pokemon.TeamId != der.Pokemon.TeamId)
        {
          aer.AddReportPm("SkillSwapDetail", d);
          der.AddReportPm("SkillSwapDetail", a);
        }
        AbilityDetach.Execute(aer);
        AbilityDetach.Execute(der);
        aer.OnboardPokemon.Ability = d;
        der.OnboardPokemon.Ability = a;
        AbilityAttach.Execute(aer);
        AbilityAttach.Execute(der);
      }
    }
    private static void SetAbility(AtkContext atk, int ability)
    {
      var former = atk.Target.Defender.Ability;
      if (former == As.TRUANT || former == As.MULTITYPE) atk.FailAll();
      else atk.Target.Defender.ChangeAbility(ability, "SetAbility");
    }
    private static void RolePlay(AtkContext atk)
    {
      int a = atk.Attacker.OnboardPokemon.Ability;
      int d = atk.Target.Defender.OnboardPokemon.Ability;
      if
        (
        a == d ||
        a == As.ILLUSION || a == As.MULTITYPE ||
        d == As.WONDER_GUARD || d == As.FORECAST || d == As.MULTITYPE || d == As.ILLUSION || d == As.ZEN_MODE
        )
        atk.FailAll();
      else atk.Attacker.ChangeAbility(d, "SetAbility");
    }
    private static void Entrainment(AtkContext atk)
    {
      int a = atk.Attacker.OnboardPokemon.Ability;
      int d = atk.Target.Defender.OnboardPokemon.Ability;
      if
        (
        a == d ||
        a == As.FORECAST || a == As.ILLUSION || a == As.ZEN_MODE || a == As.FLOWER_GIFT ||
        d == As.TRUANT || d == As.MULTITYPE
        )
        atk.FailAll();
      else atk.Target.Defender.ChangeAbility(a, "SetAbility");
    }
    private static void Conversion2(AtkContext atk)
    {
      if (atk.Target.Defender.AtkContext == null) atk.FailAll();
      else
      {
        BattleType a = atk.Target.Defender.AtkContext.Type;
        if (a == BattleType.Invalid) a = BattleType.Normal;
        BattleType type1 = atk.Attacker.OnboardPokemon.Type1;
        BattleType type2 = atk.Attacker.OnboardPokemon.Type2;
        var types = (from t in (BattleType[])Enum.GetValues(typeof(BattleType))
                     where !(t == type1 || t == type2) && (a.EffectRevise(t) < 0 || a.NoEffect(t)) //自动排除Invalid
                     select t).ToArray();
        var n = types.Length;
        if (n != 0)
        {
          var type = types[atk.Controller.GetRandomInt(0, n - 1)];
          atk.Attacker.OnboardPokemon.Type1 = type;
          atk.Attacker.OnboardPokemon.Type2 = BattleType.Invalid;
          atk.Attacker.AddReportPm("TypeChange", type);
        }
      }
    }
    private static void Encore(AtkContext atk)
    {
      var der = atk.Target.Defender;
      var last = der.LastMove;
      if (last != null && last.Id != 102 && last.Id != 144 && last.Id != 227)
        foreach (var m in der.Moves)
          if (m.Type == last)
          {
            var c = new Condition() { Turn = 3, Move = last };
            if (m.PP != 0 && der.OnboardPokemon.AddCondition("Encore", c))
            {
              der.AddReportPm("EnEncore");
              return;
            }
            break;
          }
      atk.FailAll();
    }
    private static void Mimic(AtkContext atk)
    {
      var aer = atk.Attacker;
      var last = aer.LastMove;
      if (last == null || last == atk.Move) atk.FailAll();
      else
      {
        var move = last;
        aer.ChangeMove(atk.Move, move);
        aer.Controller.ReportBuilder.Add(new MimicEvent(aer, move));
      }
    }
    private static void Spite(AtkContext atk)
    {
      var der = atk.Target.Defender;
      var move = der.LastMove;
      foreach (var m in der.Moves)
        if (m.Type == move)
        {
          if (m.PP != 0)
          {
            var fp = m.PP;
            m.PP -= 4;
            atk.Controller.ReportBuilder.Add(new GameEvents.PPChange("Spite", m, fp));
            return;
          }
          break;
        }
      atk.FailAll();
    }

    private static void FSDD(AtkContext atk)
    {
      if (atk.HasCondition("FSDD")) AttackMove(atk);
      else
      {
        var tile = MoveE.GetRangeTiles(atk, atk.Move.Range, atk.Attacker.SelectedTarget);
        if (tile.First().HasCondition("FSDD")) atk.FailAll();
        else
        {
          atk.Attacker.AddReportPm("EnFSDD" + atk.Move.Id);
          var c = new Condition();
          c.Turn = atk.Controller.TurnNumber + 2;
          c.Atk = new AtkContext(atk.Attacker) { IgnoreSwitchItem = true };
          c.Atk.SetCondition("FSDD", atk.Move);
          tile.First().AddCondition("FSDD", c);
        }
      }
    }
    private static void BeatUp(AtkContext atk)
    {
      PokemonProxy aer = atk.Attacker;

      int hits = 0;
      foreach (var pm in aer.Pokemon.Owner.Pokemons)
        if (pm == aer.Pokemon || pm.State == PokemonState.Normal)
        {
          hits++;
          atk.SetCondition("BeatUpAtk", pm == aer.Pokemon ? aer.OnboardPokemon.Form.Data.Base.Atk : pm.Form.Data.Base.Atk);
          MoveCalculateDamages.Execute(atk);
          Implement(atk.Targets);
          if (atk.Target.Defender.Hp == 0 || aer.Hp == 0 || aer.State == PokemonState.FRZ || aer.State == PokemonState.SLP) break;
        }
      atk.Controller.ReportBuilder.Add("Hits", hits);

      FinalEffect(atk);
    }
    private static void TripleKick(AtkContext atk)
    {
      PokemonProxy aer = atk.Attacker;

      //生成攻击次数
      int times = 3;

      int atkTeam = aer.Pokemon.TeamId;
      int hits = 0;
      do
      {
        hits++;
        atk.Target.BasePower += 10;
        MoveCalculateDamages.Execute(atk);
        Implement(atk.Targets);
        MoveE.FilterDefContext(atk);
      }
      while (atk.Target != null && hits < times && atk.Target.Defender.Hp != 0 && aer.Hp != 0 && aer.State != PokemonState.FRZ && aer.State != PokemonState.SLP);

      atk.Controller.ReportBuilder.Add("Hits", hits);

      FinalEffect(atk);
    }
  }
}
