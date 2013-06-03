using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace PokemonBattle.PokemonData
{
    [Serializable]
    public class PokemonData
    {
        public int Identity
        { get; set; }

        public string Name
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

        public Trait Trait3
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

        public List<MoveLearnData> Moves
        { get; private set; }

        public Item ItemRestriction
        { get; set; }

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

        public PokemonData()
        {
            Moves = new List<MoveLearnData>();
            AfterEvolution = new List<int>();
            Type1 = PokemonType.InvalidId;
            Type2 = PokemonType.InvalidId;

            FrontImage = InvalidImage;
            FrontImageF = InvalidImage;
            BackImage = InvalidImage;
            BackImageF = InvalidImage;
            Icon = InvalidImage;
            Frame = InvalidImage;
            FrameF = InvalidImage;

            
        }

        public MoveLearnData GetMove(int moveId)
        {
            return Moves.Find((move) => move.MoveId == moveId);
        }

        public void Save(Stream output)
        {
            BinaryWriter writer = new BinaryWriter(output);

            writer.Write((Int32)Trait1);

            writer.Write((Int32)Trait2);

            writer.Write((Int32)Trait3);

            writer.Write((Int32)EggGroup1);

            writer.Write((Int32)EggGroup2);

            writer.Write((Int32)GenderRestriction);

            writer.Write(Identity);

            writer.Write(Name);

            writer.Write(Number);

            writer.Write(Weight);

            writer.Write(HPBase);

            writer.Write(AttackBase);

            writer.Write(DefenceBase);

            writer.Write(SpeedBase);

            writer.Write(SpAttackBase);

            writer.Write(SpDefenceBase);

            writer.Write(Type1);

            writer.Write(Type2);

            writer.Write(BeforeEvolution);

            writer.Write(FrontImage);

            writer.Write(FrontImageF);

            writer.Write(BackImage);

            writer.Write(BackImageF);

            writer.Write(Icon);

            writer.Write(Frame);

            writer.Write(FrameF);

            writer.Write(AfterEvolution.Count);
            foreach (int identity in AfterEvolution)
            {
                writer.Write(identity);
            }
            writer.Write(Moves.Count);
            foreach (MoveLearnData move in Moves)
            {
                move.Save(output);
            }

            writer.Write((int)ItemRestriction);
        }

        public static PokemonData FromStream(Stream input)
        {
            PokemonData data = new PokemonData();
            BinaryReader reader = new BinaryReader(input);

            data.Trait1 = (Trait)reader.ReadInt32();

            data.Trait2 = (Trait)reader.ReadInt32();

            data.Trait3 = (Trait)reader.ReadInt32();

            data.EggGroup1 = (EggGroup)reader.ReadInt32();

            data.EggGroup2 = (EggGroup)reader.ReadInt32();

            data.GenderRestriction = (PokemonGenderRestriction)reader.ReadInt32();

            data.Identity = reader.ReadInt32();

            data.Name = reader.ReadString();

            data.Number = reader.ReadInt32();

            data.Weight = reader.ReadDouble();

            data.HPBase = reader.ReadByte();

            data.AttackBase = reader.ReadByte();

            data.DefenceBase = reader.ReadByte();

            data.SpeedBase = reader.ReadByte();

            data.SpAttackBase = reader.ReadByte();

            data.SpDefenceBase = reader.ReadByte();

            data.Type1 = reader.ReadInt32();

            data.Type2 = reader.ReadInt32();

            data.BeforeEvolution = reader.ReadInt32();

            data.FrontImage = reader.ReadInt64();

            data.FrontImageF = reader.ReadInt64();

            data.BackImage = reader.ReadInt64();

            data.BackImageF = reader.ReadInt64();

            data.Icon = reader.ReadInt64();

            data.Frame = reader.ReadInt64();

            data.FrameF = reader.ReadInt64();

            int count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                data.AfterEvolution.Add(reader.ReadInt32());
            }
            count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                data.Moves.Add(MoveLearnData.FromStream(input));
            }

            data.ItemRestriction = (Item)reader.ReadInt32();

            return data;
        }

        public const int InvalidImage = -1;
    }


}
