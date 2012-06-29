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
