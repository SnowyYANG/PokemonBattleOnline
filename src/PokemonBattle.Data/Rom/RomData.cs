using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using LightStudio.Tactic.DataModels.IO;

namespace LightStudio.PokemonBattle.Data
{
  [DataContract(Namespace=Namespaces.LIGHT)]
  public sealed class RomData : SimpleData
  {
    public static RomData Load()
    {
      return LoadFromDat<RomData>("Data\\rom.dat");
    }
    
    [DataMember]
    public readonly Dictionary<int, PokemonType> Pokemons;
    [DataMember]
    public readonly Dictionary<int, MoveType> Moves;
    [DataMember]
    public readonly Dictionary<int, Ability> Abilities;
    [DataMember]
    public readonly Dictionary<int, Item> Items;

    private RomData()
    {
    }

    #region GetXXX
    public PokemonType GetPokemonType(int spriteId)
    {
      return Pokemons.ValueOrDefault(spriteId);
    }

    public MoveType GetMoveType(int skillId)
    {
      return Moves.ValueOrDefault(skillId);
    }

    public Ability GetAbility(int abilityId)
    {
      return Abilities.ValueOrDefault(abilityId);
    }

    public Item GetItem(int itemId)
    {
      return Items.ValueOrDefault(itemId);
    }
    #endregion
  }
}
