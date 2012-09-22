using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightStudio.PokemonBattle.Game.Host.Effects.Moves.Attack
{
  class Counter : AttackMoveE
  {
    readonly string Condition;
    readonly UInt16 Modifier;

    public Counter(int id, string condition, UInt16 modifier)
      : base(id)
    {
      Condition = condition;
      Modifier = modifier;
    }
    protected override bool NotFail(AtkContext atk)
    {
      var o = atk.Attacker.OnboardPokemon.GetCondition(Condition);
      if (o != null)
      {
        var pm = o.By;
        return pm.Tile != null && pm.Pokemon.TeamId != atk.Attacker.Pokemon.TeamId;
      }
      return false;
    }
    protected override void CalculateTargets(AtkContext atk)
    {
      atk.SetTargets(new DefContext[] { new DefContext(atk, atk.Attacker.OnboardPokemon.GetCondition(Condition).By) });
    }
    protected override void CalculateDamage(DefContext def)
    {
      def.Damage = def.AtkContext.Attacker.OnboardPokemon.GetCondition(Condition).Damage;
      def.ModifyDamage(Modifier);
    }
  }
}
