using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightStudio.PokemonBattle.Game.Sp.Conditions
{
  public class Confuse : PmCondition
  {
    public Confuse(PokemonProxy pm, int count)
      : base("Confuse", pm, count)
    {
    }
    
    public override bool CanExecute()
    {
      if (--count > 0) AddReportPm("Confuse");
      else
      {
        Remove();
        AddReportPm("DeConfuse");
      } 
      if (pm.Controller.OneNth(2))
      {
        //自伤，送回战斗平面
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
      AddReportPm("Flinch");
      Abilities.CheckSteadfast(pm);
      return false;
    }
  }
}
