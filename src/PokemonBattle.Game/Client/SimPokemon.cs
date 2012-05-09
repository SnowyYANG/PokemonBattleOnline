using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Game;
using LightStudio.PokemonBattle.Data;

namespace LightStudio.PokemonBattle.Interactive
{
  public class SimPokemon
  {
    private readonly Pokemon pokemon;
    private readonly PokemonOutward outward;
    public int X;

    public int Id
    { get { return pokemon.Id; } }

    #region Data
    public string Name
    { get { return pokemon.Name; } }
    public int Lv
    { get { return pokemon.Lv; } }
    public int MaxHp
    { get { return pokemon.Hp.Origin; } }
    public int Hp
    {
      get { return pokemon.Hp.Value; }
      set { pokemon.SetHp(value); }
    }
    public PokemonNature Nature
    { get { return pokemon.Nature; } }
    public PokemonState State
    {
      get { return pokemon.State; }
      set { pokemon.State = value; }
    }
    public BattleType Type1
    { get { return pokemon.PokemonType.Type1; } }
    public BattleType Type2
    { get { return pokemon.PokemonType.Type2; } }
    public PokemonGender Gender
    { get { return pokemon.Gender; } }
    public Ability Ability
    { get { return pokemon.Ability; } }
    #endregion

    /// <summary>
    /// 虽然是private set，但每个技能还是能set的
    /// </summary>
    public SimMove[] Moves { get; private set; }
    public bool CanSelectMove { get; internal set; }
    public bool CanStruggle { get; internal set; }
    public bool CanSwitch { get; internal set; }

    public int StruggleId
    { get { return pokemon.StruggleId; } }
    public int SwitchId
    { get { return pokemon.SwitchId; } }

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
