using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightStudio.PokemonBattle.Game.Host.Sp.Conditions
{
  public class Confused : PmCondition
  {
    public Confused(PokemonProxy pm, int count)
      : base("Confused", pm, count)
    {
    }
    
    public override bool CanExecute()
    {
      if (--count > 0) AddReportPm("Confused");
      else
      {
        Remove();
        AddReportPm("DeConfused");
      } 
      if (Pm.Controller.OneNth(2))
      {
        var e = new GameEvents.HpChange(Pm, "ConfusedWork") { ResetY = true };
        Pm.Controller.ReportBuilder.Add(e);
        Pm.MoveHurt((Pm.Pokemon.Lv * 2 / 5 + 2) * 40 * OnboardPokemon.Get5D(Pm.OnboardPokemon.Static.Atk, Pm.OnboardPokemon.Lv5D.Atk) / OnboardPokemon.Get5D(Pm.OnboardPokemon.Static.Def, Pm.OnboardPokemon.Lv5D.Def) / 50 + 2);
        e.Hp = Pm.Hp;
        if (!Pm.CheckFaint()) Pm.Item.HpChanged(Pm);
        return false;
      }
      return true;
    }
  }
  public class Flinch : PmCondition
  {
    public Flinch(PokemonProxy pm)
      : base("Flinch", pm)
    {
    }
    
    public override bool CanExecute()
    {
      AddResetYReport("Flinch");
      Abilities.Steadfast(Pm);
      return false;
    }
  }
}
