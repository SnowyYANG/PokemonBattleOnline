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
        if (Pm.SelectedMove.Type.Category == Data.MoveCategory.Status)
        {
          Pm.Controller.ReportBuilder.Add("Taunt", Pm, Pm.SelectedMove.Move.Type);
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
