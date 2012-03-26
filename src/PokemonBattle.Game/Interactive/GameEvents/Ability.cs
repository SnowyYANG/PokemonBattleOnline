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
  public class AbilityEvent : GameEvent
  {
    [DataMember]
    int PmId;

    [DataMember(EmitDefaultValue = false)]
    string LogKey;
    
    [DataMember(EmitDefaultValue = false)]
    int OldAbId;

    [DataMember]
    int AbId;

    PokemonOutward pm;
    Ability ab;
    Ability oldAb;

    public AbilityEvent(PokemonProxy pm, Ability ab, string logKey)
    {
      PmId = pm.Id;
      AbId = ab.Id;
      LogKey = logKey;
    }
    public AbilityEvent(PokemonProxy pm, Ability from, Ability to)
    {
      PmId = pm.Id;
      OldAbId = from.Id;
      AbId = to.Id;
    }

    public override IText GetGameLog()
    {
      IText t = null;
      if (OldAbId == 0)
      {
        t = GetGameLog("AbChange");
        t.SetData(pm.Name, oldAb, ab); 
      }
      else if (LogKey != null)
      {
        t = GetGameLog(LogKey);
        t.SetData(pm.Name);
      }
      return t;
    }
    public override void Update(GameOutward game)
    {
      pm = game.GetPokemon(PmId);
      ab = DataService.GetAbility(AbId);
      oldAb = DataService.GetAbility(OldAbId);
      if (OldAbId == 0) game.Board.ShowAbility(pm, ab);
      else game.Board.AbilityChanged(pm, oldAb, ab);
    }
  }
}
