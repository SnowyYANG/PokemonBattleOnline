using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace LightStudio.PokemonBattle.Data
{
    class Helper
    {
        public static PokemonData Import(string source)
        {
            source = Regex.Replace(source, @"\r\n|\r", "\n") + "\n";
            if ( Regex.IsMatch( source, @"(.+?)（(.+?)）Lv.(\d+)")) ImportFromPBO(source);
            else if ( Regex.IsMatch( source, @"(.+?)(\-\w){0,1} (\([FM]\) ){0,1} @ (.+?)\n")) ImportFromPO(source);

            //Gliscor (M) @ Flying Gem
            //Trait: Sand Veil
            //EVs: 4 HP / 252 Atk / 252 Spd
            //Jolly Nature (+Spd, -SAtk)
            //- Guillotine
            //- Earthquake
            //- Acrobatics
            //- U-turn
        }
        
        private static ImportFromPO(string source)
        {
            // 1: Pokemon
            // 2: Form
            // 3: Gender
            // 4: Item
            // 5: Ability
            // 6: EVs
            // 7: Nature
            // 8~11: Moves
            var ms = Regex.Matches(source, @"(.+?)(\-\w){0,1} (\([FM]\) ){0,1}@ (.+?)\nTrait: (.+?)\nEVs: (.+?)\n(.+?) Nature.+\n(?:\- (.+?)\n)+");
            for (var i = 0; i < ms.Count; i++)
            {
                var pt = GameDataService.Rom.GetPokemonType(ms[i].Groups[1].Value);
                if ( pt != null ) {
                    var pm = new PokemonData(pt.Number, 0);
                    var Item = GameDataService.Rom.GetItem(ms[i].Groups[4].Value);
                    var Ability = GameDataService.Rom.GetAbility(ms[i].Groups[5].Value);
                    var Nature = PokemonNatureHelper.GetNature(ms[i].Groups[7].Value);

                    pm.Gender = GetGender(ms[i].Groups[3].Value);
                    if ( Item != null ) pm.Item = Item;
                    if ( Ability != null ) pm.Ability = Ability;
                    if ( Nature != null ) pm.Nature = Nature.Value;
                    pm.Ev.Hp = TryMatch(ms[i].Groups[6].Value, @"(\d+) HP", 1, 0);
                    pm.Ev.Atk = TryMatch(ms[i].Groups[6].Value, @"(\d+) Atk", 1, 0);
                    pm.Ev.Def = TryMatch(ms[i].Groups[6].Value, @"(\d+) Def", 1, 0);
                    pm.Ev.SpAtk = TryMatch(ms[i].Groups[6].Value, @"(\d+) SAtk", 1, 0);
                    pm.Ev.SpDef = TryMatch(ms[i].Groups[6].Value, @"(\d+) SDef",1, 0);
                    pm.Ev.Speed = TryMatch(ms[i].Groups[6].Value, @"(\d+) Spd", 1, 0);
                    for (var j = 8; j < ms[i].Length; j++) {
                        var Move = GameDataService.Rom.GetMoveType(ms[i].Groups[j].Value);
                        if ( Move != null ) pm.AddMove(Move);
                    }
                }
            }
        }
        private static ImportFromPBO(string source)
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
            var ms = Regex.Matches(source, @"(.+?)（(.+?)）Lv.(\d+)(?: *(.)){0,1}\n\* 特性： *(.+?)\n\* 性格： *(.+?)\n(?:\* 个体值： *(.+?)\n){0,1}\* 努力值： *(.+?)\n(?:\* 亲密度： *(\d+?)\n){0,1}\* 道具： *(.+?)\n\* 技能： *(.+?)\n");
            for (var i = 0; i < ms.Count; i++)
            {
                var pt = GameDataService.Rom.GetPokemonType(ms[i].Groups[2].Value);
                if ( pt != null ) {
                    var pm = new PokemonData(pt.Number, 0);
                    var Level = GameDataService.Rom.GetItem(ms[i].Groups[3].Value);
                    var Ability = GameDataService.Rom.GetAbility(ms[i].Groups[5].Value);
                    var Nature = PokemonNatureHelper.GetNature(ms[i].Groups[6].Value);
                    var Happiness = ms[i].Groups[9].Value;
                    var Item = GameDataService.Rom.GetItem(ms[i].Groups[10].Value);
                    
                    pm.Name = ms[i].Groups[1].Value;
                    pm.Gender = GetGender(ms[i].Groups[4].Value);
                    if ( Item != null ) pm.Item = Item;
                    if ( Ability != null ) pm.Ability = Ability;
                    if ( Nature != null ) pm.Nature = Nature.Value;
                    pm.Happiness = ToInt(Happiness);

                    if ( ms[i].Groups[7].Value.Length > 0 ) {
                        var IVs = ms[i].Groups[7].Value.Split('/');
                        pm.Iv.Hp  = ToInt(IVs[0]);
                        pm.Iv.Atk = ToInt(IVs[1]);
                        pm.Iv.Def = ToInt(IVs[2]);
                        pm.Iv.SpAtk = ToInt(IVs[3]);
                        pm.Iv.SpDef = ToInt(IVs[4]);
                        pm.Iv.Speed = ToInt(IVs[5]);
                    }

                    var EVs = ms[i].Groups[8].Value.Split('/');
                    pm.Ev.Hp  = ToInt(EVs[0]);
                    pm.Ev.Atk = ToInt(EVs[1]);
                    pm.Ev.Def = ToInt(EVs[2]);
                    pm.Ev.SpAtk = ToInt(EVs[3]);
                    pm.Ev.SpDef = ToInt(EVs[4]);
                    pm.Ev.Speed = ToInt(EVs[5]);
                    
                    foreach (var m in ms[i].Groups[11].Value.Split('/')) {
                        var Move = GameDataService.Rom.GetMoveType(m);
                        if ( Move != null ) pm.AddMove(Move);
                    }
                }
            }
        }
        public static string Export(PokemonData pm)
        {
            var  sb = new StringBuilder();
            sb.Append (pm.Name, "（", pm.Form.Type.Name, "）", " Lv.", pm.Lv );
            switch (pm.Gender)
            {
                case PokemonGender.Male : sb.Append(" ♂"); break;
                case PokemonGender.Female  : sb.Append(" ♀"); break;
            }
            sb.AppendLine();
            sb.AppendLine ("* 特性：  ", pm.Ability.GetLocalizedName() );
            sb.AppendLine ("* 性格：  ", pm.Nature.GetLocalizedName() );
            if (pm.Ev.Hp<31 && pm.Ev.Atk <31 && pm.Ev.Def <31 && pm.Ev.SpAtk <31 && pm.Ev.SpDef <31 && pm.Ev.Speed  <31 ) 
                sb.AppendLine ("* 个体值：", pm.Ev.Hp, "/", pm.Ev.Atk, "/",  pm.Ev.Def, "/",  pm.Ev.SpAtk, "/", pm.Ev.SpDef, "/",  pm.Ev.Speed);
            sb.AppendLine ("* 努力值：", pm.Iv.Hp, "/",pm.Iv.Atk, "/",  pm.Iv.Def, "/",  pm.Iv.SpAtk, "/",  pm.Iv.SpDef, "/",  pm.Iv.Speed);
            if (pm.Happiness < 255 ) sb.AppendLine ("* 亲密度：", pm.Happiness);
            sb.AppendLine ("* 道具：  ", pm.Item == null ? "无" : pm.Item.GetLocalizedName() );
            sb.Append ("* 技能：  " );
            if ( pm.Moves.Count() == 0 ) {
                sb.Append ("无" );
            } else {
                bool first = true;
                foreach(var m in pm.Moves){
                    sb.Append (m.Move.GetLocalizedName() );
                    if (first) first = false;
                    else sb.Append("/");
                }
            }
            return sb.ToString();
        }

        private static PokemonGender GetGender(string s)
        {
            switch (s)
            {
                case "M": return PokemonGender.Male;
                case "F": return PokemonGender.Female;
                case "♂": return PokemonGender.Male;
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
