using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using PokemonBattle.PokemonData.Custom;

namespace PokemonBattle.PokemonData
{
    public class PokemonCustomInfo
    {
        public string Nickname
        { get; set; }

        public int Identity
        { get; set; }

        public byte LV
        { get; set; }
        public byte HPEV
        { get; set; }
        public byte AttackEV
        { get; set; }
        public byte DefenceEV
        { get; set; }
        public byte SpeedEV
        { get; set; }
        public byte SpAttackEV
        { get; set; }
        public byte SpDefenceEV
        { get; set; }
        public byte HPIV
        { get; set; }
        public byte AttackIV
        { get; set; }
        public byte DefenceIV
        { get; set; }
        public byte SpeedIV
        { get; set; }
        public byte SpAttackIV
        { get; set; }
        public byte SpDefenceIV
        { get; set; }
        public PokemonGender Gender
        { get; set; }
        public PokemonCharacter Character
        { get; set; }
        public byte SelectedTrait
        { get; set; }
        public Item Item
        { get; set; }

        public int[] SelectedMoves
        { get; set; }

        public PokemonCustomInfo()
        {
            SelectedMoves = new int[4] { MoveData.InvalidId, MoveData.InvalidId, MoveData.InvalidId, MoveData.InvalidId };
        }

        public PokemonCustomInfo Clone()
        {
            PokemonCustomInfo info = this.MemberwiseClone() as PokemonCustomInfo;
            info.SelectedMoves = new int[4];
            for (int i = 0; i < 4; i++)
            {
                info.SelectedMoves[i] = SelectedMoves[i];
            }
            return info;
        }
        public bool Equals(PokemonCustomInfo info)
        {
            if (info.Identity != Identity) return false;
            if (info.Nickname != Nickname) return false;
            if (info.LV != LV) return false;

            if (info.HPEV != HPEV) return false;
            if (info.AttackEV != AttackEV) return false;
            if (info.DefenceEV != DefenceEV) return false;
            if (info.SpeedEV != SpeedEV) return false;
            if (info.SpAttackEV != SpAttackEV) return false;
            if (info.SpDefenceEV != SpDefenceEV) return false;

            if (info.HPIV != HPIV) return false;
            if (info.AttackIV != AttackIV) return false;
            if (info.DefenceIV != DefenceIV) return false;
            if (info.SpeedIV != SpeedIV) return false;
            if (info.SpAttackIV != SpAttackIV) return false;
            if (info.SpDefenceIV != SpDefenceIV) return false;

            if (info.Gender != Gender) return false;
            if (info.Character != Character) return false;
            if (info.SelectedTrait != SelectedTrait) return false;
            if (info.Item != Item) return false;

            for (int i = 0; i < 4; i++)
            {
                if (SelectedMoves[i] != info.SelectedMoves[i]) return false;
            }
            return true;
        }

        public void Save(Stream output)
        {
            BinaryWriter writer = new BinaryWriter(output);
                writer.Write(Identity);
                if (Identity != 0)
                {
                    writer.Write(Nickname);
                    writer.Write(LV);

                    writer.Write(AttackEV);
                    writer.Write(DefenceEV);
                    writer.Write(SpeedEV);
                    writer.Write(SpAttackEV);
                    writer.Write(SpDefenceEV);
                    writer.Write(HPEV);
                    writer.Write(AttackIV);
                    writer.Write(DefenceIV);
                    writer.Write(SpeedIV);
                    writer.Write(SpAttackIV);
                    writer.Write(SpDefenceIV);
                    writer.Write(HPIV);
                    writer.Write((int)Gender);
                    writer.Write(SelectedTrait);

                    writer.Write((int)Character);
                    writer.Write((int)Item);

                    foreach (var move in SelectedMoves)
                    {
                        writer.Write(move);
                    }
                }
        }

        public static PokemonCustomInfo FormStream(Stream input)
        {
            PokemonCustomInfo pm = new PokemonCustomInfo();
            BinaryReader reader = new BinaryReader(input);
            pm.Identity = reader.ReadInt32();
            if (pm.Identity != 0)
            {
                pm.Nickname = reader.ReadString();
                pm.LV = reader.ReadByte();

                pm.AttackEV = reader.ReadByte();
                pm.DefenceEV = reader.ReadByte();
                pm.SpeedEV = reader.ReadByte();
                pm.SpAttackEV = reader.ReadByte();
                pm.SpDefenceEV = reader.ReadByte();
                pm.HPEV = reader.ReadByte();
                pm.AttackIV = reader.ReadByte();
                pm.DefenceIV = reader.ReadByte();
                pm.SpeedIV = reader.ReadByte();
                pm.SpAttackIV = reader.ReadByte();
                pm.SpDefenceIV = reader.ReadByte();
                pm.HPIV = reader.ReadByte();
                pm.Gender = (PokemonGender)reader.ReadInt32();
                pm.SelectedTrait = reader.ReadByte();

                pm.Character = (PokemonCharacter)reader.ReadInt32();
                pm.Item = (Item)reader.ReadInt32();

                for (int i = 0; i < 4; i++)
                {
                    pm.SelectedMoves[i] = reader.ReadInt32();
                }
            }
            return pm;
        }
        public void CheckData()
        {
            if (AttackIV > 31) AttackIV = 0;
            if (DefenceIV > 31) AttackIV = 0;
            if (SpeedIV > 31) AttackIV = 0;
            if (SpAttackIV > 31) AttackIV = 0;
            if (SpDefenceIV > 31) AttackIV = 0;
            if (HPIV > 31) AttackIV = 0;
            if (AttackEV + DefenceEV + SpeedEV + SpAttackEV + SpDefenceEV + HPEV > 510)
            {
                AttackEV = 0;
                DefenceEV = 0;
                SpeedEV = 0;
                SpAttackEV = 0;
                SpDefenceEV = 0;
                HPEV = 0;
            }
        }
    }
}
