using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Data;
using LightStudio.PokemonBattle.Interactive;

namespace LightStudio.PokemonBattle.Game
{
  public class PokemonProxy
  { 
    internal readonly Pokemon Pokemon;
    internal readonly OnboardPokemon OnboardPokemon;
    protected readonly Controller Controller;
    private PokemonOutward Outward; //幻影

    internal PokemonProxy(Controller controller, Pokemon pokemon, Tile tile)
    {
      Controller = controller;
      Pokemon = pokemon;
      OnboardPokemon = new OnboardPokemon(pokemon, tile.X);

      Moves = new MoveProxy[4];
      for (int i = 0; i < 4; i++)
        if (pokemon.Moves[i] != null) Moves[i] = new MoveProxy(Controller, pokemon.Moves[i], this);
      StruggleMove = new MoveProxy(controller, new Move(165, Controller.Game.Settings), this);
      Action = PokemonAction.Done;

      BuildOutward();
    }

    public int Id
    { get { return Pokemon.Id; } }
    public PokemonAction Action
    { get; private set; }
    public MoveProxy SelectedMove //先取
    { get; private set; }
    internal Tile Tile
    { get { return Controller.GetTile(Position.Team, Position.X); } }

    public int Hp
    { get { return Pokemon.Hp.Value; } }
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
    { 
      get { return OnboardPokemon.Ability; }
      set { }
    }
    public Item Item
    { get { return Pokemon.Item; } }
    public Position Position
    { get { return OnboardPokemon.Position; } }
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
      return Ability.Id == abilityId && true;
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
    internal bool InputSwitch(int sendoutIndex)
    {
      #warning 踩影子 我受不了啦用事件吧
      if (CanWithdraw && Controller.CanSendout(Pokemon.Owner.GetPokemon(sendoutIndex)) &&
        true)
      {
        Action = PokemonAction.WillSwitch;
        Tile.WillSendoutPokemonIndex = sendoutIndex;
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
    internal bool UndoInput()
    {
      if (Action == PokemonAction.WillMove || Action == PokemonAction.WillSwitch)
      {
        Action = PokemonAction.WaitingForInput;
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

    public void BuildOutward()
    {
      //幻影new完后覆盖属性
      Outward = new PokemonOutward(Pokemon, OnboardPokemon.Position);
      Outward.Name = Pokemon.Name;
      Outward.Gender = Pokemon.Gender;
      Outward.ImageId = Pokemon.PokemonType.Id;
    }
    public PokemonOutward GetOutward()
    {
      return Outward;
    }
  }
}
