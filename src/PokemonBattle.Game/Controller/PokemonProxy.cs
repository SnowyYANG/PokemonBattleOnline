using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Data;

namespace LightStudio.PokemonBattle.Game
{
  internal class PokemonProxy : Tactic.DataModels.GameElementProxy<IController, OnboardPokemon>, IPokemonProxy
  { 
    internal readonly Pokemon Pokemon;
    internal readonly OnboardPokemon OnboardPokemon;
    internal PokemonAction Action;
    internal MoveProxy SelectMove;
    internal Position SelectTarget;
    internal Pokemon SwitchPokemon;

    internal PokemonProxy(IController controller, Pokemon pokemon, Position position)
      : base(controller, new OnboardPokemon(pokemon, position.X))
    {
      Moves = new IMoveProxy[4];
      for (int i = 0; i < 4; i++)
        if (pokemon.Moves[i] != null) Moves[i] = new MoveProxy(controller, pokemon.Moves[i], this);
      Pokemon = (controller as Controller).Game.GetPokemon(Model.Id);
      OnboardPokemon = Model;
    }

    public int Id
    { get { return Model.Id; } }

    public int Hp
    { get { return Model.Hp; } }
    #region 7D
    public int Atk
    { get { throw new NotImplementedException(); } }
    public int Def
    { get { throw new NotImplementedException(); } }
    public int SpAtk
    { get { throw new NotImplementedException(); } }
    public int SpDef
    { get { throw new NotImplementedException(); } }
    public int Speed
    { get { throw new NotImplementedException(); } }
    #endregion
    public Ability Ability
    { get { return Model.Ability; } }
    public Item Item
    { get { return Model.Item; } }
    public Position Position
    { get { return Model.Position; } }

    public IMoveProxy[] Moves
    { get; private set; }
    public bool HasWorkingAbility(int abilityId)
    {
      return Model.Ability.Id == abilityId && true;
    }
  }
}
