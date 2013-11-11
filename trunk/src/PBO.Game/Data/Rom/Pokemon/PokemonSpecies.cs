using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace PokemonBattleOnline.Game
{
  [DataContract(Namespace = PBOMarks.PBO)]
  public class PokemonSpecies
  {
    [DataMember(Name = "FormData", Order = 4)]
#if EDITING
    public
#else
    private readonly
#endif
      PokemonFormData[] formData;
    [DataMember(Name = "Forms", Order = 5)]
#if EDITING
    public
#else
    private readonly
#endif
      PokemonForm[] forms;

#if DEBUG
    public PokemonSpecies()
    {
    }
#else
    private PokemonSpecies()
    {
    }
#endif

    [DataMember(Name = "Number", Order = 0)]
#if EDITING
    public
#else
    private readonly
#endif
      int _number;
    public int Number
    { get { return _number; } }

    [DataMember(Name = "Gender", Order = 1)]
#if EDITING
    public
#else
    private readonly
#endif
      byte _genderBoundary;
    private static readonly IEnumerable<PokemonGender> NONE = new[] { PokemonGender.None };
    private static readonly IEnumerable<PokemonGender> MALE = new[] { PokemonGender.Male };
    private static readonly IEnumerable<PokemonGender> FEMALE = new[] { PokemonGender.Female };
    private static readonly IEnumerable<PokemonGender> BOTH = new[] { PokemonGender.Male, PokemonGender.Female };
    private IEnumerable<PokemonGender> genders;
    public IEnumerable<PokemonGender> Genders
    {
      get
      {
        if (genders == null)
          switch (_genderBoundary)
          {
            case 0x00:
              genders = MALE;
              break;
            case 0xfe:
              genders = FEMALE;
              break;
            case 0xff:
              genders = NONE;
              break;
            default:
              genders = BOTH;
              break;
          }
        return genders;
      }
    }

    [DataMember(Name = "Egg1", Order = 2)]
#if EDITING
    public
#else
    private readonly
#endif
      EggGroup _eggGroup1;
    public EggGroup EggGroup1
    { get { return _eggGroup1; } }
    [DataMember(Name = "Egg2", Order = 3, EmitDefaultValue = false)]
#if EDITING
    public
#else
    private readonly
#endif
      EggGroup _eggGroup2;
    public EggGroup EggGroup2
    { get { return _eggGroup2; } }

    /// <summary>
    /// public for Binding
    /// </summary>
    public IEnumerable<PokemonForm> Forms
    { get { return forms; } }

    internal PokemonFormData GetData(int index)
    {
      return formData[index];
    }
    
    public PokemonForm GetForm(int index)
    {
      return forms.ValueOrDefault(index);
    }

    public override string ToString()
    {
      return _number.ToString();
    }
  }
}
