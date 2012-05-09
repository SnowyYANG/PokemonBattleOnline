using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using LightStudio.PokemonBattle.Data;
using LightStudio.PokemonBattle.Game;

namespace LightStudio.PokemonBattle.Interactive.GameEvents
{
  [DataContract(Namespace = Namespaces.DEFAULT)]
  internal class BeginTurn : GameEvent
  {
    [DataMember(EmitDefaultValue = false)]
    int TurnNumber;

    public BeginTurn(int turnNumber)
    {
      TurnNumber = turnNumber;
    }
    public override IText GetGameLog()
    {
      if (TurnNumber == 0) return GetGameLog("GameStart");
      IText t = GetGameLog("BeginTurn");
      t.SetData(TurnNumber);
      return t;
    }
  }
}
