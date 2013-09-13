using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonBattleOnline.Game;
using PokemonBattleOnline.Game.GameEvents;
using PokemonBattleOnline.Game.Host.Triggers;

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
    public static bool AttackSwitch(this MoveType move)
    {
      return move.Id == Ms.UTURN || move.Id == Ms.VOLT_SWITCH;
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
      return move.Id == Ms.CURSE ? pm.OnboardPokemon.HasType(BattleType.Ghost) ? MoveRange.Single : MoveRange.User : move.Range;
    }
    public static bool FSDD(AtkContext atk)
    {
      return atk.Move.Id == Ms.FUTURE_SIGHT || atk.Move.Id == Ms.DOOM_DESIRE;
    }
  }
}
