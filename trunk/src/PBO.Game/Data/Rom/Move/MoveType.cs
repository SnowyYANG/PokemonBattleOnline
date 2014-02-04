using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace PokemonBattleOnline.Game
{
  [DataContract(Namespace = PBOMarks.PBO)]
  public class MoveType
  {
    [DataMember(Name = "Id")]
#if EDITING
    public
#else
    private readonly
#endif
    int _id;
    public int Id
    { get { return _id; } }

#if EDITING
    public
#else
    private
#endif
     MoveType(int id)
    {
      _id = id;
    }

    [DataMember(EmitDefaultValue = false)]
#if EDITING
    public
#else
    private readonly
#endif
    MoveClass _class;
    public MoveClass Class
    { get { return _class; } }

    [DataMember(EmitDefaultValue = false)]
#if EDITING
    public
#else
    private readonly
#endif
    int _accuracy;
    public int Accuracy
    { get { return _accuracy; } }

    [DataMember(EmitDefaultValue = false)]
#if EDITING
    public
#else
    private readonly
#endif
    MoveCategory _category;
    public MoveCategory Category
    { get { return _category; } }

    [DataMember(EmitDefaultValue = false)]
#if EDITING
    public
#else
    private readonly
#endif
    BattleType _type;
    public BattleType Type
    { get { return _type; } }

    [DataMember(EmitDefaultValue = false)]
#if EDITING
    public
#else
    private readonly
#endif
    int _power;
    public int Power
    { get { return _power; } }

    [DataMember(EmitDefaultValue = false)]
#if EDITING
    public
#else
    private readonly
#endif
    int _pp;
    public int PP
    { get { return _pp; } }

    [DataMember(EmitDefaultValue = false)]
#if EDITING
    public
#else
    private readonly
#endif
    MoveRange _range;
    public MoveRange Range
    { get { return _range; } }

    [DataMember(EmitDefaultValue = false)]
#if EDITING
    public
#else
    private readonly
#endif
    int _priority;
    public int Priority
    { get { return _priority; } }

    [DataMember(EmitDefaultValue = false)]
#if EDITING
    public
#else
    private readonly
#endif
    int _minTimes;
    public int MinTimes
    { get { return _minTimes; } }

    [DataMember(EmitDefaultValue = false)]
#if EDITING
    public
#else
    private readonly
#endif
    int _maxTimes;
    public int MaxTimes
    { get { return _maxTimes; } }

    [DataMember(EmitDefaultValue = false)]
    private readonly MoveAttachment _attachment;
    public MoveAttachment Attachment
    { get { return _attachment; } }

    [DataMember(EmitDefaultValue = false)]
#if EDITING
    public
#else
    private readonly
#endif
    int _flinchProbability;
    public int FlinchProbability
    { get { return _flinchProbability; } }

    [DataMember(EmitDefaultValue = false)]
#if EDITING
    public
#else
    private readonly
#endif
    int _hurtPercentage;
    public int HurtPercentage
    { get { return _hurtPercentage; } }

    [DataMember(EmitDefaultValue = false)]
#if EDITING
    public
#else
    private readonly
#endif
    int _maxHpPercentage;
    public int MaxHpPercentage
    { get { return _maxHpPercentage; } }

    [DataMember]
    private readonly MoveLv7DChange[] _lv7DChanges;
    public IEnumerable<MoveLv7DChange> Lv7DChanges
    { get { return _lv7DChanges; } }

    [DataMember]
    private readonly MoveFlags _advancedFlags;
    public MoveFlags Flags
    { get { return _advancedFlags; } }
  }
}
