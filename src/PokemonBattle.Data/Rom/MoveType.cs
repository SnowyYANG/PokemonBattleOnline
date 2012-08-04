using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using LightStudio.Tactic.DataModels;

namespace LightStudio.PokemonBattle.Data
{
  [DataContract(Namespace = Namespaces.LIGHT)]
  public class MoveType : GameElement
  {
    [DataMember]
    public MoveInnerClass Class { get; private set; }

    [DataMember(EmitDefaultValue = false)]
    public byte MinTimes { get; private set; }

    [DataMember(EmitDefaultValue = false)]
    public byte MaxTimes { get; private set; }

    [DataMember(EmitDefaultValue = false)]
    public MoveAttachment Attachment { get; private set; }

    [DataMember(EmitDefaultValue = false)]
    public byte CtLv { get; private set; }

    [DataMember(EmitDefaultValue = false)]
    public byte FlinchProbability { get; private set; }

    [DataMember(EmitDefaultValue = false)]
    public sbyte HurtPercentage { get; private set; }

    [DataMember(EmitDefaultValue = false)]
    public sbyte MaxHpPercentage { get; private set; }

    [DataMember]
    public IEnumerable<MoveLv7DChange> Lv7DChanges { get; private set; }

    [DataMember]
    public MoveTypeAdvancedFlags AdvancedFlags { get; private set; }

    private MoveType(int id)
      : base(id)
    {
    }

    [DataMember]
    public int Accuracy { get; private set; }

    [DataMember]
    public MoveCategory Category { get; private set; }

    [DataMember]
    public BattleType Type { get; private set; }

    [DataMember]
    public int Power { get; private set; }

    [DataMember]
    public int PP { get; private set; }

    [DataMember]
    public MoveRange Range { get; private set; }

    [DataMember(EmitDefaultValue = false)]
    public sbyte Priority { get; private set; }
  }
}
