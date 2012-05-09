using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using LightStudio.Tactic.DataModels.IO;

namespace LightStudio.PokemonBattle.Interactive
{
  public interface IText : Tactic.DataModels.IText<IText>
  {
    bool HiddenInBattle { get; }
    bool HiddenAfterBattle { get; }
    IText Clone();
  }
  
  [KnownType(typeof(LogText))]
  [DataContract(Namespace = Namespaces.DEFAULT)]
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

    public IText this[string eventType]
    { 
      get
      {
        IText t = logs.ValueOrDefault(eventType);
        if (t == null) System.Diagnostics.Debugger.Break();
        return t;
      }
    }
    /// <summary>
    /// 暂不支持多语言
    /// </summary>
    public string ConvertMultiObjects<T>(Func<T, string> convert, IList<T> args)
    {
      //1: XXX
      //2: XXX和XXX
      //3: XXX, XXX和XXX
      StringBuilder sb = new StringBuilder();
      sb.Append(convert(args[0]));
      if (args.Count > 1)
      {
        for (int i = 1; i < args.Count - 1; ++i)
        {
          sb.Append(", ");
          sb.Append(convert(args[i]));
        }
        sb.Append("和");
        sb.Append(convert(args[args.Count - 1]));
      }
      return sb.ToString();
    }
  }
}
