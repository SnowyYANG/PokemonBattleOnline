using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Data;
using LightStudio.PokemonBattle.Game;

namespace LightStudio.PokemonBattle.Game.Host.Effects.Moves
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
        pm.Controller.ReportBuilder.Add(GameEvents.PositionChange.Leap("Prepare" + Move.Id.ToString(), pm, Y));
        pm.Action = PokemonAction.Moving;
        pm.OnboardPokemon.CoordY = Y;
        return true;
      }
      return false;
    }
  }
  class SolarBeam : AttackMoveE
  {
    public SolarBeam(int id)
      : base(id)
    {
    }

    protected override bool PrepareOneTurn(PokemonProxy pm)
    {
      return base.PrepareOneTurn(pm) && pm.Controller.Weather != Weather.IntenseSunlight;
    }
  }
  class SkullBash : AttackMoveE
  {
    public SkullBash(int id)
      : base(id)
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
