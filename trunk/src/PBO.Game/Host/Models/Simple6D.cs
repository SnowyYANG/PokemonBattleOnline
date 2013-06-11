using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonBattleOnline.Data;

namespace PokemonBattleOnline.Game
{
    public class Simple6D : I6D
    {
        private int[] stats;

        public Simple6D()
        {
            stats = new int[6];
        }

        public Simple6D(int h, int a, int d, int sa, int sd, int s)
        {
            Hp = h;
            Atk = a;
            Def = d;
            SpAtk = sa;
            SpDef = sd;
            Speed = s;
        }

        public Simple6D(I6D values)
        {
            Set6D(values);
        }

        public int Hp
        { get; set; }

        public int Atk
        {
            get { return stats[1]; }
            set { stats[1] = value; }
        }

        public int Def
        {
            get { return stats[2]; }
            set { stats[2] = value; }
        }

        public int SpAtk
        {
            get { return stats[3]; }
            set { stats[3] = value; }
        }

        public int SpDef
        {
            get { return stats[4]; }
            set { stats[4] = value; }
        }

        public int Speed
        {
            get { return stats[5]; }
            set { stats[5] = value; }
        }

        public int GetStat(StatType type)
        {
            return type == StatType.Hp ? Hp : stats[(int)type];
        }

        public void SetStat(StatType type, int value)
        {
            if (type == StatType.Hp) Hp = value;
            else stats[(int)type] = value;
        }

        public void Set6D(I6D values)
        {
            Hp = values.Hp;
            Set5D(values);
        }

        /// <summary>
        /// all but Hp
        /// </summary>
        public void Set5D(I6D values)
        {
            Set5D(values.Atk, values.Def, values.SpAtk, values.SpDef, values.Speed);
        }

        public void Set5D(int a, int d, int sa, int sd, int s)
        {
            Atk = a;
            Def = d;
            SpAtk = sa;
            SpDef = sd;
            Speed = s;
        }

        #region IEnumerator

        public IEnumerator<int> GetEnumerator()
        {
            yield return this.Hp;
            yield return this.Atk;
            yield return this.Def;
            yield return this.SpAtk;
            yield return this.SpDef;
            yield return this.Speed;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            yield return this.Hp;
            yield return this.Atk;
            yield return this.Def;
            yield return this.SpAtk;
            yield return this.SpDef;
            yield return this.Speed;
        }

        #endregion

        #region IEquatable

        public bool Equals(I6D other)
        {
            return
              this.Hp == other.Hp &&
              this.Atk == other.Atk &&
              this.Def == other.Def &&
              this.SpAtk == other.SpAtk &&
              this.SpDef == other.SpDef &&
              this.Speed == other.Speed;
        }

        #endregion

    }
}
