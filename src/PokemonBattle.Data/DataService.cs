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
using LightStudio.Tactic.DataModels.IO;

namespace LightStudio.PokemonBattle.Data
{
  public static class DataService
  {
    private static DataCollection dataCollection;
    private static DataConfiguration configuration;
    private static RomData romData;

    public static bool IsLoaded { get; private set; }
    public static ImageService Image { get; private set; }
    public static GameLog GameLog { get; private set; }
    public static IDomainStringService String { get; private set; }
    public static IDomainStringService DataString { get; private set; }

    public static IEnumerable<Ability> Abilities
    { get { return romData.Abilities.Values; } }
    public static IEnumerable<Item> Items
    { get { return romData.Items.Values; } }
    public static IEnumerable<PokemonType> Pokemons
    { get { return romData.Pokemons.Values; } }
    public static IEnumerable<MoveType> Moves
    { get { return romData.Moves.Values; } }

    #region private methods
    private static void LoadImpl(string baseDir, IStringService stringService)
    {
      dataCollection = DataCollection.Load(Path.Combine(baseDir, CONSTS.INDEX_FILE), baseDir);
      configuration = dataCollection.ReadFile<DataConfiguration>(CONSTS.CONFIG_FILE, true);
      
      String = stringService.GetDomainService(CONSTS.BATTLE_DOMAIN);
      String.SetProvider(LoadGameStrings);
      DataString = stringService.GetDomainService(CONSTS.BATTLE_DATA_DOMAIN);
      DataString.SetProvider(LoadDataStrings);

      Image = new ImageService(dataCollection, configuration);

      romData = RomData.Load();
      GameLog = GameLog.Load(stringService.Language);
    }
    private static void ClearImpl()
    {
      String.SetProvider(null);
    }
    private static LanguagePack LoadGameStrings(string lang)
    {
      return LoadStrings(string.Format(CONSTS.STRING_RESOURCE_FORMAT, lang));
    }
    private static LanguagePack LoadDataStrings(string lang)
    {
      return LoadStrings(string.Format(CONSTS.DATA_STRING_RESOURCE_FORMAT, lang));
    }
    private static LanguagePack LoadStrings(string path)
    {
      try
      {
        using (var stream = dataCollection.OpenFile(path, false))
        {
          return LanguagePack.CreateFromXml(XElement.Load(stream));
        }
      }
      catch (Exception)
      { }
      return null;
    }
    #endregion

    /// <param name="baseDir">the value should be an absolute path</param>
    public static void Load(string baseDir, IStringService stringService)
    {
      Contract.Requires(baseDir != null);
      Contract.Requires(stringService != null);
      LoadImpl(baseDir, stringService);
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
    public static PokemonType GetPokemonType(int id)
    {
      return romData.GetPokemonType(id);
    }
    public static MoveType GetMoveType(int id)
    {
      return romData.GetMoveType(id);
    }
    #endregion
  }
}
