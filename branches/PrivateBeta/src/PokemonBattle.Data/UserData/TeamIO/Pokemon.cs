using System;
using System.Linq;
using System.Xml.Serialization;

namespace LightStudio.PokemonBattle.Data.PokemonOnline
{
    /// <summary>
    /// PO Pokemon对象
    /// </summary>
    [Serializable]
    public class Pokemon
    {
        /// <summary>
        /// 昵称
        /// </summary>
        [XmlAttribute("Nickname")]
        public string Nickname { get; set; }

        /// <summary>
        /// 全国编号
        /// </summary>
        [XmlAttribute("Num")]
        public int Num { get; set; }

        /// <summary>
        /// 闪光
        /// </summary>
        [XmlAttribute("Shiny")]
        public int Shiny { get; set; }

        /// <summary>
        /// 特性
        /// </summary>
        [XmlAttribute("Ability")]
        public int Ability { get; set; }

        /// <summary>
        /// 世代
        /// </summary>
        [XmlAttribute("Gen")]
        public int Gen { get; set; }

        /// <summary>
        /// 道具
        /// </summary>
        [XmlAttribute("Item")]
        public int Item { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        [XmlAttribute("Gender")]
        public int Gender { get; set; }

        /// <summary>
        /// 世代
        /// </summary>
        [XmlAttribute("SubGen")]
        public int SubGen { get; set; }

        /// <summary>
        /// 亲密度
        /// </summary>
        [XmlAttribute("Hapiness")]
        public int Hapiness { get; set; }

        /// <summary>
        /// 等级
        /// </summary>
        [XmlAttribute("Lvl")]
        public int Lvl { get; set; }

        /// <summary>
        /// 性格
        /// </summary>
        [XmlAttribute("Nature")]
        public int Nature { get; set; }

        /// <summary>
        /// 形态
        /// </summary>
        [XmlAttribute("Forme")]
        public int Forme { get; set; }

        /// <summary>
        /// 技能
        /// </summary>
        [XmlElement("Move")]
        public int[] Move { get; set; }

        /// <summary>
        /// Iv
        /// </summary>
        [XmlElement("DV")]
        public int[] DV { get; set; }

        /// <summary>
        /// Ev
        /// </summary>
        [XmlElement("EV")]
        public int[] EV { get; set; }

        #region po2pbo/pbo2po

        public PokemonData ToPokemonData()
        {
            if (this.Num <= 0 && this.Num > 649) return null;
            var pm = new PokemonData(this.Num, this.Forme);
            pm.Name = this.Nickname;
            pm.Ability = this.Ability > 0 ? GameDataService.Rom.GetAbility(this.Ability) : null;
            int itemId = Item2PBO(this.Item);
            pm.Item = itemId > 0 ? GameDataService.Rom.GetItem(itemId) : null;
            pm.Gender = (PokemonGender)this.Gender;
            pm.Happiness = this.Hapiness;
            pm.Lv = this.Lvl;
            pm.Nature = (PokemonNature)this.Nature;

            #region array
            for (int i = 0; i < 4 && i < Move.Length; i++)
            {
                if (Move[i] > 0)
                {
                    var move = GameDataService.Rom.GetMoveType(Move[i]);
                    if (move != null) pm.AddMove(move);
                }
            }

            for (int i = 0; i < 6 && i < DV.Length; i++)
            {
                SetByIndex(pm.Iv, i, this.DV[i]);
            }

            for (int i = 0; i < 6 && i < EV.Length; i++)
            {
                SetByIndex(pm.Ev, i, this.EV[i]);
            }
            #endregion

            return pm;
        }

        public static Pokemon FromPokemonData(PokemonData pm)
        {
            var p = new Pokemon();
            p.Num = pm.Form.Type.Number;
            p.Forme = pm.Form.Index;
            p.Ability = pm.Ability.Id;
            p.Item = Item2PO(pm.Item.Id);
            p.Gender = pm.Gender.ToString("d").ToInt();
            p.Hapiness = pm.Happiness;
            p.Lvl = pm.Lv;
            p.Nature = pm.Nature.ToString("d").ToInt();
            p.Move = (from LearnedMove move in pm.Moves select move.Move.Id).ToArray();
            p.DV = new int[6];
            p.EV = new int[6];
            for (int i = 0; i < 6; i++)
            {
                p.DV[i] = GetByIndex(pm.Iv, i);
                p.EV[i] = GetByIndex(pm.Ev, i);
            }
            return p;
        }

        private static void SetByIndex(Observable6D o6d, int index, int value)
        {
            switch (index)
            {
                case 0:
                    o6d.SetStat(StatType.Hp, value);
                    break;
                case 1:
                    o6d.SetStat(StatType.Atk, value);
                    break;
                case 2:
                    o6d.SetStat(StatType.Def, value);
                    break;
                case 3:
                    o6d.SetStat(StatType.SpAtk, value);
                    break;
                case 4:
                    o6d.SetStat(StatType.SpDef, value);
                    break;
                case 5:
                    o6d.SetStat(StatType.Speed, value);
                    break;
            }
        }

        private static int GetByIndex(I6D i6d, int index)
        {
            switch (index)
            {
                case 0:
                    return i6d.Hp;
                case 1:
                    return i6d.Atk;
                case 2:
                    return i6d.Def;
                case 3:
                    return i6d.SpAtk;
                case 4:
                    return i6d.SpDef;
                case 5:
                    return i6d.Speed;
                default:
                    return 0;
            }
        }

        private static readonly int[] PO_ITEMS = { 213, 159, 162, 3, 37, 163, 180, 17, 4, 87, 32, 131, 184, 60, 9, 125, 101, 15, 92, 34, 206, 103, 51, 48, 95, 106, 158, 166, 107, 132, 142, 57, 165, 31, 126, 29, 14, 156, 18, 200, 164, 38, 19, 39, 8, 93, 91, 22, 141, 71, 24, 10, 41, 102, 212, 13, 7, 50, 155, 33, 161, 160, 190, 5, 183, 170, 169, 171, 168, 167, 172, 30, 1, 6, 189, 197, 202, 194, 191, 188, 201, 187, 196, 195, 192, 199, 198, 185, 186, 193, 20, 27, 11, 36, 28, 181, 118, 227, 228, 229, 230, 233, 234, 235, 236, 237, 238, 239, 240, 241, 242, 243, 244, 245, 246, 247, 248, 249, 250, 251, 252, 253, 254, 255, 256, 257, 258, 259, 8000, 8001, 8002, 8003, 8004, 8005, 8006, 8007, 8008, 8009, 8010, 8011, 8012, 8013, 8014, 8015, 8016, 8017, 8018, 8019, 8020, 8021, 8022, 8023, 8024, 8025, 8026, 8027, 8028, 8029, 8030, 8031, 8032, 8033, 8034, 8035, 8036, 8037, 8038, 8039, 8040, 8041, 8042, 8043, 8044, 8045, 8046, 8047, 8048, 8049, 8050, 8051, 8052, 8053, 8054, 8055, 8056, 8057, 8058, 8059, 8060, 8061, 8062, 8063, 214, 45 };

        public static int Item2PO(int itemId)
        {
            if (itemId > 0 && itemId <= PO_ITEMS.Length) return PO_ITEMS[itemId - 1];
            return 0;
        }

        public static int Item2PBO(int itemId)
        {
            for (int i = 0; i < PO_ITEMS.Length; i++)
            {
                if (PO_ITEMS[i] == itemId) return i + 1;
            }
            return 0;
        }

        #endregion
    }
}
