using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Data;
using LightStudio.PokemonBattle.Game.GameEvents;
using LightStudio.PokemonBattle.Game.Host.Triggers;

namespace LightStudio.PokemonBattle.Game.Host
{
  internal static class As
  {
    #region ids
    public const int STENCH = 1;
    public const int DRIZZLE = 2;
    public const int SPEED_BOOST = 3;
    public const int BATTLE_ARMOR = 4;
    public const int STURDY = 5;
    public const int DAMP = 6;
    public const int LIMBER = 7;
    public const int SAND_VEIL = 8;
    public const int STATIC = 9;
    public const int VOLT_ABSORB = 10;
    public const int WATER_ABSORB = 11;
    public const int OBLIVIOUS = 12;
    public const int CLOUD_NINE = 13;
    public const int COMPOUNDEYES = 14;
    public const int INSOMNIA = 15;
    public const int COLOR_CHANGE = 16;
    public const int IMMUNITY = 17;
    public const int FLASH_FIRE = 18;
    public const int SHIELD_DUST = 19;
    public const int OWN_TEMPO = 20;
    public const int SUCTION_CUPS = 21;
    public const int INTIMIDATE = 22;
    public const int SHADOW_TAG = 23;
    public const int ROUGH_SKIN = 24;
    public const int WONDER_GUARD = 25;
    public const int LEVITATE = 26;
    public const int EFFECT_SPORE = 27;
    public const int SYNCHRONIZE = 28;
    public const int CLEAR_BODY = 29;
    public const int NATURAL_CURE = 30;
    public const int LIGHTNINGROD = 31;
    public const int SERENE_GRACE = 32;
    public const int SWIFT_SWIM = 33;
    public const int CHLOROPHYLL = 34;
    public const int ILLUMINATE = 35;
    public const int TRACE = 36;
    public const int HUGE_POWER = 37;
    public const int POISON_POINT = 38;
    public const int INNER_FOCUS = 39;
    public const int MAGMA_ARMOR = 40;
    public const int WATER_VEIL = 41;
    public const int MAGNET_PULL = 42;
    public const int SOUNDPROOF = 43;
    public const int RAIN_DISH = 44;
    public const int SAND_STREAM = 45;
    public const int PRESSURE = 46;
    public const int THICK_FAT = 47;
    public const int EARLY_BIRD = 48;
    public const int FLAME_BODY = 49;
    public const int RUN_AWAY = 50;
    public const int KEEN_EYE = 51;
    public const int HYPER_CUTTER = 52;
    public const int PICKUP = 53;
    public const int TRUANT = 54;
    public const int HUSTLE = 55;
    public const int CUTE_CHARM = 56;
    public const int PLUS = 57;
    public const int MINUS = 58;
    public const int FORECAST = 59;
    public const int STICKY_HOLD = 60;
    public const int SHED_SKIN = 61;
    public const int GUTS = 62;
    public const int MARVEL_SCALE = 63;
    public const int LIQUID_OOZE = 64;
    public const int OVERGROW = 65;
    public const int BLAZE = 66;
    public const int TORRENT = 67;
    public const int SWARM = 68;
    public const int ROCK_HEAD = 69;
    public const int DROUGHT = 70;
    public const int ARENA_TRAP = 71;
    public const int VITAL_SPIRIT = 72;
    public const int WHITE_SMOKE = 73;
    public const int PURE_POWER = 74;
    public const int SHELL_ARMOR = 75;
    public const int AIR_LOCK = 76;
    public const int TANGLED_FEET = 77;
    public const int MOTOR_DRIVE = 78;
    public const int RIVALRY = 79;
    public const int STEADFAST = 80;
    public const int SNOW_CLOAK = 81;
    public const int GLUTTONY = 82;
    public const int ANGER_POINT = 83;
    public const int UNBURDEN = 84;
    public const int HEATPROOF = 85;
    public const int SIMPLE = 86;
    public const int DRY_SKIN = 87;
    public const int DOWNLOAD = 88;
    public const int IRON_FIST = 89;
    public const int POISON_HEAL = 90;
    public const int ADAPTABILITY = 91;
    public const int SKILL_LINK = 92;
    public const int HYDRATION = 93;
    public const int SOLAR_POWER = 94;
    public const int QUICK_FEET = 95;
    public const int NORMALIZE = 96;
    public const int SNIPER = 97;
    public const int MAGIC_GUARD = 98;
    public const int NO_GUARD = 99;
    public const int STALL = 100;
    public const int TECHNICIAN = 101;
    public const int LEAF_GUARD = 102;
    public const int KLUTZ = 103;
    public const int MOLD_BREAKER = 104;
    public const int SUPER_LUCK = 105;
    public const int AFTERMATH = 106;
    public const int ANTICIPATION = 107;
    public const int FOREWARN = 108;
    public const int UNAWARE = 109;
    public const int TINTED_LENS = 110;
    public const int FILTER = 111;
    public const int SLOW_START = 112;
    public const int SCRAPPY = 113;
    public const int STORM_DRAIN = 114;
    public const int ICE_BODY = 115;
    public const int SOLID_ROCK = 116;
    public const int SNOW_WARNING = 117;
    public const int HONEY_GATHER = 118;
    public const int FRISK = 119;
    public const int RECKLESS = 120;
    public const int MULTITYPE = 121;
    public const int FLOWER_GIFT = 122;
    public const int BAD_DREAMS = 123;
    public const int PICKPOCKET = 124;
    public const int SHEER_FORCE = 125;
    public const int CONTRARY = 126;
    public const int UNNERVE = 127;
    public const int DEFIANT = 128;
    public const int DEFEATIST = 129;
    public const int CURSED_BODY = 130;
    public const int HEALER = 131;
    public const int FRIEND_GUARD = 132;
    public const int WEAK_ARMOR = 133;
    public const int HEAVY_METAL = 134;
    public const int LIGHT_METAL = 135;
    public const int MULTISCALE = 136;
    public const int TOXIC_BOOST = 137;
    public const int FLARE_BOOST = 138;
    public const int HARVEST = 139;
    public const int TELEPATHY = 140;
    public const int MOODY = 141;
    public const int OVERCOAT = 142;
    public const int POISON_TOUCH = 143;
    public const int REGENERATOR = 144;
    public const int BIG_PECKS = 145;
    public const int SAND_RUSH = 146;
    public const int WONDER_SKIN = 147;
    public const int ANALYTIC = 148;
    public const int ILLUSION = 149;
    public const int IMPOSTER = 150;
    public const int INFILTRATOR = 151;
    public const int MUMMY = 152;
    public const int MOXIE = 153;
    public const int JUSTIFIED = 154;
    public const int RATTLED = 155;
    public const int MAGIC_BOUNCE = 156;
    public const int SAP_SIPPER = 157;
    public const int PRANKSTER = 158;
    public const int SAND_FORCE = 159;
    public const int IRON_BARBS = 160;
    public const int ZEN_MODE = 161;
    public const int VICTORY_STAR = 162;
    public const int TURBOBLAZE = 163;
    public const int TERAVOLT = 164;
    #endregion

    #region extension
    public static void RaiseAbility(this PokemonProxy pm)
    {
      pm.Controller.ReportBuilder.Add(new AbilityEvent(pm));
    }
    public static bool RaiseAbility(this PokemonProxy pm, int abilityId)
    {
      if (pm.Ability != abilityId) return false;
      pm.Controller.ReportBuilder.Add(new AbilityEvent(pm));
      return true;
    }
    public static bool HasProbabilitiedAdditonalEffects(this MoveType move)
    {
      return
        (
        move.Class == MoveInnerClass.AttackWithState ||
        move.Class == MoveInnerClass.AttackWithTargetLv7DChange ||
        move.FlinchProbability > 0 ||
        (move.Attachment != null && move.Attachment.Probability > 0) ||
        (move.Class == MoveInnerClass.AttackWithSelfLv7DChange && move.Lv7DChanges.First().Change > 0)
        );
    }
    #endregion

    #region IsXXX
    public static bool IgnoreDefenderAbility(int ability)
    {
      return ability == MOLD_BREAKER || ability == TURBOBLAZE || ability == TERAVOLT;
    }
    public static bool CannotBeCted(int ability)
    {
      return ability == BATTLE_ARMOR || ability == SHELL_ARMOR;
    }
    #endregion

    public static bool IgnoreWeather(Controller c)
    {
      const int CLOUD_NINE = 13, AIR_LOCK = 76;
      foreach (PokemonProxy p in c.ActingPokemons)
      {
        var a = p.Ability;
        if (a == AIR_LOCK || a == CLOUD_NINE) return true;
      }
      return false;
    }
    public static void Pressure(AtkContext atk, MoveRange range)
    {
      const int PRESSURE = 46;
      var ts =
        atk.Move.Range == MoveRange.Field || atk.Move.Range == MoveRange.EnemyField ?
        atk.Attacker.Controller.Board[1 - atk.Attacker.Pokemon.TeamId].Pokemons :
        atk.Targets == null ?
        Enumerable.Empty<PokemonProxy>() :
        atk.Targets.Select((t) => t.Defender).Where((p) => p.Pokemon.TeamId != atk.Attacker.Pokemon.TeamId);
      foreach (var d in ts)
        if (d.Ability == PRESSURE) atk.Pressure++;     
    }
    public static void Withdrawn(PokemonProxy pm, int ability)
    {
      if (ability == REGENERATOR) pm.Pokemon.SetHp(pm.Hp + pm.Pokemon.Hp.Origin / 3);
      else if (ability == NATURAL_CURE) pm.Pokemon.State = PokemonState.Normal;
      else if (ability == UNNERVE)
        foreach (var p in pm.Controller.GetOnboardPokemons(1 - pm.Pokemon.TeamId)) STs.ItemAttach(p);
    }
    public static void Synchronize(PokemonProxy pm, PokemonProxy by, AttachedState state, int turn)
    {
      if (pm != by && pm.RaiseAbility(SYNCHRONIZE)) by.AddState(pm, state, true, turn);
    }
    public static void Defiant(PokemonProxy pm)
    {
      if (pm.CanChangeLv7D(pm, StatType.Atk, 2, false) != 0 && pm.RaiseAbility(DEFIANT)) pm.ChangeLv7D(pm, StatType.Atk, 2, false);
    }
    public static void Illusion(PokemonProxy pm)
    {
      if (pm.Ability == ILLUSION)
      {
        Pokemon o = pm.Pokemon;
        foreach (Pokemon p in pm.Pokemon.Owner.Pokemons)
          if (p.Hp.Value > 0) o = p;
        if (o != pm.Pokemon) pm.OnboardPokemon.SetCondition("Illusion", o);
      }
    }
    public static void ColorChange(DefContext def)
    {
      var type = def.AtkContext.Type;
      if (type == BattleType.Invalid) type = BattleType.Normal;
      if (
        !def.HitSubstitute &&
        !(def.Defender.OnboardPokemon.Type1 == type && def.Defender.OnboardPokemon.Type2 == BattleType.Invalid) &&
        def.Defender.RaiseAbility(COLOR_CHANGE)
        )
      {
        def.Defender.OnboardPokemon.Type1 = type;
        def.Defender.OnboardPokemon.Type2 = BattleType.Invalid;
        def.Defender.AddReportPm("TypeChange", type);
      }
    }
    public static double WeightModifier(PokemonProxy pm)
    {
      double m;
      int id = pm.Ability;
      if (id == HEAVY_METAL) m = 2d;
      else if (id == LIGHT_METAL) m = 0.5d;
      else m = 1d;
      return m;
    }
    public static bool Stench(DefContext def)
    {
      return def.AtkContext.Attacker.Ability == STENCH && def.Defender.Controller.RandomHappen(10);
    }
    public static bool Trace(int abilityId)
    {
      return !(abilityId == FORECAST || abilityId == ILLUSION || abilityId == ZEN_MODE || abilityId == MULTITYPE || abilityId == TRACE);
    }
    public static void Trace(PokemonProxy sendout)
    {
      int ab = sendout.OnboardPokemon.Ability;
      if (Trace(ab))
        foreach (var pm in sendout.Controller.Board[1 - sendout.Pokemon.TeamId].GetPokemons(sendout.OnboardPokemon.X - 1, sendout.OnboardPokemon.X + 1))
          if (pm.RaiseAbility(TRACE)) pm.ChangeAbility(sendout.OnboardPokemon.Ability, "Trace", sendout.Id);
    }
    public static bool Gluttony(PokemonProxy pm)
    {
      return pm.Hp << 2 < pm.Pokemon.Hp.Origin || (pm.Ability == GLUTTONY && pm.Hp << 1 < pm.Pokemon.Hp.Origin);
    }

    internal static void SlowStart(Controller Controller)
    {
      foreach (var pm in Controller.ActingPokemons)
        if (pm.Ability == SLOW_START)
        {
          int turn = pm.OnboardPokemon.GetCondition<int>("SlowStart");
          if (turn == Controller.TurnNumber)
          {
            pm.OnboardPokemon.RemoveCondition("SlowStart");
            pm.AddReportPm("DeSlowStart");
          }
        }
    }
    internal static void ReTarget(AtkContext atk)
    {
      if (atk.Move.Range == MoveRange.Single)
      {
        int ab = 0;
        if (atk.Type == BattleType.Electric) ab = As.LIGHTNINGROD;
        else if (atk.Type == BattleType.Water) ab = As.STORM_DRAIN;
        if (ab != 0)
          foreach (var pm in atk.Controller.Board.Pokemons)
            if (pm.Ability == ab)
            {
              if (pm != atk.Attacker && pm != atk.Target.Defender)
              {
                pm.RaiseAbility();
                pm.AddReportPm("ReTarget");
                atk.SetTargets(new DefContext[] { new DefContext(atk, pm) });
              }
              return;
            }
      }
    }
    internal static void RecoverAfterMoldBreaker(PokemonProxy pm)
    {
      int id = pm.Ability;
      if (id == LIMBER || id == OBLIVIOUS || id == IMMUNITY || id == INSOMNIA || id == OWN_TEMPO || id == MAGMA_ARMOR || id == WATER_VEIL || id == VITAL_SPIRIT)
        AbilityAttach.Execute(pm);
    }
    internal static bool Unnerve(PokemonProxy pm)
    {
      return pm.OnboardPokemon.HasCondition("Unnerve") && pm.Ability == UNNERVE;
    }
    internal static void AttachUnnerve(Controller c)
    {
      foreach(var pm in c.OnboardPokemons)
        if (!pm.OnboardPokemon.HasCondition("Unnerve") && pm.Ability == UNNERVE)
        {
          pm.OnboardPokemon.SetCondition("Unnerve");
          pm.RaiseAbility();
          pm.Controller.ReportBuilder.Add("Unnerve", 1 - pm.Pokemon.TeamId);
        }
    }
    internal static void AttachWeatherObserver(PokemonProxy pm)
    {
      var a = pm.OnboardPokemon.Ability;
      if (a == FORECAST || a == FLOWER_GIFT) pm.OnboardPokemon.SetCondition("ObserveWeather");
    }

    public static void WeatherChanged(Controller c)
    {
      foreach (var pm in c.OnboardPokemons)
        if (pm.OnboardPokemon.HasCondition("ObserveWeather"))
        {
          var ab = pm.Ability;
          if (ab == FORECAST || ab == FLOWER_GIFT) AbilityAttach.Execute(pm);
        }
    }
    public static void DeIllusion(PokemonProxy pm)
    {
      if (pm.OnboardPokemon.RemoveCondition("Illusion")) pm.Controller.ReportBuilder.Add(GameEvents.OutwardChange.DeIllusion("DeIllusion", pm));
    }
  }
}
