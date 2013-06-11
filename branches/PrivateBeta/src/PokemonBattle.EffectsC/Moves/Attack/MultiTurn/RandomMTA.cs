﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ms = LightStudio.PokemonBattle.Game.Host.Sp.Moves;

namespace LightStudio.PokemonBattle.Game.Host.Effects.Moves.Attack
{
  class RandomMTA : AttackMoveE
  {
    public RandomMTA(int id)
      : base(id)
    {
    }

    public override void InitAtkContext(AtkContext atk)
    {
      atk.SetCondition("MultiTurn", new Condition() { Turn = atk.Controller.GetRandomInt(2, 3), Bool = true });
    }
  }
}