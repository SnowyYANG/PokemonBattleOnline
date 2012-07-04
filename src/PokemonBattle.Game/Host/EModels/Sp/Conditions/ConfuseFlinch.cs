using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightStudio.PokemonBattle.Game.Sp.Conditions
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
      if (pm.Controller.OneNth(2))
      {
        var e = new Interactive.GameEvents.HpChange(pm, "ConfusedWork", true);
        pm.MoveHurt((pm.Pokemon.Lv * 2 / 5 + 2) * 40 * OnboardPokemon.Get5D(pm.OnboardPokemon.Static.Atk, pm.OnboardPokemon.Lv5D.Atk) / OnboardPokemon.Get5D(pm.OnboardPokemon.Static.Def, pm.OnboardPokemon.Lv5D.Def) / 50 + 2);
        e.Hp = pm.Hp;
        if (!pm.CheckFaint()) pm.Item.HpChanged(pm);
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
      Abilities.CheckSteadfast(pm);
      return false;
    }
  }
}
