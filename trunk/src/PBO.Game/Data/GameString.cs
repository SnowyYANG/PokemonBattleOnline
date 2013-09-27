using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokemonBattleOnline.Game
{
  public class GameString
  {
    public static GameString JP;
    public static GameString EN;
    public static GameString Current;
    public static GameString Backup;

    public readonly string Language;
    public readonly Char MinFirstChar;
    public readonly Char MaxFirstChar;

    private string[] Pokemons;
    private Dictionary<int, string> Forms;
    private string[] Abilities;
    private Dictionary<int, string> Items;
    private string[] Moves;

    private string[] PokemonStates;
    private string[] Natures;
    private string[] BattleTypes;
    private string[] MoveCategories;
    
    private string[] AbilitiesD;
    private Dictionary<int, string> ItemsD;
    private string[] MovesD;

    public string Pokemon(int number)
    {
      return Pokemons[number - 1];
    }
    public string Pokemon(int number, int form)
    {
      return Forms.ValueOrDefault(number * 100 + form);
    }
    public string Move(int move)
    {
      return Moves[move - 1];
    }
    public string Ability(int ability)
    {
      return Abilities[ability - 1];
    }
    public string Item(int item)
    {
      return Items.ValueOrDefault(item);
    }
    public string Nature(PokemonNature nature)
    {
      return Natures[(int)nature];
    }
    public string BattleType(BattleType type)
    {
      return type == Game.BattleType.Invalid ? null : BattleTypes[(int)type - 1];
    }

    private static GameString GetLanguage(string str)
    {
      var c = str[0];
      return EN.MinFirstChar <= c && c <= EN.MaxFirstChar ? EN : JP.MinFirstChar <= c && c <= JP.MaxFirstChar ? JP : Current;
    }
    public static BattleType BattleType(string name)
    {
      var gs = GetLanguage(name);
      var i = gs == null ? -1 : gs.BattleTypes.IndexOf(name);
      return i == -1 ? Game.BattleType.Invalid : (BattleType)(i + 1);
    }
    public static PokemonNature? Nature(string name)
    {
      var gs = GetLanguage(name);
      var i = gs == null ? -1 : gs.Natures.IndexOf(name);
      return i == -1 ? null : (PokemonNature?)(i);
    }
    public static PokemonSpecies PokemonSpecies(string name)
    {
      var gs = GetLanguage(name);
      var i = gs == null ? -1 : gs.Pokemons.IndexOf(name);
      if (i == -1 && gs == Current && Backup != null) i = Backup.Pokemons.IndexOf(name);
      return i == -1 ? null : RomData.GetPokemon(i + 1);
    }
    public static int? Ability(string name)
    {
      var gs = GetLanguage(name);
      var i = gs == null ? -1 : gs.Abilities.IndexOf(name);
      if (i == -1 && gs == Current && Backup != null) i = Backup.Abilities.IndexOf(name);
      return i == -1 ? null : (int?)(i + 1);
    }
    public static Item Item(string name)
    {
      var gs = GetLanguage(name);
      var i = gs == null ? 0 : gs.Items.KeyOf(name);
      if (i == 0 && gs == Current && Backup != null) i = Backup.Items.KeyOf(name);
      return i == 0 ? null : RomData.GetItem(i);
    }
    public static MoveType Move(string name)
    {
      var gs = GetLanguage(name);
      var i = gs == null ? -1 : gs.Moves.IndexOf(name);
      if (i == -1 && gs == Current && Backup != null) i = Backup.Moves.IndexOf(name);
      return i == -1 ? null : RomData.GetMove(i + 1);
    }
  }
}
