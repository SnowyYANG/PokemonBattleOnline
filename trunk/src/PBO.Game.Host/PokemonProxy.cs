using System;
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

    internal PokemonProxy(Controller controller, Pokemon pokemon)
    {
      Controller = controller;
      Pokemon = pokemon;
      NullOnboardPokemon = new OnboardPokemon(pokemon, -1);
      StruggleMove = new MoveProxy(new Move(RomData.GetMove(Ms.STRUGGLE), 1), this);
    }

    internal readonly OnboardPokemon NullOnboardPokemon;
    public OnboardPokemon OnboardPokemon
    { get; internal set; }

    private void AddReport(string key)
    {
      Controller.ReportBuilder.ShowLog(key);
    }
    public void AddReportPm(string key, object arg1 = null, object arg2 = null)
    {
      Controller.ReportBuilder.ShowLog(key, this, arg1, arg2);
    }

    #region Data
    public PokemonAction Action
    { get; internal set; }
    public Tile Tile
    { get { return Controller.Board[Pokemon.TeamId][OnboardPokemon.X]; } }
    public int Id
    { get { return Pokemon.Id; } }
    public int Hp
    { get { return Pokemon.Hp; } }
    public PokemonState State
    { get { return Pokemon.State; } }
    public MoveProxy SelectedMove //先取
    { get; private set; }
    public Tile SelectedTarget
    { get; private set; }
    private AtkContext _atkContext;
    public AtkContext AtkContext
    { get { return _atkContext; } }
    public MoveType LastMove
    { get { return AtkContext == null ? null : AtkContext.MoveProxy == null ? null : AtkContext.MoveProxy.Type; } }
    public int Ability
    { get { return OnboardPokemon == NullOnboardPokemon || OnboardPokemon.HasCondition("GastroAcid") ? 0 : OnboardPokemon.Ability; } }
    public int Item
    {
      get
      {
        return
          OnboardPokemon == NullOnboardPokemon ||
          Pokemon.Item == null ||
          !ITs.CanUseItem(this) ||
          ITs.Berry(Pokemon.Item.Id) && Controller.Board[1 - Pokemon.TeamId].Pokemons.Any(ATs.Unnerve) ?
        0 : Pokemon.Item.Id;
      }
    }
    private MoveProxy[] moves;
    public IEnumerable<MoveProxy> Moves
    { get { return moves; } }
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
    public void ChangeForm(int form)
    {
      OnboardPokemon.ChangeForm(OnboardPokemon.Form.Species.GetForm(form));
      Controller.ReportBuilder.ChangeForm(this);
    }
    public void Transform(PokemonProxy target)
    {
      OnboardPokemon.SetCondition("Transform");
      OnboardPokemon.Transform(target.OnboardPokemon);
      moves = new MoveProxy[target.moves.Length];
      for (int i = 0; i < moves.Length; ++i) moves[i] = new MoveProxy(target.moves[i].Type, this);
      Controller.ReportBuilder.Transform(this);
    }
    public void ChangeMove(MoveType from, MoveType to)
    {
      for (int i = 0; i < moves.Length; ++i)
        if (moves[i].Type == from)
        {
          moves[i] = new MoveProxy(to, this);
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
      Pokemon.Item = RomData.GetItem(item);
      Controller.ReportBuilder.SetItem(this);
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
        if (showFail) AddReportPm("FullHp");
        return false;
      }
      if (OnboardPokemon.HasCondition("HealBlock"))
      {
        AddReportPm("HealBlock");
        return false;
      }
      return true;
    }
    public bool CanEffectHurt
    {
      get { return !(OnboardPokemon == NullOnboardPokemon || Hp == 0 || Ability == As.MAGIC_GUARD); }
    }
    private bool CanExecute()
    {
      CoordY = CoordY.Plate;
      return Triggers.CanExecute.Execute(this);
    }
    public void DeAbnormalState()
    {
      if (State != PokemonState.Normal && Hp > 0)
      {
        if (Pokemon.State == PokemonState.SLP) Tile.Field.RemoveCondition("Rest" + Id);
        Pokemon.State = PokemonState.Normal;
        Controller.ReportBuilder.SetState(Pokemon);
      }
    }
    private bool CanAddState(PokemonProxy by, int ability, AttachedState state, bool showFail)
    {
      if (OnboardPokemon == NullOnboardPokemon || Hp == 0) return false;
      string fail = Controller.GameSettings.Mode.NeedTarget() ? "Fail" : "Fail0";
      switch (state)
      {
        case AttachedState.BRN:
          if (State == PokemonState.BRN) goto BEENSTATE;
          if (OnboardPokemon.HasType(BattleType.Fire)) goto NOEFFECT;
          goto STATE;
        case AttachedState.FRZ:
          if (Controller.Weather == Weather.IntenseSunlight) goto FAIL;//战报顺序未测
          if (State == PokemonState.FRZ) goto BEENSTATE;
          if (OnboardPokemon.HasType(BattleType.Ice)) goto NOEFFECT;
          goto STATE;
        case AttachedState.PAR:
          if (State == PokemonState.PAR) goto BEENSTATE;
          goto STATE;
        case AttachedState.PSN:
          if (State == PokemonState.PSN || State == PokemonState.BadlyPSN)
          {
            if (showFail) AddReportPm("BeenPSN");
            return false;
          }
          if (OnboardPokemon.HasType(BattleType.Poison) || OnboardPokemon.HasType(BattleType.Steel)) goto NOEFFECT;
          goto STATE;
        case AttachedState.SLP:
          if (ability != As.SOUNDPROOF)
            foreach (var pm in Controller.ActingPokemons)
              if (pm.Action == PokemonAction.Moving && pm.AtkContext.Move.Id == Ms.UPROAR)
              {
                if (showFail)
                  if (pm == this) AddReportPm("UproarCantSLP2");
                  else AddReportPm("UproarCantSLP");
                return false;
              }
          if (State == PokemonState.SLP) goto BEENSTATE;
          goto STATE;
        case AttachedState.Confuse:
          if (OnboardPokemon.HasCondition("Confuse")) goto BEENSTATE;
          if (Tile.Field.HasCondition("Safeguard") && this != by && by.Ability == As.INFILTRATOR) goto SAFEGUARD;
          goto GENERIC;
        case AttachedState.Attract:
          if (OnboardPokemon.Gender == PokemonGender.None || by.OnboardPokemon.Gender == PokemonGender.None || OnboardPokemon.Gender == by.OnboardPokemon.Gender) goto NOEFFECT;
          goto CONDITION;
        case AttachedState.LeechSeed:
          if (OnboardPokemon.HasType(BattleType.Grass)) goto NOEFFECT;
          goto CONDITION;
        case AttachedState.Embargo:
          if (OnboardPokemon.Ability == As.MULTITYPE) goto NOEFFECT;
          goto CONDITION;
        case AttachedState.PerishSong:
          return !OnboardPokemon.HasCondition("PerishSong"); //无需判断防音 never show fail
        case AttachedState.Yawn:
          if (CanAddState(by, ability, AttachedState.SLP, false)) goto CONDITION;
          else goto FAIL;
        default:
          goto CONDITION;
      }
    FAIL:
      if (showFail) Controller.ReportBuilder.ShowLog(fail);
      return false;
    NOEFFECT:
      if (showFail) AddReportPm("NoEffect");
      return false;
    BEENSTATE:
      if (showFail) AddReportPm("Been" + state);
      return false;
    SAFEGUARD:
      if (showFail) AddReportPm("Safeguard");
      return false;
    STATE:
      if (State != PokemonState.Normal) goto FAIL;
      if (Tile.Field.HasCondition("Safeguard") && this != by && by.Ability != As.INFILTRATOR) goto SAFEGUARD;
      goto GENERIC;
    CONDITION:
      if (OnboardPokemon.HasCondition(state.ToString())) goto FAIL;
    GENERIC:
      return Triggers.CanAddState.Execute(this, by, state, showFail) && Rules.CanAddState(this, state, by, showFail);
    }
    public bool CanAddState(PokemonProxy by, AttachedState state, bool showFail)
    {
      return CanAddState(by, Ability, state, showFail);
    }
    public int CanChangeLv7D(PokemonProxy by, StatType stat, int change, bool showFail)
    {
      if (OnboardPokemon == NullOnboardPokemon || Hp == 0 || change == 0) return 0;
      if (change < 0 && by != this && Tile.Field.HasCondition("Mist") && by.Ability != As.INFILTRATOR) //根据百科非技能似乎不该发动，但排除了一下这样写肯定是对的
      {
        if (showFail) AddReportPm("Mist");
        return 0;
      }
      change = Lv7DChanging.Execute(this, by, stat, change, showFail);
      if (change != 0)
      {
        int oldValue = stat == StatType.Accuracy ? OnboardPokemon.AccuracyLv : stat == StatType.Evasion ? OnboardPokemon.EvasionLv : OnboardPokemon.Lv5D.GetStat(stat);
        if (oldValue == 6 && change > 0)
        {
          if (showFail) AddReportPm("7DMax", stat);
          return 0;
        }
        else if (oldValue == -6 && change < 0)
        {
          if (showFail) AddReportPm("7DMin", stat);
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
      moves = Pokemon.Moves.Select((m) => new MoveProxy(m, this)).ToArray();
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
        if (Item == Is.SHED_SHELL) return true;
        if (OnboardPokemon.HasCondition("Trap") || OnboardPokemon.HasCondition("Ingrain") || OnboardPokemon.HasCondition("CantSelectWithdraw")) return false;
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
          shadowTag && Ability != As.SHADOW_TAG ||
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
      if (Controller.CanWithdraw(this) && Controller.CanSendout(Pokemon.Owner.GetPokemon(sendoutIndex)))
      {
        Action = PokemonAction.WillSwitch;
        Tile.WillSendoutPokemonIndex = sendoutIndex;
        return true;
      }
      return false;
    }
    internal bool SelectMove(MoveProxy move, Tile target)
    {
      if (Hp > 0 && move.CanBeSelected)
      {
        Action = PokemonAction.WillMove;
        SelectedMove = move;
        SelectedTarget = target;
        return true;
      }
      return false;
    }
    #endregion
    #region Turn
    internal int ItemSpeedValue;
    internal bool CanMove
    {
      get
      {
        return Hp != 0 && LastMoveTurn != Controller.TurnNumber &&
          (Action == PokemonAction.MoveAttached || Action == PokemonAction.Stiff || Action == PokemonAction.Moving);
      }
    }
    internal void Debut()
    {
      if (Action == PokemonAction.Debuting)
      {
        Tile.Debut();
        if (!OnboardPokemon.HasCondition("Substitute")) EHTs.Debut(this);
        if (!CheckFaint())
        {
          if (OnboardPokemon.Ability != As.FLOWER_GIFT && OnboardPokemon.Ability != As.FORECAST) AbilityAttach.Execute(this);
          if (!ITs.AirBalloon(this)) STs.ItemAttach(this);
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
        if (Controller.Withdraw(this, "Withdraw")) Controller.Sendout(tile);
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
          AddReportPm("Stiff");
          Action = PokemonAction.Done;
          break;
        case PokemonAction.Moving:
          if (CanExecute())
          {
            if (AtkContext.Move.Id != Ms.BIDE) AddReportPm("UseMove", AtkContext.Move.Id);
            AtkContext.ContinueExecute(SelectedTarget);
          }
          else Action = PokemonAction.Done;
          break;
        case PokemonAction.MoveAttached:
          {
            var o = OnboardPokemon.GetCondition("Encore");
            if (o != null) SelectedMove = Moves.First((m) => m.Type == o.Move);
          }
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
    private PokemonOutward outward;
    internal PokemonOutward GetOutward()
    {
      if (outward == null) outward = new PokemonOutward(Id, Pokemon.TeamId, Pokemon.MaxHp);
      Pokemon o = OnboardPokemon.GetCondition<Pokemon>("Illusion");
      var form = o == null ? OnboardPokemon.Form : o.Form;
      if (o == null) o = Pokemon;
      var name = o.Name;
      var gender = o.Gender;//即使对战画面中不显示性别，实际性别也与变身对象一致，可以被着迷。
      var lv = Pokemon.Lv;
      var chatter = o.Chatter;
      var shiny = o.Shiny;
      var position = new Position(Pokemon.TeamId, OnboardPokemon.X, OnboardPokemon.CoordY);
      var substitute = OnboardPokemon.HasCondition("Substitute");
      var hp = Pokemon.Hp;
      var state = State;
      outward.SetAll(name, form, gender, lv, position, substitute, hp, state, shiny, chatter);
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
    public int MoveHurt(int damage)
    {
      if (damage >= Hp)
      {
        damage = Hp;
        if (STs.Remaining1HP(this))
        {
          damage--;
          Pokemon.Hp = 1;
        }
        else Pokemon.Hp = 0;
      }
      else Pokemon.Hp -= damage;
      if (damage != 0) OnboardPokemon.SetTurnCondition("Assurance");
      return damage;
    }
    public void MoveHurt(DefContext def)
    {
      def.Damage = MoveHurt(def.Damage);
      {
        var o = new Condition();
        o.Damage = def.Damage;
        o.By = def.AtkContext.Attacker;
        string c = def.AtkContext.Move.Category == MoveCategory.Physical ? "PhysicalDamage" : "SpecialDamage";
        OnboardPokemon.SetTurnCondition(c, o);
        OnboardPokemon.SetTurnCondition("Damage", o);
      }
      if (Action == PokemonAction.Moving && AtkContext.Move.Id == Ms.BIDE)
      {
        var o = AtkContext.GetCondition("Bide");
        o.By = def.AtkContext.Attacker;
        o.Damage += def.Damage;
      }
    }
    public void HpRecover(int changeHp, bool showFail = false, string log = "HpRecover", int arg1 = 0, bool consumeItem = false)
    {
      if (CanHpRecover(showFail))
      {
        if (consumeItem) ConsumeItem();
        if (changeHp == 0) changeHp = 1;
        Pokemon.Hp += changeHp;
        AddReportPm(log, arg1);
        Controller.ReportBuilder.ShowHp(this);
      }
    }
    public void HpRecoverByOneNth(int n, bool showFail = false, string log = "HpRecover", int arg1 = 0, bool consumeItem = false)
    {
      int hp = Pokemon.MaxHp / n;
      HpRecover(hp, showFail, log, arg1, consumeItem);
    }
    public void EffectHurt(int changeHp, string logKey = "Hurt", int arg1 = 0, int arg2 = 0)
    {
      if (CanEffectHurt)
      {
        if (changeHp == 0) changeHp = 1;
        Pokemon.Hp -= changeHp;
        AddReportPm(logKey, arg1, arg2);
        Controller.ReportBuilder.ShowHp(this);
        HpChanged.Execute(this);
        OnboardPokemon.SetTurnCondition("Assurance");
      }
    }
    public void EffectHurtByOneNth(int n, string logKey = "Hurt", int arg1 = 0, int arg2 = 0)
    {
      int hp = Pokemon.MaxHp / n;
      EffectHurt(hp, logKey, arg1, arg2);
    }
    #endregion

    /// <summary>
    /// Item should not be null, or Unburden effect will be wrong
    /// </summary>
    public void RemoveItem()
    {
      Pokemon.Item = null;
      if (Ability == As.UNBURDEN) OnboardPokemon.SetCondition("Unburden");
      Controller.ReportBuilder.SetItem(this);
    }
    public void ConsumeItem()
    {
      OnboardPokemon.SetTurnCondition("UsedItem", Pokemon.Item);
      Tile.Field.SetCondition("UsedItem" + Id, Pokemon.Item);
      if (ITs.Berry(Pokemon.Item.Id)) Tile.Field.SetCondition("UsedBerry" + Id, Pokemon.Item);
      RemoveItem();
    }
    public bool CheckFaint()
    {
      if (Hp == 0 && OnboardPokemon != NullOnboardPokemon)
      {
        Tile.Field.SetCondition("FaintTurn", Controller.TurnNumber);
        Pokemon.State = PokemonState.Faint;
        Controller.Withdraw(this, "Faint", false);
        return true;
      }
      return false;
    }
    private bool ChangeLv7DImplement(PokemonProxy by, StatType stat, int change, bool showFail, string log)
    {
      change = CanChangeLv7D(by, stat, change, showFail);
      if (change != 0)
      {
        if (stat == StatType.Accuracy) OnboardPokemon.AccuracyLv += change;
        else if (stat == StatType.Evasion) OnboardPokemon.EvasionLv += change;
        else OnboardPokemon.ChangeLv7D(stat, change);
        if (log == null)
          switch (change)
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
              if (change > 0) log = "7DUp3";
              else log = "7DDown3";
              break;
          }
        AddReportPm(log, stat);
        if (by.Pokemon.TeamId != Pokemon.TeamId && change < 0) ATs.Defiant(this);
        return true;
      }
      return false;
    }
    public bool ChangeLv7D(PokemonProxy by, StatType stat, int change, bool showFail, string log = null)
    {
      if (ChangeLv7DImplement(by, stat, change, showFail, log))
      {
        ITs.WhiteHerb(this);
        return true;
      }
      return false;
    }
    public bool ChangeLv7D(PokemonProxy by, bool showFail, int a, int d = 0, int sa = 0, int sd = 0, int s = 0, int ac = 0, int e = 0)
    {
      bool r = false;
      r |= ChangeLv7DImplement(by, StatType.Atk, a, showFail, null);
      r |= ChangeLv7DImplement(by, StatType.Def, d, showFail, null);
      r |= ChangeLv7DImplement(by, StatType.SpAtk, sa, showFail, null);
      r |= ChangeLv7DImplement(by, StatType.SpDef, sd, showFail, null);
      r |= ChangeLv7DImplement(by, StatType.Speed, s, showFail, null);
      r |= ChangeLv7DImplement(by, StatType.Accuracy, ac, showFail, null);
      r |= ChangeLv7DImplement(by, StatType.Evasion, e, showFail, null);
      if (r) ITs.WhiteHerb(this);
      return r;
    }
    public bool ChangeLv7D(PokemonProxy by, MoveType move)
    {
      bool r = false;
      if (move.Lv7DChanges.Any())
      {
        bool showFail = move.Category == MoveCategory.Status;
        foreach (MoveLv7DChange c in move.Lv7DChanges)
          {
            if (c.Type == StatType.All)
            {
              r |= ChangeLv7DImplement(by, StatType.Atk, c.Change, showFail, null);
              r |= ChangeLv7DImplement(by, StatType.Def, c.Change, showFail, null);
              r |= ChangeLv7DImplement(by, StatType.SpAtk, c.Change, showFail, null);
              r |= ChangeLv7DImplement(by, StatType.SpDef, c.Change, showFail, null);
              r |= ChangeLv7DImplement(by, StatType.Speed, c.Change, showFail, null);
            }
            else r |= ChangeLv7DImplement(by, c.Type, c.Change, showFail, null);
          }
        ITs.WhiteHerb(this);
      }
      return r;
    }
    public bool ChangeLv7D(DefContext def)
    {
      var c = def.AtkContext.Move.Lv7DChanges.FirstOrDefault();
      return c != null && def.RandomHappen(c.Probability) && ChangeLv7D(def.AtkContext.Attacker, def.AtkContext.Move);
    }
    private void AddStateImplement(PokemonProxy by, AttachedState state, int turn, string log, int arg1)
    {
      switch (state)
      {
        case AttachedState.BRN:
          Pokemon.State = PokemonState.BRN;
          goto POKEMON_STATE;
        case AttachedState.FRZ:
          Pokemon.State = PokemonState.FRZ;
          Controller.ReportBuilder.SetState(Pokemon);
          AddReportPm(log, arg1);
          if (CanChangeForm(492, 0))
          {
            ChangeForm(0);
            Pokemon.Form = OnboardPokemon.Form;
          }
          goto DONE;
        case AttachedState.PAR:
          Pokemon.State = PokemonState.PAR;
          goto POKEMON_STATE;
        case AttachedState.PSN:
          if (turn == 0) Pokemon.State = PokemonState.PSN;
          else
          {
            Pokemon.State = PokemonState.BadlyPSN;
            OnboardPokemon.SetCondition("PSN", Controller.TurnNumber);
          }
          goto POKEMON_STATE;
        case AttachedState.SLP:
          Pokemon.State = PokemonState.SLP;
          OnboardPokemon.SetCondition("SLP", turn == 0 ? Controller.GetRandomInt(2, 4) : turn);
          goto POKEMON_STATE;
        case AttachedState.Confuse:
          OnboardPokemon.SetCondition("Confuse", turn == 0 ? Controller.GetRandomInt(2, 5) : turn);
          AddReportPm(log ?? "Confuse");
          goto DONE;
        case AttachedState.Attract:
          OnboardPokemon.SetCondition("Attract", by);
          AddReportPm(log ?? "EnAttract", arg1);
          ITs.DestinyKnot(this, by);
          goto DONE;
        case AttachedState.Trap:
          {
            var move = by.AtkContext.Move;
            var c = new Condition();
            c.By = by;
            c.Turn = Controller.TurnNumber + turn - 1;
            c.Move = move;
            c.Bool = by.Item == Is.BINDING_BAND;
            OnboardPokemon.SetCondition("Trap", c);
            AddReportPm("EnTrap" + move.Id.ToString(), by);
          }
          goto DONE;
        case AttachedState.Nightmare:
          OnboardPokemon.SetCondition("Nightmare");
          AddReportPm("EnNightmare");
          goto DONE;
        case AttachedState.Torment:
          OnboardPokemon.SetCondition("Torment", by);
          AddReportPm("EnTorment");
          goto DONE;
        case AttachedState.Disable:
          {
            var c = new Condition();
            c.Move = LastMove;
            c.Turn = Controller.TurnNumber + turn - 1;
            OnboardPokemon.SetCondition("Disable", c);
            AddReportPm("EnDisable", c.Move.Id);
          }
          goto DONE;
        case AttachedState.Yawn:
          {
            var o = new Condition();
            o.Turn = Controller.TurnNumber + 1;
            o.By = by; //睡眠规则
            OnboardPokemon.AddCondition("Yawn", o);
          }
          AddReportPm("EnYawn");
          goto DONE;
        case AttachedState.HealBlock:
          {
            var o = new Condition();
            o.Turn = Controller.TurnNumber + turn;
            o.By = by;
            OnboardPokemon.SetCondition("HealBlock", o);
          }
          AddReportPm("EnHealBlock");
          goto DONE;
        case AttachedState.CanAttack:
          {
            var o = new Condition();
            o.BattleType = by.AtkContext.Move.Id == Ms.MIRACLE_EYE ? BattleType.Dark : BattleType.Ghost;
            o.By = by;
            OnboardPokemon.SetCondition("CanAttack", o);
            AddReportPm("CanAttack");
          }
          goto DONE;
        case AttachedState.LeechSeed:
          OnboardPokemon.SetCondition("LeechSeed", by.Tile);
          AddReportPm("EnLeechSeed");
          goto DONE;
        case AttachedState.Embargo:
          OnboardPokemon.SetCondition("Embargo");
          AddReportPm("EnEmbargo");
          goto DONE;
        case AttachedState.PerishSong:
          OnboardPokemon.SetCondition("PerishSong", 3);
          goto DONE;
        case AttachedState.Ingrain:
          OnboardPokemon.SetCondition("Ingrain");
          AddReportPm("EnIngrain");
          goto DONE;
#if DEBUG
        default:
          System.Diagnostics.Debugger.Break();
          return;
#endif
      }
    POKEMON_STATE:
      Controller.ReportBuilder.SetState(Pokemon);
      AddReportPm(log, arg1);
      if (state != AttachedState.FRZ && state != AttachedState.SLP) ATs.Synchronize(this, by, state, turn);
    DONE:  
      StateAdded.Execute(this);
    }
    public bool AddState(PokemonProxy by, AttachedState state, bool showFail, int turn = 0, string log = null, int arg1 = 0)
    {
      if (CanAddState(by, state, showFail))
      {
        AddStateImplement(by, state, turn, log, arg1);
        return true;
      }
      return false;
    }
    public bool AddState(DefContext def)
    {
      var aer = def.AtkContext.Attacker;
      MoveAttachment attachment = def.AtkContext.Move.Attachment;
      if (def.RandomHappen(attachment.Probability) && CanAddState(aer, def.Ability, attachment.State, def.AtkContext.Move.Category == MoveCategory.Status))
      {
        int turn;
        if (attachment.State == AttachedState.Trap && aer.Item == Is.GRIP_CLAW) turn = 8;
        else if (attachment.MinTurn != attachment.MaxTurn) turn = Controller.GetRandomInt(attachment.MinTurn, attachment.MaxTurn);
        else turn = attachment.MinTurn;
        AddStateImplement(aer, attachment.State, turn, null, 0);
        return true;
      }
      return false;
    }
  }
}
