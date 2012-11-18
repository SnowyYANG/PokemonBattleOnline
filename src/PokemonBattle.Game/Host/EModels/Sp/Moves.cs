using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Data;
using LightStudio.PokemonBattle.Game.GameEvents;

namespace LightStudio.PokemonBattle.Game.Host.Sp
{
  public static class Moves
  {
    public const int STRUGGLE = 165;
    internal const int CURSE = 174;
    internal const int SPIKES = 191;
    internal const int PURSUIT = 228;
    private const int FUTURE_SIGHT = 248;
    public const int UPROAR = 253;
    private const int DOOM_DESIRE = 353;
    internal const int ME_FIRST = 382;
    internal const int TOXIC_SPIKES = 390;
    internal const int STEALTH_ROCK = 446;

    public static bool AvailableEvenSleeping(this MoveProxy move)
    {
      const int SNORE = 173, SLEEP_TALK = 214;
      return move.Type.Id == SLEEP_TALK || move.Type.Id == SNORE;
    }
    internal static bool SkipSleepMTA(this MoveType move)
    {
      const int THRASH = 37, PETAL_DANCE = 80, OUTRAGE = 200, ROLLOUT = 205, ICE_BALL = 301;
      int id = move.Id;
      return id == THRASH || id == PETAL_DANCE || id == OUTRAGE || id == ROLLOUT || id == ICE_BALL;
    }

    public static Modifier SolarBeam(DefContext def)
    {
      if (def.AtkContext.Move.Id == 76)
      {
        Weather w = def.Defender.Controller.Weather;
        if (w != Weather.IntenseSunlight && w != Weather.Normal)
          return 0x800;
      }
      return 0x1000;
    }
    public static bool ThunderWave(this MoveType move)
    {
      return move.Id == 86;
    }

    public static bool FocusPunch(this MoveProxy move)
    {
      return move.Type.Id == 264;
    }
    public static bool IgnoreDefenderLv7D(this MoveType move)
    {
      return move.Id == 498 || move.Id == 533;
    }
    public static void SkyDrop(AtkContext atk)
    {
      if (atk.Move.Id == 507)
      {
        var d = atk.Target.Defender;
        d.OnboardPokemon.CoordY = CoordY.Plate;
        d.Controller.ReportBuilder.Add(PositionChange.Reset("DeSkyDrop", d));
      }
    }
    public static bool UseDefenderAtk(this MoveType move)
    {
      const int PSYSHOCK = 473, FOUL_PLAY = 492, PSYSTRIKE = 540, SECRET_SWORD = 548;
      return move.Id == PSYSHOCK || move.Id == FOUL_PLAY || move.Id == PSYSTRIKE || move.Id == SECRET_SWORD;
    }

    internal static MoveRange GetRange(PokemonProxy pm, MoveType move)
    {
      return move.Id == CURSE ? pm.OnboardPokemon.HasType(BattleType.Ghost) ? MoveRange.Single : MoveRange.User : move.Range;
    }
    internal static bool FSDD(AtkContext atk)
    {
      return atk.Move.Id == FUTURE_SIGHT || atk.Move.Id == DOOM_DESIRE;
    }
    internal static bool Bide(this MoveType move)
    {
      const int BIDE = 117;
      return move.Id == BIDE;
    }
  }
}
