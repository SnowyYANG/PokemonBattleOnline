using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using LightStudio.Tactic.DataModels;

namespace LightStudio.PokemonBattle.Data
{
  [DataContract(Namespace=Namespaces.LIGHT)]
  public class PokemonType : GameElement
  {
    public PokemonType(int id) : base(id)
    {
      Abilities = new Collection<int>();
      Learnset = new Collection<MoveLearnItem>();
    }

    [DataMember]
    public int Number
    { get; private set; }

    [DataMember(EmitDefaultValue = false)]
    public int Form
    { get; private set; }

    #region Stats

    [DataMember]
    public int BaseHp
    { get; private set; }

    [DataMember]
    public int BaseAtk
    { get; private set; }

    [DataMember]
    public int BaseDef
    { get; private set; }

    [DataMember]
    public int BaseSpAtk
    { get; private set; }

    [DataMember]
    public int BaseSpDef
    { get; private set; }

    [DataMember]
    public int BaseSpeed
    { get; private set; }

    #endregion

    [DataMember]
    public BattleType Type1 { get; private set; }
    [DataMember(EmitDefaultValue = false)]
    public BattleType Type2 { get; private set; }

    [DataMember]
    internal Collection<int> Abilities { get; private set; }

    [DataMember]
    public double Height { get; private set; }
    [DataMember]
    public double Weight { get; private set; }
    [DataMember]
    public double MaleRatio { get; private set; }
    [DataMember]
    public double FemaleRatio { get; private set; }
    [DataMember]
    public EggGroup EggGroup1 { get; private set; }
    [DataMember(EmitDefaultValue = false)]
    public EggGroup EggGroup2 { get; private set; }

    [DataMember]
    public IList<MoveLearnItem> Learnset { get; private set; }

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
    public Ability[] GetAvailableAbilities()
    {
      return Abilities.Select(id => DataService.GetAbility(id)).ToArray();
    }
    public Ability GetAbility(int index)
    {
      return DataService.GetAbility(Abilities[index]);
    }
    public int GetBaseStat(StatType stat)
    {
      switch (stat)
      {
        case StatType.Hp:
          return BaseHp;
        case StatType.Atk:
          return BaseAtk;
        case StatType.Def:
          return BaseDef;
        case StatType.SpAtk:
          return BaseSpAtk;
        case StatType.SpDef:
          return BaseSpDef;
        case StatType.Speed:
          return BaseSpeed;
        default:
          throw new Exception("PokemonType.GetStat");
      }
    }
  }
}
