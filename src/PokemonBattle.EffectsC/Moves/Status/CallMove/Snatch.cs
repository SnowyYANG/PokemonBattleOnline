using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightStudio.PokemonBattle.Game.Host.Effects.Moves.Status
{
  class Snatch : StatusMoveE
  {
    public Snatch(int id)
      : base(id)
    {
    }
    public override void Execute(AtkContext atk)
    {
      atk.Attacker.OnboardPokemon.SetTurnCondition("Snatch");
      atk.Attacker.AddReportPm("EnSnatch");
      atk.Attacker.Action = PokemonAction.Done;
    }
  }
}
