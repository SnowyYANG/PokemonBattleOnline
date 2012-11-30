using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Data;
using LightStudio.PokemonBattle.Game.Host.Sp;
using LightStudio.PokemonBattle.Game.GameEvents;
using As = LightStudio.PokemonBattle.Game.Host.Sp.Abilities;

namespace LightStudio.PokemonBattle.Game.Host.Effects.Triggers
{
  class CanExecute : ICanExecute
  {
    public bool Execute(PokemonProxy pm)
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
    private static void AddResetYReport(PokemonProxy p, string key, int arg1 = 0)
    {
      p.Controller.ReportBuilder.Add(PositionChange.Reset(key, p, arg1));
    }
    private static bool Sleeping(PokemonProxy pm)
    {
      if (pm.State == PokemonState.SLP)
      {
        int count = pm.OnboardPokemon.GetCondition<int>("SLP");
        count--;
        if (pm.Ability.EarlyBird()) count--;
        if (count <= 0)
        {
          pm.OnboardPokemon.RemoveCondition("SLP");
          pm.DeAbnormalState();
        }
        else
        {
          pm.OnboardPokemon.SetCondition("SLP", count);
          AddResetYReport(pm, "SLP");
          return pm.SelectedMove.AvailableEvenSleeping();
        }
      }
      return true;
    }
    private static bool Frozen(PokemonProxy p)
    {
      if (p.State == PokemonState.FRZ)
      {
        if (p.SelectedMove.Type.Flags.AvailableEvenFrozen)
        {
          p.Pokemon.State = PokemonState.Normal;
          p.Controller.ReportBuilder.Add(new StateChange(p, "DeFRZ2", p.SelectedMove.Type.Id));
        }
        else if (p.Controller.GetRandomInt(0, 3) == 0) p.DeAbnormalState();
        else
        {
          AddResetYReport(p, "FRZ");
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
        AddResetYReport(p, "Disable", p.SelectedMove.Type.Id);
        return false;
      }
      return true;
    }
    private static bool Truant(PokemonProxy p)
    {
      if (p.Ability.Id == As.TRUANT)
      {
        if (p.OnboardPokemon.GetCondition<int>("Truant") == p.Controller.TurnNumber)
        {
          p.RaiseAbility();
          p.AddReportPm("Truant");
          return false;
        }
        p.OnboardPokemon.AddCondition("Truant", p.Controller.TurnNumber + 1);
      }
      return true;
    }
    private static bool Imprison(PokemonProxy p)
    {
      MoveType move = p.SelectedMove.Type;
      foreach (PokemonProxy pm in p.Tile.Field.GetPokemons(p.OnboardPokemon.X - 1, p.OnboardPokemon.X + 1))
        if (pm.OnboardPokemon.HasCondition("Imprison"))
          foreach (MoveProxy m in pm.Moves)
            if (m.Type == move)
            {
              AddResetYReport(p, "Imprison", move.Id);
              return false;
            }
      return true;
    }
    private static bool HealBlock(PokemonProxy pm)
    {
      if (pm.SelectedMove.Move.Type.Flags.IsHeal && pm.OnboardPokemon.HasCondition("HealBlock"))
      {
        AddResetYReport(pm, "HealBlockCantUseMove", pm.SelectedMove.Move.Type.Id);
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
          pm.AddReportPm("Confuse");
          pm.OnboardPokemon.SetCondition("Confuse", count);
          if (pm.Controller.OneNth(2))
          {
            var e = new GameEvents.HpChange(pm, "ConfuseWork") { ResetY = true };
            pm.Controller.ReportBuilder.Add(e);
            pm.MoveHurt((pm.Pokemon.Lv * 2 / 5 + 2) * 40 * OnboardPokemon.Get5D(pm.OnboardPokemon.FiveD.Atk, pm.OnboardPokemon.Lv5D.Atk) / OnboardPokemon.Get5D(pm.OnboardPokemon.FiveD.Def, pm.OnboardPokemon.Lv5D.Def) / 50 + 2);
            e.Hp = pm.Hp;
            //if (!pm.CheckFaint()) pm.Item.HpChanged(pm); //◇硝子玩偶◇ 22:21:00 你知道混乱打自己的时候不触发加HP的果子么
            return false;
          }
        }
        else
        {
          pm.OnboardPokemon.RemoveCondition("Confuse");
          pm.AddReportPm("DeConfuse");
        }
      return true;
    }
    private static bool Flinch(PokemonProxy pm)
    {
      if (pm.OnboardPokemon.HasCondition("Flinch"))
      {
        AddResetYReport(pm, "Flinch");
        if (pm.RaiseAbility(As.STEADFAST)) pm.ChangeLv7D(pm, false, 0, 0, 0, 0, 1);
        return false;
      }
      return true;
    }
    private static bool Taunt(PokemonProxy p)
    {
      if (p.SelectedMove.Type.Category == MoveCategory.Status && p.OnboardPokemon.HasCondition("Taunt"))
      {
        AddResetYReport(p, "Taunt", p.SelectedMove.Type.Id);
        return false;
      }
      return true;
    }
    private static bool Gravity(PokemonProxy p)
    {
      if (p.SelectedMove.Type.Flags.UnavailableWithGravity && p.Controller.Board.HasCondition("Gravity"))
      {
        AddResetYReport(p, "GravityCantUseMove", p.SelectedMove.Type.Id);
        return false;
      }
      return true;
    }
    private static bool Attract(PokemonProxy p)
    {
      var pm = p.OnboardPokemon.GetCondition<PokemonProxy>("Attract");
      if (pm != null)
      {
        p.AddReportPm("Attract", pm);
        if (p.Controller.RandomHappen(50))
        {
          p.AddReportPm("AttractWork");
          return false;
        }
      }
      return true;
    }
    private static bool Paralyzed(PokemonProxy p)
    {
      if (p.State == PokemonState.PAR && p.Controller.OneNth(4))
      {
        AddResetYReport(p, "PARWork");
        return false;
      }
      return true;
    }
    private static bool FocusPunch(PokemonProxy p)
    {
      if (p.SelectedMove.FocusPunch() && p.OnboardPokemon.HasCondition("Damage"))
      {
        p.AddReportPm("DeFocusPunch");
        return false;
      }
      return true;
    }
  }
}
