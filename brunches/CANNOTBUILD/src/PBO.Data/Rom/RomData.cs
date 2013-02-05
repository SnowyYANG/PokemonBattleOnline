using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using PokemonBattleOnline.Tactic.DataModels;

namespace PokemonBattleOnline.Data
{
  [DataContract(Namespace=PBOMarks.PBO)]
  public sealed class RomData : SimpleData
  {
#if EDITING
    public
#else
    internal
#endif
     static RomData Load(string path)
    {
      var rom = LoadFromDat<RomData>(path);
      foreach (var pm in rom.Pokemons)
        foreach (var form in pm.Forms) form.Type = pm;
      return rom;
    }

    [DataMember]
#if EDITING
    public
#else
    private readonly
#endif
     PokemonType[] pokemons;

    [DataMember]
#if EDITING
    public
#else
    private readonly
#endif
     MoveType[] moves;

    [DataMember]
#if EDITING
    public
#else
    private readonly
#endif
     Ability[] abilities;

    [DataMember]
#if EDITING
    public
#else
    private readonly
#endif
     Dictionary<int, Item> items;

    [DataMember]
#if EDITING
    public
#else
    private readonly
#endif
     Evolution[] evolutions;

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

    public PokemonType GetPokemonType(string name)
    {
      return pokemons.FirstOrDefault((p) => p.Name == name || p.EnglishName == name);
    }
    public MoveType GetMoveType(string name)
    {
      return moves.FirstOrDefault((m) => m.Name == name || m.EnglishName == name);
    }
    public Ability GetAbility(string name)
    {
      return abilities.FirstOrDefault((a) => a.Name == name || a.EnglishName == name);
    }
    public Item GetItem(string name)
    {
      return items.Values.FirstOrDefault((i) => i.Name == name || i.EnglishName == name);
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
