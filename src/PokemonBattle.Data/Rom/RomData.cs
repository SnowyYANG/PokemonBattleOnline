using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using LightStudio.Tactic.DataModels;

namespace LightStudio.PokemonBattle.Data
{
  [DataContract(Namespace=Namespaces.PBO)]
  public sealed class RomData : SimpleData
  {
    public static RomData Load(string file)
    {
      return LoadFromDat<RomData>(file);
    }
    
    [DataMember]
    public PokemonType[] Pokemons;
    [DataMember]
    public MoveType[] Moves
    { get; set; }
    [DataMember]
    public Ability[] Abilities
    { get; set; }
    [DataMember]
    public readonly Dictionary<int, Item> Items;

    private RomData()
    {
    }

    #region GetXXX
    public PokemonForme GetPokemon(int number, int forme)
    {
      return Pokemons[number].GetForme(forme);
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

    #region if DEBUG
#if DEBUG
    public static RomData LoadFromXml()
    {
      return LoadFromXml<RomData>("Data\\rom.xml");
    }
    public void SaveXml()
    {
      SaveXml("Data\\rom.xml");
    }
    public void SaveDat()
    {
      SaveDat("Data\\rom.dat");
    }
#endif
    #endregion
  }
}
