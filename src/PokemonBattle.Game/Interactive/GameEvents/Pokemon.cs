using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using LightStudio.Tactic.DataModels;
using LightStudio.PokemonBattle.Data;
using LightStudio.PokemonBattle.Game;

namespace LightStudio.PokemonBattle.Interactive.GameEvents
{
  [DataContract(Namespace = Namespaces.DEFAULT)]
  internal class PokemonStateChange : GameEvent
  {
    [DataMember]
    int Id;
    [DataMember(EmitDefaultValue = false)]
    PokemonState State;
    
    public PokemonStateChange(PokemonProxy pm)
    {
      Id = pm.Id;
      State = pm.Pokemon.State;
    }

    PokemonState oldState;
    PokemonOutward pm;
    public override void Update(SimGame game)
    {
      base.Update(game);
    }
    public override void Update(Game.GameOutward game)
    {
      pm = game.GetPokemon(Id);
      oldState = pm.State;
      pm.State = State;
    }
    public override IText GetGameLog()
    {
      string key = null;
      if (State == PokemonState.Normal)
        switch (oldState)
        {
          case PokemonState.BadlyPoisoned:
          case PokemonState.Poisoned:
            key = "DePosioned";
            //免疫
            break;
          case PokemonState.Burned:
          case PokemonState.Frozen:
          case PokemonState.Paralyzed:
          case PokemonState.Sleeping:
            key = "De" + oldState.ToString();
            break;
        }
      else
        if (State == PokemonState.Paralyzed) key = "Paralyzed";
        else key = "En" + State.ToString();
      IText t = GameService.Logs[key];
      t.SetData(pm);
      return t;
    }
  }
}
