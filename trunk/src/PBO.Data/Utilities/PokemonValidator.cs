using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace PokemonBattleOnline.Data
{
    public static class PokemonValidator
    {

        #region 6D

        public static bool ValidateIv(this I6D iv)
        {
            return !iv.Any((p) => p < 0 && p > 31);
        }

        public static bool ValidateEv(this I6D ev)
        {
            return !ev.Any((p) => p < 0 && p > 255) && ev.Sum() <= 510;
        }

        #endregion

        #region IPokemonData

        public static bool ValidateName(string name)
        {
            return Regex.IsMatch(name, @"^\w{1,20}$", RegexOptions.Compiled);
            //return name == null || name.Length < 11 && !name.Any((c) => c == '\n' || c == '\r' || c == '\t');
        }

        public static bool Shiney(IPokemonData pm, int random)
        {
            return random % 1366 == 0;
        }

        public static bool ValidateLv(int lv)
        {
            return 0 < lv && lv <= 100;
        }

        public static bool ValidateAbility(IPokemonData pm)
        {
            return pm.Form.Data.GetAbility(pm.AbilityIndex) != null;
        }

        public static bool ValidateMoves(IPokemonData pm)
        {
            return pm.Moves.Count() <= 4;
        }

        public static bool Validate(this IPokemonData pm)
        {
            return true;
            return
              pm.Form != null &&
              ValidateAbility(pm) &&
              ValidateEv(pm.Ev) &&
              ValidateLv(pm.Lv) &&
              ValidateIv(pm.Iv) &&
              ValidateMoves(pm);
        }

        /// <summary>
        /// all but Name
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool ValueEquals(this IPokemonData a, IPokemonData b)
        {
            return
              a.AbilityIndex == b.AbilityIndex &&
              a.Ev.Equals(b.Ev) &&
              a.Iv.Equals(b.Iv) &&
              a.Form == b.Form &&
              a.Gender == b.Gender &&
              a.Happiness == b.Happiness &&
              a.ItemId == b.ItemId && 
              a.Lv == b.Lv &&
              a.Moves.SequenceEqual(b.Moves) &&
              a.Nature == b.Nature;
        }

        #endregion

    }
}