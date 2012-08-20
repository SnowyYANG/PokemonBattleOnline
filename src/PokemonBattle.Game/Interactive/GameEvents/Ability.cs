using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using LightStudio.PokemonBattle.Data;
using LightStudio.PokemonBattle.Game.Host;

namespace LightStudio.PokemonBattle.Game.GameEvents
{
  [DataContract(Namespace = Namespaces.LIGHT)]
  public class AbilityEvent : GameEvent
  {
    [DataMember]
    int Pm;

    [DataMember(EmitDefaultValue = false)]
    int OldAb;

    [DataMember]
    int Ab;

    public AbilityEvent(PokemonProxy pm)
    {
      Pm = pm.Id;
      Ab = pm.OnboardPokemon.Ability;
    }
    public AbilityEvent(PokemonProxy pm, int fromId, int toId)
    {
      Pm = pm.Id;
      OldAb = fromId;
      Ab = toId;
    }

    protected override void Update()
    {
      var pm = GetPokemon(Pm);
      var ab = DataService.GetAbility(Ab);
      if (OldAb == 0)
      {
        Game.Board.ShowAbility(pm, ab);
        AppendGameLog("Ability", Pm, Ab);
      }
      else
      {
        Game.Board.AbilityChanged(pm, DataService.GetAbility(OldAb), ab);
        AppendGameLog("AbChange", Pm, OldAb, Ab);
      }
    }
  }
}
