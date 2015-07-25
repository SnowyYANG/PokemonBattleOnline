using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace PokemonBattleOnline.Game
{
    public class PokemonForm
    {
        internal PokemonForm(PokemonSpecies species, int index)
        {
            _species = species;
            _index = index;
        }

        private readonly PokemonSpecies _species;
        public PokemonSpecies Species
        { get { return _species; } }

        private readonly int _index;
        public int Index
        { get { return _index; } }

        private PokemonFormData _data;
        public PokemonFormData Data
        {
            get { return _data; }
            internal set { _data = value; }
        }

        private static readonly BattleType[] ARCEUS = new BattleType[] { BattleType.Normal, BattleType.Fire, BattleType.Water, BattleType.Electric, BattleType.Grass, BattleType.Ice, BattleType.Fighting, BattleType.Poison, BattleType.Ground, BattleType.Flying, BattleType.Psychic, BattleType.Bug, BattleType.Rock, BattleType.Ghost, BattleType.Dragon, BattleType.Dark, BattleType.Steel, BattleType.Fairy };
        public BattleType Type1
        { get { return Species.Number == Ps.ARCEUS ? ARCEUS[_index] : _data.Type1; } }
        public BattleType Type2
        { get { return _data.Type2; } }
    }
}
