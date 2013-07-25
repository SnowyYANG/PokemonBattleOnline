﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using LightStudio.PokemonBattle.Game.Host;

namespace LightStudio.PokemonBattle.Game.GameEvents
{
  [DataContract(Namespace = Namespaces.PBO)]
  class Mimic : GameEvent
  {
    [DataMember]
    int Pm;
    [DataMember]
    int Move;

    public Mimic(PokemonProxy pm, Data.MoveType move)
    {
      Pm = pm.Id;
      Move = move.Id;
    }
    protected override void Update()
    {
      AppendGameLog("Mimic", Pm, Move);
    }
    public override void Update(SimGame game)
    {
      foreach (var pm in game.OnboardPokemons)
        if (pm.Id == Pm && pm.Pokemon.Owner == game.Player) pm.ChangeMove(102, Move);
    }
  }
}