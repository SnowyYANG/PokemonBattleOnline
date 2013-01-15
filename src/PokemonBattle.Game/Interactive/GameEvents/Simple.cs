using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using PokemonBattleOnline.Game.Host;

namespace PokemonBattleOnline.Game.GameEvents
{
  [DataContract(Name = "e", Namespace = Namespaces.JSON)]
  public class SimpleEvent : GameEvent
  {
    protected static void Filter(object o, ref int i, ref string s)
    {
      if (o != null)
      {
        if (o is PokemonProxy) i = ((PokemonProxy)o).Id;
        else if (o is PokemonBattleOnline.Data.GameElement) i = ((PokemonBattleOnline.Data.GameElement)o).Id;
        else if (o is int) i = (int)o;
#if DEBUG
        else if (o is Enum) s = o.ToString();
        else if (o is string) s = (string)o;
        else throw new Exception("bad event arg");
#else
        else s = o.ToString();
#endif
      }
    }
    protected static object Filter(int i, string s)
    {
      if (s == null) return i;
      return s;
    }
    
    [DataMember(Name = "a")]
    protected string Key;

    [DataMember(Name = "b", EmitDefaultValue = false)]
    protected int I0;

    [DataMember(Name = "d", EmitDefaultValue = false)]
    protected int I1;

    [DataMember(Name = "e", EmitDefaultValue = false)]
    protected int I2;

    [DataMember(Name = "c", EmitDefaultValue = false)]
    protected string S0;

    [DataMember(Name = "f", EmitDefaultValue = false)]
    protected string S1;

    [DataMember(Name = "g", EmitDefaultValue = false)]
    protected string S2;

    /// <param name="args">string and int is fine</param>
    public SimpleEvent(string gameLogKey, object arg0 = null, object arg1 = null, object arg2 = null)
    {
      Key = gameLogKey;
      List<object> _args = new List<object>();
      Filter(arg0, ref I0, ref S0);
      Filter(arg1, ref I1, ref S1);
      Filter(arg2, ref I2, ref S2);
    }
    protected override void Update()
    {
      AppendGameLog(Key, Filter(I0, S0), Filter(I1, S1), Filter(I2, S2));
    }
  }
}
