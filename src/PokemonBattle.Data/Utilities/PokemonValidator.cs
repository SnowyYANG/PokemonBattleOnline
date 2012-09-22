using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightStudio.PokemonBattle.Data
{
    public static class PokemonValidator
    {
        public static bool ValidateLv(int value)
        {
            return value > 0 && value <= 100;
        }

        public static bool ValidateIv(int value)
        {
            return value >= 0 && value <= 31;
        }

        public static bool ValidateEv(PokemonCustomInfo pm)
        {
            return pm.HpEv + pm.AtkEv + pm.DefEv + pm.SpeedEv +
                pm.SpAtkEv + pm.SpDefEv <= 510;
        }

        public static bool ValidateMoves(PokemonCustomInfo pm)
        {
            return pm.MoveIds.Count() <= 4;
        }

        public static bool Validate(PokemonCustomInfo pm)
        {
            PokemonType pmType = DataService.GetPokemonType(pm.PokemonTypeId);
            if (pmType == null)
                return false;
            if (pm.AbilityIndex < 0 || pm.AbilityIndex >= pmType.Abilities.Count)
                return false;
            if (!ValidateEv(pm))
                return false;
            if (!ValidateLv(pm.Lv))
                return false;
            if (!ValidateIv(pm.HpIv))
                return false;
            if (!ValidateIv(pm.AtkIv))
                return false;
            if (!ValidateIv(pm.DefIv))
                return false;
            if (!ValidateIv(pm.SpeedIv))
                return false;
            if (!ValidateIv(pm.SpAtkIv))
                return false;
            if (!ValidateIv(pm.SpDefIv))
                return false;
            if (!ValidateMoves(pm))
                return false;
            return true;
        }

    }
}
