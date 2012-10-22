using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace LightStudio.PokemonBattle.Data
{
  [DataContract(Namespace = Namespaces.PBO)]
  public class PokemonFormData
  {
    [DataMember]
    private readonly int[] abilities;

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

    public Ability[] Abilities
    { 
      get
      { 
        var abs = abilities.Select(id => GameDataService.GetAbility(id)).ToArray();
        if (abs[0] == abs[1]) abs[1] = null;
        return abs;
      }
    }
    public Ability GetAbility(int index)
    {
      return GameDataService.GetAbility(abilities.ValueOrDefault(index));
    }
  }
}
