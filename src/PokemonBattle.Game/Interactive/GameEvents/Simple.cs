using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using LightStudio.Tactic.DataModels;

namespace LightStudio.PokemonBattle.Interactive.GameEvents
{
  [DataContract(Namespace = Namespaces.DEFAULT)]
  public class SoloEvent: GameEvent
  {
    [DataMember]
    string Solo;

    [DataMember]
    string GameLogKey;
    
    public SoloEvent(string gameLogKey, string solo)
    {
      GameLogKey = gameLogKey;
      Solo = solo;
    }
    public SoloEvent(string gameLogKey, GameElement solo)
      : this(gameLogKey, solo.Name)
    {
    }
    
    public override IText GetGameLog()
    {
      IText t = GetGameLog(GameLogKey);
      if (t != null) t.SetData(Solo);
      return t;
    }
  }

  [DataContract(Namespace = Namespaces.DEFAULT)]
  public class SimpleEvent : GameEvent
  {
    [DataMember(EmitDefaultValue = false)]
    string[] Data;

    [DataMember]
    string GameLogKey;

    public SimpleEvent(string gameLogKey, params string[] data)
    {
      GameLogKey = gameLogKey;
      if (data.Length > 0) Data = data;
    }

    public override IText GetGameLog()
    {
      IText t = GetGameLog(GameLogKey);
      if (t != null && Data != null) t.SetData(Data);
      return t;
    }
  }
}
