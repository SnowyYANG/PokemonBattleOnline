using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace PokemonBattleOnline.Game
{
    public static class Helper
    {
        public static void Import(string source, PokemonData[] target)
        {
            source = Regex.Replace(source, @"\r\n|\r", "\n") + "\n";
            if (Regex.IsMatch(source, @".+?（.+?） *Lv\.\d+")) ImportFromPBO(source, target);
            else if (Regex.IsMatch(source, @".+?(\-\w){0,1} (\(.+?\) ){0,1}(\([FM]\) ){0,1}@ .+?\n", RegexOptions.IgnoreCase)) ImportFromPO(source, target);
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
            // 1: Nickname
            // 2: Form
            // 3: Pokemon
            // 4: Gender
            // 5: Item
            // 6: Ability
            // 7: EVs
            // 8: Nature
            // 9~12: Moves

            var hasNickname = m.Groups[3].Value.Length > 0;
            var pname = m.Groups[3].Value;
            if (!hasNickname) pname = m.Groups[1].Value;
            var pm = new PokemonData(GameString.PokemonSpecies(pname).Number, 0);
            var Item = GameString.Item(m.Groups[5].Value);
            var Ability = GameString.Ability(m.Groups[6].Value);
            var Nature = GameString.Nature(m.Groups[8].Value);

            pm.Gender = GetGender(m.Groups[4].Value);
            if (hasNickname) pm.Name = m.Groups[1].Value;
            if (Item != null) pm.Item = Item;
            if (Ability != null) pm.Ability = Ability.Value;
            if (Nature != null) pm.Nature = Nature.Value;
            pm.Ev.Hp = TryMatch(m.Groups[7].Value, @"(\d+) HP", 1, 0);
            pm.Ev.Atk = TryMatch(m.Groups[7].Value, @"(\d+) Atk", 1, 0);
            pm.Ev.Def = TryMatch(m.Groups[7].Value, @"(\d+) Def", 1, 0);
            pm.Ev.SpAtk = TryMatch(m.Groups[7].Value, @"(\d+) SAtk", 1, 0);
            pm.Ev.SpDef = TryMatch(m.Groups[7].Value, @"(\d+) SDef", 1, 0);
            pm.Ev.Speed = TryMatch(m.Groups[7].Value, @"(\d+) Spd", 1, 0);
            foreach (Match m2 in Regex.Matches(m.Groups[9].Value, @"\- (.+?)(?: \[(.+?)\])*\n"))
            {
                var Move = GameString.Move(m2.Groups[1].Value);
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
            var pm = new PokemonData(GameString.PokemonSpecies(m.Groups[2].Value).Number, 0);
            pm.Name = m.Groups[1].Value;
            pm.Lv = ToInt(m.Groups[3].Value);
            pm.Gender = GetGender(m.Groups[4].Value);
            var ab = GameString.Ability(m.Groups[5].Value);
            if (ab != null) pm.Ability = ab.Value;
            pm.Nature = GameString.Nature(m.Groups[6].Value) ?? PokemonNature.Hardy;
            if (m.Groups[9].Value.Length > 0) pm.Happiness = ToInt(m.Groups[9].Value);
            pm.Item = GameString.Item(m.Groups[10].Value);

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

            foreach (var s in Regex.Replace(m.Groups[11].Value, @"\[.+?\]", "").Split('/'))
            {
                var move = GameString.Move(s);
                if (move != null) pm.AddMove(move);
            }

            return pm;
        }

        private static void ImportFromPO(string source, PokemonData[] target)
        {
            int i = 0;
            foreach (Match m in Regex.Matches(source, @"(.+?)(\-\w){0,1} (?:\((.{2,}?)(?:\-\w){0,1}\) ){0,1}(?:\(([FM])\) ){0,1}@ (.+?)\nTrait: (.+?)\nEVs: (.+?)\n(.+?) Nature.*\n((?:\- .+?\n)+)"))
            {
                try
                {
                    target[i++] = ImportFromPO(m);
                }
                catch { }
                if (i == target.Length) break;
            }
        }

        private static void ImportFromPBO(string source, PokemonData[] target)
        {
            int i = 0;  
            foreach (Match m in Regex.Matches(source, @"(.+?)（(.+?)） *Lv.(\d+)(?: *(.)){0,1}\n\* 特性：[ 　]*(.+?)\n\* 性格：[ 　]*(.+?)\n(?:\* 个体值{0,1}：[ 　]*(.+?)\n){0,1}\* 努力值{0,1}：[ 　]*(.+?)\n(?:\* 亲密度：[ 　]*(\d+?)\n){0,1}\* 道具：[ 　]*(.+?)\n\* 技能：[ 　]*(.+?)\n"))
            {
                try
                {
                    target[i++] = ImportFromPBO(m);
                }
                catch { }
                if (i == target.Length) break;
            }
        }

        private static void Export(StringBuilder sb, PokemonData pm)
        {
            const string space = "";//"　";
            sb.Append(pm.Name, "（", GameString.Current.Pokemon(pm.Form.Species.Number), "）", " Lv.", pm.Lv);
            if (pm.Gender == PokemonGender.Male) sb.Append(" ♂");
            else if (pm.Gender == PokemonGender.Female) sb.Append(" ♀");
            sb.AppendLine();
            sb.AppendLine("* 特性：", space, GameString.Current.Ability(pm.Ability));
            sb.AppendLine("* 性格：", space, GameString.Current.Nature(pm.Nature));
            {
                var ss = pm.Iv;
                if (ss.Hp != 31 || ss.Atk != 31 || ss.Def < 31 || ss.SpAtk != 31 || ss.SpDef != 31 || ss.Speed != 31)
                    sb.AppendLine("* 个体：", space, ss.Hp, "/", ss.Atk, "/", ss.Def, "/", ss.SpAtk, "/", ss.SpDef, "/", ss.Speed);
                ss = pm.Ev;
                sb.AppendLine("* 努力：", space, ss.Hp, "/", ss.Atk, "/", ss.Def, "/", ss.SpAtk, "/", ss.SpDef, "/", ss.Speed);
            }
            if (pm.Happiness < 255) sb.AppendLine("* 亲密度：", pm.Happiness);
            sb.AppendLine("* 道具：", space, pm.Item == null ? "无" : GameString.Current.Item(pm.Item));
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
                    sb.Append(GameString.Current.Move(m.Move.Id));
                    if (m.Move.Id == 237)
                    {
                        int pI;
                        pI = pm.Iv.Hp & 1;
                        pI |= (pm.Iv.Atk & 1) << 1;
                        pI |= (pm.Iv.Def & 1) << 2;
                        pI |= (pm.Iv.Speed & 1) << 3;
                        pI |= (pm.Iv.SpAtk & 1) << 4;
                        pI |= (pm.Iv.SpDef & 1) << 5;
                        pI = pI * 15 / 63;
                        sb.Append("[", "斗飞毒地岩虫鬼钢火水草电超冰龙恶".Substring(pI, 1), "]");
                    }
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
            return sb.ToString().Trim(new char[] { '\r', '\n' });
        }

        private static PokemonGender GetGender(string s)
        {
            switch (s.ToUpper())
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
            var m = Regex.Match(input, pattern, RegexOptions.IgnoreCase);
            if (m.Success) return m.Groups[group].Value.ToInt();
            else return defaultvalue;
        }

        public static int ToInt(this string s)
        {
            int i = 0;
            int.TryParse(s, out i);
            return i;
        }
    }
}
