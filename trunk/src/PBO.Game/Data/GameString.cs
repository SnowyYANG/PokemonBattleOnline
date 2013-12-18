using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace PokemonBattleOnline.Game
{
  public class GameString
  {
    public static GameString JP
    { get; private set; }
    public static GameString EN
    { get; private set; }
    public static GameString Current
    { get; private set; }
    public static GameString Backup
    { get; private set; }
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
    private static GameString GetLanguage(string str)
    {
      var c = str.FirstOrDefault((ch) => !char.IsDigit(ch));
      if ('a' <= c && c <= 'z' || 'A' <= c && c <= 'Z' && EN != null) return EN;
      if (0x3040 < c && c < 0x309f && JP != null) return JP;
      return Current;
    }
    private static int IndexOf(string[] list, string name)
    {
      if (list != null)
      {
        int r;
        for (r = 0; r < list.Length; ++r)
          if (list[r] != null && list[r].StartsWith(name, StringComparison.CurrentCultureIgnoreCase)) break;
        if (r != list.Length)
        {
          for (int i = r; i < list.Length; ++i)
            if (list[i] != null && list[i].Equals(name, StringComparison.CurrentCultureIgnoreCase)) return i;
          return r;
        }
      }
      return -1;
    }
    public static BattleType BattleType(string name)
    {
      var gs = GetLanguage(name);
      var i = gs == null ? -1 : IndexOf(gs.BattleTypes, name);
      return i == -1 ? Game.BattleType.Invalid : (BattleType)(i + 1);
    }
    public static PokemonNature? Nature(string name)
    {
      var gs = GetLanguage(name);
      var i = gs == null ? -1 : IndexOf(gs.Natures, name);
      return i == -1 ? null : (PokemonNature?)(i);
    }
    public static PokemonSpecies PokemonSpecies(string name)
    {
      var gs = GetLanguage(name);
      var i = gs == null ? -1 : IndexOf(gs.Pokemons, name);
      if (i == -1 && gs == Current && Backup != null) i = IndexOf(Backup.Pokemons, name);
      return i == -1 ? null : RomData.GetPokemon(i + 1);
    }
    internal static PokemonForm PokemonForm(string name)
    {
      var gs = GetLanguage(name);
      foreach (var pair in gs.Forms)
      {
        var n = pair.Key / 100;
        var f = pair.Key % 100;
        var str = string.Format(pair.Value, gs.Pokemon(n));
        if (str == name) return RomData.GetPokemon(n).GetForm(f);
      }
      return PokemonSpecies(name).GetForm(0);
    }
    public static int Ability(string name)
    {
      var gs = GetLanguage(name);
      var i = gs == null ? -1 : IndexOf(gs.Abilities, name);
      if (i == -1 && gs == Current && Backup != null) i = IndexOf(Backup.Abilities, name);
      return i + 1;
    }
    private static int KeyOf(Dictionary<int, string> list, string name)
    {
      int sw = 0;
      if (list != null)
      {
        foreach (var p in list)
        {
          if (sw == 0 && p.Value.StartsWith(name, StringComparison.CurrentCultureIgnoreCase)) sw = p.Key;
          if (p.Value.Equals(name, StringComparison.CurrentCultureIgnoreCase)) return p.Key;
        }
      }
      return sw;
    }
    public static int Item(string name)
    {
      var gs = GetLanguage(name);
      var i = gs == null ? 0 : KeyOf(gs.Items, name);
      if (i == 0 && gs == Current && Backup != null) i = KeyOf(Backup.Items, name);
      return i;
    }
    public static MoveType Move(string name)
    {
      var gs = GetLanguage(name);
      var i = gs == null ? -1 : IndexOf(gs.Moves, name);
      if (i == -1 && gs == Current && Backup != null) i = IndexOf(Backup.Moves, name);
      return i == -1 ? null : RomData.GetMove(i + 1);
    }

    public readonly string Language;

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
    private string[] PokemonStates;
    private string[] StatTypes;

    private readonly Dictionary<string, string> BattleLogs;

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
          string str;
          var comma = line.IndexOf(':');
          str = line.Substring(comma + 1);
          if (char.IsDigit(line[1]))
          {
            var num = line.Substring(1, comma - 1).ToInt();
            var h = line[0];
            switch (h)
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
              case 'S':
                if (PokemonStates == null) PokemonStates = new string[7];
                PokemonStates[num] = str;
                break;
              case 's':
                if (StatTypes == null) StatTypes = new string[8];
                StatTypes[num] = str;
                break;
            }
          }
          else
          {
            var key = line.Substring(0, comma);
            if (BattleLogs == null) BattleLogs = new Dictionary<string, string>();
            BattleLogs[key] = str;
          }
        }//for (string line
    }

    public string Pokemon(int number)
    {
      var i = number - 1;
      return Pokemons[i] ?? InnerBackup.Pokemons[i];
    }
    public string Pokemon(int number, int form)
    {
      var i = number * 100 + form;
      return string.Format(Forms.ValueOrDefault(i) ?? InnerBackup.Forms.ValueOrDefault(i) ?? Pokemon(number), Pokemon(number));
    }
    public string PokemonForm(int number, int form)
    {
      var i = number * 100 + form;
      return string.Format(Forms.ValueOrDefault(i) ?? InnerBackup.Forms.ValueOrDefault(i) ?? Pokemon(number), string.Empty).Trim();
    }
    public string Pokemon(PokemonSpecies pokemon)
    {
      return Pokemon(pokemon.Number);
    }
    public string Pokemon(PokemonForm form)
    {
      return Pokemon(form.Species.Number, form.Index);
    }
    public string PokemonForm(PokemonForm form)
    {
      return PokemonForm(form.Species.Number, form.Index);
    }
    public string Move(int move)
    {
      var i = move - 1;
      return Moves.ValueOrDefault(i) ?? InnerBackup.Moves.ValueOrDefault(i);
    }
    public string Ability(int ability)
    {
      var i = ability - 1;
      return Abilities.ValueOrDefault(i) ?? InnerBackup.Abilities.ValueOrDefault(i);
    }
    public string Item(int item)
    {
      return Items.ValueOrDefault(item) ?? InnerBackup.Items.ValueOrDefault(item);
    }
    public string MoveD(int move)
    {
      var i = move - 1;
      var backup = InnerBackup.MovesD;
      return MovesD == null ? backup == null ? null : backup.ValueOrDefault(i) : MovesD.ValueOrDefault(i);
    }
    public string AbilityD(int ability)
    {
      var i = ability - 1;
      var backup = InnerBackup.AbilitiesD;
      return AbilitiesD == null ? backup == null ? null : backup.ValueOrDefault(i) : AbilitiesD.ValueOrDefault(i);
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
      return Natures == null ? backup == null ? null : backup.ValueOrDefault(i) : Natures.ValueOrDefault(i);
    }
    public string BattleType(BattleType type)
    {
      if (type == Game.BattleType.Invalid) return null;
      var i = (int)type - 1;
      var backup = InnerBackup.BattleTypes;
      return BattleTypes == null ? backup == null ? null : backup.ValueOrDefault(i) : BattleTypes.ValueOrDefault(i);
    }
    public string MoveCategory(MoveCategory category)
    {
      var i = (int)category;
      var backup = InnerBackup.MoveCategories;
      return MoveCategories == null ? backup == null ? null : backup.ValueOrDefault(i) : MoveCategories.ValueOrDefault(i);
    }
    public string StatType(StatType stat)
    {
      if (stat == Game.StatType.Invalid || stat == Game.StatType.All || StatTypes == null) return null;
      return StatTypes.ValueOrDefault(stat == Game.StatType.Hp ? 7 : (int)stat - 1) ?? stat.ToString();
    }
    public string PokemonState(PokemonState state)
    {
      if (state == Game.PokemonState.Normal || PokemonStates == null) return null;
      return PokemonStates.ValueOrDefault((int)state - 1) ?? state.ToString();
    }
    public string BattleLog(string key)
    {
      return BattleLogs.ValueOrDefault(key);
    }
  }
}
