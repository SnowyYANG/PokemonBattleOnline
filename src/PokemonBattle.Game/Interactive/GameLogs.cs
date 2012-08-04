using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using LightStudio.Tactic.DataModels.IO;

namespace LightStudio.PokemonBattle.Game
{
  public interface IText : Tactic.DataModels.IText<IText>
  {
    bool HiddenInBattle { get; }
    bool HiddenAfterBattle { get; }
    IText Clone(IFormatProvider formatter);
  }
  
  [KnownType(typeof(LogText))]
  [DataContract(Namespace = Namespaces.LIGHT)]
  public class GameLogs : SimpleData
  {
    public static GameLogs Load(string language)
    {
      GameLogs i = LoadFromXml<GameLogs>("Data\\log\\" + language + ".xml");
      HashSet<string> badKeys = new HashSet<string>();
      foreach (KeyValuePair<string,IText> t in i.logs)
        if (t.Value == null || (t.Value.Contents == null && t.Value.Text == null))
        {
          System.Diagnostics.Debugger.Break();
          badKeys.Add(t.Key);
        }
      foreach (string k in badKeys) i.logs.Remove(k);
      return i;
    }
    
    [DataMember]
    public Dictionary<string, IText> logs;

    public GameLogs()
    {
      logs = new Dictionary<string, IText>();
    }

    public IText this[string key]
    { get { return logs.ValueOrDefault(key); } }
  }
}
