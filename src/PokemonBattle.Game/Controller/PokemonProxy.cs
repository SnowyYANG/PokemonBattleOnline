using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Data;
using LightStudio.PokemonBattle.Interactive;

namespace LightStudio.PokemonBattle.Game
{
  public class PokemonProxy : Tactic.DataModels.GameElementProxy<Controller, OnboardPokemon>
  { 
    internal readonly Pokemon Pokemon;
    internal readonly OnboardPokemon OnboardPokemon;

    internal PokemonProxy(Controller controller, Pokemon pokemon, Tile tile)
      : base(controller, new OnboardPokemon(pokemon, tile.X))
    {
      Moves = new MoveProxy[4];
      for (int i = 0; i < 4; i++)
        if (pokemon.Moves[i] != null) Moves[i] = new MoveProxy(Controller, pokemon.Moves[i], this);
      StruggleMove = new MoveProxy(controller, new Move(165, Controller.Game.Settings), this);
      Pokemon = (controller as Controller).Game.GetPokemon(Model.Id);
      OnboardPokemon = Model;
      Action = PokemonAction.Done;
    }

    public int Id
    { get { return Model.Id; } }
    public PokemonAction Action
    { get; private set; }
    public MoveProxy SelectedMove //先取
    { get; private set; }
    internal Pokemon SwitchPokemon
    { get; private set; }
    internal Tile Tile
    { get { return Controller.GetTile(Position.Team, Position.X); } }

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
    public MoveProxy[] Moves
    { get; private set; }
    public MoveProxy StruggleMove
    { get; private set; }

    #region Predict
    public bool CanWithdraw
    { get { return Controller.CanWithdraw(this); } }
    public bool CanSelectMove
    { get { throw new NotImplementedException(); } }//Hp>0
    #endregion

    public bool HasWorkingAbility(int abilityId)
    {
      return Model.Ability.Id == abilityId && true;
    }

    #region Input
    internal bool CheckNeedInput()
    {
      if (Action == PokemonAction.Done)
      {
        Action = PokemonAction.WaitingForInput;
        return true;
      }
      return false;
    }
    internal bool InputSwitch(Pokemon sendout)
    {
      #warning 踩影子
      if (CanWithdraw && Controller.CanSendout(sendout, Tile) &&
        true)
      {
        Action = PokemonAction.WillSwitch;
        SwitchPokemon = sendout;
        return true;
      }
      return false;
    }
    internal bool SelectMove(MoveProxy move, Tile target)
    {
      if (CanSelectMove && move.CanBeSelected)
      {
        Action = PokemonAction.WillMove;
        SelectedMove = move;
        SelectedMove.SelectedTarget = target;
        return true;
      }
      return false;
    }
    #endregion

    public void Prepare()
    {
    }
    public void Act()
    {
    }
  }
}
