﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonBattleOnline.Data;
using As = PokemonBattleOnline.Game.Host.Sp.Abilities;

namespace PokemonBattleOnline.Game.Host.Effects.Moves.Attack
{
  class SmackDown : AttackMoveE
  {
    public SmackDown(int id)
      : base(id)
    {
    }
    
    protected override void PostEffect(DefContext def)
    {
      var der = def.Defender;
      if (der.OnboardPokemon.HasType(BattleType.Flying) || der.Ability.Id == As.LEVITATE)
      {
        der.OnboardPokemon.SetCondition("SmackDown");
        der.AddReportPm("EnSmackDown");
      }
      if (der.OnboardPokemon.RemoveCondition("MagnetRise")) der.AddReportPm("DeMagnetRise");
      if (der.OnboardPokemon.RemoveCondition("Telekinesis")) der.AddReportPm("DeTelekinesis");
    }
  }
}
