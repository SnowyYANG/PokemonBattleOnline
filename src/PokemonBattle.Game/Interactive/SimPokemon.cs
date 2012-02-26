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
    public readonly Position Position;
    public PokemonOutward Outward
    { get; private set; }

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
      set { pokemon.Hp.Value = value; }
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
    public Move[] Moves { get; private set; }
    public bool IsActive { get; internal set; }
    public bool CanUseMove { get; internal set; }
    public bool CanStruggle { get; internal set; }
    public bool CanSwitch { get; internal set; }

    public int StruggleId
    { get { return pokemon.StruggleId; } }
    public int SwitchId
    { get { return pokemon.SwitchId; } }

    internal SimPokemon(Pokemon pokemon, PokemonOutward outward)
    {
      this.pokemon = pokemon;
      Outward = outward;
      //Owner = pokemon.Owner;
      Position = outward.Position;
      Moves = new Move[4] { pokemon.Moves[0], pokemon.Moves[1], pokemon.Moves[2], pokemon.Moves[3] };
      IsActive = true;
      foreach (Move m in Moves)
        if (m != null && m.PP.Value > 0) CanUseMove = true;
      CanStruggle = !CanUseMove;
      CanSwitch = true;
    }
  }
}
