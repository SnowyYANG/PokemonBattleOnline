using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokemonBattleOnline.Game.Host.Effects.Moves.Attack
{
  class Counter : AttackMoveE
  {
    readonly string Condition;
    readonly Modifier Modifier;

    public Counter(int id, string condition, Modifier modifier)
      : base(id)
    {
      Condition = condition;
      Modifier = modifier;
    }
    protected override void BuildDefContext(AtkContext atk, Tile select)
    {
      var o = atk.Attacker.OnboardPokemon.GetCondition(Condition);
      if (o != null)
      {
        var pm = o.By;
        if (pm.Tile != null && pm.Pokemon.TeamId != atk.Attacker.Pokemon.TeamId)
        {
          atk.SetTargets(new DefContext[] { new DefContext(atk, pm) });
          return;
        }
      }
      atk.SetTargets(new DefContext[0]);
    }
    protected override void FilterDefContext(AtkContext atk)
    {
    }
    protected override void CalculateDamage(DefContext def)
    {
      def.Damage = def.AtkContext.Attacker.OnboardPokemon.GetCondition(Condition).Damage;
      def.ModifyDamage(Modifier);
    }
  }
}
