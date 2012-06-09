using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Data;
using LightStudio.PokemonBattle.Interactive.GameEvents;
using LightStudio.PokemonBattle.Game.Sp;

namespace LightStudio.PokemonBattle.Game
{
  public class PokemonProxy
  {
    public readonly Pokemon Pokemon;
    public readonly OnboardPokemon OnboardPokemon;
    public readonly Controller Controller;

    internal PokemonProxy(Controller controller, Pokemon pokemon, Tile tile)
    {
      Controller = controller;
      Pokemon = pokemon;
      Tile = tile;
      OnboardPokemon = new OnboardPokemon(pokemon, tile.X);

      Moves = new MoveProxy[4];
      for (int i = 0; i < 4; i++)
        if (pokemon.Moves[i] != null) Moves[i] = new MoveProxy(pokemon.Moves[i], this);
      StruggleMove = new MoveProxy(new Move(pokemon.StruggleId, Sp.Moves.STRUGGLE, Controller.Game.Settings), this);
      Action = PokemonAction.Debuting;

      Abilities.CheckIllusion(this);
    }

    private void AddReport(Interactive.GameEvent e)
    {
      Controller.ReportBuilder.Add(e);
    }
    private void AddReport(string key)
    {
      Controller.ReportBuilder.Add(key);
    }
    internal void AddReportPm(string key, params string[] data)
    {
      Controller.ReportBuilder.Add(key, this, data);
    }

    #region Data
    public int Id
    { get { return Pokemon.Id; } }
    public Tile Tile
    { get; internal set; }

    public int Hp
    { 
      get { return Pokemon.Hp.Value; }
      set
      {
        Pokemon.SetHp(value);
        Item.HpChanged(this);
      }
    }
    public PokemonState State
    { 
      get { return Pokemon.State; }
      set
      {
        Pokemon.State = value;
        if (State != PokemonState.Faint)
          AddReport(new Interactive.GameEvents.StateChange(this));
      }
    }
    private AtkContext atkContext;
    public AtkContext AtkContext
    { 
      get { return atkContext; }
      set
      {
        if (atkContext == null)
          atkContext = value;
      }
    }
    public IAbilityE Ability
    { get { return Tile == null || OnboardPokemon.HasCondition("GastroAcid") ? GameService.NULL_ABILITY : GameService.GetAbility(OnboardPokemon.Ability); } }
    public IItemE Item
    { get { return Tile == null || OnboardPokemon.HasCondition("Detain") ? GameService.NULL_ITEM : GameService.GetItem(Pokemon.Item); } }
    public MoveProxy[] Moves
    { get; private set; }
    public MoveProxy StruggleMove
    { get; private set; }
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
        return Hp > 0 && lastActTurn != Controller.TurnNumber &&
          (Action == PokemonAction.MoveAttached || Action == PokemonAction.Stiff || Action == PokemonAction.Moving);
      }
    }
    internal bool CanExecute()
    {
      return
        OnboardPokemon.GetCondition("Sleeping").CanExecute() &&
        Sp.Conditions.Frozen.CanExecute(this) &&
        //懒惰
        OnboardPokemon.GetCondition("Disable").CanExecute() &&
        OnboardPokemon.GetCondition("Imprison").CanExecute() &&
        OnboardPokemon.GetCondition("HealBlock").CanExecute() &&
        //混乱
        OnboardPokemon.GetCondition("Confuse").CanExecute() &&
        //害怕，不屈之心
        OnboardPokemon.GetCondition("Flinch").CanExecute() &&
        //挑拨 
        //重力 
        //着迷 
        Sp.Conditions.Paralyzed.CanExecute(this);
    }
    public bool CanAddState(AttachedState state)
    {
      switch (state)
      {
        case AttachedState.Burn:
        case AttachedState.Freeze:
        case AttachedState.Paralysis:
        case AttachedState.Poison:
        case AttachedState.Sleep:
          return State == PokemonState.Normal;
        default:
          return OnboardPokemon.HasCondition(state.ToString());
      }
    }
    #endregion

    #region Input
    public PokemonAction Action
    { get; set; }
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
        Ability.Attach(this);//特性
        Items.CheckAirBalloon(this);//道具
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
        Tile tile = Tile;
        if (Controller.Withdraw(this)) //追击，无论死没死都已经收回了
        {
          if (this.Hp == 0) Controller.PauseForSendoutInput(Controller.Switch, Tile);
          else Controller.Sendout(tile);
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
      if (Action == PokemonAction.MoveAttached)
        SelectedMove.PreMove();
    }
    internal void ActMove()
    {
      if (!CanActMove) return;
      lastActTurn = Controller.TurnNumber;
      switch (Action)
      {
        case PokemonAction.Stiff:
          Action = PokemonAction.Done;
          break;
        case PokemonAction.Moving:
          bool c = CanExecute();
          Sp.Moves.CheckSkyDrop(AtkContext);
          if (c) AtkContext.Execute();
          else OnboardPokemon.CoordY = CoordY.Plate;
          break;
        case PokemonAction.MoveAttached:
          if (!CanExecute()) goto case PokemonAction.Stiff;
          if (SelectedMove.Move.Id != Sp.Moves.STRUGGLE)
            SelectedMove.PP--;
          atkContext = null;
          SelectedMove.Execute();
          break;
      }
    }
    internal PokemonOutward GetOutward()
    {
      return new PokemonOutward(this);
    }
    #endregion

    #region ChangeHp
    public void MoveHurt(DefContext def)
    {
      if (def.Damage >= Hp)
      {
        def.Damage = Hp;
        if (Abilities.Remain1Hp(this) || Items.Remain1Hp(this))
        {
          def.Damage--;
          ((PairValue)Pokemon.Hp).Value = 1;
        }
        else ((PairValue)Pokemon.Hp).Value = 0;
      }
      else ((PairValue)Pokemon.Hp).Value -= def.Damage;
      OnboardPokemon.SetCondition("Damage", new { Damage = def.Damage, By = def.AtkContext.Attacker.Id });
    }
    public void DamagePercentage(DefContext def, sbyte percentage)
    {
      int v = def.Damage * percentage / 100;
      if (percentage > 0)
      {
        if (Item.BigRoot()) v = (int)(v * 1.3);
        if (Abilities.RaiseAbility(def.Defender, Abilities.LIQUID_OOZE))
        {
          Hp -= v;
          Controller.ReportBuilder.Add(new Interactive.GameEvents.HpChange(this, "Hurt"));
        }
        else
        {
          Hp += v;
          Controller.ReportBuilder.Add(new Interactive.GameEvents.HpChange(this, "HpRecover"));
        }
      }
      else //ReHurt
      {
        if (Ability.RockHead()) return;
        Hp -= v;
        Controller.ReportBuilder.Add(new Interactive.GameEvents.HpChange(this, "ReHurt"));
      }
    }
    public void EffectHurt()
    {
    }
    #endregion

    public void ConsumeItem()
    {
      OnboardPokemon.SetCondition("UsedItem", Pokemon.Item);
      Pokemon.Item = null;
    }
    public bool CheckFaint()
    {
      if (Hp == 0)
      {
        Controller.Withdraw(this, false);
        State = PokemonState.Faint;
        Controller.Board[Pokemon.TeamId].SetCondition("LastFaintTurn", Controller.TurnNumber);
        return true;
      }
      return false;
    }
    private void ChangeLv7D(PokemonProxy by, StatType stat, int change)
    {
      if (change == 0) return;
      if (by != this) Ability.Lv7DChanging(ref stat, ref change);
      if (change == 0) return;

      string statName = DataService.String[stat.ToString()];
      int oldValue = stat == StatType.Accuracy ? OnboardPokemon.AccuracyLv : stat == StatType.Evasion ? OnboardPokemon.EvasionLv : OnboardPokemon.Lv5D.GetStat(stat);
      if (oldValue > 6 && change > 0)
      {
        AddReportPm("Lv7DMax", statName);
        return;
      }
      else if (oldValue < -6 && change < 0)
      {
        AddReportPm("Lv7DMin", statName);
        return;
      }
      if (stat == StatType.Accuracy) OnboardPokemon.AccuracyLv += change;
      else if (stat == StatType.Evasion) OnboardPokemon.EvasionLv += change;
      else OnboardPokemon.ChangeLv7D(stat, change);
      if (change == 1) AddReportPm("Lv7DUp", statName);
      else if (change == -1) AddReportPm("Lv7DDown", statName);
      else if (change > 1) AddReportPm("Lv7DUp2", statName);
      else AddReportPm("Lv7DDown2", statName);
      if (by != this && change < 0) Abilities.CheckDefiant(this);
    }
    public void ChangeLv7D(PokemonProxy by, int a, int d = 0, int sa = 0, int sd = 0, int s = 0, int ac = 0, int e = 0)
    {
      ChangeLv7D(by, StatType.Atk, a);
      ChangeLv7D(by, StatType.Def, d);
      ChangeLv7D(by, StatType.SpAtk, sa);
      ChangeLv7D(by, StatType.SpDef, sd);
      ChangeLv7D(by, StatType.Speed, s);
      ChangeLv7D(by, StatType.Accuracy, ac);
      ChangeLv7D(by, StatType.Evasion, e);
      Items.CheckWhiteHerb(this);
    }
    public void ChangeLv7D(AtkContext atk)
    {
      foreach (MoveLv7DChange c in atk.Move.Lv7DChanges)
      {
        if (c.Probability == 0 || atk.RandomHappen(c.Probability))
          ChangeLv7D(atk.Attacker, c.Type, c.Change);
      }
      Items.CheckWhiteHerb(this);
    }
    private void AddStateImplement(PokemonProxy by, AttachedState state, int turn)
    {
      switch (state)
      {
        case AttachedState.Burn:
          State = PokemonState.Burned;
          goto POKEMON_STATE;
        case AttachedState.Freeze:
          State = PokemonState.Frozen;
          goto POKEMON_STATE;
        case AttachedState.Paralysis:
          State = PokemonState.Paralyzed;
          goto POKEMON_STATE;
        case AttachedState.Poison:
          if (turn == 0) State = PokemonState.Poisoned;
          else
          {
            State = PokemonState.BadlyPoisoned;
            OnboardPokemon.SetCondition("BadlyPoison", new Sp.Conditions.BadlyPoison(this));
          }
          goto POKEMON_STATE;
        case AttachedState.Sleep:
          OnboardPokemon.SetCondition("Sleeping", new Sp.Conditions.Sleeping(this, turn));
          goto POKEMON_STATE;
        default:
          System.Diagnostics.Debugger.Break();
          goto DONE;
      }
    POKEMON_STATE:
      Abilities.CheckSynchronize(this, by, state, turn);
    DONE:  
      Item.StateAdded(this, state);
    }
    public void AddState(PokemonProxy by, AttachedState state, int turn = 0)
    {
      if (CanAddState(state) && Ability.CanAddState(by, state))
        AddStateImplement(by, state, turn);
    }
    public void AddState(AtkContext atk)
    {
      MoveAttachment attachment = atk.Move.Attachment;
      if (CanAddState(attachment.State) &&
         atk.RandomHappen(attachment.Probability) &&
         Ability.CanAddState(atk.Attacker, attachment.State))
      {
        int turn;
        #warning 特性道具对回合影响
        if (attachment.MinTurn != attachment.MaxTurn) turn = Controller.GetRandomInt(attachment.MinTurn, attachment.MaxTurn);
        else turn = attachment.MinTurn;
        AddStateImplement(atk.Attacker, attachment.State, turn);
      }
    }
  }
}
