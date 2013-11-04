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
    public string Name;
    private BallState[] balls;

    public TeamOutward(string name, BallState[] state)
    {
      Name = name;
      balls = state;
    }

    public BallState this[int index]
    {
      get { return balls.ValueOrDefault(index); }
      private set { if (0 <= index && index < balls.Length) balls[index] = value; }
    }
    public int AliveCount
    { get { return balls.Count((b) => b == BallState.Normal || b == BallState.Abnormal); } }

    internal void StateChanged(PokemonOutward pm)
    {
      this[pm.TeamIndex] = pm.Hp.Value == 0 ? BallState.Faint : pm.State == PokemonState.Normal ? BallState.Normal : BallState.Abnormal;
      OnPropertyChanged();
    }
    internal void SwitchPokemon(int a, int b)
    {
      var t = this[a];
      this[a] = this[b];
      this[b] = t;
      OnPropertyChanged();
    }
  }
}
