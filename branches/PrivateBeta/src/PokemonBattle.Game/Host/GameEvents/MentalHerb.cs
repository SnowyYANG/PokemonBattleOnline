﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace LightStudio.PokemonBattle.Game.Host
{
  [DataContract(Namespace = Namespaces.PBO)]
  internal class MentalHerbEvent : GameEvent
  {
    [DataMember(EmitDefaultValue = false)]
    int Pm;
    [DataMember(EmitDefaultValue = false)]
    bool Attract;
    [DataMember(EmitDefaultValue = false)]
    bool Encore;
    [DataMember(EmitDefaultValue = false)]
    bool Taunt;
    [DataMember(EmitDefaultValue = false)]
    bool Torment;
    [DataMember(EmitDefaultValue = false)]
    bool Disable;

    public MentalHerbEvent(PokemonProxy pm, bool i, bool e, bool ta, bool to, bool d)
    {
      Pm = pm.Id;
      Attract = i;
      Encore = e;
      Taunt = ta;
      Torment = to;
      Disable = d;
    }

    protected override void Update()
    {
      if (Attract) AppendGameLog("ItemDeAttract", Pm, 8);
      if (Encore) AppendGameLog("DeEncore", Pm);
      if (Taunt) AppendGameLog("DeTaunt", Pm);
      if (Torment) AppendGameLog("DeTorment", Pm);
      if (Disable) AppendGameLog("DeDisable", Pm);
    }
    public override void Update(SimGame game)
    {
      var pm = GetPokemon(game, Pm);
      if (pm != null) pm.Item = null;
    }
  }
}