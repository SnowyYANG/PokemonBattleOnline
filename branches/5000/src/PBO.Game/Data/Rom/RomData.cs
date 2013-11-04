﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace PokemonBattleOnline.Game
{
  [DataContract(Namespace=PBOMarks.PBO)]
  public sealed class RomData : SimpleData
  {
    private static RomData current;
    public static void Load(ZipData zip, string path)
    {
      using (var stream = zip.GetStream(path))
        current = LoadFromXml<RomData>(stream);
      foreach (var pm in Pokemons)
        foreach (var form in pm.Forms) form.Species = pm;
    }

    [DataMember]
#if EDITING
    public
#else
    private readonly
#endif
     PokemonSpecies[] pokemons;

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
     Evolution[] evolutions;

    private RomData()
    {
    }

    public static IEnumerable<PokemonSpecies> Pokemons
    { get { return current.pokemons; } }
    public static IEnumerable<MoveType> Moves
    { get { return current.moves; } }
    public static int Abilities
    { get { return 164; } }
    public static int Items
    { get { return 195; } }
    public static int BattleTypes
    { get { return 17; } }
    public static int MoveCategories
    { get { return 3; } }
    public static int Natures
    { get { return 25; } }

    public static PokemonSpecies GetPokemon(int number)
    {
      return current.pokemons.ValueOrDefault(number - 1);
    }
    public static PokemonForm GetPokemon(int number, int form)
    {
      if (number == 0 || number > current.pokemons.Length) return null;
      return current.pokemons[number - 1].GetForm(form);
    }
    public static MoveType GetMove(int moveId)
    {
      return current.moves.ValueOrDefault(moveId - 1);
    }

    public static int? GetPreEvolution(int number)
    {
      var r = current.evolutions.FirstOrDefault((e) => e.To == number);
      return r == null ? null : (int?)r.From;
    }
    public static IEnumerable<int> GetEvolutions(int number)
    {
      return current.evolutions.Where((e) => e.From == number).Select((e) => e.To);
    }
  }
}