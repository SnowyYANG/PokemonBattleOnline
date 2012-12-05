using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightStudio.PokemonBattle.Game.Host.Effects.Moves.Status
{
  class Gravity : StatusMoveE
  {
    public Gravity(int id)
      : base(id)
    {
    }
    protected override void Act(AtkContext atk)
    {
      var c = atk.Controller;
      if (c.Board.AddCondition("Gravity", c.TurnNumber + 4))
      {
        c.ReportBuilder.Add("EnGravity");
        foreach (var pm in c.Board.Pokemons)
        {
          if (pm.OnboardPokemon.CoordY == CoordY.Air)
          {
            pm.OnboardPokemon.CoordY = CoordY.Plate;
            pm.CancelMove();
            pm.OnboardPokemon.RemoveCondition("SkyDrop");
            goto SHOW;
          }
          if (!EffectsService.IsGroundAffectable.Execute(pm, true, false, false)) continue;
        SHOW:
          pm.AddReportPm("Gravity");
        }
      }
      else atk.FailAll();
    }
  }
}
