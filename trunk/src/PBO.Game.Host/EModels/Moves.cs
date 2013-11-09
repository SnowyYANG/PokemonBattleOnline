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

    #region consts
    private static readonly int[] FLING_POWER = new int[] { 0,
60,
60,
60,
10,
10,
60,
80,
10,
10,
30,
10,
30,
90,
30,
10,
30,
30,
10,
30,
10,
100,
30,
30,
30,
30,
30,
50,
70,
30,
30,
30,
30,
70,
10,
30,
10,
10,
40,
10,
90,
60,
10,
10,
10,
10,
30,
30,
10,
30,
30,
10,
10,
10,
30,
130,
10,
10,
30,
40,
10,
60,
60,
90,
10,
80,
70,
70,
70,
70,
70,
70,
10,
10,
10,
90,
90,
90,
90,
90,
90,
90,
90,
90,
90,
90,
90,
90,
90,
90,
90,
10,
10,
10,
10,
10,
80,
30,
70,
70,
70,
70,
40,
30,
60,
10,
10,
10,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
10,
10,
10,
10,
10,
10,
10,
10,
10,
10,
10,
10,
10,
10,
10,
10,
10,
10,
10,
10,
10,
10,
10,
10,
10,
10,
10,
10,
10,
10,
10,
10,
10,
10,
10,
10,
10,
10,
10,
10,
10,
10,
10,
10,
10,
10,
10,
10,
10,
10,
10,
10,
10,
10,
10,
10,
10,
10,
10,
10,
10,
10,
10,
10,
0,
30,
100 };
    #endregion
    public static int FlingPower(int item)
    {
      return FLING_POWER.ValueOrDefault(item);
    }
  }
}
