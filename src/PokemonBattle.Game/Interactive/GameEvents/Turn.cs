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
  public class BeginTurn : GameEvent
  {
    [DataMember(EmitDefaultValue = false)]
    int TurnNumber;

    public BeginTurn(int turnNumber)
    {
      TurnNumber = turnNumber;
    }
    public override IText GetGameLog(GameOutward game)
    {
      if (TurnNumber == 0) return GetGameLog(GAMESTART);
      IText t = GetGameLog(BEGINTURN);
      t.SetData(TurnNumber);
      return t;
    }
  }
}
