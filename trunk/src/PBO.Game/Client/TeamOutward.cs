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
  [DataContract(Namespace = PBOMarks.PBO)]
  public class TeamOutward : ObservableObject
  {
    [DataMember]
    private readonly byte[] balls;

    public TeamOutward(int players, int pokemonsPerPlayer)
    {
      balls = new byte[players * pokemonsPerPlayer];
    }

    public BallState this[int index]
    {
      get { return (BallState)balls.ValueOrDefault(index); }
      set { if (0 <= index && index < balls.Length) balls[index] = (byte)value; }
    }
    public int AliveCount
    { get { return balls.Count((b) => b == (byte)BallState.Normal || b == (byte)BallState.Abnormal); } }

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
    public void HealBell()
    {
      for (int i = 0; i < balls.Length; ++i)
        if (this[i] == BallState.Abnormal) this[i] = BallState.Normal;
      OnPropertyChanged();
    }
  }
}
