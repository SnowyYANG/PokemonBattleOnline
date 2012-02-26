using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using LightStudio.Tactic.DataModels.IO;

namespace LightStudio.PokemonBattle.Data
{
  [DataContract(Namespace=Namespaces.DEFAULT)]
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
      Pokemons = new Dictionary<int, PokemonType>();
      Moves = new Dictionary<int, MoveType>();
      Abilities = new Dictionary<int, Ability>();
      Items = new Dictionary<int, Item>();
    }

    public void Save()
    {
      SaveDat("Data\\rom.dat");
    }

    #region AddXXX
    /// <summary>
    /// existing SpriteType with the same id will be overwritten
    /// </summary>
    public void AddPokemonType(PokemonType pokemon)
    {
      Pokemons[pokemon.Id] = pokemon;
    }

    /// <summary>
    /// existing SkillType with the same id will be overwritten
    /// </summary>
    public void AddMoveType(MoveType move)
    {
      Moves[move.Id] = move;
    }

    /// <summary>
    /// existing Ability with the same id will be overwritten
    /// </summary>
    public void AddAbility(Ability ability)
    {
      Abilities[ability.Id] = ability;
    }

    /// <summary>
    /// existing Item with the same id will be overwritten
    /// </summary>
    public void AddItem(Item item)
    {
      Items[item.Id] = item;
    }
    #endregion

    //#region AddXXXs
    //public void AddPokemonTypes(IEnumerable<PokemonType> pokemons)
    //{
    //  if (pokemons == null)
    //    return;
    //  foreach (var sprite in pokemons)
    //    AddPokemonType(sprite);
    //}
    //public void AddMoveTypes(IEnumerable<MoveType> moves)
    //{
    //  if (moves == null)
    //    return;
    //  foreach (var skill in moves)
    //    AddMoveType(skill);
    //}
    //public void AddAbilities(IEnumerable<Ability> abilities)
    //{
    //  if (abilities == null)
    //    return;
    //  foreach (var ability in abilities)
    //    AddAbility(ability);
    //}
    //public void AddItems(IEnumerable<Item> items)
    //{
    //  if (items == null)
    //    return;
    //  foreach (var item in items)
    //    AddItem(item);
    //}
    //#endregion

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
