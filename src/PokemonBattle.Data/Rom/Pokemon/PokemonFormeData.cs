using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace LightStudio.PokemonBattle.Data
{
  [DataContract(Namespace = Namespaces.PBO)]
  public class PokemonFormeData
  {
    [DataMember]
    private int[] abilities;

    [DataMember]
    public ReadOnly6D Base
    { get; private set; }
    [DataMember]
    public BattleType Type1
    { get; private set; }
    [DataMember(EmitDefaultValue = false)]
    public BattleType Type2
    { get; private set; }

    public Ability[] GetAvailableAbilities()
    {
      return abilities.Select(id => DataService.GetAbility(id)).ToArray();
    }
    public Ability GetAbility(int index)
    {
      return DataService.GetAbility(abilities.ValueOrDefault(index));
    }
  }
}
