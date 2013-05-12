using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Data;
using LightStudio.PokemonBattle.Game;

namespace LightStudio.PokemonBattle.Game.Host.Effects.Moves.Attack
{
  class Leap : AttackMoveE
  {
    private readonly CoordY Y;
    
    public Leap(int id, CoordY y)
      : base(id)
    {
      Y = y;
    }
    
    protected override bool PrepareOneTurn(AtkContext atk)
    {
      var pm = atk.Attacker;
      if (pm.Action == PokemonAction.MoveAttached)
      {
        pm.Controller.ReportBuilder.Add(GameEvents.PositionChange.Leap("Prepare" + Move.Id.ToString(), pm, Y));
        atk.SetAttackerAction(PokemonAction.Moving);
        pm.OnboardPokemon.CoordY = Y;
        return true;
      }
      return false;
    }
    protected override void ImplementEffect(DefContext def)
    {
      if (!Move.Flags.Protectable && def.Defender.OnboardPokemon.RemoveCondition("Protect")) def.Defender.AddReportPm("DeProtect");
    }
  }
  class SolarBeam : AttackMoveE
  {
    public SolarBeam(int id)
      : base(id)
    {
    }

    protected override bool PrepareOneTurn(AtkContext atk)
    {
      return base.PrepareOneTurn(atk) && atk.Controller.Weather != Weather.IntenseSunlight;
    }
  }
  class SkullBash : AttackMoveE
  {
    public SkullBash(int id)
      : base(id)
    {
    }
    protected override bool PrepareOneTurn(AtkContext atk)
    {
      if (base.PrepareOneTurn(atk))
      {
        atk.Attacker.ChangeLv7D(atk.Attacker, false, 0, 1);
        return true;
      }
      return false;
    }
  }
}
