using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace LightStudio.PokemonBattle.Data
{
  class Helper
  {
    public static PokemonBT Import(string source, int size)
    {
      source = Regex.Replace(source, @"\r\n|\r", "\n") + "\n";
      if (Regex.IsMatch(source, @".+?（.+?） *Lv\.\d+")) return ImportFromPBO(source, size);
      else if (Regex.IsMatch(source, @".+?(\-\w){0,1} (\([FM]\) ){0,1}@ .+?\n")) return ImportFromPO(source, size);
      return null;
      //Gliscor (M) @ Flying Gem
      //Trait: Sand Veil
      //EVs: 4 HP / 252 Atk / 252 Spd
      //Jolly Nature (+Spd, -SAtk)
      //- Guillotine
      //- Earthquake
      //- Acrobatics
      //- U-turn
    }

    private static PokemonData ImportFromPO(Match m)
    {
      // 1: Pokemon
      // 2: Form
      // 3: Gender
      // 4: Item
      // 5: Ability
      // 6: EVs
      // 7: Nature
      // 8~11: Moves
      var pm = new PokemonData(GameDataService.Rom.GetPokemonType(m.Groups[1].Value).Number, 0);
      var Item = GameDataService.Rom.GetItem(m.Groups[4].Value);
      var Ability = GameDataService.Rom.GetAbility(m.Groups[5].Value);
      var Nature = PokemonNatureHelper.GetNature(m.Groups[7].Value);

      pm.Gender = GetGender(m.Groups[3].Value);
      if (Item != null) pm.Item = Item;
      if (Ability != null) pm.Ability = Ability;
      if (Nature != null) pm.Nature = Nature.Value;
      pm.Ev.Hp = TryMatch(m.Groups[6].Value, @"(\d+) HP", 1, 0);
      pm.Ev.Atk = TryMatch(m.Groups[6].Value, @"(\d+) Atk", 1, 0);
      pm.Ev.Def = TryMatch(m.Groups[6].Value, @"(\d+) Def", 1, 0);
      pm.Ev.SpAtk = TryMatch(m.Groups[6].Value, @"(\d+) SAtk", 1, 0);
      pm.Ev.SpDef = TryMatch(m.Groups[6].Value, @"(\d+) SDef", 1, 0);
      pm.Ev.Speed = TryMatch(m.Groups[6].Value, @"(\d+) Spd", 1, 0);
      foreach ( Match m2 in Regex.Matches(m.Groups[8].Value, @"\- (.+?)\n") )
      {
        var Move = GameDataService.Rom.GetMoveType(m2.Groups[1].Value);
        if (Move != null) pm.AddMove(Move);
      }
      return pm;
    }
    private static PokemonData ImportFromPBO(Match m)
    {
      // 1: Nickname
      // 2: Pokemon
      // 3: Level
      // 4: Gender
      // 5: Ability
      // 6: Nature
      // 7: IVs
      // 8: EVs
      // 9: Happiness
      // 10: Items
      // 11: Moves
      var pm = new PokemonData(GameDataService.Rom.GetPokemonType(m.Groups[2].Value).Number, 0);
      pm.Name = m.Groups[1].Value;
      pm.Lv = ToInt(m.Groups[3].Value);
      pm.Gender = GetGender(m.Groups[4].Value);
      pm.Ability = GameDataService.Rom.GetAbility(m.Groups[5].Value);
      pm.Nature = PokemonNatureHelper.GetNature(m.Groups[6].Value) ?? PokemonNature.Hardy;
      if ( m.Groups[9].Value.Length > 0 ) pm.Happiness = ToInt(m.Groups[9].Value);
      pm.Item = GameDataService.Rom.GetItem(m.Groups[10].Value);

      if (!string.IsNullOrEmpty(m.Groups[7].Value))
      {
        var ivs = m.Groups[7].Value.Split('/');
        pm.Iv.Hp = ToInt(ivs[0]);
        pm.Iv.Atk = ToInt(ivs[1]);
        pm.Iv.Def = ToInt(ivs[2]);
        pm.Iv.SpAtk = ToInt(ivs[3]);
        pm.Iv.SpDef = ToInt(ivs[4]);
        pm.Iv.Speed = ToInt(ivs[5]);
      }

      var evs = m.Groups[8].Value.Split('/');
      pm.Ev.Hp = ToInt(evs[0]);
      pm.Ev.Atk = ToInt(evs[1]);
      pm.Ev.Def = ToInt(evs[2]);
      pm.Ev.SpAtk = ToInt(evs[3]);
      pm.Ev.SpDef = ToInt(evs[4]);
      pm.Ev.Speed = ToInt(evs[5]);

      foreach (var s in m.Groups[11].Value.Split('/'))
      {
        var move = GameDataService.Rom.GetMoveType(s);
        if (move != null) pm.AddMove(move);
      }

      return pm;
    }
    private static PokemonBT ImportFromPO(string source, int size)
    {
      var pms = new PokemonBT();
      foreach (Match m in Regex.Matches(source, @"(.+?)(\-\w){0,1} (\([FM]\) ){0,1}@ (.+?)\nTrait: (.+?)\nEVs: (.+?)\n(.+?) Nature.+\n((?:\- .+?\n)+)"))
      {
        try
        {
          pms.Add(ImportFromPO(m));
        }
        catch { }
        if (--size == 0) break;
      }
      return pms;
    }
    private static PokemonBT ImportFromPBO(string source, int size)
    {
      var pms = new PokemonBT();
      foreach (Match m in Regex.Matches(source, @"(.+?)（(.+?)） *Lv.(\d+)(?: *(.)){0,1}\n\* 特性：[ 　]*(.+?)\n\* 性格：[ 　]*(.+?)\n(?:\* 个体值{0,1}：[ 　]*(.+?)\n){0,1}\* 努力值{0,1}：[ 　]*(.+?)\n(?:\* 亲密度：[ 　]*(\d+?)\n){0,1}\* 道具：[ 　]*(.+?)\n\* 技能：[ 　]*(.+?)\n"))
      {
        try
        {
          pms.Add(ImportFromPBO(m));
        }
        catch { }
        if (--size == 0) break;
      }
      return pms;
    }
    private static void Export(StringBuilder sb, PokemonData pm)
    {
      const string space = "";//"　";
      sb.Append(pm.Name, "（", pm.Form.Type.Name, "）", " Lv.", pm.Lv);
      if (pm.Gender == PokemonGender.Male) sb.Append(" ♂");
      else if (pm.Gender == PokemonGender.Female) sb.Append(" ♀");
      sb.AppendLine();
      sb.AppendLine("* 特性：", space, pm.Ability.GetLocalizedName());
      sb.AppendLine("* 性格：", space, pm.Nature.GetLocalizedName());
      {
        var ss = pm.Iv;
        if (ss.Hp != 31 || ss.Atk != 31 || ss.Def < 31 || ss.SpAtk != 31 || ss.SpDef != 31 || ss.Speed != 31)
            sb.AppendLine("* 个体：", space, ss.Hp, "/", ss.Atk, "/", ss.Def, "/", ss.SpAtk, "/", ss.SpDef, "/", ss.Speed);
        ss = pm.Ev;
        sb.AppendLine("* 努力：", space, ss.Hp, "/", ss.Atk, "/", ss.Def, "/", ss.SpAtk, "/", ss.SpDef, "/", ss.Speed);
      }
      if (pm.Happiness < 255) sb.AppendLine("* 亲密度：", pm.Happiness);
      sb.AppendLine("* 道具：", space, pm.Item == null ? "无" : pm.Item.GetLocalizedName());
      sb.Append("* 技能：", space);
      if (pm.Moves.Count() == 0)
      {
        sb.Append("无");
      }
      else
      {
        bool first = true;
        foreach (var m in pm.Moves)
        {
          if (first) first = false;
          else sb.Append("/");
          sb.Append(m.Move.GetLocalizedName());
        }
      }
    }
    public static string Export(PokemonData pm)
    {
      var sb = new StringBuilder();
      Export(sb, pm);
      return sb.ToString();
    }
    public static string Export(IEnumerable<PokemonData> pms)
    {
      var sb = new StringBuilder();
      foreach (var pm in pms)
      {
        Export(sb, pm);
        sb.AppendLine();
        sb.AppendLine();
      }
      return sb.ToString();
    }

    private static PokemonGender GetGender(string s)
    {
      switch (s)
      {
        case "M":
        case "♂": return PokemonGender.Male;
        case "F":
        case "♀": return PokemonGender.Female;
        default: return PokemonGender.None;
      }
    }
    private static int TryMatch(string input, string pattern, short group = 0, int defaultvalue = 0)
    {
      var m = Regex.Match(input, pattern);
      if (m.Success) return Convert.ToInt32(m.Groups[group].Value);
      else return defaultvalue;
    }

    private static int ToInt(string s)
    {
      return Convert.ToInt32(s);
    }
  }
}
