using System;
using System.Collections.Generic;
using System.Text;

namespace PokemonBattle.PokemonData.Custom
{
    [Serializable()]
    public class CustomPokemonData
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
         
        public EggGroup EggGroup1
        { get; set; } 

        public EggGroup EggGroup2
        { get; set; }
         
        public int BeforeEvolution
        { get; set; }

        public List<int> AfterEvolution
        { get; private set; }

        public PokemonGenderRestriction GenderRestriction
        { get; set; }

        public List<int> Moves
        { get; private set; }

        #region Image
        public long FrontImage
        { get; set; }

        public long FrontImageF
        { get; set; }

        public long BackImage
        { get; set; } 

        public long BackImageF
        { get; set; } 

        public long Icon
        { get; set; } 

        public long Frame
        { get; set; } 

        public long FrameF
        { get; set; }
        #endregion

        public CustomPokemonData()
        {
            Moves = new List<int>();
            AfterEvolution = new List<int>();

            Type1 = PokemonType.InvalidId;
            Type2 = PokemonType.InvalidId;

            FrontImage = PokemonData.InvalidImage;
            FrontImageF = PokemonData.InvalidImage;
            BackImage = PokemonData.InvalidImage;
            BackImageF = PokemonData.InvalidImage;
            Icon = PokemonData.InvalidImage;
            Frame = PokemonData.InvalidImage;
            FrameF = PokemonData.InvalidImage;
        }

        public CustomPokemonData Clone()
        {
            CustomPokemonData data = MemberwiseClone() as CustomPokemonData;
            data.AfterEvolution = new List<int>(AfterEvolution);
            data.Moves = new List<int>(Moves);
            return data;
        }

        public bool Equals(CustomPokemonData data)
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
            if (data.EggGroup1 != EggGroup1) return false;
            if (data.EggGroup2 != EggGroup2) return false;

            if (data.BeforeEvolution != BeforeEvolution) return false;
            if (data.AfterEvolution.Count != AfterEvolution.Count) return false;
            for (int i = 0; i < data.AfterEvolution.Count; i++)
            {
                if (data.AfterEvolution[i] != AfterEvolution[i]) return false;
            }
            if (data.GenderRestriction != GenderRestriction) return false;

            if (data.Moves.Count != Moves.Count) return false;
            for (int i = 0; i < data.Moves.Count; i++)
            {
                if (data.Moves[i] != Moves[i]) return false;
            }

            if (data.FrontImage != FrontImage) return false;
            if (data.FrontImageF != FrontImageF) return false;
            if (data.BackImage != BackImage) return false;
            if (data.BackImageF != BackImageF) return false;
            if (data.Frame != Frame) return false;
            if (data.FrameF != FrameF) return false;
            if (data.Icon != Icon) return false;

            return true;
        }
    }
}
