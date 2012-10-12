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
    public BattleType Type1
    { get { return _type1; } }
    [DataMember(EmitDefaultValue = false)]
    private readonly BattleType _type2;
    public BattleType Type2
    { get { return _type2 == _type1 ? BattleType.Invalid : _type2; } }

    public Ability[] Abilities
    { get { return abilities.Select(id => GameDataService.GetAbility(id)).ToArray(); } }
    public Ability GetAbility(int index)
    {
      return GameDataService.GetAbility(abilities.ValueOrDefault(index));
    }
  }
}
