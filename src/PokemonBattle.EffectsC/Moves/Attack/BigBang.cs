using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Data;
using LightStudio.PokemonBattle.Game.Host.Sp;
using As = LightStudio.PokemonBattle.Game.Host.Sp.Abilities;

namespace LightStudio.PokemonBattle.Game.Host.Effects.Moves.Attack
{
  class BigBang : AttackMoveE
  {
    public BigBang(int id)
      : base(id)
    {
    }

    public override void Execute(AtkContext atk)
    {
      if (atk.Controller.Board.Pokemons.Any((p) => p.RaiseAbility(As.DAMP))) atk.FailAll("FailSp", atk.Attacker.Id, Move.Id);
      else
      {
        base.Execute(atk);
        if (atk.Fail) atk.Attacker.Faint();
      }
    }
    protected override void CalculateDamages(AtkContext atk)
    {
      base.CalculateDamages(atk);
      atk.Attacker.Faint();
    }
  }
}
