using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace PokemonBattleOnline.Game
{
  public class GameString
  {
    public static GameString JP;
    public static GameString EN;
    public static GameString Current;
    public static GameString Backup;
    private static GameString InnerBackup;

    public static void Load(string path, string language, string backup)
    {
      Current = TryLoad(path, language);
      EN = language == "en" ? Current : TryLoad(path, "en");
      JP = language == "jp" ? Current : TryLoad(path, "jp");
      if (backup != null) Backup = TryLoad(path, backup);
      InnerBackup = Backup ?? EN ?? JP ?? Current;
      if (Current == null) Current = InnerBackup;
    }
    private static GameString TryLoad(string basePath, string language)
    {
      try
      {
        return new GameString(basePath + "\\" + language, language);
      }
      catch
      {
        return null;
      }
    }

    public readonly string Language;
    public readonly Char MinFirstChar;
    public readonly Char MaxFirstChar;

    private string[] Pokemons;
    private Dictionary<int, string> Forms;
    private string[] Moves;
    private string[] Abilities;
    private Dictionary<int, string> Items;
    
    private string[] MovesD;
    private string[] AbilitiesD;
    private Dictionary<int, string> ItemsD;

    private string[] Natures;
    private string[] BattleTypes;
    private string[] MoveCategories;

    private GameString(string path, string language)
    {
      Language = language;
      Pokemons = new string[RomData.Pokemons.Count()];
      Forms = new Dictionary<int, string>();
      Moves = new string[RomData.Moves.Count()];
      Abilities = new string[RomData.Abilities];
      Items = new Dictionary<int, string>();
      using (var sr = new StreamReader(path))
        for (string line = sr.ReadLine(); !string.IsNullOrWhiteSpace(line); line = sr.ReadLine())
        {
          int num;
          string str;
          {
            var c = line[4] == ':' ? 4 : line[6] == ':' ? 6 : line[3] == ':' ? 3 : 2;
            num = Convert.ToInt32(line.Substring(1, c - 1));
            str = line.Substring(c + 1);
            if (str[0] < MinFirstChar) MinFirstChar = str[0];
            else if (str[0] > MaxFirstChar) MaxFirstChar = str[0];
          }
          switch (line[0])
          {
            case 'p':
              if (line[4] == ':') Pokemons[num - 1] = str;
              else Forms[num] = str;
              break;
            case 'm':
              Moves[num - 1] = str;
              break;
            case 'a':
              Abilities[num - 1] = str;
              break;
            case 'i':
              Items[num] = str;
              break;
            case 'M':
              if (MovesD == null) MovesD = new string[Moves.Length];
              MovesD[num - 1] = str;
              break;
            case 'A':
              if (AbilitiesD == null) AbilitiesD = new string[Abilities.Length];
              AbilitiesD[num - 1] = str;
              break;
            case 'I':
              if (ItemsD == null) ItemsD = new Dictionary<int, string>();
              ItemsD[num] = str;
              break;
            case 'n':
              if (Natures == null) Natures = new string[RomData.Natures];
              Natures[num] = str;
              break;
            case 'b':
              if (BattleTypes == null) BattleTypes = new string[RomData.BattleTypes];
              BattleTypes[num] = str;
              break;
            case 'c':
              if (MoveCategories == null) MoveCategories = new string[RomData.MoveCategories];
              MoveCategories[num] = str;
              break;
          }
        }
    }

    public string Pokemon(int number)
    {
      var i = number - 1;
      return Pokemons[i] ?? InnerBackup.Pokemons[i];
    }
    public string Pokemon(int number, int form)
    {
      var i = number * 100 + form;
      return Forms.ValueOrDefault(i) ?? InnerBackup.Forms.ValueOrDefault(i);
    }
    public string Move(int move)
    {
      var i = move - 1;
      return Moves[i] ?? InnerBackup.Moves[i];
    }
    public string Ability(int ability)
    {
      var i = ability - 1;
      return Abilities[i] ?? InnerBackup.Abilities[i];
    }
    public string Item(int item)
    {
      return Items.ValueOrDefault(item) ?? InnerBackup.Items.ValueOrDefault(item);
    }
    public string MoveD(int move)
    {
      var i = move - 1;
      var backup = InnerBackup.MovesD;
      return MovesD == null ? backup == null ? null : backup[i] : MovesD[i];
    }
    public string AbilityD(int ability)
    {
      var i = ability - 1;
      var backup = InnerBackup.AbilitiesD;
      return AbilitiesD == null ? backup == null ? null : backup[i] : AbilitiesD[i];
    }
    public string ItemD(int item)
    {
      var backup = InnerBackup.ItemsD;
      return ItemsD == null ? backup == null ? null : backup.ValueOrDefault(item) : ItemsD.ValueOrDefault(item);
    }
    public string Nature(PokemonNature nature)
    {
      var i = (int)nature;
      var backup = InnerBackup.Natures;
      return Natures == null ? backup == null ? null : backup[i] : Natures[i];
    }
    public string BattleType(BattleType type)
    {
      if (type == Game.BattleType.Invalid) return null;
      var i = (int)type - 1;
      var backup = InnerBackup.BattleTypes;
      return BattleTypes == null ? backup == null ? null : backup[i] : BattleTypes[i];
    }
    public string MoveCategory(MoveCategory category)
    {
      var i = (int)category;
      var backup = InnerBackup.MoveCategories;
      return MoveCategories == null ? backup == null ? null : backup[i] : MoveCategories[i];
    }

    private static GameString GetLanguage(string str)
    {
      var c = str[0];
      return EN != null && EN.MinFirstChar <= c && c <= EN.MaxFirstChar ? EN : JP != null && JP.MinFirstChar <= c && c <= JP.MaxFirstChar ? JP : Current;
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
