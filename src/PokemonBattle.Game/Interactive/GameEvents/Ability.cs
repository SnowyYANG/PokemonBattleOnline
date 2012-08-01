using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using LightStudio.PokemonBattle.Data;
using LightStudio.PokemonBattle.Game.Host;

namespace LightStudio.PokemonBattle.Game.GameEvents
{
  [DataContract(Namespace = Namespaces.DEFAULT)]
  public class AbilityEvent : GameEvent
  {
    [DataMember]
    int PmId;

    [DataMember(EmitDefaultValue = false)]
    int OldAbId;

    [DataMember]
    int AbId;

    PokemonOutward pm;
    Ability ab;
    Ability oldAb;

    public AbilityEvent(PokemonProxy pm)
    {
      PmId = pm.Id;
      AbId = pm.Ability.Id;
    }
    public AbilityEvent(PokemonProxy pm, int fromId, int toId)
    {
      PmId = pm.Id;
      OldAbId = fromId;
      AbId = toId;
    }

    public override IText GetGameLog()
    {
      IText t = null;
      if (OldAbId == 0)
      {
        t = GetGameLog("Ability");
        t.SetData(pm, ab);
      }
      else
      {
        t = GetGameLog("AbChange");
        t.SetData(pm, oldAb, ab); 
      }
      return t;
    }
    public override void Update(GameOutward game)
    {
      base.Update(game);
      pm = game.GetPokemon(PmId);
      ab = DataService.GetAbility(AbId);
      if (OldAbId == 0) game.Board.ShowAbility(pm, ab);
      else
      {
        oldAb = DataService.GetAbility(OldAbId);
        game.Board.AbilityChanged(pm, oldAb, ab);
      }
    }
  }
}
