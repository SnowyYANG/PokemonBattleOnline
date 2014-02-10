﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonBattleOnline.Game;
using PokemonBattleOnline.Game.GameEvents;
using PokemonBattleOnline.Game.Host.Triggers;

namespace PokemonBattleOnline.Game.Host
{
  internal class PokemonProxy
  {
    public readonly Pokemon Pokemon;
    public readonly Controller Controller;

    internal PokemonProxy(Pokemon pokemon)
    {
      Controller = pokemon.Controller;
      Pokemon = pokemon;
      NullOnboardPokemon = new OnboardPokemon(pokemon, -1);
      StruggleMove = new MoveProxy(new Move(RomData.GetMove(Ms.STRUGGLE), 1), this);
      _moves = new List<MoveProxy>(4);
    }

    internal readonly OnboardPokemon NullOnboardPokemon;
    public OnboardPokemon OnboardPokemon
    { get; internal set; }

    public void ShowLogPm(string key, int arg1 = 0, int arg2 = 0)
    {
      Controller.ReportBuilder.ShowLog(key, Id, arg1, arg2);
    }

    #region Data
    public PokemonAction Action
    { get; internal set; }
    public Field Field
    { get { return Controller.Board[Pokemon.TeamId]; } }
    public Tile Tile
    { get { return Controller.Board[Pokemon.TeamId][OnboardPokemon.X]; } }
    public int Id
    { get { return Pokemon.Id; } }
    public int Hp
    { 
      get { return Pokemon.Hp; }
      set
      {
        if (value < 0) value = 0;
        else if (value > Pokemon.MaxHp) value = Pokemon.MaxHp;
        if (Pokemon._hp != value)
        {
          Pokemon._hp = value;
          Controller.ReportBuilder.ShowHp(this);
        }
      }
    }
    public PokemonState State
    { get { return Pokemon.State; } }
    public MoveProxy SelectedMove //先取
    { get; private set; }
    public Tile SelectedTarget
    { get; private set; }
    public bool SelectMega
    { get; private set; }
    private AtkContext _atkContext;
    public AtkContext AtkContext
    { get { return _atkContext; } }
    public MoveType LastMove
    { get { return AtkContext == null ? null : AtkContext.MoveProxy == null ? null : AtkContext.MoveProxy.Type; } }
    private bool NoAbilityE
    { get { return OnboardPokemon == NullOnboardPokemon || OnboardPokemon.HasCondition("GastroAcid"); } }
    public int Ability
    { get { return NoAbilityE ? 0 : OnboardPokemon.Ability; } }
    public bool AbilityE(int ability)
    {
      return OnboardPokemon.Ability == ability && !NoAbilityE;
    }
    private bool NoItemE
    { 
      get
      {
        return
          OnboardPokemon == NullOnboardPokemon ||
          Pokemon.Item == 0 ||
          !ITs.CanUseItem(this) ||
          ITs.Berry(Pokemon.Item) && Controller.Board[1 - Pokemon.TeamId].Pokemons.Any(ATs.Unnerve);
      }
    }
    public int Item
    { get { return NoItemE ? 0 : Pokemon.Item; } }
    public bool ItemE(int item)
    {
      return Pokemon.Item == item && !NoItemE;
    }
    private List<MoveProxy> _moves;
    public IEnumerable<MoveProxy> Moves
    { get { return _moves; } }
    public MoveProxy StruggleMove
    { get; private set; }
    public int Speed
    { get { return SpeedRevise.Execute(this, OnboardPokemon.Get5D(OnboardPokemon.FiveD.Speed, OnboardPokemon.Lv5D.Speed)); } }
    public double Weight
    {
      get
      {
        double w = OnboardPokemon.Weight;
        w *= ATs.WeightModifier(this);
        w *= ITs.FloatStone(this);
        return w;
      }
    }
    public CoordY CoordY
    {
      get { return OnboardPokemon.CoordY; }
      set
      {
        if (OnboardPokemon.CoordY != value)
        {
          OnboardPokemon.CoordY = value;
          Controller.ReportBuilder.SetY(this);
        }
      }
    }
    public void ChangeForm(int form, bool forever = false, string log = "FormChange")
    {
      OnboardPokemon.ChangeForm(OnboardPokemon.Form.Species.GetForm(form));
      if (forever) Pokemon.Form = OnboardPokemon.Form;
      Controller.ReportBuilder.ChangeForm(this);
      if (log != null) ShowLogPm(log);
      AbilityAttach.Execute(this);
    }
    public void Transform(PokemonProxy target)
    {
      OnboardPokemon.SetCondition("Transform");
      OnboardPokemon.Transform(target.OnboardPokemon);
      _moves.Clear();
      foreach (var m in target._moves) _moves.Add(new MoveProxy(m.Type, this));
      Controller.ReportBuilder.Transform(this);
    }
    public void ChangeMove(MoveType from, MoveType to)
    {
      for (int i = 0; i < _moves.Count; ++i)
        if (_moves[i].Type == from)
        {
          _moves[i] = new MoveProxy(to, this);
          break;
        }
    }
    public void ChangeAbility(int ab)
    {
      AbilityDetach.Execute(this);
      OnboardPokemon.Ability = ab;
      AbilityAttach.Execute(this);
    }
    public void SetItem(int item)
    {
      Pokemon.Item = item;
      OnboardPokemon.RemoveCondition("Unburden");
      OnboardPokemon.RemoveCondition("ChoiceItem");
    }
    #endregion

    #region Predict
    public int LastMoveTurn
    { get; private set; }
    public bool CanHpRecover(bool showFail = false)
    {
      if (OnboardPokemon == NullOnboardPokemon || Hp == 0) return false;
      if (Hp == Pokemon.MaxHp)
      {
        if (showFail) ShowLogPm("FullHp");
        return false;
      }
      if (OnboardPokemon.HasCondition("HealBlock"))
      {
        ShowLogPm("HealBlock");
        return false;
      }
      return true;
    }
    public bool CanEffectHurt
    {
      get { return !(OnboardPokemon == NullOnboardPokemon || Hp == 0 || AbilityE(As.MAGIC_GUARD)); }
    }
    private bool CanExecute()
    {
      CoordY = CoordY.Plate;
      return Triggers.CanExecute.Execute(this);
    }
    /// <summary>
    /// null log to show default log
    /// </summary>
    /// <param name="log"></param>
    /// <param name="arg1"></param>
    public void DeAbnormalState(string log = null, int arg1 = 0)
    {
      if (State != PokemonState.Normal && Hp > 0)
      {
        ShowLogPm(log ?? "De" + Pokemon.State.ToString(), arg1);
        Pokemon.State = PokemonState.Normal;
      }
    }
    public int CanChangeLv7D(PokemonProxy by, StatType stat, int change, bool showFail)
    {
      if (OnboardPokemon == NullOnboardPokemon || Hp == 0 || change == 0) return 0;
      change = Lv7DChanging.Execute(this, by, stat, change, showFail);
      if (change != 0)
      {
        int oldValue = stat == StatType.Accuracy ? OnboardPokemon.AccuracyLv : stat == StatType.Evasion ? OnboardPokemon.EvasionLv : OnboardPokemon.Lv5D.GetStat(stat);
        if (oldValue == 6 && change > 0)
        {
          if (showFail) ShowLogPm("7DMax", (int)stat);
          return 0;
        }
        else if (oldValue == -6 && change < 0)
        {
          if (showFail) ShowLogPm("7DMin", (int)stat);
          return 0;
        }
        int value = oldValue + change;
        if (value > 6) change = 6 - oldValue;
        else if (value < -6) change = -6 - oldValue;
      }
      return change;
    }
    public bool CanTransform(PokemonProxy target)
    {
      if (target == null) return false;
      var to = target.OnboardPokemon;
      return !(OnboardPokemon.HasCondition("Transform") || to.HasCondition("Illusion") || to.HasCondition("Transform") || to.HasCondition("Substitute"));
    }
    public bool CanChangeForm(int number)
    { 
      return Pokemon.Form.Species.Number == number && !OnboardPokemon.HasCondition("Transform");
    }
    public bool CanChangeForm(int number, int form)
    {
      return OnboardPokemon.Form.Index != form && CanChangeForm(number);
    }
    #endregion

    #region internal
    internal void Reset()
    {
      _atkContext = null;
      SelectedMove = null;
      SelectedTarget = null;
      _moves.Clear();
      foreach (var m in Pokemon.Moves) _moves.Add(new MoveProxy(m, this));
      LastMoveTurn = 0;
    }
    internal void BuildAtkContext(MoveProxy move)
    {
      if (move.Type.Id == Ms.STRUGGLE) _atkContext = new AtkContext(this);
      else _atkContext = new AtkContext(move);
    }
    #region Input
    internal bool CanSelectWithdraw
    {
      get
      {
        if (OnboardPokemon.HasType(BattleType.Ghost) || ItemE(Is.SHED_SHELL)) return true;
        if (OnboardPokemon.HasCondition("Trap") || OnboardPokemon.HasCondition("Ingrain") || OnboardPokemon.HasCondition("CantSelectWithdraw") || Controller.Board.GetCondition<int>("FairyLock", -1) == Controller.TurnNumber) return false;
        bool arenaTrap = false, magnetPull = false, shadowTag = false;
        foreach (var pm in Controller.GetOnboardPokemons(1 - Pokemon.TeamId))
        {
          int ab = pm.Ability;
          if (ab == As.SHADOW_TAG) shadowTag = true;
          else if (ab == As.ARENA_TRAP) arenaTrap = true;
          else if (ab == As.MAGNET_PULL) magnetPull = true;
        }
        return
          !
          (
          magnetPull && OnboardPokemon.HasType(BattleType.Steel) ||
          shadowTag && !AbilityE(As.SHADOW_TAG) ||
          arenaTrap && HasEffect.IsGroundAffectable(this, true, false)
          );
      }
    }
    /// <summary>
    /// 和Struggle一起的
    /// </summary>
    internal bool CanInput
    { get { return Action == PokemonAction.WaitingForInput; } }
    internal bool CheckNeedInput()
    {
      if (Action == PokemonAction.Done || State == PokemonState.SLP && Action == PokemonAction.Moving && AtkContext.Move.SkipSleepMTA())
      {
        {
          var o = OnboardPokemon.GetCondition("Encore");
          if (o != null)
          {
            foreach(var m in Moves)
              if (m.Type == o.Move)
              {
                if (m.PP == 0) break;
                else goto DONE1;
              }
            OnboardPokemon.RemoveCondition("Encore");
          }
        }
      DONE1:
        if (ITs.ChoiceItem(Item))
        {
          var o = OnboardPokemon.GetCondition<MoveType>("ChoiceItem");
          if (o != null)
          {
            foreach (var m in Moves)
              if (m.Type == o) goto DONE2;
            OnboardPokemon.RemoveCondition("ChoiceItem");
          }
        }
      DONE2:
        Action = PokemonAction.WaitingForInput;
        return true;
      }
      return false;
    }
    internal bool InputSwitch(int sendoutIndex)
    {
      if (Controller.CanWithdraw(this) && Controller.CanSendOut(Pokemon.Owner.GetPokemon(sendoutIndex)))
      {
        Action = PokemonAction.WillSwitch;
        Tile.WillSendOutPokemonIndex = sendoutIndex;
        return true;
      }
      return false;
    }
    internal bool SelectMove(MoveProxy move, Tile target, bool mega)
    {
      if (Hp > 0 && move.CanBeSelected && (!mega || CanMega))
      {
        Action = PokemonAction.WillMove;
        SelectedMove = move;
        SelectedTarget = target;
        SelectMega = mega;
        return true;
      }
      return false;
    }
    #endregion
    #region Turn
    public int ItemSpeedValue;
    public int Priority;
    internal bool CanMove
    { get { return Hp != 0 && LastMoveTurn != Controller.TurnNumber && (Action == PokemonAction.MoveAttached || Action == PokemonAction.Stiff || Action == PokemonAction.Moving); } }
    public bool CanMega
    { get { return  Pokemon.Owner.Pokemons.All((p) => !(p.SelectMega || p.Pokemon.Mega)) && CanChangeForm(ITs.MegaNumber(Pokemon.Item), ITs.MegaNumber(Pokemon.Item)); } }

    internal void Debut()
    {
      if (Action == PokemonAction.Debuting)
      {
        Tile.Debut();
        if (!(OnboardPokemon.HasCondition("Substitute") || AbilityE(As.OVERCOAT))) EHTs.Debut(this);
        if (!CheckFaint())
        {
          if (OnboardPokemon.Ability != As.FLOWER_GIFT && OnboardPokemon.Ability != As.FORECAST) AbilityAttach.Execute(this);
          if (!ITs.AirBalloon(this)) ITs.Attach(this);
        }
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
        STs.WillAct(this);
        Action = PokemonAction.Switching;
        Tile tile = Tile;
        if (Controller.Withdraw(this, "Withdraw", 0, true)) Controller.SendOut(tile);
        Action = PokemonAction.InBall;
      }
    }
    internal void Move()
    {
      LastMoveTurn = Controller.TurnNumber;
      STs.WillAct(this);
      switch (Action)
      {
        case PokemonAction.Stiff:
          ShowLogPm("Stiff");
          Action = PokemonAction.Done;
          break;
        case PokemonAction.Moving:
          if (CanExecute())
          {
            if (AtkContext.Move.Id != Ms.BIDE) ShowLogPm("UseMove", AtkContext.Move.Id);
            AtkContext.ContinueExecute(SelectedTarget);
          }
          else Action = PokemonAction.Done;
          break;
        case PokemonAction.MoveAttached:
          {
            var o = OnboardPokemon.GetCondition("Encore");
            if (o != null)
              foreach (var m in Moves)
                if (m.Type == o.Move) SelectedMove = m;
          }
          ATs.StanceChange(this);
          if (CanExecute() && SelectedMove.CanExecute())
          {
            _atkContext = null;
            SelectedMove.Execute();
            var o = OnboardPokemon.GetCondition("LastMove");
            if (o == null)
            {
              o = new Condition();
              o.Move = AtkContext.Move;
              OnboardPokemon.SetCondition("LastMove", o);
            }
            else if (o.Move != AtkContext.Move)
            {
              o.Move = AtkContext.Move;
              o.Int = 0;
            }
            if (AtkContext.Fail) o.Int = 0;
            else o.Int++;
            Controller.Board.SetCondition("LastMove", o);
          }
          else
          {
            OnboardPokemon.RemoveCondition("LastMove");
            Action = PokemonAction.Done;
          }
          break;
      } //switch(Action)
    }
    /// <summary>
    /// may multi referenced in one report fragment
    /// </summary>
    /// <returns></returns>
    internal PokemonOutward GetOutward()
    {
      var outward = new PokemonOutward(Id, Pokemon.TeamId, Pokemon.MaxHp);
      Pokemon o = OnboardPokemon.GetCondition<Pokemon>("Illusion");
      var form = o == null ? OnboardPokemon.Form : o.Form;
      if (o == null) o = Pokemon;
      var name = o.Name;
      var gender = o.Gender;//即使对战画面中不显示性别，实际性别也与变身对象一致，可以被着迷。
      var lv = Pokemon.Lv;
      var shiny = o.Shiny;
      var position = new Position(Pokemon.TeamId, OnboardPokemon.X, OnboardPokemon.CoordY);
      var substitute = OnboardPokemon.HasCondition("Substitute");
      var hp = Pokemon.Hp;
      var state = State;
      var mega = Pokemon.Mega;
      outward.SetAll(name, form, gender, lv, position, substitute, hp, state, shiny, mega);
      return outward;
    }
    #endregion
    #endregion

    #region ChangeHp
    public void Faint()
    {
      Pokemon.Hp = 0;
      CheckFaint();
    }
    public int MoveHurt(int damage, bool ability)
    {
      if (damage >= Hp)
      {
        damage = Hp;
        if (STs.Remaining1HP(this, ability))
        {
          damage--;
          Pokemon.SetHp(1);
        }
        else Pokemon.SetHp(0);
      }
      else Pokemon.SetHp(Hp - damage);
      if (damage != 0) OnboardPokemon.SetTurnCondition("Assurance");
      return damage;
    }
    public void HpRecover(int changeHp, bool showFail = false, string log = "m_HpRecover", int arg1 = 0, bool consumeItem = false)
    {
      if (CanHpRecover(showFail))
      {
        if (consumeItem) ConsumeItem();
        if (changeHp == 0) changeHp = 1;
        ShowLogPm(log, arg1);
        Hp += changeHp;
      }
    }
    public void HpRecoverByOneNth(int n, bool showFail = false, string log = "m_HpRecover", int arg1 = 0, bool consumeItem = false)
    {
      int hp = Pokemon.MaxHp / n;
      HpRecover(hp, showFail, log, arg1, consumeItem);
    }
    public bool EffectHurt(int hp, string log = "m_Hurt", int arg1 = 0, int arg2 = 0)
    {
      if (CanEffectHurt)
      {
        EffectHurtImplement(hp, log, arg1, arg2);
        return true;
      }
      return false;
    }
    public void EffectHurtImplement(int hp, string log = "m_Hurt", int arg1 = 0, int arg2 = 0)
    {
      if (hp == 0) hp = 1;
      ShowLogPm(log, arg1, arg2);
      Hp -= hp;
      HpChanged.Execute(this);
      OnboardPokemon.SetTurnCondition("Assurance");
    }
    public bool EffectHurtByOneNth(int n, string log = "m_Hurt", int arg1 = 0, int arg2 = 0)
    {
      return EffectHurt(Pokemon.MaxHp / n, log, arg1, arg2);
    }
    public void EffectHurtByOneNthImplement(int n, string log = "m_Hurt", int arg1 = 0, int arg2 = 0)
    {
      EffectHurtImplement(Pokemon.MaxHp / n, log, arg1, arg2);
    }
    #endregion

    /// <summary>
    /// Item should not be null, or Unburden effect will be wrong
    /// </summary>
    public void RemoveItem()
    {
      Pokemon.Item = 0;
      if (AbilityE(As.UNBURDEN)) OnboardPokemon.SetCondition("Unburden");
    }
    public void ConsumeItem(bool cheekPouch = true)
    {
      OnboardPokemon.SetTurnCondition("UsedItem", Pokemon.Item);
      Field.SetCondition("UsedItem" + Id, Pokemon.Item);
      if (ITs.Berry(Pokemon.Item))
      {
        OnboardPokemon.SetCondition("Belch");
        Field.SetCondition("UsedBerry" + Id, Pokemon.Item);
        if (CanHpRecover() && ATs.RaiseAbility(this, As.CHEEK_POUCH)) HpRecoverByOneNth(3);
      }
      RemoveItem();
    }
    public bool CheckFaint()
    {
      if (Hp == 0 && OnboardPokemon != NullOnboardPokemon)
      {
        Field.SetCondition("FaintTurn", Controller.TurnNumber);
        Pokemon.State = PokemonState.Faint;
        Controller.Withdraw(this, "Faint", 0, false);
        return true;
      }
      return false;
    }
    private void ChangeLv7DImplement(PokemonProxy by, StatType stat, int actualChange, string log)
    {
      if (actualChange != 0)
      {
        if (stat == StatType.Accuracy) OnboardPokemon.AccuracyLv += actualChange;
        else if (stat == StatType.Evasion) OnboardPokemon.EvasionLv += actualChange;
        else OnboardPokemon.ChangeLv7D(stat, actualChange);
        if (log == null)
          switch (actualChange)
          {
            case 1:
              log = "7DUp1";
              break;
            case 2:
              log = "7DUp2";
              break;
            case -1:
              log = "7DDown1";
              break;
            case -2:
              log = "7DDown2";
              break;
            default:
              if (actualChange > 0) log = "7DUp3";
              else log = "7DDown3";
              break;
          }
        ShowLogPm(log, (int)stat);
        if ((by == null || by.Pokemon.TeamId != Pokemon.TeamId) && actualChange < 0) STs.Lv7DDown(this);
      }
    }
    /// <summary>
    /// null log to show default log
    /// </summary>
    /// <param name="by"></param>
    /// <param name="stat"></param>
    /// <param name="change"></param>
    /// <param name="showFail"></param>
    /// <param name="log"></param>
    /// <returns></returns>
    public bool ChangeLv7D(PokemonProxy by, StatType stat, int change, bool showFail, bool ability = false, string log = null)
    {
      change = CanChangeLv7D(by, stat, change, showFail);
      if (change != 0)
      {
        if (ability) ATs.RaiseAbility(this);
        ChangeLv7DImplement(by, stat, change, log);
        ITs.WhiteHerb(this);
        return true;
      }
      return false;
    }
    public bool ChangeLv7D(PokemonProxy by, bool showFail, bool ability, int a, int d = 0, int sa = 0, int sd = 0, int s = 0, int ac = 0, int e = 0)
    {
      a = CanChangeLv7D(by, StatType.Atk, a, false);
      d = CanChangeLv7D(by, StatType.Def, d, false);
      sa = CanChangeLv7D(by, StatType.SpAtk, sa, false);
      sd = CanChangeLv7D(by, StatType.SpDef, sd, false);
      s = CanChangeLv7D(by, StatType.Speed, s, false);
      ac = CanChangeLv7D(by, StatType.Accuracy, ac, false);
      e = CanChangeLv7D(by, StatType.Evasion, e, false);
      if (a != 0 || d != 0 || sa != 0 || sd != 0 || s != 0 || ac != 0 || e != 0)
      {
        if (ability) ATs.RaiseAbility(this);
        ChangeLv7DImplement(by, StatType.Atk, a, null);
        ChangeLv7DImplement(by, StatType.SpAtk, sa, null);
        ChangeLv7DImplement(by, StatType.Def, d, null);
        ChangeLv7DImplement(by, StatType.SpDef, sd, null);
        ChangeLv7DImplement(by, StatType.Speed, s, null);
        ChangeLv7DImplement(by, StatType.Accuracy, ac, null);
        ChangeLv7DImplement(by, StatType.Evasion, e, null);
        ITs.WhiteHerb(this);
        return true;
      }
      return false;
    }
    public bool ChangeLv7D(PokemonProxy by, MoveType move)
    {
      bool r = false;
      var c0 = move.Lv7DChanges.FirstOrDefault();
      if (c0 != null)
      {
        bool showFail = move.Category == MoveCategory.Status;
        if (c0.Type == StatType.All) r = ChangeLv7D(by, showFail, false, c0.Change, c0.Change, c0.Change, c0.Change, c0.Change);
        else
        {
          foreach (MoveLv7DChange c in move.Lv7DChanges)
          {
            var ac = CanChangeLv7D(by, c.Type, c.Change, showFail);
            if (ac != 0)
            {
              ChangeLv7DImplement(by, c.Type, ac, null);
              r = true;
            }
          }
          if (r) ITs.WhiteHerb(this);
        }
      }
      return r;
    }
    public bool ChangeLv7D(DefContext def)
    {
      var c = def.AtkContext.Move.Lv7DChanges.FirstOrDefault();
      return c != null && def.RandomHappen(c.Probability) && ChangeLv7D(def.AtkContext.Attacker, def.AtkContext.Move);
    }
    public void CalculatePriority()
    {
      Priority = 0;
      ItemSpeedValue = 0;
      if (Action != PokemonAction.WillSwitch)
      {
        var m = SelectedMove.Type;
        Priority = m.Priority;
        if (m.Category == MoveCategory.Status && AbilityE(As.PRANKSTER) || m.Type == BattleType.Flying && AbilityE(As.GALE_WINGS)) Priority++;
        switch (Item)
        {
          case Is.LAGGING_TAIL:
          case Is.FULL_INCENSE:
            ItemSpeedValue = -1;
            break;
          case Is.QUICK_CLAW:
            if (Controller.RandomHappen(20))
            {
              ShowLogPm("QuickItem", Is.QUICK_CLAW);
              ItemSpeedValue = 1;
            }
            break;
          case Is.CUSTAP_BERRY:
            if (ATs.Gluttony(this))
            {
              ShowLogPm("QuickItem", Is.CUSTAP_BERRY);
              ConsumeItem();
              ItemSpeedValue = 1;
            }
            break;
        }
      }
    }
  }
}
