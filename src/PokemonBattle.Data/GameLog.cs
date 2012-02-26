using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using LightStudio.Tactic.DataModels;
using LightStudio.Tactic.DataModels.IO;

namespace LightStudio.PokemonBattle.Data
{
  [DataContract(Namespace = Namespaces.DEFAULT)]
  public class GameLog : SimpleData
  {
    public static GameLog Load(string language)
    {
      return LoadFromXml<GameLog>("log\\" + language + ".xml");
    }
    
    [DataMember]
    private Dictionary<string, IText> logs;

    internal GameLog()
    {
      logs = new Dictionary<string, IText>();
    }

    public IText this[string eventType]
    { get { return logs.ValueOrDefault(eventType); } }
  }
}
