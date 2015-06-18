using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace PokemonBattleOnline.Game
{
    public enum BallState : byte
    {
        None,
        Normal,
        Abnormal,
        Faint
    }
    public class TeamOutward : ObservableObject
    {
        private static readonly BallState[] DEFAULT = new BallState[6];

        public string Name;
        private BallState[] balls;

        public TeamOutward(string name)
        {
            Name = name;
            balls = DEFAULT;
        }

        public BallState this[int index]
        {
            get { return balls.ValueOrDefault(index); }
            private set { if (0 <= index && index < balls.Length) balls[index] = value; }
        }
        public int AliveCount
        { get { return balls.Count((b) => b == BallState.Normal || b == BallState.Abnormal); } }

        public void SetAll(BallState[] state)
        {
            balls = state;
            OnPropertyChanged();
        }
        internal void StateChanged(PokemonOutward pm)
        {
            this[pm.TeamIndex] = pm.Hp.Value == 0 ? BallState.Faint : pm.State == PokemonState.Normal ? BallState.Normal : BallState.Abnormal;
            OnPropertyChanged();
        }
        internal void SwitchPokemon(int index, int a, int b)
        {
            //00 01 02 03 04 05
            //00 01 02 10 11 12
            if (index == 1)
            {
                a += 3;
                b += 3;
            }
            var t = this[a];
            this[a] = this[b];
            this[b] = t;
            OnPropertyChanged();
        }
    }
}
