﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Data;
using LightStudio.PokemonBattle.Interactive;

namespace LightStudio.PokemonBattle.Game
{
  public class PokemonProxy
  {
    private static PokemonOutward BuildOutward(PokemonProxy p)
    {
      //幻影new完后覆盖属性
      Pokemon opm = p.Pokemon;
      PokemonOutward o = new PokemonOutward(opm, p.OnboardPokemon.Position);
      if (p.HasWorkingAbility(AbilityIds.ILLUSION))
      {
        foreach (Pokemon pm in p.Pokemon.Owner.Pokemons)
          if (pm.Hp.Value > 0) opm = pm;
        if (opm != p.Pokemon) ;//追加状态，幻影解除时使用
      }
      else
      {
        o.Name = opm.Name;
        o.Gender = opm.Gender;
        o.ImageId = opm.PokemonType.Id;
      }
      return o;
    }

    internal readonly Pokemon Pokemon;
    internal readonly OnboardPokemon OnboardPokemon;
    protected readonly Controller Controller;
    private readonly PokemonOutward Outward; //幻影

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

      Outward = BuildOutward(this);
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
    #region 5D
    public int Atk
    { get { throw new NotImplementedException(); } }
    public int Def
    { get { throw new NotImplementedException(); } }
    public int SpAtk
    { get { throw new NotImplementedException(); } }
    public int SpDef
    { get { throw new NotImplementedException(); } }
    public int Speed
    { get { return (int)OnboardPokemon.Get5D(StatType.Speed); } }
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
    /// <summary>
    /// 和Struggle一起的
    /// </summary>
    public bool CanSelectMove
    { get { return Hp > 0; } }
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
    internal string InputSwitch(int sendoutIndex)
    {
      if (!CanWithdraw) return "不能把精灵收回来";
#warning 踩影子 我受不了啦用事件吧
      if (!Controller.CanSendout(Pokemon.Owner.GetPokemon(sendoutIndex))) return "无法送出";
      Action = PokemonAction.WillSwitch;
      Tile.WillSendoutPokemonIndex = sendoutIndex;
      return null;
    }
    internal string SelectMove(MoveProxy move, Tile target)
    {
      if (!CanSelectMove) return "";
      if (!move.CanBeSelected) return "";
      Action = PokemonAction.WillMove;
      SelectedMove = move;
      SelectedMove.SelectedTarget = target;
      return null;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns>是否可输入</returns>
    internal bool UndoInput()
    {
      if (Action == PokemonAction.WillMove || Action == PokemonAction.WillSwitch)
        Action = PokemonAction.WaitingForInput;
      return Action == PokemonAction.WaitingForInput;
    }
    #endregion

    public void Debut()
    {
      ;//场地效果
      ;//特性
      ;//道具
    }
    internal void Prepare()
    {
      if (Action == PokemonAction.WillMove)
      {
        System.Diagnostics.Debugger.Break();
        Action = PokemonAction.MovePrepared;
      }
      else if (Action == PokemonAction.WillSwitch)
      {
        Action = PokemonAction.SwitchPrepared;
      }
    }
    private int lastActTurn = 0;
    public bool Act()
    {
      if (Hp == 0 || lastActTurn == Controller.ReportBuilder.TurnNumber) return false;
      lastActTurn = Controller.ReportBuilder.TurnNumber;
      switch(Action)
      {
        case PokemonAction.SwitchPrepared:
          Action = PokemonAction.Switching;
          if (Controller.Withdraw(this))
            if (Controller.Sendout(Tile))
              Tile.Pokemon.Debut();
          Action = PokemonAction.Done;
          break;
        case PokemonAction.MovePrepared:
          System.Diagnostics.Debugger.Break();
          break;
        default:
          return false;
      }
      return true;
    }

    public PokemonOutward GetOutward()
    {
      return Outward;
    }
  }
}
