using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Data;
using LightStudio.PokemonBattle.Game.Host.Sp;
using LightStudio.PokemonBattle.Game.Host.Triggers;
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
          //懒惰
        pm.OnboardPokemon.GetCondition("Disable").CanExecute() &&
        Imprison(pm) &&
        pm.OnboardPokemon.GetCondition("HealBlock").CanExecute() &&
        Confuse(pm) &&
        Flinch(pm) &&
          //挑拨 
          //重力  
        Infatuation(pm) &&
        Paralyzed(pm);
    }
    private void AddResetYReport(PokemonProxy p, string key)
    {
      p.Controller.ReportBuilder.Add(PositionChange.Reset(key, p));
    }
    private bool Sleeping(PokemonProxy pm)
    {
      if (pm.State == PokemonState.Sleeping)
      {
        int count = pm.OnboardPokemon.GetCondition<int>("Sleeping");
        count--;
        if (pm.Ability.EarlyBird()) count--;
        pm.OnboardPokemon.SetCondition("Sleeping", count);
        if (count <= 0) pm.State = PokemonState.Normal; //auto Remove
        else
        {
          AddResetYReport(pm, "Sleeping");
          if (!pm.SelectedMove.AvailableEvenSleeping()) return false;
        }
      }
      return true;
    }
    private bool Frozen(PokemonProxy p)
    {
      if (p.State == PokemonState.Frozen)
      {
        if (p.SelectedMove.Type.AdvancedFlags.AvailableEvenFrozen || p.Controller.GetRandomInt(0, 3) == 0) p.State = PokemonState.Normal;
        else
        {
          AddResetYReport(p, "Frozen");
          return false;
        }
      }
      return true;
    }
    private bool Imprison(PokemonProxy p)
    {
      MoveType move = p.SelectedMove.Type;
      foreach (PokemonProxy pm in p.Controller.Board[1 - p.Pokemon.TeamId].GetPokemons(p.OnboardPokemon.X - 1, p.OnboardPokemon.X + 1))
        if (pm.OnboardPokemon.HasCondition("Imprison"))
          foreach (MoveProxy m in pm.Moves)
            if (m.Type == move)
            {
              p.Controller.ReportBuilder.Add(
                GameEvents.PositionChange.Reset("Imprison", p, move));
              return false;
            }
      return true;
    }
    private bool Confuse(PokemonProxy pm)
    {
      int count = pm.OnboardPokemon.GetCondition<int>("Confuse");
      if (count != 0)
      {
        if (--count > 0)
        {
          pm.AddReportPm("Confused");
          pm.OnboardPokemon.SetCondition("Confuse", count);
        }
        else
        {
          pm.OnboardPokemon.RemoveCondition("confuse");
          pm.AddReportPm("DeConfused");
        }
        if (pm.Controller.OneNth(2))
        {
          var e = new GameEvents.HpChange(pm, "ConfusedWork") { ResetY = true };
          pm.Controller.ReportBuilder.Add(e);
          pm.MoveHurt((pm.Pokemon.Lv * 2 / 5 + 2) * 40 * OnboardPokemon.Get5D(pm.OnboardPokemon.Static.Atk, pm.OnboardPokemon.Lv5D.Atk) / OnboardPokemon.Get5D(pm.OnboardPokemon.Static.Def, pm.OnboardPokemon.Lv5D.Def) / 50 + 2);
          e.Hp = pm.Hp;
          if (!pm.CheckFaint()) pm.Item.HpChanged(pm);
          return false;
        }
      }
      return true;
    }
    private bool Flinch(PokemonProxy pm)
    {
      if (pm.OnboardPokemon.HasCondition("Flinch"))
      {
        AddResetYReport(pm, "Flinch");
        if (pm.RaiseAbility(As.STEADFAST)) pm.ChangeLv7D(pm, false, 0, 0, 0, 0, 1);
        return false;
      }
      return true;
    }
    private bool Infatuation(PokemonProxy p)
    {
      var pm = p.OnboardPokemon.GetCondition<PokemonProxy>("Infatuation");
      if (pm != null)
      {
        p.AddReportPm("Infatuation", pm);
        if (p.Controller.RandomHappen(50))
        {
          p.AddReportPm("InfatuationWork");
          return false;
        }
      }
      return true;
    }
    private bool Paralyzed(PokemonProxy p)
    {
      if (p.State == PokemonState.Paralyzed)
      {
        if (p.Controller.OneNth(4))
        {
          AddResetYReport(p, "ParalyzedWork");
          return false;
        }
      }
      return true;
    }
  }
}
