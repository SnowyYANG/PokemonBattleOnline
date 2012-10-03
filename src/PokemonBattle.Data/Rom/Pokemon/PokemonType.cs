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
#if DEBUG
    public PokemonType()
    {
    }
#endif

    [DataMember]
    public short Number { get; private set; }
    [DataMember]
    public string Name { get; private set; }
    [DataMember]
    public double Height { get; private set; }
    [DataMember]
    public double Weight { get; private set; }
    [DataMember(EmitDefaultValue = false)]
    public double MaleRatio { get; private set; }
    [DataMember(EmitDefaultValue = false)]
    public double FemaleRatio { get; private set; }
    [DataMember]
    public EggGroup EggGroup1 { get; private set; }
    [DataMember(EmitDefaultValue = false)]
    public EggGroup EggGroup2 { get; private set; }

    [DataMember]
    private readonly PokemonFormeData[] formeData;
    [DataMember]
    private readonly PokemonForme[] formes;
    public IEnumerable<PokemonForme> Formes
    { get { return formes; } }

    [DataMember(EmitDefaultValue = false)]
    public bool AreFormesPostnatal { get; private set; }
    [DataMember(EmitDefaultValue = false)]
    public bool CanChooseFormes { get; private set; }

    public PokemonGender[] GetAvailableGenders()
    {
      if (MaleRatio == 0)
      {
        if (FemaleRatio == 0)
        {
          return new[] { PokemonGender.None };
        }
        else
        {
          return new[] { PokemonGender.Female };
        }
      }
      else if (FemaleRatio == 0)
      {
        return new[] { PokemonGender.Male };
      }
      return new[] { PokemonGender.Male, PokemonGender.Female };
    }
    internal PokemonFormeData GetData(int index)
    {
      return formeData[index];
    }
    public PokemonForme GetForme(int index)
    {
      return formes.ValueOrDefault(index);
    }
  }
}
