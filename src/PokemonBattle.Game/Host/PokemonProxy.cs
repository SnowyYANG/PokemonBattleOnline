using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Data;
using LightStudio.PokemonBattle.Game.GameEvents;
using LightStudio.PokemonBattle.Game.Host.Sp;

namespace LightStudio.PokemonBattle.Game.Host
{
  public class PokemonProxy
  {
    public readonly Pokemon Pokemon;
    public readonly Controller Controller;

    internal PokemonProxy(Controller controller, Pokemon pokemon)
    {
      Controller = controller;
      Pokemon = pokemon;
      nullOnboard = new OnboardPokemon(pokemon, -1);

      moves = new List<MoveProxy>();
      for (int i = 0; i < 4; i++)
        if (pokemon.Moves[i] != null) moves.Add(new MoveProxy(pokemon.Moves[i], this));
      StruggleMove = new MoveProxy(new Move(Sp.Moves.STRUGGLE, Controller.Game.Settings), this);
    }

    private readonly OnboardPokemon nullOnboard;
    public OnboardPokemon OnboardPokemon
    { get; private set; }
    internal void Sendout(Tile tile)
    {
      Action = PokemonAction.Debuting;
      Tile = tile;
      tile.Pokemon = this;
      tile.WillSendoutPokemonIndex = Tile.NOPM_INDEX;
      OnboardPokemon = new OnboardPokemon(Pokemon, tile.X);
      Controller.OnboardPokemons.Add(this);
      Abilities.CheckIllusion(this);
    }
    internal void Withdraw()
    {
      Action = PokemonAction.InBall;
      OnboardPokemon = nullOnboard;
      Tile = null;
      Tile.Pokemon = null;
      Controller.OnboardPokemons.Remove(this);
      Abilities.Withdrawn(this);
    }

    private void AddReport(GameEvent e)
    {
      Controller.ReportBuilder.Add(e);
    }
    private void AddReport(string key)
    {
      Controller.ReportBuilder.Add(key);
    }
    internal void AddReportPm(string key, object arg0 = null, object arg1 = null)
    {
      Controller.ReportBuilder.Add(key, this, arg0, arg1);
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
          AddReport(new GameEvents.StateChange(this));
      }
    }
    public PokemonAction Action
    { get; set; }
    public MoveProxy SelectedMove //先取
    { get; private set; }
    public Tile SelectedTarget
    { get; private set; }
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
    public DefContext DefContext
    { get; private set; }
    public IAbilityE Ability
    { get { return Tile == null || OnboardPokemon.HasCondition("GastroAcid") ? EffectsService.NULL_ABILITY : EffectsService.GetAbility(OnboardPokemon.Ability); } }
    public IItemE Item
    { get { return Tile == null || OnboardPokemon.HasCondition("Detain") || Controller.Board.HasCondition("MagicRoom") || Ability.Klutz() ? EffectsService.NULL_ITEM : EffectsService.GetItem(Pokemon.Item); } }
    private List<MoveProxy> moves;
    public IEnumerable<MoveProxy> Moves
    { get { return moves; } }
    public MoveProxy StruggleMove
    { get; private set; }
    public int Speed
    { 
      get
      {
        int speed = OnboardPokemon.Get5D(OnboardPokemon.Static.Speed, OnboardPokemon.Lv5D.Speed);
        if (State == PokemonState.Paralyzed) speed >>= 1;
        speed *= Ability.ADSModifier(this, StatType.Speed);
        speed *= Item.ADSModifier(this, StatType.Speed);
        if (Controller.Board[Pokemon.TeamId].HasCondition("TailWind")) speed <<= 1;
        if (Controller.Board[Pokemon.TeamId].HasCondition("Swamp")) speed = (speed + 1) >> 2; //小数点是0.5以下就舍去，如果是0.75就四舍五入
        return speed;
      }
    }
    public double Weight
    {
      get
      {
        double w = Pokemon.PokemonType.Weight;
        w *= Abilities.WeightModifier(this);
        w *= Items.FloatStone(this);
        return w;
      }
    }
    public void BuildAtkContext(MoveType move)
    {
      AtkContext = new AtkContext(this, move);
    }
    public void BuildDefContext(AtkContext atk)
    {
      DefContext = new DefContext(atk, this);
    }
    #endregion

    #region Predict
    internal WithdrawFail IfSelectWithdraw()
    {
      return null;
    }
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
      OnboardPokemon.CoordY = CoordY.Plate;
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
    internal bool CanInput
    { get { return Action == PokemonAction.WaitingForInput; } }
    internal bool CheckNeedInput()
    {
      if (Action == PokemonAction.Done)
      {
        Action = PokemonAction.WaitingForInput;
        return true;
      }
      return false;
    }
    internal void InputSwitch(int sendoutIndex)
    {
      //if (!CanWithdraw || !Controller.CanSendout(Pokemon.Owner.GetPokemon(sendoutIndex))) return false;
      //Action = PokemonAction.WillSwitch;
      //Tile.WillSendoutPokemonIndex = sendoutIndex;
      //return true;
    }
    internal void SelectMove(MoveProxy move, Tile target)
    {
      //if (!CanSelectMove) return "";
      //if (!move.CanBeSelected) return "";
      //Action = PokemonAction.WillMove;
      //SelectedMove = move;
      //SelectedTarget = target;
      //return null;
    }
    #endregion

    #region internal
    internal void Debut()
    {
      if (Action == PokemonAction.Debuting)
      {
        Ability.Attach(this);//特性
        Items.AirBalloon(this);//道具
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
        Sp.Moves.FocusPunch(SelectedMove);
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
          Sp.Moves.SkyDrop(AtkContext);
          if (c) AtkContext.Execute();
          break;
        case PokemonAction.MoveAttached:
          if (CanExecute() && SelectedMove.CanExecute())
          {
            atkContext = null;
            SelectedMove.Execute();
          }
          else Action = PokemonAction.Done;
          break;
      }
    }
    internal PokemonOutward GetOutward()
    {
      return new PokemonOutward(this);
    }
    #endregion

    #region ChangeHp
    internal int MoveHurt(int damage)
    {
      if (damage >= Hp)
      {
        damage = Hp;
        if (Abilities.Remain1Hp(this) || Items.Remain1Hp(this))
        {
          damage--;
          Pokemon.SetHp(1);
        }
        else Pokemon.SetHp(0);
      }
      else Pokemon.SetHp(Hp - damage);
      return damage;
    }
    public void MoveHurt(DefContext def)
    {
      def.Damage = MoveHurt(def.Damage);
      object o = new { Damage = def.Damage, By = def.AtkContext.Attacker.Id };
      string c = def.AtkContext.Move.Category == MoveCategory.Physical ? "PhysicalDamage" : "SpecialDamage";
      OnboardPokemon.SetTurnCondition(c, o);
      OnboardPokemon.SetTurnCondition("Damage", o);
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
          Controller.ReportBuilder.Add(new GameEvents.HpChange(this, "Hurt"));
        }
        else
        {
          Hp += v;
          Controller.ReportBuilder.Add(new GameEvents.HpChange(this, "HpRecover"));
        }
      }
      else //ReHurt
      {
        if (Ability.RockHead()) return;
        Hp -= v;
        Controller.ReportBuilder.Add(new GameEvents.HpChange(this, "ReHurt"));
      }
    }
    public void EffectHurt()
    {
      System.Diagnostics.Debugger.Break();
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

      int oldValue = stat == StatType.Accuracy ? OnboardPokemon.AccuracyLv : stat == StatType.Evasion ? OnboardPokemon.EvasionLv : OnboardPokemon.Lv5D.GetStat(stat);
      if (oldValue > 6 && change > 0)
      {
        AddReportPm("Lv7DMax", stat);
        return;
      }
      else if (oldValue < -6 && change < 0)
      {
        AddReportPm("Lv7DMin", stat);
        return;
      }
      if (stat == StatType.Accuracy) OnboardPokemon.AccuracyLv += change;
      else if (stat == StatType.Evasion) OnboardPokemon.EvasionLv += change;
      else OnboardPokemon.ChangeLv7D(stat, change);
      {
        string logKey;
        switch (change)
        {
          case 1:
            logKey = "7DUp";
            break;
          case 2:
            logKey = "7DUp2";
            break;
          case -1:
            logKey = "7DDown";
            break;
          case -2:
            logKey = "7DDown2";
            break;
          default:
            if (change > 0) logKey = "7DUp3";
            else logKey = "7DDown3";
            break;
        }
        AddReportPm(logKey, stat);
      }
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
      Items.WhiteHerb(this);
    }
    public void ChangeLv7D(AtkContext atk)
    {
      foreach (MoveLv7DChange c in atk.Move.Lv7DChanges)
      {
        if (c.Probability == 0 || atk.RandomHappen(c.Probability))
          ChangeLv7D(atk.Attacker, c.Type, c.Change);
      }
      Items.WhiteHerb(this);
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
