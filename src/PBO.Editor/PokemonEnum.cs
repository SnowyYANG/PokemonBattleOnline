using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Data;

namespace LightStudio.PokemonBattle.PBO.Editor
{
    public static class PokemonEnum
    {
        public static readonly PokemonNature[] Natures;

        public static readonly BattleType[] HiddenPowerTypes;

        public static readonly BattleType[] BattleTypes;

        //public static readonly string[] Languages = new[] { "English", "Chinese" };

        static PokemonEnum()
        {
            Natures = (PokemonNature[])Enum.GetValues(typeof(PokemonNature));
            BattleTypes = (BattleType[])Enum.GetValues(typeof(BattleType));
            HiddenPowerTypes = BattleTypes.
                Where(t => t != BattleType.Invalid && t != BattleType.Normal).ToArray();
        }
    }
}
