using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Data;
using LightStudio.PokemonBattle.Game;

namespace LightStudio.PokemonBattle.Game.Host.Effects.Conditions
{
  public class Taunt : PmCondition
  {
    public Taunt(PokemonProxy pm)
      : base("Taunt", pm, 3)
    {
    }
    
    public override bool CanExecute()
    {
      if (count > 0)
      {
        count--;
        if (pm.SelectedMove.Type.Category == Data.MoveCategory.Status)
        {
          pm.Controller.ReportBuilder.Add("Taunt", pm, pm.SelectedMove.Move.Type.GetLocalizedName());
          return false;
        }
      }
      else
      {
        AddReportPm("DeTaunt");
        Remove();
      }
      return true;
    }
  }
}
