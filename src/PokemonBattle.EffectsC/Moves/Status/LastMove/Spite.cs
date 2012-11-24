using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightStudio.PokemonBattle.Game.Host.Effects.Moves.Status
{
  class Spite : StatusMoveE
  {
    public Spite(int id)
      : base(id)
    {
    }
    protected override void Act(AtkContext atk)
    {
      var der = atk.Target.Defender;
      var move = der.LastMove;
      foreach(var m in der.Moves)
        if (m.Type == move)
        {
          if (m.PP != 0)
          {
            var fp = m.PP;
            m.PP -= 4;
            atk.Controller.ReportBuilder.Add(new GameEvents.PPChange("Spite", m, fp));
            return;
          }
          break;
        }
      FailAll(atk);
    }
  }
}
