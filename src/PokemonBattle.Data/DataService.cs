using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Linq;
using System.Windows.Media;
using System.Diagnostics.Contracts;
using LightStudio.PokemonBattle.Data;
using LightStudio.Tactic.Globalization;
using LightStudio.Tactic.DataModels;

namespace LightStudio.PokemonBattle.Data
{
  public static class DataService
  {
    private const string STRING_RESOURCE_FORMAT = @"string\{0}.xml";
    private const string DATA_STRING_RESOURCE_FORMAT = @"string\data\{0}.xml";
    private const string BATTLE_DOMAIN = "PokemonBattle";
    private const string BATTLE_DATA_DOMAIN = "PokemonBattleData";
    private const string ROM_FILE = "Data\\rom.dat";

    private static RomData romData;

    public static bool IsLoaded { get; private set; }
    public static string CurrentLanguage { get; private set; }
    public static DomainStringService String { get; private set; }
    public static DomainStringService DataString { get; private set; }

    public static IEnumerable<Ability> Abilities
    { get; private set; }
    public static IEnumerable<Item> Items
    { get; private set; }
    public static IEnumerable<PokemonType> Pokemons
    { get; private set; }
    public static IEnumerable<MoveType> Moves
    { get; private set; }

    #region private methods
    private static void LoadImpl(string baseDir, StringService stringService)
    {
      String = stringService.GetDomainService(BATTLE_DOMAIN);
      String.SetProvider(LoadGameStrings);
      DataString = stringService.GetDomainService(BATTLE_DATA_DOMAIN);
      DataString.SetProvider(LoadDataStrings);

      romData = RomData.Load(ROM_FILE);
      Moves = romData.Moves;
      Abilities = romData.Abilities;
      Items = romData.Items.Values;
      Pokemons = romData.Pokemons;
    }
    private static void ClearImpl()
    {
      String.SetProvider(null);
    }
    private static LanguagePack LoadGameStrings(string lang)
    {
      return LoadStrings(STRING_RESOURCE_FORMAT, lang);
    }
    private static LanguagePack LoadDataStrings(string lang)
    {
      return LoadStrings(DATA_STRING_RESOURCE_FORMAT, lang);
    }
    private static LanguagePack LoadStrings(string path, string lang)
    {
      try
      {
        using (var stream = new StreamReader(string.Format(path, lang)))
        {
          return LanguagePack.CreateFromXml(XElement.Load(stream));
        }
      }
      catch (Exception)
      { }
      return null;
    }
    #endregion

    public static void Load(string baseDir, StringService stringService)
    {
      Contract.Requires(baseDir != null);
      Contract.Requires(stringService != null);
      LoadImpl(baseDir, stringService);
      CurrentLanguage = stringService.Language;
      IsLoaded = true;
    }
    public static void Clear()
    {
      ClearImpl();
      IsLoaded = false;
    }

    #region DataService
    public static Ability GetAbility(int id)
    {
      return romData.GetAbility(id);
    }
    public static Item GetItem(int id)
    {
      return romData.GetItem(id);
    }
    public static PokemonForm GetPokemon(int number, int form)
    {
      return romData.GetPokemon(number, form);
    }
    public static MoveType GetMove(int id)
    {
      return romData.GetMoveType(id);
    }
    public static string GetLocalizedName(this GameElement e)
    {
      return e == null ? String["<error>"] : DataString[e.Name];
    }
    public static string GetLocalizedName(this PokemonType pm)
    {
      return pm == null ? String["<error>"] : DataString[pm.Name];
    }
    public static string GetLocalizedName(this Enum type)
    {
      return DataService.String[type.ToString()];
    }
    #endregion
  }
}
