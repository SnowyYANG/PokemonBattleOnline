using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using LightStudio.PokemonBattle.Data;
using LightStudio.PokemonBattle.Game.Host;

namespace LightStudio.PokemonBattle.Game.GameEvents
{
  [DataContract(Namespace = Namespaces.PBO)]
  public class AbilityEvent : GameEvent
  {
    [DataMember(EmitDefaultValue = false)]
    string Log;
    
    [DataMember]
    int Pm;

    [DataMember(EmitDefaultValue = false)]
    int OldAb;

    [DataMember]
    int Ab;

    [DataMember(EmitDefaultValue = false)]
    public int Arg3;

    internal AbilityEvent(PokemonProxy pm)
    {
      Pm = pm.Id;
      Ab = pm.OnboardPokemon.Ability;
    }
    internal AbilityEvent(PokemonProxy pm, string log, int fromId, int toId)
    {
      Pm = pm.Id;
      Log = log == "SetAbility" ? null : log;
      OldAb = fromId;
      Ab = toId;
    }

    protected override void Update()
    {
      var pm = GetPokemon(Pm);
      var ab = GameDataService.GetAbility(Ab);
      if (OldAb == 0)
      {
        Game.Board.ShowAbility(pm, ab);
        AppendGameLog("Ability", Pm, Ab);
      }
      else
      {
        Game.Board.AbilityChanged(pm, GameDataService.GetAbility(OldAb), ab);
        AppendGameLog(Log ?? "SetAbility", Pm, Ab, OldAb, Arg3);
      }
    }
  }
}
