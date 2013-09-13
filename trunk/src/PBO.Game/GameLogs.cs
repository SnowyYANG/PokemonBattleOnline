using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace PokemonBattleOnline.Game
{
  [DataContract(Namespace = PBOMarks.PBO)]
  public class GameLogs : SimpleData
  {
    public static GameLogs Load(string path, string language)
    {
      GameLogs i = LoadFromXml<GameLogs>(path + "\\" + language + ".xml");
#if DEBUG
      HashSet<string> badKeys = new HashSet<string>();
      foreach (KeyValuePair<string,LogText> t in i.logs)
        if (t.Value == null || (t.Value.Contents == null && t.Value.Text == null))
        {
          System.Diagnostics.Debugger.Break();
          badKeys.Add(t.Key);
        }
      foreach (string k in badKeys) i.logs.Remove(k);
#endif
      return i;
    }
    
    [DataMember]
    private Dictionary<string, LogText> logs;

    public GameLogs()
    {
      logs = new Dictionary<string, LogText>();
    }

    public LogText this[string key]
    { get { return logs.ValueOrDefault(key); } }
  }
}
