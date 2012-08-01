using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using LightStudio.PokemonBattle.Game.Host;

namespace LightStudio.PokemonBattle.Game.GameEvents
{
  [DataContract(Namespace = Namespaces.DEFAULT)]
  public class SimpleEvent : GameEvent
  {
    private static object Filter(object o)
    {
      if (o is PokemonProxy) o = ((PokemonProxy)o).Id;
      else if (o is Tactic.DataModels.GameElement) o = ((Tactic.DataModels.GameElement)o).Id;
      else if (o is int || o is Enum) o = o.ToString();
#if DEBUG
      else if (!(o is string)) throw new Exception("bad event arg");
#endif
      return o;
    }
    
    [DataMember]
    string Key;

    [DataMember(EmitDefaultValue = false)]
    object Arg;

    [DataMember(EmitDefaultValue = false)]
    object[] Args;

    /// <param name="args">string and int is fine</param>
    public SimpleEvent(string gameLogKey, params object[] args)
    {
      Key = gameLogKey;
      if (args.Length == 1) Arg = Filter(args[0]);
      else if (args.Length > 1) Args = args.Select(Filter).ToArray();
    }
    public override IText GetGameLog()
    {
      var t = GetGameLog(Key);
      if (Arg != null) t.SetData(Arg);
      else if (Args != null) t.SetData(Args);
      return t;
    }
  }
}
