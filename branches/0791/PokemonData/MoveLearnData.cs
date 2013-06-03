using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace PokemonBattle.PokemonData
{
    [Serializable()]
    public class MoveLearnData
    {
        public int MoveId
        { get; set; }

        //public string MoveName
        //{ get; set; }

        public string LearnBy
        { get; set; }

        public string Info
        { get; set; }

        public Trait WithoutTrait
        { get; set; }

        public MoveLearnData()
        {
            //MoveName = string.Empty;
            LearnBy = string.Empty;
            Info = string.Empty;
        }

        public void Save(Stream output)
        {
            BinaryWriter writer = new BinaryWriter(output);

            writer.Write(MoveId);

            writer.Write((Int32)WithoutTrait);

            //writer.Write(MoveName);

            writer.Write(LearnBy);

            writer.Write(Info);

        }
        public static MoveLearnData FromStream(Stream input)
        {
            MoveLearnData data = new MoveLearnData();
            BinaryReader reader = new BinaryReader(input);

            data.MoveId = reader.ReadInt32();

            data.WithoutTrait = (Trait)reader.ReadInt32();
            //data.MoveName = reader.ReadString();
            data.LearnBy = reader.ReadString();
            data.Info = reader.ReadString();


            return data;
        }
    
    }
}
