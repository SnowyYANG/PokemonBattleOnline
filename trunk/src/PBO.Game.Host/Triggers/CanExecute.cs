using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonBattleOnline.Game;
using PokemonBattleOnline.Game.GameEvents;

namespace PokemonBattleOnline.Game.Host.Triggers
{
  internal static class CanExecute
  {
    public static bool Execute(PokemonProxy pm)
    {
      return
        Sleeping(pm) &&
        Frozen(pm) &&
        Disable(pm) &&
        Truant(pm) &&
        Imprison(pm) &&
        HealBlock(pm) &&
        Confuse(pm) &&
        Flinch(pm) &&
        Taunt(pm) && 
        Gravity(pm) &&  
        Attract(pm) &&
        Paralyzed(pm) &&
        FocusPunch(pm);
    }
    private static bool Sleeping(PokemonProxy pm)
    {
      if (pm.State == PokemonState.SLP)
      {
        int count = pm.OnboardPokemon.GetCondition<int>("SLP");
        count--;
        if (pm.Ability == As.EARLY_BIRD) count--;
        if (count <= 0)
        {
          pm.OnboardPokemon.RemoveCondition("SLP");
          pm.DeAbnormalState();
        }
        else
        {
          pm.OnboardPokemon.SetCondition("SLP", count);
          pm.ShowLogPm("SLP");
          return pm.SelectedMove.AvailableEvenSleeping();
        }
      }
      return true;
    }
    private static bool Frozen(PokemonProxy p)
    {
      if (p.State == PokemonState.FRZ)
      {
        if (p.SelectedMove.Type.SelfDeFrozen()) p.DeAbnormalState("DeFRZ2", p.SelectedMove.Type.Id);
        else if (p.Controller.GetRandomInt(0, 3) == 0) p.DeAbnormalState();
        else
        {
          p.ShowLogPm("FRZ");
          return false;
        }
      }
      return true;
    }
    private static bool Disable(PokemonProxy p)
    {
      var c = p.OnboardPokemon.GetCondition("Disable");
      if (c != null && p.SelectedMove.Type == c.Move) 
      {
        p.ShowLogPm("Disable", p.SelectedMove.Type.Id);
        return false;
      }
      return true;
    }
    private static bool Truant(PokemonProxy p)
    {
      if (p.Ability == As.TRUANT)
      {
        if (p.OnboardPokemon.GetCondition<int>("Truant") == p.Controller.TurnNumber)
        {
          p.RaiseAbility();
          p.ShowLogPm("Truant");
          return false;
        }
        p.OnboardPokemon.SetCondition("Truant", p.Controller.TurnNumber + 1);
      }
      return true;
    }
    private static bool Imprison(PokemonProxy p)
    {
      MoveType move = p.SelectedMove.Type;
      foreach (PokemonProxy pm in p.Controller.Board[1 - p.Pokemon.TeamId].GetPokemons(p.OnboardPokemon.X - 1, p.OnboardPokemon.X + 1))
        if (pm.OnboardPokemon.HasCondition("Imprison"))
          foreach (MoveProxy m in pm.Moves)
            if (m.Type == move)
            {
              p.ShowLogPm("Imprison", move.Id);
              return false;
            }
      return true;
    }
    private static bool HealBlock(PokemonProxy pm)
    {
      if (pm.SelectedMove.Move.Type.Heal() && pm.OnboardPokemon.HasCondition("HealBlock"))
      {
        pm.ShowLogPm("HealBlockCantUseMove", pm.SelectedMove.Move.Type.Id);
        return false;
      }
      return true;
    }
    private static bool Confuse(PokemonProxy pm)
    {
      int count = pm.OnboardPokemon.GetCondition<int>("Confuse");
      if (count != 0)
        if (--count > 0)
        {
          pm.ShowLogPm("Confuse");
          pm.OnboardPokemon.SetCondition("Confuse", count);
          if (pm.Controller.OneNth(2))
          {
            pm.ShowLogPm("m_ConfuseWork");
            var e = new GameEvents.ShowHp();
            pm.Controller.ReportBuilder.Add(e);
            pm.MoveHurt((pm.Pokemon.Lv * 2 / 5 + 2) * 40 * OnboardPokemon.Get5D(pm.OnboardPokemon.FiveD.Atk, pm.OnboardPokemon.Lv5D.Atk) / OnboardPokemon.Get5D(pm.OnboardPokemon.FiveD.Def, pm.OnboardPokemon.Lv5D.Def) / 50 + 2);
            e.Hp = pm.Hp;
            pm.CheckFaint();
            //if (!pm.CheckFaint()) pm.Item.HpChanged(pm); //◇硝子玩偶◇ 22:21:00 你知道混乱打自己的时候不触发加HP的果子么
            return false;
          }
        }
        else
        {
          pm.OnboardPokemon.RemoveCondition("Confuse");
          pm.ShowLogPm("DeConfuse");
        }
      return true;
    }
    private static bool Flinch(PokemonProxy pm)
    {
      if (pm.OnboardPokemon.HasCondition("Flinch"))
      {
        pm.ShowLogPm("Flinch");
        if (pm.RaiseAbility(As.STEADFAST)) pm.ChangeLv7D(pm, StatType.Speed, 1, false);
        return false;
      }
      return true;
    }
    private static bool Taunt(PokemonProxy p)
    {
      if (p.SelectedMove.Type.Category == MoveCategory.Status && p.OnboardPokemon.HasCondition("Taunt"))
      {
        p.ShowLogPm("Taunt", p.SelectedMove.Type.Id);
        return false;
      }
      return true;
    }
    private static bool Gravity(PokemonProxy p)
    {
      if (p.SelectedMove.Type.UnavailableWithGravity() && p.Controller.Board.HasCondition("Gravity"))
      {
        p.ShowLogPm("GravityCantUseMove", p.SelectedMove.Type.Id);
        return false;
      }
      return true;
    }
    private static bool Attract(PokemonProxy p)
    {
      var pm = p.OnboardPokemon.GetCondition<PokemonProxy>("Attract");
      if (pm != null)
      {
        p.ShowLogPm("Attract", pm.Id);
        if (p.Controller.RandomHappen(50))
        {
          p.ShowLogPm("AttractWork");
          return false;
        }
      }
      return true;
    }
    private static bool Paralyzed(PokemonProxy p)
    {
      if (p.State == PokemonState.PAR && p.Controller.OneNth(4))
      {
        p.ShowLogPm("PARWork");
        return false;
      }
      return true;
    }
    private static bool FocusPunch(PokemonProxy p)
    {
      if (p.SelectedMove.Type.Id == Ms.FOCUS_PUNCH && p.OnboardPokemon.HasCondition("Damage"))
      {
        p.ShowLogPm("DeFocusPunch");
        return false;
      }
      return true;
    }
  }
}
