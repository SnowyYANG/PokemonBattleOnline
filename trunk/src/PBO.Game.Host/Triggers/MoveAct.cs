using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonBattleOnline.Game;

namespace PokemonBattleOnline.Game.Host.Triggers
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
          aer.ShowLogPm("Fling", aer.Pokemon.Item);
          break;
        case Ms.UPROAR:
          if (atk.GetCondition("MultiTurn").Turn == 3)
          {
            atk.Attacker.ShowLogPm("EnUproar");
            foreach (var p in atk.Controller.Board.Pokemons)
              if (p.State == PokemonState.SLP) p.DeAbnormalState("UproarDeSLP");
          }
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
          aer.Controller.ReportBuilder.ShowLog("Haze");
          break;
        case Ms.BIDE: //117
          {
            var turn = atk.GetCondition("MultiTurn").Turn;
            if (turn == 1)
            {
              atk.Attacker.ShowLogPm("DeBide");
              StatusMove(atk);
            }
            else if (turn == 2) atk.Attacker.ShowLogPm("Bide");
          }
          break;
        case Ms.TRANSFORM: //144
          if (aer.CanTransform(atk.Target.Defender)) aer.Transform(atk.Target.Defender);
          else atk.FailAll();
          break;
        case Ms.SPLASH: //150
          aer.Controller.ReportBuilder.ShowLog("Splash");
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
          SelfProtect(atk, "Protect", "EnProtect");
          break;
        case Ms.ENDURE: //203
          SelfProtect(atk, "Endure", "EnEndure");
          break;
        case Ms.BELLY_DRUM: //187
          BellyDrum(atk);
          break;
        case Ms.SPIKES: //191
          EntryHazards(atk, move, "EnSpikes");
          break;
        case Ms.TOXIC_SPIKES: //390
          EntryHazards(atk, move, "EnToxicSpikes");
          break;
        case Ms.STEALTH_ROCK: //446
          EntryHazards(atk, move, "EnStealthRock");
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
            aer.ShowLogPm("PsychUp", atk.Target.Defender.Id);
          }
          break;
        case Ms.FUTURE_SIGHT: //248
        case Ms.DOOM_DESIRE: //353
          FSDD(atk);
          break;
        case Ms.BEAT_UP: //251
          BeatUp(atk);
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
          atk.Target.Defender.ChangeLv7D(aer, true, false, -2, 0, -2);
          break;
        case Ms.TAUNT: //269
          {
            var der = atk.Target.Defender;
            if (der.OnboardPokemon.AddCondition("Taunt", 3)) der.ShowLogPm("EnTaunt");
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
          if (!aer.Tile.AddCondition("Wish", new Condition() { Turn = aer.Controller.TurnNumber + 1, Int = aer.Pokemon.MaxHp >> 1 })) atk.FailAll();
          break;
        case Ms.MAGIC_COAT: //277
          aer.OnboardPokemon.SetTurnCondition("MagicCoat");
          aer.ShowLogPm("EnMagicCoat");
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
          if (!aer.OnboardPokemon.HasCondition("Ingrain") && aer.OnboardPokemon.AddCondition("MagnetRise", aer.Controller.TurnNumber + 5)) aer.ShowLogPm("EnMagnetRise");
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
          aer.ChangeLv7D(aer, true, false, 2, -1, 2, -1, 2);
          break;
        case Ms.REFLECT_TYPE: //513
          ReflectType(atk);
          break;
        case Ms.BESTOW: //516
          Bestow(atk);
          break;
        case Ms.HEAL_PULSE:
          HealPulse(atk);
          break;
        case Ms.MAT_BLOCK:
          if (aer.Field.AddCondition("MatBlock")) aer.ShowLogPm("EnMatBlock");
          else atk.FailAll();
          break;
        case Ms.STICKY_WEB:
          {
            var team = 1 - aer.Pokemon.TeamId;
            if (aer.Controller.Board[team].AddCondition("StickyWeb")) aer.Controller.ReportBuilder.ShowLog("EnStickyWeb", team);
            else atk.FailAll();
          }
          break;
        case Ms.TRICKORTREAT:
          AddType(atk, BattleType.Ghost);
          break;
        case Ms.FORESTS_CURSE:
          AddType(atk, BattleType.Grass);
          break;
        case Ms.GRASSY_TERRAIN:
          Terrain(atk, "GrassyTerrain");
          break;
        case Ms.MISTY_TERRAIN:
          Terrain(atk, "MistyTerrain");
          break;
        case Ms.ELECTRIC_TERRAIN:
          Terrain(atk, "ElectricTerrain");
          break;
        case Ms.FAIRY_LOCK:
          aer.Controller.Board.SetCondition("FairyLock", aer.Controller.TurnNumber + 1);
          aer.Controller.ReportBuilder.ShowLog("EnFairyLock");
          break;
        case Ms.TOPSYTURVY:
          TorsyTurvy(atk);
          break;
        case Ms.SPIKY_SHIELD:
          SelfProtect(atk, "SpikyShield", "EnProtect");
          break;
        case Ms.KINGS_SHIELD:
          SelfProtect(atk, "KingsShield", "EnProtect");
          break;
        case Ms.POWDER:
          Powder(atk);
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
          aer.ShowLogPm("Charge");
          break;
        case Ms.ROOST: //355
          aer.OnboardPokemon.SetTurnCondition("Roost");
          break;
        case Ms.AUTOTOMIZE: //475
          if (!atk.Fail && atk.Attacker.OnboardPokemon.Weight > 0.1)
          {
            aer.OnboardPokemon.Weight -= 100;
            aer.ShowLogPm("Autotomize");
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
      var aa = aer.Ability;

      //生成攻击次数
      if (move.MaxTimes == 0)
        if (!atk.MultiTargets && aa == As.PARENTAL_BOND)
        {
          atk.Hits = 2;
          atk.AddCondition("ParentalBond");
        }
        else atk.Hits = 1;
      else if (move.MinTimes == move.MaxTimes || aa == As.SKILL_LINK) atk.Hits = move.MaxTimes;
      else atk.Hits = TIMES25[atk.Controller.GetRandomInt(0, 7)];

      int atkTeam = aer.Pokemon.TeamId;
      atk.Hit = 0;
      do
      {
        atk.Hit++;
        CalculateDamages.Execute(atk);
        if (atk.Target.Damage == 0 && atk.Hit == 2 && atk.HasCondition("ParentalBond")) break;
        Implement(atk.Targets.Where((d) => d.Defender.Pokemon.TeamId == atkTeam));
        Implement(atk.Targets.Where((d) => d.Defender.Pokemon.TeamId != atkTeam));
      }
      while (atk.Hit < atk.Hits && atk.Target.Defender.Hp != 0 && aer.Hp != 0 && aer.State != PokemonState.FRZ && aer.State != PokemonState.SLP);

      if (atk.Hits != 1)
      {
        if (atk.Target.EffectRevise > 0) aer.Controller.ReportBuilder.ShowLog("SuperHurt0");
        else if (atk.Target.EffectRevise < 0) aer.Controller.ReportBuilder.ShowLog("WeakHurt0");
        aer.Controller.ReportBuilder.ShowLog("Hits", atk.Hit);
      }
      if (atk.Type == BattleType.Fire)
        foreach (DefContext d in atk.Targets)
          if (d.Defender.State == PokemonState.FRZ) d.Defender.DeAbnormalState();

      FinalEffect(atk);
    }
    private static bool SetHurt(GameEvents.MoveHurt e, IEnumerable<DefContext> defs, bool effects) //auto delay
    {
      List<int> pms = new List<int>();
      List<int> damages = new List<int>();
      List<int> sh = new List<int>();
      List<int> wh = new List<int>();
      List<int> ct = new List<int>();
      foreach (DefContext d in defs)
        if (!d.HitSubstitute)
        {
          int id = d.Defender.Id;
          pms.Add(id);
          damages.Add(d.Damage);
          if (d.EffectRevise > 0) sh.Add(id);
          else if (d.EffectRevise < 0) wh.Add(id);
          if (d.IsCt) ct.Add(id);
        }
      if (pms.Any()) e.Pms = pms.ToArray();
      else return false;
      if (damages.Any()) e.Damages = damages.ToArray();
      if (effects)
      {
        if (sh.Any()) e.SH = sh.ToArray();
        if (wh.Any()) e.WH = wh.ToArray();
      }
      if (ct.Any()) e.CT = ct.ToArray();
      return true;
    }
    private static void Implement(IEnumerable<DefContext> defs)
    {
      DefContext def = defs.FirstOrDefault();

      if (def == null) return;
      var atk = def.AtkContext;
      var aer = atk.Attacker;
      var move = def.AtkContext.Move;

      if (move.Class == MoveInnerClass.OHKO)
      {
        if (!SubstituteTriggers.OHKO(def))
        {
          def.Defender.Controller.ReportBuilder.ShowLog("OHKO");
          def.Damage = def.Defender.Hp;
          var e = new GameEvents.MoveHurt();
          def.Defender.Controller.ReportBuilder.Add(e);
          def.MoveHurt();
          SetHurt(e, defs, true);
          PassiveEffect(def);
        }
      }
      else
      {
        bool nonSub = false;
        if (move.Flags.IgnoreSubstitute || aer.Ability == As.INFILTRATOR)
          foreach (DefContext d in defs) nonSub |= !SubstituteTriggers.Hurt(d);
        if (!nonSub)
        {
          foreach (DefContext d in defs)
            if (d.RemoveCondition("Antiberry"))
            {
              d.Defender.ShowLogPm("Antiberry", d.Defender.Pokemon.Item);
              d.Defender.ConsumeItem();
            }
          var e = new GameEvents.MoveHurt();
          aer.Controller.ReportBuilder.Add(e);
          foreach (DefContext d in defs)
          {
            d.MoveHurt();
            atk.TotalDamage += d.Damage;
          }
          SetHurt(e, defs, atk.Hits == 1);
        }

        if (move.HurtPercentage > 0)
        {
          int v = def.Damage * move.HurtPercentage / 100;
          if (aer.Item == Is.BIG_ROOT) v *= (Modifier)0x14cc;
          if (aer.Ability != As.MAGIC_GUARD && def.Defender.RaiseAbility(As.LIQUID_OOZE)) aer.EffectHurt(v);
          else aer.HpRecover(v, false);
        }
        if (move.Class == MoveInnerClass.AttackWithSelfLv7DChange && atk.RandomHappen(move.Lv7DChanges.First().Probability)) aer.ChangeLv7D(atk.Attacker, move);

        foreach (DefContext d in defs)
          if (!d.HitSubstitute)
          {
            MoveImplementEffect.Execute(d);
            PassiveEffect(d);
          }

        if (aer.Hp != 0) MovePostEffect.Execute(def);
        aer.CheckFaint();
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
        foreach (DefContext d in atk.Targets) ATs.ColorChange(d);
        ITs.AttackPostEffect(atk);
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
            if (notAllFail) atk.Controller.ReportBuilder.ShowLog("EnPerishSong");
            else atk.FailAll();
          break;
        case MoveInnerClass.Lv7DChange:
          foreach (var d in atk.Targets) notAllFail |= d.Defender.ChangeLv7D(d);
          atk.Fail = !notAllFail;
          break;
        case MoveInnerClass.HpRecover:
          foreach (var d in atk.Targets)
            d.Defender.HpRecover(d.Defender.Pokemon.MaxHp * atk.Move.MaxHpPercentage / 100, true);
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

    #region Gen5
    private static void Curse(AtkContext atk)
    {
      var aer = atk.Attacker;
      if (aer.OnboardPokemon.HasType(BattleType.Ghost))
      {
        if (atk.Target.Defender.OnboardPokemon.AddCondition("Curse"))
        {
          aer.ShowLogPm("m_EnCurse", atk.Target.Defender.Id);
          aer.Hp -= (aer.Pokemon.MaxHp >> 1);
          aer.CheckFaint();
        }
        else atk.FailAll();
      }
      else aer.ChangeLv7D(aer, true, false, 1, 1, 0, 0, -1);
    }
    private static void StockPile(AtkContext atk)
    {
      var aer = atk.Attacker;
      int i = aer.OnboardPokemon.GetCondition<int>("Stockpile") + 1;
      aer.OnboardPokemon.SetCondition("Stockpile", i);
      aer.ShowLogPm("EnStockpile", i);
      aer.ChangeLv7D(aer, false, false, 0, 1, 0, 1);
    }
    private static void CantSelectWithdraw(AtkContext atk)
    {
      if (atk.Target.Defender.OnboardPokemon.AddCondition("CantSelectWithdraw", atk.Attacker)) atk.Target.Defender.ShowLogPm("EnCantSelectWithdraw");
      else atk.FailAll();
    }
    private static void AddCondition(AtkContext atk, string condition)
    {
      if (atk.Target.Defender.OnboardPokemon.AddCondition(condition)) atk.Target.Defender.ShowLogPm("En" + condition);
      else atk.FailAll();
    }
    private static void Telekinesis(AtkContext atk)
    {
      var der = atk.Target.Defender.OnboardPokemon;
      if (der.Form.Species.Number == 50 || der.Form.Species.Number == 51) atk.Target.Defender.ShowLogPm("NoEffect");
      else if (der.AddCondition("Telekinesis", atk.Controller.TurnNumber + 2)) atk.Target.Defender.ShowLogPm("EnTelekinesis");
      else atk.FailAll();
    }
    private static void Sport(AtkContext atk, string condition)
    {
      if (atk.Attacker.OnboardPokemon.AddCondition(condition)) atk.Controller.ReportBuilder.ShowLog(condition);
      else atk.FailAll();
    }
    private static void Room(AtkContext atk, string condition)
    {
      var c = atk.Controller;
      if (c.Board.AddCondition(condition, c.TurnNumber + 4)) c.ReportBuilder.ShowLog("En" + condition, atk.Attacker.Id);
      else
      {
        c.Board.RemoveCondition(condition);
        c.ReportBuilder.ShowLog("De" + condition);
      }
    }
    private static void AddTeamCondition(AtkContext atk, string condition, int turn = 5)
    {
      var team = atk.Attacker.Pokemon.TeamId;
      if (atk.Controller.Board[team].AddCondition(condition, atk.Controller.TurnNumber + turn - 1)) atk.Controller.ReportBuilder.ShowLog("En" + condition, team);
      else atk.FailAll();
    }
    private static void EntryHazards(AtkContext atk, MoveType move, string log)
    {
      var team = 1 - atk.Attacker.Pokemon.TeamId;
      if (EHTs.En(atk.Controller.Board[team], move)) atk.Controller.ReportBuilder.ShowLog(log, team);
      else atk.FailAll();
    }
    private static void LockOn(AtkContext atk)
    {
     var der = atk.Target.Defender;
     var c = new Condition() { By = atk.Attacker, Turn = atk.Controller.TurnNumber + 1 };
     if (der.OnboardPokemon.AddCondition("NoGuard", c)) atk.Attacker.ShowLogPm("LockOn", der.Id);
     else atk.FailAll();
    }
    private static void BellyDrum(AtkContext atk)
    {
      var aer = atk.Attacker;
      if (aer.OnboardPokemon.Lv5D.Atk != 6 && aer.Hp > aer.Pokemon.MaxHp >> 1)
      {
        aer.OnboardPokemon.ChangeLv7D(StatType.Atk, 12);
        aer.ShowLogPm("m_BellyDrum");
        aer.Hp -= (aer.Pokemon.MaxHp >> 1);
      }
      else atk.FailAll();
    }
    private static void KOedCondition(AtkContext atk, string condition)
    {
      atk.Attacker.OnboardPokemon.SetCondition(condition);
      atk.Attacker.ShowLogPm("En" + condition);
    }
    private static void Camouflage(AtkContext atk)
    {
      var aer = atk.Attacker;
      var t = aer.Controller.GameSettings.Terrain.GetBattleType();
      if (aer.OnboardPokemon.SetTypes(t)) aer.ShowLogPm("TypeChange", (int)t);
      else atk.FailAll();
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
      atk.Attacker.ShowLogPm("PowerTrick");
    }
    private static void GastroAcid(AtkContext atk)
    {
      var der = atk.Target.Defender;
      var ab = der.Ability;
      if (ab != As.MULTITYPE && der.OnboardPokemon.AddCondition("GastroAcid"))
      {
        der.ShowLogPm("EnGastroAcid");
        AbilityDetach.Execute(der, ab);
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
      aer.Controller.ReportBuilder.ShowLog(log, aer.Id, der.Id);
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
      aer.Controller.ReportBuilder.ShowLog(log, aer.Id, der.Id);
    }
    private static void Defog(AtkContext atk)
    {
      var r = atk.Controller.ReportBuilder;
      atk.Target.Defender.ChangeLv7D(atk.Attacker, StatType.Evasion, -1, true);
      var t = atk.Target.Defender.Pokemon.TeamId;
      var f = atk.Controller.Board[t];
      EHTs.De(r, f);
      if (f.RemoveCondition("Reflect")) r.ShowLog("DeReflect", t);
      if (f.RemoveCondition("LightScreen")) r.ShowLog("DeLightScreen", t);
      if (f.RemoveCondition("Mist")) r.ShowLog("DeMist", t);
      if (f.RemoveCondition("Safeguard")) r.ShowLog("DeSafeguard", t);
    }
    private static void Soak(AtkContext atk)
    {
      var der = atk.Target.Defender;
      if (!ITs.PlatedArceus(der.Pokemon) && der.OnboardPokemon.SetTypes(BattleType.Water)) der.ShowLogPm("TypeChange", (int)BattleType.Water);
      else atk.FailAll();
    }
    private static void ReflectType(AtkContext atk)
    {
      var ao = atk.Attacker.OnboardPokemon;
      var types = atk.Target.Defender.OnboardPokemon.Types.ToArray();
      if (ao.SetTypes(types[0], types.ValueOrDefault(1))) atk.Attacker.ShowLogPm("ReflectType", atk.Target.Defender.Id);
      else atk.FailAll();
    }
    private static void Bestow(AtkContext atk)
    {
      var aer = atk.Attacker;
      var der = atk.Target.Defender;
      if (der.Pokemon.Item == 0)
      {
        var i = aer.Pokemon.Item;
        aer.RemoveItem();
        der.SetItem(i);
        der.ShowLogPm("Bestow", i, aer.Id);
      }
      else atk.FailAll();
    }
    private static void Moonlight(AtkContext atk)
    {
      var aer = atk.Attacker;
      if (aer.CanHpRecover(true))
      {
        var hp = aer.Pokemon.MaxHp;
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
      if (atk.Attacker.Hp > 1 && atk.Attacker.Hp > atk.Attacker.Pokemon.MaxHp >> 2)
      {
        if (aer.OnboardPokemon.HasCondition("Substitute")) aer.ShowLogPm("HasSubstitute");
        else
        {
          int hp = aer.Pokemon.MaxHp >> 2;
          aer.OnboardPokemon.SetCondition("m_Substitute", hp);
          aer.Pokemon.Hp -= hp;
          aer.Controller.ReportBuilder.EnSubstitute(aer);
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
      if (di == 0 && ai == 0 || ai != 0 && ITs.NeverLostItem(aer.Pokemon) || di != 0 && !ITs.CanLostItem(der)) atk.FailAll();
      else
      {
        aer.ShowLogPm("Trick");
        if (ai != 0) aer.RemoveItem();
        if (di != 0) der.RemoveItem();
        if (ai != 0)
        {
          der.SetItem(ai);
          der.ShowLogPm("GetItem", ai);
        }
        if (di != 0)
        {
          aer.SetItem(di);
          aer.ShowLogPm("GetItem", di);
        }
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
      if (c.Board.AddCondition("WonderRoom", c.TurnNumber + 4)) c.ReportBuilder.ShowLog("EnWonderRoom");
      else
      {
        c.Board.RemoveCondition("WonderRoom");
        c.ReportBuilder.ShowLog("DeWonderRoom");
      }
    }
    private static void Rest(AtkContext atk)
    {
      var aer = atk.Attacker;
      if (aer.Hp == aer.Pokemon.MaxHp) atk.FailAll();
      else if (CanAddState.Execute(aer, aer, AttachedState.SLP, true))
      {
        aer.Pokemon.Hp = aer.Pokemon.MaxHp;
        aer.Controller.ReportBuilder.SetHp(aer.Pokemon);
        aer.Pokemon.State = PokemonState.SLP;
        aer.OnboardPokemon.SetCondition("SLP", 3);
        aer.Field.SetCondition("Rest" + aer.Id);
        aer.ShowLogPm("Rest");
        StateAdded.Execute(aer);
      }
    }
    private static void Recycle(AtkContext atk)
    {
      var aer = atk.Attacker;
      if (aer.Pokemon.Item == 0)
      {
        var item = aer.Field.GetCondition<int>("UsedItem" + aer.Id);
        if (item == 0) atk.FailAll();
        else
        {
          aer.SetItem(item);
          aer.ShowLogPm("Recycle", item);
          aer.Field.RemoveCondition("UsedItem" + aer.Id);
        }
      }
      else atk.FailAll();
    }
    private static void PainSplit(AtkContext atk)
    {
      var aer = atk.Attacker;
      var der = atk.Target.Defender;
      int hp = (aer.Hp + der.Hp) >> 1;
      aer.Pokemon.Hp = hp;
      der.Pokemon.Hp = hp;
      atk.Controller.ReportBuilder.ShowLog("PainSplit", aer.Id, der.Id);
    }
    private static void HealBell(AtkContext atk, string log)
    {
      var aer = atk.Attacker;
      foreach (var pm in aer.Field.Pokemons)
        if (pm.State != PokemonState.Normal)
        {
          if (pm.Pokemon.State == PokemonState.SLP) pm.Field.RemoveCondition("Rest" + pm.Id);
          pm.Pokemon.State = PokemonState.Normal;
        }
      foreach (var pm in aer.Pokemon.Owner.Pokemons)
        if (pm.Hp > 0 && pm.State != PokemonState.Normal) pm.Pokemon.State = PokemonState.Normal;
      aer.Controller.ReportBuilder.ShowLog(log);
    }
    private static void Gravity(AtkContext atk)
    {
      var c = atk.Controller;
      if (c.Board.AddCondition("Gravity", c.TurnNumber + 4))
      {
        c.ReportBuilder.ShowLog("EnGravity");
        foreach (var pm in c.Board.Pokemons)
        {
          if (pm.CoordY == CoordY.Air)
          {
            pm.CoordY = CoordY.Plate;
            pm.Action = PokemonAction.Done;
            pm.OnboardPokemon.RemoveCondition("SkyDrop");
            c.ReportBuilder.SetY(pm);
            pm.ShowLogPm("Gravity");
          }
          else if (!HasEffect.IsGroundAffectable(pm, true, false, false)) pm.ShowLogPm("Gravity");
        }
      }
      else atk.FailAll();
    }
    private static void Conversion(AtkContext atk)
    {
      var aer = atk.Attacker;
      var types = aer.OnboardPokemon.Types;
      var ms = (from m in aer.Moves
                where !(m.Type.Id == Ms.CONVERSION || types.Contains(m.Type.Type))
                select m.Type.Type).ToArray();
      if (ms.Length == 0) atk.FailAll();
      else
      {
        var type = ms[aer.Controller.GetRandomInt(0, ms.Length - 1)];
        aer.OnboardPokemon.SetTypes(type);
        aer.ShowLogPm("TypeChange", (int)type);
      }
    }
    private static void BatonPass(AtkContext atk)
    {
      var aer = atk.Attacker;
      var t = aer.Tile;
      var o = aer.OnboardPokemon;
      if (aer.Controller.Withdraw(aer, "SelfWithdraw", 0, false))
      {
        t.SetCondition("BatonPass", o);
        aer.Controller.PauseForSendOutInput(t);
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
        aer.Controller.ReportBuilder.ShowLog("SkillSwap");
        if (aer.Pokemon.TeamId != der.Pokemon.TeamId)
        {
          aer.ShowLogPm("skillswap", d);
          der.ShowLogPm("skillswap", a);
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
      var fa = atk.Target.Defender.Ability;
      if (fa == As.TRUANT || fa == As.MULTITYPE) atk.FailAll();
      else
      {
        atk.Target.Defender.ChangeAbility(ability);
        atk.Target.Defender.ShowLogPm("m_SetAbility", ability);
        atk.Controller.ReportBuilder.ShowLog("setability", fa);
      }
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
      else
      {
        atk.Attacker.ChangeAbility(d);
        atk.Attacker.ShowLogPm("m_SetAbility", d);
        atk.Controller.ReportBuilder.ShowLog("setability", a);
      }
    }
    private static void Entrainment(AtkContext atk)
    {
      int a = atk.Attacker.OnboardPokemon.Ability;
      int d = atk.Target.Defender.OnboardPokemon.Ability;
      if
        (
        a == d ||
        a == As.FORECAST || a == As.ILLUSION || a == As.ZEN_MODE || a == As.FLOWER_GIFT ||
        d == As.TRUANT || d == As.MULTITYPE || d == As.FLOWER_GIFT
        )
        atk.FailAll();
      else
      {
        atk.Target.Defender.ChangeAbility(a);
        atk.Target.Defender.ShowLogPm("m_SetAbility", a);
        atk.Controller.ReportBuilder.ShowLog("setability", d);
      }
    }
    private static void Conversion2(AtkContext atk)
    {
      if (atk.Target.Defender.AtkContext == null) atk.FailAll();
      else
      {
        BattleType a = atk.Target.Defender.AtkContext.Type;
        if (a == BattleType.Invalid) a = BattleType.Normal;
        var rawtypes = atk.Attacker.OnboardPokemon.Types; //避免反复调用HasType性能
        var types = (from t in (BattleType[])Enum.GetValues(typeof(BattleType))
                     where !rawtypes.Contains(t) && (a.EffectRevise(t) < 0 || a.NoEffect(t)) //自动排除Invalid
                     select t).ToArray();
        var n = types.Length;
        if (n != 0)
        {
          var type = types[atk.Controller.GetRandomInt(0, n - 1)];
          atk.Attacker.OnboardPokemon.SetTypes(type);
          atk.Attacker.ShowLogPm("TypeChange", (int)type);
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
              der.ShowLogPm("EnEncore");
              return;
            }
            break;
          }
      atk.FailAll();
    }
    private static void Mimic(AtkContext atk)
    {
      var aer = atk.Attacker;
      var last = atk.Target.Defender.LastMove;
      if (last == null || aer.Moves.Any((m) => m.Type == last)) atk.FailAll();
      else
      {
        aer.ChangeMove(atk.Move, last);
        aer.Controller.ReportBuilder.Mimic(aer, last);
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
            der.ShowLogPm("Spite", m.Type.Id, fp - m.PP);
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
          atk.Attacker.ShowLogPm("EnFSDD" + atk.Move.Id);
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
        if (pm == aer || pm.State == PokemonState.Normal)
        {
          hits++;
          atk.SetCondition("BeatUpAtk", pm.OnboardPokemon.Form.Data.Base.Atk);
          CalculateDamages.Execute(atk);
          Implement(atk.Targets);
          if (atk.Target.Defender.Hp == 0 || aer.Hp == 0 || aer.State == PokemonState.FRZ || aer.State == PokemonState.SLP) break;
        }
      atk.Controller.ReportBuilder.ShowLog("Hits", hits);

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
        CalculateDamages.Execute(atk);
        Implement(atk.Targets);
        MoveE.FilterDefContext(atk);
      }
      while (atk.Target != null && hits < times && atk.Target.Defender.Hp != 0 && aer.Hp != 0 && aer.State != PokemonState.FRZ && aer.State != PokemonState.SLP);

      atk.Controller.ReportBuilder.ShowLog("Hits", hits);

      FinalEffect(atk);
    }
    #endregion

    private static void HealPulse(AtkContext atk)
    {
      atk.Target.Defender.HpRecover(atk.Target.Defender.Pokemon.MaxHp * (atk.Attacker.Ability == As.MEGA_LAUNCHER ? 75 : 50) / 100, true);
    }
    private static void AddType(AtkContext atk, BattleType type)
    {
      var der = atk.Target.Defender;
      if (der.OnboardPokemon.HasType(type)) atk.FailAll();
      else
      {
        der.OnboardPokemon.SetCondition("Type3", type);
        der.ShowLogPm("AddType", (int)type);
      }
    }
    private static void Terrain(AtkContext atk, string terrain)
    {
      var c = atk.Controller;
      var b = c.Board;
      if (b.AddCondition(terrain, c.TurnNumber + 4))
      {
        if (!(terrain != "GrassyTerrain" && b.RemoveCondition("GrassyTerrain") || terrain != "ElectricTerrain" && b.RemoveCondition("ElectricTerrain")) && terrain != "MistyTerrain") b.RemoveCondition("MistyTerrain");
        c.ReportBuilder.ShowLog("En" + terrain);
      }
      else atk.FailAll();
    }
    private static void TorsyTurvy(AtkContext atk)
    {
      var op = atk.Target.Defender.OnboardPokemon;
      op.SetLv7D(0 - op.Lv5D.Atk, 0 - op.Lv5D.SpAtk, 0 - op.Lv5D.Def, 0 - op.Lv5D.SpDef, 0 - op.Lv5D.Speed, 0 - op.AccuracyLv, 0 - op.EvasionLv);
      atk.Target.Defender.ShowLogPm("TorsyTurvy");
    }
    private static void SelfProtect(AtkContext atk, string condition, string log)
    {
      atk.Attacker.OnboardPokemon.SetTurnCondition(condition);
      atk.Attacker.ShowLogPm(log);
    }
    private static void TeamProtect(AtkContext atk, string condition)
    {
      var team = atk.Attacker.Pokemon.TeamId;
      atk.Controller.Board[team].SetTurnCondition(condition);
      atk.Controller.ReportBuilder.ShowLog("En" + condition, team);
    }
    private static void WeatherMove(AtkContext atk, Weather weather, int item)
    {
      var c = atk.Controller;
      if (c.Board.Weather == weather) atk.FailAll();
      else
      {
        c.Weather = weather;
        c.Board.SetCondition("Weather", c.TurnNumber + atk.Attacker.Item == item ? 9 : 4);
      }
    }
    private static void Powder(AtkContext atk)
    {
      foreach (var d in atk.Targets)
      {
        d.Defender.OnboardPokemon.SetTurnCondition("Powder");
        d.Defender.ShowLogPm("EnPowder");
      }
    }
  }
}
