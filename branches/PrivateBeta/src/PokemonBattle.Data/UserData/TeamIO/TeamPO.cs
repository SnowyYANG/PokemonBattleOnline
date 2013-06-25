using System;
using System.Linq;
using System.Text;
using System.Xml;

namespace LightStudio.PokemonBattle.Data
{
    /// <summary>
    /// http://sourceforge.net/p/pogeymon-online/code/ci/master/tree/src/PokemonInfo/pokemonstructs.cpp
    /// https://github.com/po-devs/pokemon-online/blob/master/src/PokemonInfo/pokemonstructs.cpp
    /// </summary>
    public class TeamPO : ITeamIO
    {

        #region  po (*.tp)

        private static PokemonBT LoadXmlTeam(XmlDocument doc)
        {
            var data = new PokemonBT();
            var team = doc.DocumentElement;
            //string gen = team.GetAttribute("gen");
            //string subgen = team.GetAttribute("subgen");
            //string defaultTier = team.GetAttribute("defaultTier");
            //string version = team.GetAttribute("version");

            foreach (XmlNode pm in team.ChildNodes)
            {
                try
                {
                    var pd = LoadXmlData(pm);
                    if (pd != null) data.Add(pd);
                }
                catch { }
            }
            return data;
        }

        private static PokemonData LoadXmlData(XmlNode pm)
        {
            int num = pm.Attributes.GetNamedItem("Num").Value.ToInt();
            int forme = pm.Attributes.GetNamedItem("Forme").Value.ToInt();
            if (num == 0) return null;
            var pd = new PokemonData(num, forme);

            int itemId = Item2PBO(pm.Attributes.GetNamedItem("Item").Value.ToInt());
            int abilityId = pm.Attributes.GetNamedItem("Ability").Value.ToInt();

            pd.Name = pm.Attributes.GetNamedItem("Nickname").Value;
            pd.Lv = pm.Attributes.GetNamedItem("Lvl").Value.ToInt(100);
            pd.Happiness = pm.Attributes.GetNamedItem("Happiness").Value.ToInt();
            pd.Nature = (PokemonNature)pm.Attributes.GetNamedItem("Nature").Value.ToInt();
            pd.Gender = (PokemonGender)pm.Attributes.GetNamedItem("Gender").Value.ToInt();

            if (itemId > 0) pd.Item = GameDataService.Rom.GetItem(itemId);
            if (abilityId > 0) pd.Ability = GameDataService.Rom.GetAbility(abilityId);

            var moves = (from XmlNode node in pm.ChildNodes where node.Name == "Move" select node.InnerText.ToInt()).ToArray();
            var dvs = (from XmlNode node in pm.ChildNodes where node.Name == "DV" select node.InnerText.ToInt()).ToArray();
            var evs = (from XmlNode node in pm.ChildNodes where node.Name == "EV" select node.InnerText.ToInt()).ToArray();

            for (int i = 0; i < 4 && i < moves.Length; i++)
            {
                if (moves[i] > 0)
                {
                    var move = GameDataService.Rom.GetMoveType(moves[i]);
                    pd.AddMove(move);
                }
            }

            for (int i = 0; i < 6 && i < dvs.Length; i++)
            {
                SetByIndex(pd.Iv, i, dvs[i]);
            }

            for (int i = 0; i < 6 && i < evs.Length; i++)
            {
                SetByIndex(pd.Ev, i, evs[i]);
            }

            return pd;
        }

        private static XmlDocument TeamToXml(PokemonBT bt)
        {
            var doc = new XmlDocument();
            var team = doc.CreateElement("Team");
            foreach (var pm in bt)
            {
                var pe = DataToXml(pm, doc);
                team.AppendChild(pe);
            }
            doc.AppendChild(team);
            return doc;
        }

        private static XmlElement DataToXml(PokemonData pm, XmlDocument doc)
        {
            var pe = doc.CreateElement("Pokemon");
            pe.SetAttribute("Gender", pm.Gender.ToString("d"));
            pe.SetAttribute("Item", pm.Item != null ? Item2PO(pm.Item.Id).ToString() : "0");
            pe.SetAttribute("Nickname", pm.Name);
            pe.SetAttribute("SubGen", "1");
            pe.SetAttribute("Lvl", pm.Lv.ToString());
            pe.SetAttribute("Nature", pm.Nature.ToString("d"));
            pe.SetAttribute("Num", pm.Form.Type.Number.ToString());
            pe.SetAttribute("Forme", pm.Form.Index.ToString());
            pe.SetAttribute("Happiness", pm.Happiness.ToString());
            pe.SetAttribute("Gen", "5");
            pe.SetAttribute("Ability", pm.Ability.Id.ToString());
            pe.SetAttribute("Shiny", "0");

            foreach (var move in pm.Moves.Select((lm) => lm.Move.Id.ToString()))
            {
                var key = doc.CreateElement("Move");
                key.InnerText = move;
                pe.AppendChild(key);
            }

            foreach (var iv in pm.Iv)
            {
                var key = doc.CreateElement("DV");
                key.InnerText = iv.ToString();
                pe.AppendChild(key);
            }

            foreach (var ev in pm.Ev)
            {
                var key = doc.CreateElement("EV");
                key.InnerText = ev.ToString();
                pe.AppendChild(key);
            }

            return pe;
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

        #region Text

        private static PokemonBT ImportFromTxt(string str)
        {
            PokemonBT bt = null;
            str = str.Replace("---", "");
            str = str.Replace("\r\n", "\n"); // for windows
            var pokes = str.Split(new string[] { "\n\n" }, 6, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < bt.Size; i++)
            {
                var pokeDetail = pokes[i].Split(new char[] { '\n' }, 5, StringSplitOptions.RemoveEmptyEntries);
                if (pokeDetail.Length < 5) continue;
                var first = pokeDetail[0].Split('@');
                bt.RemoveAt(i);
                //coding
            }

            bt.Name = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            return bt;
        }

        private static PokemonData ImportPO(string str)
        {
            var pokeDetail = str.Split(new char[] { '\n' }, 5, StringSplitOptions.RemoveEmptyEntries);
            if (pokeDetail.Length < 5) return null;
            var first = pokeDetail[0].Split('@');
            //coding
            return null;
        }

        private static string ToPOTxt(PokemonBT bt)
        {
            var ret = new StringBuilder();

            foreach (var pm in bt)
            {
                if (pm == null) continue;

                ret.Append(pm.Name);

                if (pm.Name != pm.Form.Type.GetLocalizedName())
                {
                    ret.AppendFormat(" ({0})", pm.Form.Type.GetLocalizedName());
                }

                if (pm.Gender != PokemonGender.None)
                {
                    ret.AppendFormat(" ({0})", pm.Gender == PokemonGender.Male ? "M" : "F");
                }

                if (pm.Item != null)
                {
                    ret.Append(" @ " + pm.Item.Name);
                }
                ret.AppendLine();//使用crlf

                ret.AppendLine("Trait: " + pm.Ability.Name);
                ret.Append("EVs: " + pm.Ability.Name);
                string[] stats = { "HP", "Atk", "Def", "SAtk", "SDef", "Spd" };
                int i = 0; bool started = false;
                foreach (int ev in pm.Ev)
                {
                    if (ev != 0)
                    {
                        if (started) ret.Append(" / ");
                        started = true;
                        ret.Append(ev + " " + stats[i]);
                    }
                    i++;
                }
                ret.AppendLine();

                //Nature + -
                ret.Append(pm.Nature.ToString() + " Nature");
                int up = (int)pm.Nature.StatBoosted();
                if (up != 0)
                {
                    int down = (int)pm.Nature.StatHindered();
                    ret.AppendFormat(" (+{0}, -{1})", stats[up], stats[down]);
                }
                ret.AppendLine();

                foreach (var move in pm.Moves.Select((m) => m.Move))
                {
                    ret.Append("- " + move.Name);
                    //Hidden Power
                    if (move.Id == 237)
                    {
                        const string HIDE_TYPE = "斗飞毒地岩虫鬼钢火水草电超冰龙恶";
                        ret.AppendFormat(" [{0}]", HIDE_TYPE.Substring(pm.Iv.GetHiddenType(), 1));
                    }
                    ret.AppendLine();
                }
                ret.AppendLine();
            }
            return ret.ToString();
        }

        #endregion

        #region instance

        private TeamPO() { }

        private static TeamPO instance = new TeamPO();

        public static TeamPO GetInstance()
        {
            return instance;
        }

        #endregion

        #region IPokemonDataIO

        public PokemonBT Read(string path)
        {
            var doc = new XmlDocument();
            doc.Load(path);
            return LoadXmlTeam(doc);
        }

        public void Write(PokemonBT bt, string path)
        {
            var doc = TeamToXml(bt);
            doc.Save(path);
        }

        public string ExportString(PokemonBT bt)
        {
            return ToPOTxt(bt);
        }

        public PokemonBT ImportString(string body)
        {
            return Helper.Import(body, 6);
        }

        #endregion

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
    }
}
