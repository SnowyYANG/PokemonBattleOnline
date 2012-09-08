using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Data;

namespace LightStudio.PokemonBattle.Game.Host.Effects.Moves.Status
{
  class Conversion2 : StatusMoveE
  {
    public Conversion2(int id)
      : base(id)
    {
    }

    protected override void Act(AtkContext atk)
    {
      var con = atk.Target.Defender.OnboardPokemon.GetCondition("LastMove");
      if (con == null) FailAll(atk);
      else
      {
        BattleType a = con.Move.Type;
        var types = (from t in (BattleType[])Enum.GetValues(typeof(BattleType))
                     where
                     t != BattleType.Invalid &&
                     (
                     AttackMoveE.CalculateEffectRevise(a, t, BattleType.Invalid) < 0 ||
                     (a == BattleType.Normal || a == BattleType.Fighting) && t == BattleType.Ghost ||
                     a == BattleType.Electric && t == BattleType.Ground ||
                     a == BattleType.Poison && t == BattleType.Steel ||
                     a == BattleType.Psychic && t == BattleType.Dark ||
                     a == BattleType.Ghost && t == BattleType.Normal ||
                     a == BattleType.Ground && t == BattleType.Flying
                     )
                     select t).ToArray();
        var type = types[atk.Controller.GetRandomInt(0, types.Length - 1)];
        atk.Attacker.OnboardPokemon.Type1 = type;
        atk.Attacker.OnboardPokemon.Type2 = BattleType.Invalid;
        atk.Attacker.AddReportPm("TypeChange", type);
      }
    }
  }
}
