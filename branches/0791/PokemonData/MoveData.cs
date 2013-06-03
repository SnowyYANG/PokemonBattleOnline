using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace PokemonBattle.PokemonData
{
    [Serializable()]
    public class MoveData
    {
        public int Identity
        { get; set; }

        public string Name
        { get; set; }

        public int Type
        { get; set; }

        public MoveType MoveType
        { get; set; }

        public int PP
        { get; set; }

        public int Power
        { get; set; }

        public int Priority
        { get; set; }

        public double Accuracy
        { get; set; }

        public MoveTarget Target
        { get; set; }

        public MoveEffect Effect
        { get; set; }

        public MoveAdditionalEffect AddEffect1
        { get; set; }

        public MoveAdditionalEffect AddEffect2
        { get; set; }

        public double AddEffectOdds
        { get; set; }

        public bool Contact
        { get; set; }
        
        public bool Sound
        { get; set; }

        public bool KingRock
        { get; set; }

        public bool Snatchable
        { get; set; }

        public bool AttackAtTarget
        { get; set; }

        public bool Substitute
        { get; set; }

        public bool Protectable
        { get; set; }

        public bool Punch
        { get; set; }

        public string Info
        { get; set; }

        public MoveData()
        {
            Type = PokemonType.InvalidId;
            Info = string.Empty;
        }

        public void Save(Stream output)
        {
            BinaryWriter writer = new BinaryWriter(output);

            writer.Write(Identity);

            writer.Write((Int32)MoveType);

            writer.Write((Int32)Target);

            writer.Write((Int32)Effect);

            writer.Write((Int32)AddEffect1);

            writer.Write((Int32)AddEffect2);

            writer.Write(Name);

            writer.Write(Type);

            writer.Write(Info);

            writer.Write(PP);

            writer.Write(Power);

            writer.Write(Priority);

            writer.Write(Accuracy);

            writer.Write(AddEffectOdds);

            writer.Write(Contact);

            writer.Write(Sound);

            writer.Write(KingRock);

            writer.Write(Snatchable);

            writer.Write(AttackAtTarget);

            writer.Write(Substitute);

            writer.Write(Protectable);

            writer.Write(Punch);

        }

        public static MoveData FromStream(Stream input)
        {
            MoveData data = new MoveData();
            BinaryReader reader = new BinaryReader(input);

            data.Identity = reader.ReadInt32();

            data.MoveType = (MoveType)reader.ReadInt32();

            data.Target = (MoveTarget)reader.ReadInt32();

            data.Effect = (MoveEffect)reader.ReadInt32();

            data.AddEffect1 = (MoveAdditionalEffect)reader.ReadInt32();

            data.AddEffect2 = (MoveAdditionalEffect)reader.ReadInt32();

            data.Name = reader.ReadString();

            data.Type = reader.ReadInt32();

            data.Info = reader.ReadString();

            data.PP = reader.ReadInt32();

            data.Power = reader.ReadInt32();

            data.Priority = reader.ReadInt32();

            data.Accuracy = reader.ReadDouble();

            data.AddEffectOdds = reader.ReadDouble();

            data.Contact = reader.ReadBoolean();

            data.Sound = reader.ReadBoolean();

            data.KingRock = reader.ReadBoolean();

            data.Snatchable = reader.ReadBoolean();

            data.AttackAtTarget = reader.ReadBoolean();

            data.Substitute = reader.ReadBoolean();

            data.Protectable = reader.ReadBoolean();

            data.Punch = reader.ReadBoolean();

            return data;
        }


        public const int InvalidId = -1;

        public const int MaxAccuracy = 100;
    }
    

}
