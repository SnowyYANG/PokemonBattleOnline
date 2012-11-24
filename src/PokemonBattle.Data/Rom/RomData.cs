using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using LightStudio.Tactic.DataModels;

namespace LightStudio.PokemonBattle.Data
{
  [DataContract(Namespace=Namespaces.PBO)]
#if DEBUG
  public
#else
  internal
#endif
    sealed class RomData : SimpleData
  {
    internal static RomData Load(string path)
    {
      var rom = LoadFromDat<RomData>(path);
      foreach (var pm in rom.Pokemons)
        foreach (var form in pm.Forms) form.Type = pm;
      return rom;
    }

    [DataMember]
    private readonly PokemonType[] pokemons;
    [DataMember]
    private readonly MoveType[] moves;
    [DataMember]
    private readonly Ability[] abilities;
    [DataMember]
    private readonly Dictionary<int, Item> items;
    [DataMember]
    private readonly Evolution[] evolutions;

    private RomData()
    {
    }

    public IEnumerable<Ability> Abilities
    { get { return abilities; } }
    public IEnumerable<Item> Items
    { get { return items.Values; } }
    public IEnumerable<PokemonType> Pokemons
    { get { return pokemons; } }
    public IEnumerable<MoveType> Moves
    { get { return moves; } }

    public PokemonType GetPokemonType(int number)
    {
      return pokemons.ValueOrDefault(number - 1);
    }
    public PokemonForm GetPokemonForm(int number, int form)
    {
      if (number == 0 || number > pokemons.Length) return null;
      return pokemons[number - 1].GetForm(form);
    }
    public MoveType GetMoveType(int moveId)
    {
      return moves.ValueOrDefault(moveId - 1);
    }
    public Ability GetAbility(int abilityId)
    {
      return abilities.ValueOrDefault(abilityId - 1);
    }
    public Item GetItem(int itemId)
    {
      return items.ValueOrDefault(itemId);
    }
    public int? GetPreEvolution(int number)
    {
      var r = evolutions.FirstOrDefault((e) => e.To == number);
      return r == null ? null : (int?)r.From;
    }
    public IEnumerable<int> GetEvolutions(int number)
    {
      return evolutions.Where((e) => e.From == number).Select((e) => e.To);
    }
  }
}
