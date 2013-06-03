using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace PokemonBattle.PokemonData
{
    [Serializable()]
    public class PokemonType
    {
        public int Identity
        { get; set; }

        public string Name
        { get; set; }

        public Dictionary<int, double> TypeEffects
        { get; set; }

        public Dictionary<Weather, double> WeatherEffects
        { get; set; }

        public Image Image
        { get; set; }

        public PokemonType()
        {
            TypeEffects = new Dictionary<int, double>();
            WeatherEffects = new Dictionary<Weather, double>();
        }

        public void Save(Stream output)
        {
            BinaryWriter writer = new BinaryWriter(output);
            writer.Write(Identity);
            writer.Write(Name);
            writer.Write(TypeEffects.Count);
            foreach (int key in TypeEffects.Keys)
            {
                writer.Write(key);
                writer.Write(TypeEffects[key]);
            }
            writer.Write(WeatherEffects.Count);
            foreach (Weather weather in WeatherEffects.Keys)
            {
                writer.Write((int)weather);
                writer.Write(WeatherEffects[weather]);
            }
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(output, Image);
        }

        public static PokemonType FromStream(Stream input)
        {
            PokemonType data = new PokemonType();
            BinaryReader reader = new BinaryReader(input);
            data.Identity = reader.ReadInt32();
            data.Name = reader.ReadString();
            int count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                data.TypeEffects.Add(reader.ReadInt32(), reader.ReadDouble());
            }
            count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                data.WeatherEffects.Add((Weather)reader.ReadInt32(), reader.ReadDouble());
            }
            BinaryFormatter formatter = new BinaryFormatter();
            data.Image = formatter.Deserialize(input) as Image;
            return data;

        }

        public const int InvalidId = -1;
    }
}
