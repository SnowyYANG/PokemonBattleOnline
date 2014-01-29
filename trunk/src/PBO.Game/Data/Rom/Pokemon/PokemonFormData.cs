using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace PokemonBattleOnline.Game
{
  [DataContract(Namespace = PBOMarks.PBO)]
  public class PokemonFormData
  {

#if EDITING
    public PokemonFormData()
    {
    }
#else
    private PokemonFormData()
    {
    }
#endif

    [DataMember(Name = "T1", Order = 0)]
#if EDITING
    public
#else
    private readonly
#endif
    BattleType _type1;
    internal BattleType Type1
    { get { return _type1; } }
    [DataMember(Name = "T2", Order = 1, EmitDefaultValue = false)]
#if EDITING
    public
#else
    private readonly
#endif
    BattleType _type2;
    internal BattleType Type2
    { get { return _type2; } }

    [DataMember(Name = "A", Order = 2)]
    private readonly int[] abilities;
    public int[] Abilities
    {
      get
      {
        if (abilities[0] == abilities[1]) abilities[1] = 0;
        return abilities;
      }
    }

    [DataMember(Name = "Base", Order = 3)]
#if DEBUG
    public
#else
    private readonly
#endif
     ReadOnly6D _base;
    public I6D Base
    { get { return _base; } }

    [DataMember(Name = "H", Order = 4)]
#if DEBUG
    public
#else
    private readonly
#endif
      float _height;
    public float Height
    { get { return _height; } }
    [DataMember(Name = "W", Order = 5)]
#if DEBUG
    public
#else
    private readonly
#endif
      float _weight;
    public float Weight
    { get { return _weight; } }

    public int GetAbility(int index)
    {
      var ab = abilities.ValueOrDefault(index);
      return ab == 0 ? abilities[0] : ab;
    }
  }
}
