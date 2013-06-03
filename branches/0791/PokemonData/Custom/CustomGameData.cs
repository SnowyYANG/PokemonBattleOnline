using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace PokemonBattle.PokemonData.Custom
{
    [Serializable()]
    public class CustomGameData
    {
        #region Varibles and Properties

        public string Name
        { get; set; }

        public List<CustomPokemonData> CustomPokemons
        { get; private set; }

        public List<UpdatePokemonData> UpdatePokemons
        { get; private set; }

        public List<int> RemovePokemons
        { get; private set; }

        public List<Bitmap> Images
        { get; private set; }

        private int _idBase = 1000;

        public long ImageOffset
        { get; set; }

        #endregion

        public CustomGameData(string name)
        {
            Name = name;
            CustomPokemons = new List<CustomPokemonData>();
            UpdatePokemons = new List<UpdatePokemonData>();
            RemovePokemons = new List<int>();
            Images = new List<Bitmap>();
        }

        public Bitmap GetImage(long index)
        {
            if (index == -1) return null;
            return Images[(int)index];
        }

        public CustomPokemonData AddNewPokemon(string name)
        {
            _idBase++;
            CustomPokemonData pm = new CustomPokemonData();
            pm.Identity = _idBase;
            pm.NameBase = name;
            pm.Type1 = 1;
            pm.Trait1 = Trait.自我中心;
            pm.EggGroup1 = EggGroup.人型;
            pm.Number = 494 + CustomPokemons.Count;
            CustomPokemons.Add(pm);
            return pm;
        }

        public void RemoveCustomPokemon(CustomPokemonData pm)
        {
            CustomPokemons.Remove(pm);
            foreach (CustomPokemonData data in CustomPokemons)
            {
                if (data.Number > pm.Number)
                {
                    data.Number -= 1;
                }
            }
        }

        public UpdatePokemonData AddUpdatePokemon(int identity, string name)
        {
            UpdatePokemonData pm = new UpdatePokemonData();
            pm.Identity = identity;
            pm.NameBase = name;
            UpdatePokemons.Add(pm);
            return pm;
        }

        public CustomGameData Clone()
        {
            CustomGameData data = MemberwiseClone() as CustomGameData;
            data.Images = new List<Bitmap>(Images);
            data.RemovePokemons = new List<int>(RemovePokemons);

            data.CustomPokemons = new List<CustomPokemonData>();
            foreach (CustomPokemonData pm in CustomPokemons)
            {
                data.CustomPokemons.Add(pm.Clone());
            }

            data.UpdatePokemons = new List<UpdatePokemonData>();
            foreach (UpdatePokemonData pm in UpdatePokemons)
            {
                data.UpdatePokemons.Add(pm.Clone());
            }

            return data;
        }

        public bool Equals(CustomGameData data)
        {
            if (data == null) return false;
            if (_idBase != data._idBase) return false;

            if (data.CustomPokemons.Count != CustomPokemons.Count) return false;
            for (int i = 0; i < data.CustomPokemons.Count; i++)
            {
                if (!data.CustomPokemons[i].Equals(CustomPokemons[i])) return false;
            }

            if (data.RemovePokemons.Count != RemovePokemons.Count) return false;
            for (int i = 0; i < data.RemovePokemons.Count; i++)
            {
                if (!data.RemovePokemons[i].Equals(RemovePokemons[i])) return false;
            }

            if (data.Images.Count != Images.Count) return false;
            for (int i = 0; i < data.Images.Count; i++)
            {
                if (!data.Images[i].Equals(Images[i])) return false;
            }

            if (data.RemovePokemons.Count != RemovePokemons.Count) return false;
            for (int i = 0; i < data.RemovePokemons.Count; i++)
            {
                if (data.RemovePokemons[i] != RemovePokemons[i]) return false;
            }
            return true;
        }

        public void Save(string path)
        {
            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                using (BinaryWriter writer = new BinaryWriter(stream))
                {
                    writer.Write(Name);
                    writer.Write(_idBase);
                    formatter.Serialize(stream, CustomPokemons);
                    formatter.Serialize(stream, UpdatePokemons);
                    formatter.Serialize(stream, RemovePokemons);

                    long[] positions = new long[Images.Count];
                    byte[] buffer;
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        for (int i = 0; i < Images.Count; i++)
                        {
                            positions[i] = memoryStream.Position;
                            formatter.Serialize(memoryStream, Images[i]);
                        }
                        buffer = memoryStream.GetBuffer();
                    }

                    formatter.Serialize(stream, positions);
                    writer.Write(buffer);
                }
            }
        }

        public static CustomGameData FromFile(string path)
        {
            CustomGameData data;

            using (FileStream stream = new FileStream(path, FileMode.Open))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                using (BinaryReader reader = new BinaryReader(stream))
                {
                    data = new CustomGameData(reader.ReadString());
                    data._idBase = reader.ReadInt32();
                    data.CustomPokemons = formatter.Deserialize(stream) as List<CustomPokemonData>;
                    data.UpdatePokemons = formatter.Deserialize(stream) as List<UpdatePokemonData>;
                    data.RemovePokemons = formatter.Deserialize(stream) as List<int>;
                    long[] positions = formatter.Deserialize(stream) as long[];
                    data.ImageOffset = stream.Position;
                    foreach (CustomPokemonData pm in data.CustomPokemons)
                    {
                        if (pm.FrontImage != -1) pm.FrontImage = positions[(int)pm.FrontImage];
                        if (pm.FrontImageF != -1) pm.FrontImageF = positions[(int)pm.FrontImageF];
                        if (pm.BackImage != -1) pm.BackImage = positions[(int)pm.BackImage];
                        if (pm.BackImageF != -1) pm.BackImageF = positions[(int)pm.BackImageF];
                        if (pm.Frame != -1) pm.Frame = positions[(int)pm.Frame];
                        if (pm.FrameF != -1) pm.FrameF = positions[(int)pm.FrameF];
                        if (pm.Icon != -1) pm.Icon = positions[(int)pm.Icon];
                    }
                }
            }

            return data;
        }
    }
}
