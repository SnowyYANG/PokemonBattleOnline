using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace PokemonBattleOnline.Game.GameEvents
{
  [DataContract(Name = "e", Namespace = PBOMarks.JSON)]
  public class ShowLog : GameEvent
  {
    protected object Filter(int i)
    {
      if (i != 0) return i;
      if (S != StatType.Invalid) return S;
      if (B != BattleType.Invalid) return B;
      return 0;
    }
    private void Filter(object o, ref int i)
    {
      if (o is int) i = (int)o;
      else if (o is StatType) S = (StatType)o;
      else if (o is BattleType) B = (BattleType)o;
    }
    
    [DataMember(Name = "a")]
    protected string Key;

    [DataMember(Name = "b", EmitDefaultValue = false)]
    protected int I0;

    [DataMember(Name = "c", EmitDefaultValue = false)]
    protected int I1;

    [DataMember(Name = "d", EmitDefaultValue = false)]
    protected int I2;

    [DataMember(Name = "e", EmitDefaultValue = false)]
    protected StatType S;

    [DataMember(Name = "f", EmitDefaultValue = false)]
    protected BattleType B;

    /// <param name="args">string and int is fine</param>
    public ShowLog(string gameLogKey, object arg0 = null, object arg1 = null, object arg2 = null)
    {
      Key = gameLogKey;
      List<object> _args = new List<object>();
      Filter(arg0, ref I0);
      Filter(arg1, ref I1);
      Filter(arg2, ref I2);
    }
    protected override void Update()
    {
      AppendGameLog(Key, Filter(I0), Filter(I1), Filter(I2));
    }
  }
}
