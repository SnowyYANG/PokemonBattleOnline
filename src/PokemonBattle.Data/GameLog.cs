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

    //public void Save(string language)
    //{
    //  base.SaveXml("Data\\logs\\" + language + ".xml");
    //}
  }
}
