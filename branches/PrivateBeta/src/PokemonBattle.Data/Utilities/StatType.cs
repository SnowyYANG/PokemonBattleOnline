using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightStudio.PokemonBattle.Data
{
    public enum StatType : byte
    {
        Invalid,
        Atk,
        Def,
        SpAtk,
        SpDef,
        Speed,
        Accuracy,
        Evasion,
        /// <summary>
        /// 是指A/D/SA/SD/S，共5个
        /// </summary>
        All,
        Hp
    }

    public static class StatTypeHelper
    {
        public readonly static StatType[] Type5D = { StatType.Atk, StatType.Def, StatType.Speed, StatType.SpAtk, StatType.SpDef };
        public readonly static StatType[] Type6D = { StatType.Hp, StatType.Atk, StatType.Def, StatType.Speed, StatType.SpAtk, StatType.SpDef };
        public readonly static StatType[] Type7D = { StatType.Atk, StatType.Def, StatType.Speed, StatType.SpAtk, StatType.SpDef, StatType.Accuracy, StatType.Evasion };
    }
}
