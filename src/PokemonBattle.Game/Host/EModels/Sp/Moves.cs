using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Data;

namespace LightStudio.PokemonBattle.Game.Sp
{
  public static class Moves
  {
    const int SOLORBEAM = 76;
    const int TRI_ATTACK = 161;
    public const int STRUGGLE = 165;
    const int SNORE = 173;
    const int SLEEP_TALK = 214;
    const int FOCUS_PUNCH = 264;
    const int FOUL_PLAY = 492;

    public static bool AvailableEvenSleeping(this MoveProxy move)
    {
      return move.Type.Id == SLEEP_TALK || move.Type.Id == SNORE;
    }

    public static void PreMove(this MoveProxy move)
    {
      if (move.Type.Id == FOCUS_PUNCH)
      {
      }
    }

    public static bool CalculateFoulPlayAtk(AtkContext atk)
    {
      PokemonProxy pm = atk.Target.Defender;
      if (atk.Move.Id == FOUL_PLAY && pm != null)
      {
        atk.AtkRaw = atk.Attacker.OnboardPokemon.Static.Atk;
        atk.AtkRaw = (int)(atk.AtkRaw * atk.Attacker.Ability.Get5DRevise(pm, StatType.Atk));
        atk.AtkRaw = (int)(atk.AtkRaw * atk.Attacker.Item.Get5DRevise(pm, StatType.Atk));
        return true;
      }
      return false;
    }
    public static void CheckSolarbeam(AtkContext atk)
    {
      if (atk.Move.Id == SOLORBEAM)
      {
      }
    }
    public static bool CheckTriAttack(DefContext def)
    {
      Controller c = def.Defender.Controller;
      if (def.AtkContext.Move.Id == TRI_ATTACK)
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
  }
}
