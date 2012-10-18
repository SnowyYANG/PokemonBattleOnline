﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightStudio.PokemonBattle.Game.Host.Effects.Moves.Attack
{
  class AttackAndForceSwitch : AttackMoveE
  {
    public AttackAndForceSwitch(int id)
      : base(id)
    {
    }
    
    protected override void ImplementEffect(DefContext def)
    {
      base.ImplementEffect(def);
      MoveE.ForceSwitch(def);
    }
  }
}
