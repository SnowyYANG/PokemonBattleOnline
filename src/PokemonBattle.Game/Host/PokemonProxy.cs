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
    private static PokemonOutward BuildOutward(PokemonProxy p)
    {
      //幻影new完后覆盖属性
      Pokemon opm = p.Pokemon;
      PokemonOutward o = new PokemonOutward(opm, p.OnboardPokemon.Position);
      if (p.OnboardPokemon.Ability == AbilityIds.ILLUSION)
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

    public readonly Pokemon Pokemon;
    public readonly OnboardPokemon OnboardPokemon;
    public readonly PokemonOutward Outward; //幻影
    public readonly Controller Controller;

    internal PokemonProxy(Controller controller, Pokemon pokemon, Tile tile)
    {
      Controller = controller;
      Pokemon = pokemon;
      OnboardPokemon = new OnboardPokemon(pokemon, tile.X);

      Moves = new MoveProxy[4];
      for (int i = 0; i < 4; i++)
        if (pokemon.Moves[i] != null) Moves[i] = new MoveProxy(Controller, pokemon.Moves[i], this);
      StruggleMove = new MoveProxy(controller, new Move(165, Controller.Game.GameSettings), this);
      Action = PokemonAction.Debuting;

      Outward = BuildOutward(this);
    }

    #region Data
    public int Id
    { get { return Pokemon.Id; } }
    internal Tile Tile
    { get { return Controller.GetTile(Position.Team, Position.X); } }

    public int Hp
    { get { return Pokemon.Hp.Value; } }
    public PokemonState State
    {
      get { return Pokemon.State; }
      set
      {
        if (State != value)
        {
          State = value;
          if (State != PokemonState.Faint)
            Controller.ReportBuilder.AddStateChanged(this);
        }
      }
    }
    public IAbilityE Ability
    { 
      get
      { 
        //需要检查胃液状态么？
        return GameService.GetAbility(OnboardPokemon.Ability);
      }
    }
    public IItemE Item
    { get { return GameService.GetItem(Pokemon.Item); } }
    public Position Position
    { get { return OnboardPokemon.Position; } }
    public MoveProxy[] Moves
    { get; private set; }
    public MoveProxy StruggleMove
    { get; private set; }
    #endregion

    #region 7D
    private int Get5D(StatType stat)
    {
      int v = OnboardPokemon.Get5D(stat);
      v = (int)(v * Ability.Get5DRevise(this, stat));
      v *= (int)(v * Item.Get5DRevise(this, stat));
      return v;
    }
    public int Atk
    { get { return Get5D(StatType.Atk); } }
    public int Def
    { get { return Get5D(StatType.Def); } }
    public int SpAtk
    { get { return Get5D(StatType.SpAtk); } }
    public int SpDef
    { get { return Get5D(StatType.SpDef); } }
    public int Speed
    { get { return Get5D(StatType.Speed); } }
    #endregion

    #region Predict
    public bool CanWithdraw
    { get { return Controller.CanWithdraw(this); } }
    /// <summary>
    /// 和Struggle一起的
    /// </summary>
    public bool CanSelectMove
    { get { return Hp > 0; } }
    private int lastActTurn = 0;
    public bool CanActMove
    {
      get
      {
        return Hp > 0 && lastActTurn != Controller.ReportBuilder.TurnNumber &&
          (Action == PokemonAction.MoveAttached || Action == PokemonAction.Stiff || Action == PokemonAction.Moving);
      }
    }
    internal bool CanExecute() //梦话...
    {
      //冰冻
      if (State == PokemonState.Frozen)
      {
        if (SelectedMove.Move.Type.AdvancedFlags.AvailableEvenFrozen || Controller.GetRandomInt(0, 3) == 0)
          State = PokemonState.Normal;
        else Controller.ReportBuilder.Add("Frozen", Pokemon.Name);
      }
      //睡觉..梦话
      if (State == PokemonState.Sleeping)
      {
        int count = OnboardPokemon.GetCondition<int>("Sleeping");
        count--;
        if (Ability.Id == AbilityIds.EARLY_BIRD) count--;
        OnboardPokemon.SetCondition("Sleeping", count);
        if (count == 0) State = PokemonState.Normal;
        else Controller.ReportBuilder.Add("Sleeping", Pokemon.Name);
      }
      //懒惰 
      //残废 
      //封印 
      //回复封印 
      //混乱 
      //害怕 
      //  不屈之心
      //挑拨 
      //重力 
      //着迷 
      //麻痹
      return true;
    }
    #endregion

    #region Input
    public PokemonAction Action
    { get; private set; }
    public MoveProxy SelectedMove //先取
    { get; private set; }
    public Tile SelectedTarget
    { get; private set; }
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
      SelectedTarget = target;
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

    #region internal
    internal void Debut()
    {
      if (Action == PokemonAction.Debuting)
      {
        ;//场地效果
        Ability.Debut(this);//特性
        Item.Debut(this);//道具
        Action = PokemonAction.Done;
      }
    }
    internal void AttachBehaviors()
    {
      if (Action == PokemonAction.WillMove)
        Action = PokemonAction.MoveAttached;
    }

    internal void Switch()
    {
      if (Action == PokemonAction.WillSwitch)
      {
        Action = PokemonAction.Switching;
        if (Controller.Withdraw(this)) //追击，无论死没死都已经收回了
        {
          if (this.Hp == 0) Controller.PauseForSendoutInput(Controller.Switch, Tile);
          else Controller.Sendout(Tile);
        }
        Action = PokemonAction.Done;
      }
    }
    /// <summary>
    /// 就为了气合拳...
    /// </summary>
    /// <returns></returns>
    internal void PreMove()
    {
      SelectedMove.PreMove();
    }
    internal void ActMove()
    {
      if (!CanActMove) return;
      lastActTurn = Controller.ReportBuilder.TurnNumber;
      switch(Action)
      {
        case PokemonAction.MoveAttached:
          Action = PokemonAction.Moving;
          SelectedMove.Act();
          break;
        case PokemonAction.Moving:
          System.Diagnostics.Debugger.Break();
          //多回合技能，还没想好就这样写大丈夫？
          break;
        case PokemonAction.Stiff:
          Action = PokemonAction.Done;
          break;
      }
    }
    #endregion
  }
}
