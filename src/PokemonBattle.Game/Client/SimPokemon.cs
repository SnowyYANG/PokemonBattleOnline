using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Data;

namespace LightStudio.PokemonBattle.Game
{
  public class SimPokemon
  {
    private readonly Pokemon pokemon;
    public int X;

    public int Id
    { get { return pokemon.Id; } }

    public string Name
    { get { return pokemon.Name; } }
    public int Lv
    { get { return pokemon.Lv; } }
    public int Hp
    { get { return pokemon.Hp.Value; } }
    /// <summary>
    /// 虽然是private set，但每个技能还是能set的
    /// </summary>
    public SimMove[] Moves { get; private set; }
    public bool CanSelectMove { get; internal set; }
    public bool CanStruggle { get; internal set; }
    public bool CanSwitch { get; internal set; }

    internal SimPokemon(Pokemon pokemon, PokemonOutward outward)
    {
      this.pokemon = pokemon;
      X = outward.Position.X;
      Moves = new SimMove[4];
      for (int i = 0; i < 4; i++)
        if (pokemon.Moves[i] != null) Moves[i] = new SimMove(pokemon.Moves[i]);
      foreach (SimMove m in Moves)
        if (m != null && m.CanBeSelected) CanSelectMove = true;
      CanStruggle = !CanSelectMove;
      CanSwitch = true;
    }
  }
}
