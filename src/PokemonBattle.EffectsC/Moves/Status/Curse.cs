using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Data;

namespace LightStudio.PokemonBattle.Game.Host.Effects.Moves.Status
{
  class Curse : StatusMoveE
  {
    public Curse(int id)
      : base(id)
    {
    }

    protected override void Act(AtkContext atk)
    {
      if (atk.Attacker.OnboardPokemon.HasType(BattleType.Ghost))
      {
        if (atk.Target.Defender.OnboardPokemon.SetCondition("Curse"))
        {
          atk.Attacker.Hp -= atk.Attacker.Pokemon.Hp.Origin >> 1;
          atk.Controller.ReportBuilder.Add(new GameEvents.HpChange(atk.Attacker, "EnCurse", atk.Target.Defender.Id));
        }
        else atk.Attacker.AddReportPm("Fail0");
      }
      else atk.Attacker.ChangeLv7D(atk.Attacker, true, 1, 1, 0, 0, -1);
    }
    protected override MoveRange GetRange(AtkContext atk)
    {
      return atk.Attacker.OnboardPokemon.HasType(BattleType.Ghost) ? MoveRange.Single : MoveRange.User;
    }
  }
}
