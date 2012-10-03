using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using LightStudio.Tactic.DataModels;

namespace LightStudio.PokemonBattle.Data
{
  [DataContract(Namespace = Namespaces.PBO)]
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

#if DEBUG
    public MoveType(int id)
      : base(id)
    {

    }
#else
    private MoveType(int id)
      : base(id)
    {
    }
#endif

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
