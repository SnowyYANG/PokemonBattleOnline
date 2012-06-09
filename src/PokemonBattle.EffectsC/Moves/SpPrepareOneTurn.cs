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
    private CoordY y;
    
    public Leap(int id, CoordY y)
      : base(id)
    {
    }
    
    protected override bool PrepareOneTurn(PokemonProxy pm)
    {
      if (base.PrepareOneTurn(pm))
      {
        pm.OnboardPokemon.CoordY = y;
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
