using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightStudio.PokemonBattle.Game.Host.Effects.Moves.Attack
{
  class Feint : AttackMoveE
  {
    public Feint(int id)
      : base(id)
    {
    }
    protected override void CalculateDamages(AtkContext atk)
    {
      var der = atk.Target.Defender;
      bool show = der.OnboardPokemon.RemoveCondition("Protect") | (der.Pokemon.TeamId != atk.Attacker.Pokemon.TeamId && (der.Tile.Field.RemoveCondition("QuickGuard") | der.Tile.Field.RemoveCondition("WideGuard")));
      base.CalculateDamages(atk);
      if (show) der.AddReportPm("Feint");
    }
  }
}
