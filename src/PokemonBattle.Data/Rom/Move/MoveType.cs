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

    public override string Description
    { get { return DataService.DataString["M" + Id.ToString("000")]; } }

    [DataMember(EmitDefaultValue = false)]
    private readonly MoveInnerClass _class;
    public MoveInnerClass Class
    { get { return _class; } }

    [DataMember(EmitDefaultValue = false)]
    private readonly sbyte _minTimes;
    public int MinTimes
    { get { return _minTimes; } }

    [DataMember(EmitDefaultValue = false)]
    private readonly sbyte _maxTimes;
    public int MaxTimes
    { get { return _maxTimes; } }

    [DataMember(EmitDefaultValue = false)]
    private readonly MoveAttachment _attachment;
    public MoveAttachment Attachment
    { get { return _attachment; } }

    [DataMember(EmitDefaultValue = false)]
    private readonly sbyte _ctLv;
    public int CtLv
    { get { return _ctLv; } }

    [DataMember(EmitDefaultValue = false)]
    private readonly sbyte _flinchProbability;
    public int FlinchProbability
    { get { return _flinchProbability; } }

    [DataMember(EmitDefaultValue = false)]
    private readonly sbyte _hurtPercentage;
    public int HurtPercentage
    { get { return _hurtPercentage; } }

    [DataMember(EmitDefaultValue = false)]
    private readonly sbyte _maxHpPercentage;
    public int MaxHpPercentage
    { get { return _maxHpPercentage; } }

    [DataMember]
    private readonly MoveLv7DChange[] _lv7DChanges;
    public IEnumerable<MoveLv7DChange> Lv7DChanges
    { get { return _lv7DChanges; } }

    [DataMember]
    private readonly MoveTypeAdvancedFlags _advancedFlags;
    public MoveTypeAdvancedFlags Flags
    { get { return _advancedFlags; } }

    [DataMember(EmitDefaultValue = false)]
    private readonly sbyte _accuracy;
    public int Accuracy
    { get { return _accuracy; } }

    [DataMember(EmitDefaultValue = false)]
    private readonly MoveCategory _category;
    public MoveCategory Category
    { get { return _category; } }

    [DataMember(EmitDefaultValue = false)]
    private readonly BattleType _type;
    public BattleType Type
    { get { return _type; } }

    [DataMember(EmitDefaultValue = false)]
    private readonly byte _power;
    public int Power
    { get { return _power; } }

    [DataMember(EmitDefaultValue = false)]
    private readonly sbyte _pp;
    public int PP
    { get { return _pp; } }

    [DataMember(EmitDefaultValue = false)]
    private readonly MoveRange _range;
    public MoveRange Range
    { get { return _range; } }

    [DataMember(EmitDefaultValue = false)]
    private readonly sbyte _priority;
    public int Priority
    { get { return _priority; } }
  }
}
