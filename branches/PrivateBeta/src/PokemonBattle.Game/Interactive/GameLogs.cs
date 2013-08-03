using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using LightStudio.Tactic.DataModels;

namespace LightStudio.PokemonBattle.Game
{
  public interface IText : Tactic.DataModels.IText<IText>
  {
    bool HiddenInBattle { get; }
    bool HiddenAfterBattle { get; }
    IText Clone(IFormatProvider formatter);
  }
  
  [KnownType(typeof(LogText))]
  [DataContract(Namespace = PBOMarks.PBO)]
  public class GameLogs : SimpleData
  {
    public static GameLogs Load(string path, string language)
    {
      GameLogs i = LoadFromXml<GameLogs>(path + "\\" + language + ".xml");
      HashSet<string> badKeys = new HashSet<string>();
      foreach (KeyValuePair<string,IText> t in i.logs)
        if (t.Value == null || (t.Value.Contents == null && t.Value.Text == null))
        {
#if DEBUG
          System.Diagnostics.Debugger.Break();
#endif
          badKeys.Add(t.Key);
        }
      foreach (string k in badKeys) i.logs.Remove(k);
      return i;
    }
    
    [DataMember]
    private Dictionary<string, IText> logs;

    public GameLogs()
    {
      logs = new Dictionary<string, IText>();
    }

    public IText this[string key]
    { get { return logs.ValueOrDefault(key); } }
  }
}
