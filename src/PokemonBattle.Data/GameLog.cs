using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using LightStudio.Tactic.DataModels;
using LightStudio.Tactic.DataModels.IO;

namespace LightStudio.PokemonBattle.Data
{
  [KnownType(typeof(LogText))]
  [KnownType(typeof(LogLine))]
  [KnownType(typeof(LogObject))]
  [DataContract(Namespace = Namespaces.DEFAULT)]
  public class GameLog : SimpleData
  {
    public static GameLog Load(string language)
    {
      return LoadFromXml<GameLog>("Data\\log\\" + language + ".xml");
    }
    
    [DataMember]
    public Dictionary<string, IText> logs;

    public GameLog()
    {
      logs = new Dictionary<string, IText>();
    }

    public IText this[string eventType]
    { get { return logs.ValueOrDefault(eventType); } }

    public void Save(string language)
    {
      base.SaveXml("Data\\logs\\" + language + ".xml");
    }
  }
}
