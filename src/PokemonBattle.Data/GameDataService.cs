using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Linq;

namespace LightStudio.PokemonBattle.Data
{
  public static class GameDataService
  {
    private const string ROM_FILE = "\\rom.dat";

    private static RomData rom;

    public static void Load(string path)
    {
      rom = RomData.Load(path + ROM_FILE);
    }

    public static IEnumerable<Ability> Abilities
    { get { return rom.Abilities; } }
    public static IEnumerable<Item> Items
    { get { return rom == null ? null : rom.Items; } }
    public static IEnumerable<PokemonType> Pokemons
    { get { return rom == null ? null : rom.Pokemons; } }
    public static IEnumerable<MoveType> Moves
    { get { return rom.Moves; } }

    public static PokemonType GetPokemon(int number)
    {
      return rom.GetPokemonType(number);
    }
    public static PokemonForm GetPokemon(int number, int form)
    {
      return rom.GetPokemonForm(number, form);
    }
    public static MoveType GetMove(int moveId)
    {
      return rom.GetMoveType(moveId);
    }
    public static Ability GetAbility(int abilityId)
    {
      return rom.GetAbility(abilityId);
    }
    public static Item GetItem(int itemId)
    {
      return rom.GetItem(itemId);
    }
  }
}
