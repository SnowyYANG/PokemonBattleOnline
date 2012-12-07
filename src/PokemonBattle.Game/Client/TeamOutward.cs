using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel;
using LightStudio.PokemonBattle.Game.Host;

namespace LightStudio.PokemonBattle.Game
{
  public enum BallState : byte
  {
    None,
    Normal,
    Abnormal,
    Faint
  }
  [DataContract(Namespace = Namespaces.PBO)]
  public class TeamOutward : ObservableObject
  {
    [DataMember]
    private readonly byte[] balls;

    internal TeamOutward(IEnumerable<Player> players, int pokemonsPerPlayer)
    {
      balls = new byte[pokemonsPerPlayer * players.Count()];
      int baseI = 0;
      foreach (var p in players)
      {
        int i = baseI;
        foreach (var pm in p.Pokemons) this[i++] = pm.Hp.Value == 0 ? BallState.Faint : pm.State == PokemonState.Normal ? BallState.Normal : BallState.Abnormal;
        baseI += pokemonsPerPlayer;
      }
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
