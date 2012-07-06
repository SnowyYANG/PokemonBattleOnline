using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Data;
using LightStudio.PokemonBattle.Interactive.GameEvents;

namespace LightStudio.PokemonBattle.Game.Sp
{
  public static class Moves
  {
    public const int STRUGGLE = 165;

    public static bool AvailableEvenSleeping(this MoveProxy move)
    {
      const int SNORE = 173;
      const int SLEEP_TALK = 214;
      return move.Type.Id == SLEEP_TALK || move.Type.Id == SNORE;
    }

    public static Modifier SolarBeam(DefContext def)
    {
      if (def.AtkContext.Move.Id == 76)
      {
        Weather w = def.Defender.Controller.GetAvailableWeather();
        if (w != Weather.IntenseSunlight && w != Weather.Normal)
          return 0x800;
      }
      return 0x1000;
    }
    public static bool ThunderWave(this MoveType move)
    {
      return move.Id == 86;
    }
    public static bool CheckTriAttack(DefContext def)
    {
      Controller c = def.Defender.Controller;
      if (def.AtkContext.Move.Id == 161)
      {
        if (c.OneNth(5))
        {
          int i = c.GetRandomInt(0, 2);
          AttachedState s = i == 0 ? AttachedState.Paralysis : i == 1 ? AttachedState.Burn : AttachedState.Freeze;
          def.Defender.AddState(def.AtkContext.Attacker, s);
        }
        return true;
      }
      return false;
    }
    public static void Pursuit(PokemonProxy target)
    {
      //228
    }
    public static void FocusPunch(MoveProxy move)
    {
      if (move.Type.Id == 264)
      {
        move.Owner.OnboardPokemon.SetCondition("FocusPunch");
        move.Owner.AddReportPm("EnFocusPunch");
      }
    }

    public static bool FoulPlay(this MoveType move)
    {
      return move.Id == 492;
    }
    public static bool ChipAway(this MoveType move)
    {
      return move.Id == 498;
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
  }
}
