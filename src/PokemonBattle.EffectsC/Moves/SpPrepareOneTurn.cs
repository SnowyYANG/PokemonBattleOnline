using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Data;
using LightStudio.PokemonBattle.Game;

namespace LightStudio.PokemonBattle.Effects.Moves
{
  class Leap : AttackMoveE
  {
    private readonly CoordY Y;
    
    public Leap(int id, CoordY y)
      : base(id)
    {
      Y = y;
    }
    
    protected override bool PrepareOneTurn(PokemonProxy pm)
    {
      if (pm.Action == PokemonAction.MoveAttached)
      {
        pm.Controller.ReportBuilder.Add(new Interactive.GameEvents.PositionChange("Prepare" + Move.Id.ToString(), pm, Y));
        pm.Action = PokemonAction.Moving;
        pm.OnboardPokemon.CoordY = Y;
        return true;
      }
      return false;
    }
  }
  class SolarBeam : AttackMoveE
  {
    public SolarBeam()
      : base(76)
    {
    }

    protected override bool PrepareOneTurn(PokemonProxy pm)
    {
      return base.PrepareOneTurn(pm) && pm.Controller.GetAvailableWeather() != Weather.IntenseSunlight;
    }
  }
  class SkullBash : AttackMoveE
  {
    public SkullBash()
      : base(130)
    {
    }
    protected override bool PrepareOneTurn(PokemonProxy pm)
    {
      if (base.PrepareOneTurn(pm))
      {
        pm.ChangeLv7D(pm, 0, 1);
        return true;
      }
      return false;
    }
  }
}
