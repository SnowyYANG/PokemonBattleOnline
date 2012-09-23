using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightStudio.PokemonBattle.Game.Host.Effects.Moves.Status
{
  class Conversion : StatusMoveE
  {
    public Conversion(int id)
      : base(id)
    {
    }
    protected override void Act(AtkContext atk)
    {
      var aer = atk.Attacker;
      var type1 = aer.OnboardPokemon.Type1;
      var type2 = Data.BattleType.Invalid;
      var ms = (from m in aer.Moves
               where !(m.Type.Id == Move.Id || m.Type.Type == type1 || m.Type.Type == type2)
               select m.Type.Type).ToArray();
      if (ms.Length == 0) FailAll(atk);
      else
      {
        var type = ms[aer.Controller.GetRandomInt(0, ms.Length - 1)];
        aer.OnboardPokemon.Type1 = type;
        aer.OnboardPokemon.Type2 = Data.BattleType.Invalid;
        aer.AddReportPm("TypeChange", type);
      }
    }
  }
}
