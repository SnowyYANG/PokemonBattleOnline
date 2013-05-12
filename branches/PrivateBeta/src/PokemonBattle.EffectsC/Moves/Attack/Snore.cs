using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightStudio.PokemonBattle.Game.Host.Effects.Moves.Attack
{
  class Snore : AttackMoveE
  {
    public Snore(int id)
      : base(id)
    {
    }
    protected override bool NotFail(AtkContext atk)
    {
      return atk.Attacker.State == PokemonState.SLP && base.NotFail(atk);
    }
  }
}
