﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonBattleOnline.Data;

namespace PokemonBattleOnline.Game.Host.Effects.Moves.Status
{
  class Conversion2 : StatusMoveE
  {
    public Conversion2(int id)
      : base(id)
    {
    }

    protected override void Act(AtkContext atk)
    {
      if (atk.Target.Defender.AtkContext == null) atk.FailAll();
      else
      {
        BattleType a = atk.Target.Defender.AtkContext.Type;
        if (a == BattleType.Invalid) a = BattleType.Normal;
        BattleType type1 = atk.Attacker.OnboardPokemon.Type1;
        BattleType type2 = atk.Attacker.OnboardPokemon.Type2;
        var types = (from t in (BattleType[])Enum.GetValues(typeof(BattleType))
                     where !(t == type1 || t == type2) && (a.EffectRevise(t) < 0 || a.NoEffect(t)) //自动排除Invalid
                     select t).ToArray();
        if (types.Length != 0)
        {
          var type = types[atk.Controller.GetRandomInt(0, types.Length - 1)];
          atk.Attacker.OnboardPokemon.Type1 = type;
          atk.Attacker.OnboardPokemon.Type2 = BattleType.Invalid;
          atk.Attacker.AddReportPm("TypeChange", type);
          return;
        }
      }
    }
  }
}