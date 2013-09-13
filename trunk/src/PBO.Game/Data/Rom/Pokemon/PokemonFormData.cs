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

#if DEBUG
    public PokemonFormData()
    {
    }
#else
    private PokemonFormData()
    {
    }
#endif

    [DataMember]
    private readonly ReadOnly6D _base;
    public I6D Base
    { get { return _base; } }
    [DataMember]
    private readonly BattleType _type1;
    internal BattleType Type1
    { get { return _type1; } }
    [DataMember]
    private readonly BattleType _type2;
    internal BattleType Type2
    { get { return _type2; } }

    [DataMember]
    private readonly int[] abilities;
    public int[] Abilities
    { 
      get
      {
        if (abilities[0] == abilities[1]) abilities[1] = 0;
        return abilities;
      }
    }
    public int GetAbility(int index)
    {
      return abilities.ValueOrDefault(index);
    }
  }
}
