using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Linq;
using LightStudio.Tactic.Globalization;
using LightStudio.Tactic.DataModels;

namespace LightStudio.PokemonBattle.Data
{
  public static class DataService
  {
    private const string STRING_RESOURCE_FORMAT = @"..\res\Data\string\{0}.xml";
    private const string DATA_STRING_RESOURCE_FORMAT = @"..\res\Data\string\data\{0}.xml";
    private const string BATTLE_DOMAIN = "PokemonBattle";
    private const string BATTLE_DATA_DOMAIN = "PokemonBattleData";
    private const string USER_DATA = "user.dat";

    public static string CurrentLanguage { get; private set; }
    public static DomainStringService String { get; private set; }
    public static DomainStringService DataString { get; private set; }
    public static UserData UserData { get; private set; }

    public static void Load(StringService stringService)
    {
      String = stringService.GetDomainService(BATTLE_DOMAIN);
      String.SetProvider(LoadGameStrings);
      String.ReturnKeyOnFallback = true;
      DataString = stringService.GetDomainService(BATTLE_DATA_DOMAIN);
      DataString.SetProvider(LoadDataStrings);
      DataString.ReturnKeyOnFallback = true;
      CurrentLanguage = stringService.Language;
    }
    public static void LoadUserData()
    {
      UserData = UserData.Load(USER_DATA);
    }

    #region private methods
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

    #region DataService
    public static string GetLocalizedName(this GameElement e)
    {
      return e == null ? String["<error>"] : e.Name;
    }
    public static string GetLocalizedName(this PokemonType pm)
    {
      return pm == null ? String["<error>"] : pm.Name;
    }
    public static string GetLocalizedName(this Enum type)
    {
      return String[type.ToString()];
    }
    #endregion
  }
}
