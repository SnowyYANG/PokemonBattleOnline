using System;
using System.Collections.Generic;
using System.Text;

namespace PokemonBattle.PokemonData.Custom
{
    [Serializable()]
    public class UpdatePokemonData
    {
        public int Identity
        { get; set; }

        public string NameBase
        { get; set; }

        public double Weight
        { get; set; }

        public int Number
        { get; set; }

        public byte HPBase
        { get; set; }

        public byte AttackBase
        { get; set; }

        public byte DefenceBase
        { get; set; }

        public byte SpeedBase
        { get; set; }

        public byte SpAttackBase
        { get; set; }

        public byte SpDefenceBase
        { get; set; }

        public int Type1
        { get; set; }

        public int Type2
        { get; set; }

        public Trait Trait1
        { get; set; }

        public Trait Trait2
        { get; set; }

        public List<int> AddMoves
        { get; private set; }

        public List<int> RemoveMoves
        { get; private set; }

        public UpdatePokemonData()
        {
            AddMoves = new List<int>();
            RemoveMoves = new List<int>();
            Type1 = PokemonType.InvalidId;
            Type2 = PokemonType.InvalidId;
        }

        public UpdatePokemonData Clone()
        {
            UpdatePokemonData data = MemberwiseClone() as UpdatePokemonData;
            data.AddMoves = new List<int>(AddMoves);
            data.RemoveMoves = new List<int>(RemoveMoves);
            return data;
        }

        public bool Equals(UpdatePokemonData data)
        {
            if (data == null) return false;
            if (data.Identity != Identity) return false;
            if (data.NameBase != NameBase) return false;
            if (data.Weight != Weight) return false;
            if (data.Number != Number) return false;

            if (data.HPBase != HPBase) return false;
            if (data.AttackBase != AttackBase) return false;
            if (data.DefenceBase != DefenceBase) return false;
            if (data.SpeedBase != SpeedBase) return false;
            if (data.SpAttackBase != SpAttackBase) return false;
            if (data.SpDefenceBase != SpDefenceBase) return false;

            if (data.Type1 != Type1) return false;
            if (data.Type2 != Type2) return false;
            if (data.Trait1 != Trait1) return false;
            if (data.Trait2 != Trait2) return false;

            if (data.AddMoves.Count != AddMoves.Count) return false;
            for (int i = 0; i < data.AddMoves.Count; i++)
            {
                if (data.AddMoves[i] != AddMoves[i]) return false;
            }
            if (data.RemoveMoves.Count != RemoveMoves.Count) return false;
            for (int i = 0; i < data.RemoveMoves.Count; i++)
            {
                if (data.RemoveMoves[i] != RemoveMoves[i]) return false;
            }
            return true;
        }
    }
}
