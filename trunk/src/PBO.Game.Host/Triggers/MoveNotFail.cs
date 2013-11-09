using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonBattleOnline.Game;

namespace PokemonBattleOnline.Game.Host.Triggers
{
  internal static class MoveNotFail
  {
    /// <summary>
    /// contains battle log
    /// </summary>
    /// <param name="atk"></param>
    /// <returns></returns>
    public static bool Execute(AtkContext atk)
    {
      var aer = atk.Attacker;
      if (atk.Targets == null || atk.Target != null)
        switch (atk.Move.Id)
        {
          case Ms.STOCKPILE:
            if (aer.OnboardPokemon.GetCondition<int>("Stockpile") != 3) return true;
            break;
          case Ms.SPIT_UP:
          case Ms.SWALLOW:
            if (aer.OnboardPokemon.HasCondition("Stockpile")) return true;
            break;
          case Ms.SELFDESTRUCT: //120
          case Ms.EXPLOSION: //153
            if (aer.Controller.Board.Pokemons.Any((p) => p.RaiseAbility(As.DAMP)))
            {
              atk.FailAll("FailSp", atk.Attacker.Id, atk.Move.Id);
              return false;
            }
            return true;
          case Ms.REST: //156
            if (aer.Hp == aer.Pokemon.MaxHp)
            {
              atk.FailAll("FullHp", aer.Id);
              return false;
            }
            if (!aer.CanAddState(aer, AttachedState.SLP, true))
            {
              atk.FailAll(null);
              return false;
            }
            return true;
          case Ms.SNORE: //173
          case Ms.SLEEP_TALK: //214
            if (aer.State == PokemonState.SLP) return true;
            break;
          case Ms.FAKE_OUT: //252
          case Ms.MAT_BLOCK:
            if (!aer.Moves.Any((m) => m.HasUsed)) return true;
            break;
          case Ms.NATURAL_GIFT: //363
            if (ITs.CanLostItem(aer) && ITs.CanUseItem(aer) && ITs.Berry(aer.Pokemon.Item)) return true;
            break;
          case Ms.FLING: //374
            if (ITs.CanLostItem(aer) && ITs.CanUseItem(aer) && !ITs.Gem(aer.Pokemon.Item)) return true;
            break;
          case Ms.LAST_RESORT: //387
            if (aer.Moves.All((m) => m.HasUsed || m.Type.Id == Ms.LAST_RESORT)) return true;
            break;
          case Ms.BESTOW: //516
            if (aer.Pokemon.Item == 0 || ITs.NeverLostItem(aer.Pokemon)) return true;
            break;
          case Ms.ME_FIRST: //382
          case Ms.SUCKER_PUNCH: //389
            {
              var der = atk.Target.Defender;
              var dm = der.SelectedMove;
              if (!(der.LastMoveTurn == der.Controller.TurnNumber || dm == null || dm.Move.Type.Category == MoveCategory.Status)) return true;
            }
            break;
          case Ms.BELCH:
            if (aer.OnboardPokemon.HasCondition("Belch")) return true;
            break;
          default:
            if (!atk.Move.HardToUseContinuously() || ContinuousUse(atk)) return true;
            break;
        }
      atk.FailAll();
      return false;
    }
    private static bool ContinuousUse(AtkContext atk)
    {
      if (atk.Controller.ActingPokemons[atk.Controller.ActingPokemons.Count - 1] == atk.Attacker)
      {
        atk.Attacker.OnboardPokemon.RemoveCondition("ContinuousUse");
        return false;
      }
      var o = atk.Attacker.OnboardPokemon;
      var c = o.GetCondition("LastMove");
      if (c != null)
      {
        if (c.Move.HardToUseContinuously())
        {
          var count = o.GetCondition<int>("ContinuousUse");
          if (atk.Controller.GetRandomInt(0, 0xffff - 1) < 0xffff >> count)
          {
            o.SetCondition("ContinuousUse", count + 1);
            return true;
          }
          atk.Attacker.OnboardPokemon.RemoveCondition("ContinuousUse");
          return false;
        }
      }
      o.SetCondition("ContinuousUse", 1);
      return true;
    }
  }
}
