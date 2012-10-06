using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using LightStudio.Tactic.DataModels;

namespace LightStudio.PokemonBattle.Data
{
  [DataContract(Namespace = Namespaces.PBO)]
  public class PokemonType
  {
    [DataMember]
    private readonly PokemonForm[] forms;
    [DataMember]
    private readonly PokemonFormData[] formData;

#if DEBUG
    public PokemonType()
    {
    }
#else
    private PokemonType()
    {
    }
#endif

    [DataMember]
    private readonly short _number;
    public short Number
    { get { return _number; } }

    [DataMember]
    private readonly string _name;
    public string Name
    { get { return _name; } }

    [DataMember]
    private readonly float _height;
    public float Height
    { get { return _height; } }
    [DataMember]
    private readonly float _weight;
    public float Weight
    { get { return _weight; } }

    [DataMember(EmitDefaultValue = false)]
    private byte _genderBoundary;
    public byte GenderBoundary
    { get { return _genderBoundary; } }

    [DataMember]
    private readonly EggGroup _eggGroup1;
    public EggGroup EggGroup1
    { get { return _eggGroup1; } }
    [DataMember(EmitDefaultValue = false)]
    private readonly EggGroup _eggGroup2;
    public EggGroup EggGroup2
    { get { return _eggGroup2; } }

    public IEnumerable<PokemonForm> Forms
    { get { return forms; } }

    private static readonly IEnumerable<PokemonGender> NONE = new[] { PokemonGender.None };
    private static readonly IEnumerable<PokemonGender> MALE = new[] { PokemonGender.Male };
    private static readonly IEnumerable<PokemonGender> FEMALE = new[] { PokemonGender.Female };
    private static readonly IEnumerable<PokemonGender> BOTH = new[] { PokemonGender.Male, PokemonGender.Female };
    public IEnumerable<PokemonGender> GetAvailableGenders()
    {
      switch (GenderBoundary)
      {
        case 0x00:
          return MALE;
        case 0xfe:
          return FEMALE;
        case 0xff:
          return NONE;
        default:
          return BOTH;
      }
    }
    
    internal PokemonFormData GetData(int index)
    {
      return formData[index];
    }
    
    public PokemonForm GetForm(int index)
    {
      return forms.ValueOrDefault(index);
    }
  }
}
