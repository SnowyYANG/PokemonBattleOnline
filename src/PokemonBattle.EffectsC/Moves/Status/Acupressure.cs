using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Data;

namespace LightStudio.PokemonBattle.Game.Host.Effects.Moves.Status
{
  class Acupressure : StatusMoveE
  {
    private static readonly StatType[] SEVEN_D = { StatType.Atk, StatType.Def, StatType.SpAtk, StatType.SpDef, StatType.Speed, StatType.Accuracy, StatType.Evasion };

    public Acupressure(int id)
      : base(id)
    {
    }
    protected override void Act(AtkContext atk)
    {
      var der = atk.Target.Defender;
      var aer = atk.Attacker;
      StatType[] ss = SEVEN_D.Where((s) => der.CanChangeLv7D(aer, s, 2, false) != 0).ToArray();
      if (ss.Length == 0) FailAll(atk);
      else der.ChangeLv7D(aer, ss[aer.Controller.GetRandomInt(0, ss.Length - 1)], 2, true);
    }
  }
}
