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
   
    [DataMember(Name = "a")]
    protected string Key;

    [DataMember(Name = "b", EmitDefaultValue = false)]
    protected int I0;

    [DataMember(Name = "c", EmitDefaultValue = false)]
    protected int I1;

    [DataMember(Name = "d", EmitDefaultValue = false)]
    protected int I2;

    /// <param name="args">string and int is fine</param>
    public ShowLog(string gameLogKey, int arg0 = 0, int arg1 = 0, int arg2 = 0)
    {
      Key = gameLogKey;
      I0 = arg0;
      I1 = arg1;
      I2 = arg2;
    }
    protected override void Update()
    {
      if (Key.StartsWith("rf_"))
      {
        AppendGameLog(Key + "_r", LogStyle.HiddenAfterBattle, I0, I1, I2);
        AppendGameLog(Key + "_f", LogStyle.HiddenInBattle, I0, I1, I2);
      }
      else
      {
        var style = Key.StartsWith("m_") ? LogStyle.NoBr : Key.StartsWith("SYS_") ? LogStyle.SYS : char.IsLower(Key[0]) ? LogStyle.Detail : LogStyle.Default;
        AppendGameLog(Key, style, I0, I1, I2);
      }
    }
  }
}
