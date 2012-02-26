using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightStudio.PokemonBattle.Game
{
  public class PokemonProxy
  {
    private readonly IController Controller;
    private readonly OnboardPokemon pokemon;
    internal Action Action;
    internal Move SelectMove;
    internal Pokemon SwitchPokemon;

    public PokemonProxy(IController controller, OnboardPokemon pokemon)
    {
      Controller = controller;
      this.pokemon = pokemon;
    }

    public int Id
    { get { return pokemon.Id; } }
    public Player Owner
    { get { return pokemon.Owner; } }

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

    public bool HasWorkingAbility(int abilityId)
    {
      return pokemon.Ability.Id == abilityId && true;
    }
  }
}
