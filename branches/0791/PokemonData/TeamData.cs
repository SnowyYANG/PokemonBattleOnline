using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using PokemonBattle.PokemonData.Custom;

namespace PokemonBattle.PokemonData
{
    public class TeamData
    {
        public PokemonCustomInfo[] Pokemons
        {
            get;
            private set;
        }

        public CustomDataInfo CustomInfo
        { get; set; }

        public TeamData()
        {
            Pokemons = new PokemonCustomInfo[6]; 
            for (int i = 0; i < 6; i++)
            {
                Pokemons[i] = new PokemonCustomInfo();
            }
            CustomInfo = new CustomDataInfo();
        }

        public byte[] ToBytes()
        {
            byte[] bytes;
            using (MemoryStream stream = new MemoryStream())
            {
                Save(stream);
                bytes = new byte[(int)stream.Position];
                Array.Copy(stream.GetBuffer(), bytes, bytes.Length);
            }
            return bytes;
        }
        public static TeamData FromBytes(byte[] bytes)
        {
            TeamData data;
            using (MemoryStream stream = new MemoryStream())
            {
                stream.Write(bytes, 0, bytes.Length);
                stream.Seek(0, SeekOrigin.Begin);
                data = FormStream(stream);
            }
            return data;
        }

        public TeamData Clone()
        {
            TeamData data = new TeamData();
            data.CustomInfo.DataHash = CustomInfo.DataHash;
            data.CustomInfo.DataName = CustomInfo.DataName;
            for (int i = 0; i < 6; i++)
            {
                data.Pokemons[i] = Pokemons[i].Clone();
            }
            return data;
        }

        public bool Equals(TeamData data)
        {
            if (data == null) return false;
            for (int i = 0; i < 6; i++)
            {
                if (!Pokemons[i].Equals(data.Pokemons[i])) return false;
            }
            if (CustomInfo.DataName != data.CustomInfo.DataName) return false;
            if (CustomInfo.DataHash != data.CustomInfo.DataHash) return false;
            return true;
        }

        public void Save(Stream output)
        {
            BinaryWriter writer = new BinaryWriter(output);
            writer.Write(CustomInfo.DataName);
            writer.Write(CustomInfo.DataHash);
            foreach (PokemonCustomInfo pm in Pokemons)
            {
                pm.Save(output);
            }
        }

        public static TeamData FormStream(Stream input)
        {
            TeamData data = new TeamData();
            BinaryReader reader = new BinaryReader(input);
            data.CustomInfo.DataName = reader.ReadString();
            data.CustomInfo.DataHash = reader.ReadString();
            for (int i=0;i<data.Pokemons.Length;i++)
            {
                data.Pokemons[i] = PokemonCustomInfo.FormStream(input);
            }
            return data;
        }

        public void CheckData()
        {
            foreach (PokemonCustomInfo pm in Pokemons)
            {
                if (pm.Identity == 0) continue;
                if (!BattleData.CheckPokemon(pm))
                {
                    pm.SelectedTrait = 1;
                    pm.SelectedMoves[0] = BattleData.GetPokemon(pm.Identity).Moves[0].MoveId;
                    pm.SelectedMoves[1] = MoveData.InvalidId;
                    pm.SelectedMoves[2] = MoveData.InvalidId;
                    pm.SelectedMoves[3] = MoveData.InvalidId;
                }
                pm.CheckData();
            }
        }
    }
}
