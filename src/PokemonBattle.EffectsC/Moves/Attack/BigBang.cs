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
    private static void Suicide(PokemonProxy pm)
    {
      pm.Pokemon.SetHp(0);
      pm.Controller.ReportBuilder.Add(new GameEvents.HpChange(pm, null));
      pm.CheckFaint();
    }

    public BigBang(int id)
      : base(id)
    {
    }

    public override void Execute(AtkContext atk)
    {
      if (atk.Controller.Board.Pokemons.Any((p) => p.RaiseAbility(As.DAMP))) FailAll(atk, "FailSp", atk.Attacker.Id, Move.Id);
      else
      {
        base.Execute(atk);
        if (atk.FailAll) Suicide(atk.Attacker);
      }
    }
    protected override void CalculateDamages(AtkContext atk)
    {
      base.CalculateDamages(atk);
      Suicide(atk.Attacker);
    }
  }
}
