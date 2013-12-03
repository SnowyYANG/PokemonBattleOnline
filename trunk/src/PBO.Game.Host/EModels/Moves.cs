using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokemonBattleOnline.Game.Host
{
  internal static class MTs
  {
    public static bool AvailableEvenSleeping(this MoveProxy move)
    {
      return move.Type.Id == Ms.SLEEP_TALK || move.Type.Id == Ms.SNORE;
    }
    public static bool SkipSleepMTA(this MoveType move)
    {
      int id = move.Id;
      return id == Ms.THRASH || id == Ms.PETAL_DANCE || id == Ms.OUTRAGE || id == Ms.ROLLOUT || id == Ms.ICE_BALL;
    }
    public static bool Switch(this MoveType move)
    {
      return move.Id == Ms.UTURN || move.Id == Ms.VOLT_SWITCH || move.Id == Ms.PARTING_SHOT;
    }
    public static bool IgnoreDefenderLv7D(this MoveType move)
    {
      return move.Id == Ms.CHIP_AWAY || move.Id == Ms.SACRED_SWORD;
    }
    public static bool UsePhysicalDef(this MoveType move)
    {
      return move.Id == Ms.PSYSHOCK || move.Id == Ms.PSYSTRIKE || move.Id == Ms.SECRET_SWORD;
    }
    public static MoveRange GetRange(PokemonProxy pm, MoveType move)
    {
      return move.Id == Ms.CURSE ? pm.OnboardPokemon.HasType(BattleType.Ghost) ? MoveRange.Single : MoveRange.Self : move.Range;
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

    private static int[] CONTINUOUS_USE = new int[] { Ms.PROTECT, Ms.DETECT, Ms.ENDURE, Ms.QUICK_GUARD, Ms.WIDE_GUARD, Ms.CRAFTY_SHIELD, Ms.SPIKY_SHIELD, Ms.KINGS_SHIELD };
    public static bool HardToUseContinuously(this MoveType move)
    {
      return CONTINUOUS_USE.Contains(move.Id);
    }

    private static int[] AROMA_VEILS = new int[] { Ms.DISABLE, Ms.ATTRACT, Ms.ENCORE, Ms.TORMENT, Ms.TAUNT };
    public static bool AromaVeil(this MoveType move)
    {
      return AROMA_VEILS.Contains(move.Id);
    }

    private static int[] STRONG_JAWS = new int[] { Ms.BITE, Ms.CRUNCH, Ms.FIRE_FANG, Ms.HYPER_FANG, Ms.HYPER_FANG, Ms.ICE_FANG, Ms.POISON_FANG, Ms.THUNDER_FANG };
    public static bool StrongJaw(this MoveType move)
    {
      return STRONG_JAWS.Contains(move.Id);
    }
    private static int[] BULLETPROOFS = new int[] { Ms.ACID_SPRAY, Ms.AURA_SPHERE, Ms.BARRAGE, Ms.BULLET_SEED, Ms.EGG_BOMB, Ms.ELECTRO_BALL, Ms.ENERGY_BALL, Ms.FOCUS_BLAST, Ms.GYRO_BALL, Ms.ICE_BALL, Ms.MAGNET_BOMB, Ms.OCTAZOOKA, Ms.SEED_BOMB, Ms.SHADOW_BALL, Ms.SLUDGE_BOMB, Ms.WEATHER_BALL };
    public static bool Bulletproof(this MoveType move)
    {
      return BULLETPROOFS.Contains(move.Id);
    }
    private static int[] MEGA_LAUNCHER = new int[] { Ms.AURA_SPHERE, Ms.DARK_PULSE, Ms.DRAGON_PULSE, Ms.WATER_PULSE };
    public static bool MegaLaucher(this MoveType move)
    {
      return MEGA_LAUNCHER.Contains(move.Id);
    }

    private static int[] POWDER = new int[] { Ms.POWDER, Ms.POWDER_SNOW, Ms.POISONPOWDER, Ms.RAGE_POWDER, Ms.SLEEP_POWDER };
    public static bool Powder(this MoveType move)
    {
      return POWDER.Contains(move.Id);
    }
    private static int[] SPORE = new int[] { Ms.SPORE, Ms.COTTON_SPORE, Ms.STUN_SPORE };
    public static bool Spore(this MoveType move)
    {
      return SPORE.Contains(move.Id);
    }
    private static int[] CT1 = new int[] { Ms.KARATE_CHOP, Ms.RAZOR_WIND, Ms.RAZOR_LEAF, Ms.SKY_ATTACK, Ms.CRABHAMMER, Ms.SLASH, Ms.AEROBLAST, Ms.CROSS_CHOP, Ms.BLAZE_KICK, Ms.AIR_CUTTER, Ms.POISON_TAIL, Ms.LEAF_BLADE, Ms.NIGHT_SLASH, Ms.SHADOW_CLAW, Ms.PSYCHO_CUT, Ms.CROSS_POISON, Ms.STONE_EDGE, Ms.ATTACK_ORDER, Ms.SPACIAL_REND, Ms.DRILL_RUN };
    public static bool Ct1(this MoveType move)
    {
      return CT1.Contains(move.Id);
    }
    public static bool MustCt(this MoveType move)
    {
      return move.Id == Ms.STORM_THROW || move.Id == Ms.FROST_BREATH;
    }

    public static int FlingPower(int item)
    {
      switch (item)
      {
        case Is.IRON_BALL:
          return 130;
        case Is.RARE_BONE:
        case Is.HARD_STONE:
          return 100;
        case Is.DEEPSEATOOTH:
        case Is.THICK_CLUB:
        case Is.GRIP_CLAW:
        case Is.FLAME_PLATE:
        case Is.SPLASH_PLATE:
        case Is.ZAP_PLATE:
        case Is.MEADOW_PLATE:
        case Is.ICICLE_PLATE:
        case Is.FIST_PLATE:
        case Is.TOXIC_PLATE:
        case Is.EARTH_PLATE:
        case Is.SKY_PLATE:
        case Is.MIND_PLATE:
        case Is.INSECT_PLATE:
        case Is.STONE_PLATE:
        case Is.SPOOKY_PLATE:
        case Is.DRACO_PLATE:
        case Is.DREAD_PLATE:
        case Is.IRON_PLATE:
        case Is.PIXIE_PLATE:
          return 90;
        case Is.RAZOR_CLAW:
        case Is.QUICK_CLAW:
        case Is.STICKY_BARB:
          return 80;
        case Is.POWER_BRACER:
        case Is.POWER_BELT:
        case Is.POWER_LENS:
        case Is.POWER_BAND:
        case Is.POWER_ANKLET:
        case Is.POWER_WEIGHT:
        case Is.DOUSE_DRIVE:
        case Is.SHOCK_DRIVE:
        case Is.BURN_DRIVE:
        case Is.CHILL_DRIVE:
        case Is.POISON_BARB:
        case Is.DRAGON_FANG:
          return 70;
        case Is.ROCKY_HELMET:
        case Is.MACHO_BRACE:
        case Is.GRISEOUS_ORB:
        case Is.ADAMANT_ORB:
        case Is.LUSTROUS_ORB:
        case Is.STICK:
        case Is.HEAT_ROCK:
        case Is.DAMP_ROCK:
          return 60;
        case Is.SHARP_BEAK:
          return 50;
        case Is.EVIOLITE:
        case Is.LUCKY_PUNCH:
        case Is.ICY_ROCK:
          return 40;
        case Is.BERRY_JUICE:
        case Is.LIFE_ORB:
        case Is.SHELL_BELL:
        case Is.METRONOME:
        case Is.SCOPE_LENS:
        case Is.RAZOR_FANG:
        case Is.KINGS_ROCK:
        case Is.FLOAT_STONE:
        case Is.BLACK_SLUDGE:
        case Is.TOXIC_ORB:
        case Is.FLAME_ORB:
        case Is.EJECT_BUTTON:
        case Is.ABSORB_BULB:
        case Is.CELL_BATTERY:
        case Is.LIGHT_BALL:
        case Is.SOUL_DEW:
        case Is.DEEPSEASCALE:
        case Is.LIGHT_CLAY:
        case Is.BINDING_BAND:
        case Is.METAL_COAT:
        case Is.MIRACLE_SEED:
        case Is.BLACKGLASSES:
        case Is.BLACK_BELT:
        case Is.MAGNET:
        case Is.MYSTIC_WATER:
        case Is.NEVERMELTICE:
        case Is.SPELL_TAG:
        case Is.TWISTEDSPOON:
        case Is.CHARCOAL:
          return 30;
        case Is.RING_TARGET:
        case Is.BRIGHT_POWDER:
        case Is.WIDE_LENS:
        case Is.ZOOM_LENS:
        case Is.MUSCLE_BAND:
        case Is.WISE_GLASSES:
        case Is.EXPERT_BELT:
        case Is.WHITE_HERB:
        case Is.MENTAL_HERB:
        case Is.DESTINY_KNOT:
        case Is.LAGGING_TAIL:
        case Is.SHED_SHELL:
        case Is.LEFTOVERS:
        case Is.FOCUS_BAND:
        case Is.FOCUS_SASH:
        case Is.AIR_BALLOON:
        case Is.RED_CARD:
        case Is.METAL_POWDER:
        case Is.QUICK_POWDER:
        case Is.POWER_HERB:
        case Is.BIG_ROOT:
        case Is.SMOOTH_ROCK:
        case Is.CHOICE_BAND:
        case Is.CHOICE_SCARF:
        case Is.CHOICE_SPECS:
        case Is.SILVERPOWDER:
        case Is.SOFT_SAND:
        case Is.SILK_SCARF:
        case Is.SEA_INCENSE:
        case Is.LAX_INCENSE:
        case Is.ODD_INCENSE:
        case Is.ROCK_INCENSE:
        case Is.FULL_INCENSE:
        case Is.WAVE_INCENSE:
        case Is.ROSE_INCENSE:
        case Is.CHERI_BERRY:
        case Is.CHESTO_BERRY:
        case Is.PECHA_BERRY:
        case Is.RAWST_BERRY:
        case Is.ASPEAR_BERRY:
        case Is.LEPPA_BERRY:
        case Is.ORAN_BERRY:
        case Is.PERSIM_BERRY:
        case Is.LUM_BERRY:
        case Is.SITRUS_BERRY:
        case Is.FIGY_BERRY:
        case Is.WIKI_BERRY:
        case Is.MAGO_BERRY:
        case Is.AGUAV_BERRY:
        case Is.IAPAPA_BERRY:
        case Is.RAZZ_BERRY:
        case Is.BLUK_BERRY:
        case Is.NANAB_BERRY:
        case Is.WEPEAR_BERRY:
        case Is.PINAP_BERRY:
        case Is.POMEG_BERRY:
        case Is.KELPSY_BERRY:
        case Is.QUALOT_BERRY:
        case Is.HONDEW_BERRY:
        case Is.GREPA_BERRY:
        case Is.TAMATO_BERRY:
        case Is.CORNN_BERRY:
        case Is.MAGOST_BERRY:
        case Is.RABUTA_BERRY:
        case Is.NOMEL_BERRY:
        case Is.SPELON_BERRY:
        case Is.PAMTRE_BERRY:
        case Is.WATMEL_BERRY:
        case Is.DURIN_BERRY:
        case Is.BELUE_BERRY:
        case Is.OCCA_BERRY:
        case Is.PASSHO_BERRY:
        case Is.WACAN_BERRY:
        case Is.RINDO_BERRY:
        case Is.YACHE_BERRY:
        case Is.CHOPLE_BERRY:
        case Is.KEBIA_BERRY:
        case Is.SHUCA_BERRY:
        case Is.COBA_BERRY:
        case Is.PAYAPA_BERRY:
        case Is.TANGA_BERRY:
        case Is.CHARTI_BERRY:
        case Is.KASIB_BERRY:
        case Is.HABAN_BERRY:
        case Is.COLBUR_BERRY:
        case Is.BABIRI_BERRY:
        case Is.ROSELI_BERRY:
        case Is.CHILAN_BERRY:
        case Is.LIECHI_BERRY:
        case Is.GANLON_BERRY:
        case Is.SALAC_BERRY:
        case Is.PETAYA_BERRY:
        case Is.APICOT_BERRY:
        case Is.LANSAT_BERRY:
        case Is.STARF_BERRY:
        case Is.ENIGMA_BERRY:
        case Is.MICLE_BERRY:
        case Is.CUSTAP_BERRY:
        case Is.JABOCA_BERRY:
        case Is.ROWAP_BERRY:
        case Is.KEE_BERRY:
        case Is.MARANGA_BERRY:
          return 10;
        default:
          return 0;
      }
    }
  }
}
