﻿using System;
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
                     where a.EffectRevise(t) < 0 || a.NoEffect(t) //自动排除Invalid
                     select t).ToArray();
        var type = types[atk.Controller.GetRandomInt(0, types.Length - 1)];
        atk.Attacker.OnboardPokemon.Type1 = type;
        atk.Attacker.OnboardPokemon.Type2 = BattleType.Invalid;
        atk.Attacker.AddReportPm("TypeChange", type);
      }
    }
  }
}
