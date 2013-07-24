using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace LightStudio.PokemonBattle.Game.Host
{
  [DataContract(Namespace = Namespaces.PBO)]
  class MimicEvent : GameEvent
  {
    [DataMember]
    int Pm;
    [DataMember]
    int Move;

    public MimicEvent(PokemonProxy pm, Data.MoveType move)
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
