using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Data;

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

    public static void PreMove(this MoveProxy move)
    {
      if (move.Type.Id == 264)
      {
      }
    }

    public static bool CalculateFoulPlayAtk(AtkContext atk)
    {
      PokemonProxy pm = atk.Target.Defender;
      if (atk.Move.Id == 492 && pm != null)
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
      if (atk.Move.Id == 76)
      {
      }
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
  }
}
