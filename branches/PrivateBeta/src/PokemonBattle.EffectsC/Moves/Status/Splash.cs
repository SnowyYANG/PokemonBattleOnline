using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Data;

namespace LightStudio.PokemonBattle.Game.Host.Effects.Moves.Status
{  
  class Splash : StatusMoveE
  {
    public Splash(int id)
      : base(id)
    {
    }

    protected override void Act(AtkContext atk)
    {
      atk.Controller.ReportBuilder.Add("Splash");
    }
  }
  
}
