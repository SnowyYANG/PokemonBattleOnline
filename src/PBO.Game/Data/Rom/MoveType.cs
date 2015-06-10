﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace PokemonBattleOnline.Game
{
    public enum MoveCategory : byte
    {
        Status,
        Physical,
        Special
    }
    public enum MoveRange
    {
        /// <summary>
        /// 单体选择00
        /// </summary>
        Single,
        /// <summary>
        /// 本方随机01
        /// </summary>
        RandomSelfPokemon,
        /// <summary>
        /// 本方选择02
        /// </summary>
        SingleAlly,
        /// <summary>
        /// 对方选择03
        /// </summary>
        SingleFoe,
        /// <summary>
        /// 所有临近 自爆、冲浪04
        /// </summary>
        Adjacent,
        /// <summary>
        /// 对方临近05
        /// </summary>
        FoePokemons,
        /// <summary>
        /// 自己队伍与场上队友06
        /// </summary>
        SelfPokemons,
        /// <summary>
        /// 自己07
        /// </summary>
        Self,
        /// <summary>
        /// 所有精灵08
        /// </summary>
        All,
        /// <summary>
        /// 对方随机09
        /// </summary>
        RandomFoePokemon,
        /// <summary>
        /// 全场0A
        /// </summary>
        Board,
        /// <summary>
        /// 对方场地0B
        /// </summary>
        FoeField,
        /// <summary>
        /// 本方场地0C
        /// </summary>
        SelfField,
        /// <summary>
        /// 0D
        /// </summary>
        Varies,
    }
    public class MoveType
    {
        internal MoveType(int id, BattleType type, MoveCategory category, int power, int accuracy, int pp, MoveRange range)
        {
            _id = id;
            _type = type;
            _category = category;
            _power = power;
            _accuracy = accuracy;
            _pp = pp;
            _range = range;
        }

        private readonly int _id;
        public int Id
        { get { return _id; } }

        private readonly BattleType _type;
        public BattleType Type
        { get { return _type; } }

        private readonly MoveCategory _category;
        public MoveCategory Category
        { get { return _category; } }

        private readonly int _power;
        public int Power
        { get { return _power; } }

        private readonly int _accuracy;
        public int Accuracy
        { get { return _accuracy; } }

        private readonly int _pp;
        public int PP
        { get { return _pp; } }

        private readonly MoveRange _range;
        public MoveRange Range
        { get { return _range; } }
    }
}