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
      NullOnboardPokemon = new OnboardPokemon(pokemon, -1);
      moves = pokemon.Moves.Select((m) => new MoveProxy(m, this)).ToArray();
      StruggleMove = new MoveProxy(new Move(Sp.Moves.STRUGGLE, Controller.Game.Settings), this);
    }

    internal readonly OnboardPokemon NullOnboardPokemon;
    public OnboardPokemon OnboardPokemon
    { get; internal set; }

    private void AddReport(GameEvent e)
    {
      Controller.ReportBuilder.Add(e);
    }
    private void AddReport(string key)
    {
      Controller.ReportBuilder.Add(key);
    }
    public void AddReportPm(string key, object arg1 = null, object arg2 = null)
    {
      Controller.ReportBuilder.Add(key, this, arg1, arg2);
    }

    #region Data
    public PokemonAction Action;
    public bool UsingItem;
    public Tile Tile
    { get { return Controller.Board[Pokemon.TeamId][OnboardPokemon.X]; } }
    public int Id
    { get { return Pokemon.Id; } }
    public int Hp
    { 
      get { return Pokemon.Hp.Value; }
      private set
      {
        Pokemon.SetHp(value);
        Item.HpChanged(this);
      }
    }
    public PokemonState State
    { get { return Pokemon.State; } }
    public MoveProxy SelectedMove //先取
    { get; private set; }
    public Tile SelectedTarget
    { get; private set; }
    private AtkContext _atkContext;
    public AtkContext AtkContext
    { get { return _atkContext; } }
    public AbilityE Ability
    { get { return OnboardPokemon == NullOnboardPokemon || OnboardPokemon.HasCondition("GastroAcid") ? EffectsService.NULL_ABILITY : EffectsService.GetAbility(OnboardPokemon.Ability); } }
    public ItemE Item
    { get { return OnboardPokemon == NullOnboardPokemon || OnboardPokemon.HasCondition("Detain") || Controller.Board.HasCondition("MagicRoom") || Ability.Klutz() ? EffectsService.NULL_ITEM : EffectsService.GetItem(Pokemon.Item); } }
    private MoveProxy[] moves;
    public IEnumerable<MoveProxy> Moves
    { get { return moves; } }
    public MoveProxy StruggleMove
    { get; private set; }
    public int Speed
    {
      get
      {
        int speed = OnboardPokemon.Get5D(OnboardPokemon.FiveD.Speed, OnboardPokemon.Lv5D.Speed);
        if (State == PokemonState.PAR) speed >>= 1;
        speed *= Ability.SModifier(this);
        speed *= Item.SModifier(this);
        if (Tile.Field.HasCondition("Tailwind")) speed <<= 1;
        if (Tile.Field.HasCondition("Swamp")) speed = (speed + 1) >> 2; //小数点是0.5以下就舍去，如果是0.75就四舍五入
        return speed;
      }
    }
    public double Weight
    {
      get
      {
        double w = OnboardPokemon.Weight;
        w *= Abilities.WeightModifier(this);
        w *= Items.FloatStone(this);
        return w;
      }
    }
    public void ChangeForm(int number, int form)
    {
      throw new NotImplementedException();
      //图像、重算数据、类型、体重、特性，不包括技能
    }
    public void ChangeAbility(int ab, string log, int arg3 = 0)
    {
      AddReport(new AbilityEvent(this, log, OnboardPokemon.Ability, ab) { Arg3 = arg3 });
      Ability.Detach(this);
      OnboardPokemon.Ability = ab;
      Ability.Attach(this);
    }
    public void ChangeItem(int item, string log, PokemonProxy formerOwner = null, bool attach = true) //lost and found
    {
      UsingItem = false;
      Pokemon.Item = GameDataService.GetItem(item);
      Controller.ReportBuilder.Add(new GetItem(this, log, formerOwner));
      if (attach) Item.Attach(this);
      OnboardPokemon.RemoveCondition("Unburden");
      OnboardPokemon.RemoveCondition("ChoiceItem");
    }
    internal void BuildAtkContext(MoveProxy move)
    {
      _atkContext = new AtkContext(move);
    }
    #endregion

    #region Predict
    internal bool CanSelectWithdraw
    {
      get
      {
        if (Item.ShedShell()) return true;
        if (OnboardPokemon.HasCondition("Ingrain") || OnboardPokemon.HasCondition("CantSelectWithdraw")) return false;
        bool arenaTrap = false, magnetPull = false, shadowTag = false;
        foreach (var pm in Controller.GetOnboardPokemons(1 - Pokemon.TeamId))
        {
          int ab = pm.Ability.Id;
          if (ab == Abilities.SHADOW_TAG) shadowTag = true;
          else if (ab == Abilities.ARENA_TRAP) arenaTrap = true;
          else if (ab == Abilities.MAGNET_PULL) magnetPull = true;
        }
        return
          !
          (
          magnetPull && OnboardPokemon.HasType(BattleType.Steel) ||
          shadowTag && Ability.Id != Abilities.SHADOW_TAG ||
          arenaTrap && EffectsService.IsGroundAffectable.Execute(this, false, false)
          );
      }
    }
    /// <summary>
    /// 和Struggle一起的
    /// </summary>
    public bool CanSelectMove
    { get { return Hp > 0; } }
    public int LastMoveTurn
    { get; private set; }
    internal bool CanMove
    {
      get
      {
        return Hp > 0 && LastMoveTurn != Controller.TurnNumber &&
          (Action == PokemonAction.MoveAttached || Action == PokemonAction.Stiff || Action == PokemonAction.Moving);
      }
    }
    public bool CanHpRecover(bool showFail = false)
    {
      if (OnboardPokemon == NullOnboardPokemon || Hp == 0) return false;
      if (Hp == Pokemon.Hp.Origin)
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
      get { return !(OnboardPokemon == NullOnboardPokemon || Hp == 0 || Ability.MagicGuard()); }
    }
    public bool CanLostItem
    { 
      get
      {
        Item i = Pokemon.Item;
        return !
          (
          i == null ||
          Items.CantLostItem(Pokemon) ||
          Ability.StickyHold()
          );
      }
    }
    private bool CanExecute()
    {
      OnboardPokemon.CoordY = CoordY.Plate;
      return EffectsService.CanExecute.Execute(this);
    }
    public void DeAbnormalState(bool item = false)
    {
      if (State != PokemonState.Normal && Hp > 0)
      {
        if (Pokemon.State == PokemonState.SLP) Tile.Field.RemoveCondition("Rest" + Id);
        Pokemon.State = PokemonState.Normal;
        int i;
        if (item)
        {
          i = Pokemon.Item.Id;
          if (Pokemon.Item.Type != ItemType.Normal) ConsumeItem();
        }
        else i = 0;
        AddReport(new GameEvents.StateChange(this, null , i) { Item = item });
      }
    }
    private bool CanAddState(PokemonProxy by, AbilityE ability, AttachedState state, bool showFail)
    {
      if (OnboardPokemon == NullOnboardPokemon || Hp == 0) return false;
      string fail = Controller.Game.Settings.Mode.NeedTarget() ? "Fail" : "Fail0";
      switch (state)
      {
        case AttachedState.BRN:
          if (State == PokemonState.BRN) goto BEENSTATE;
          if (OnboardPokemon.HasType(BattleType.Fire)) goto NOEFFECT;
          goto STATE;
        case AttachedState.FRZ:
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
          if (OnboardPokemon.HasType(BattleType.Poison) || OnboardPokemon.HasType(BattleType.Steel)) goto FAIL;
          goto STATE;
        case AttachedState.SLP:
          foreach (var pm in Controller.OnboardPokemons)
            if (pm.Action == PokemonAction.Moving && pm.AtkContext.Move.Id == Sp.Moves.UPROAR)
            {
              if (showFail)
              {
                if (pm == this) AddReportPm("UproarCantSLP2");
                else AddReportPm("UproarCantSLP");
              }
              return false;
            }
          if (State == PokemonState.SLP) goto BEENSTATE;
          goto STATE;
        case AttachedState.Confuse:
          if (OnboardPokemon.HasCondition("Confuse")) goto BEENSTATE;
          if (Tile.Field.HasCondition("Safeguard")) goto SAFEGUARD;
          goto GENERIC;
        case AttachedState.Attract:
          if (OnboardPokemon.Gender == PokemonGender.None || by.OnboardPokemon.Gender == PokemonGender.None || OnboardPokemon.Gender == by.OnboardPokemon.Gender) goto NOEFFECT;
          goto CONDITION;
        case AttachedState.LeechSeed:
          if (OnboardPokemon.HasType(BattleType.Grass)) goto NOEFFECT;
          goto CONDITION;
        case AttachedState.Embargo:
          if (OnboardPokemon.Ability == Abilities.MULTITYPE) goto NOEFFECT;
          goto CONDITION;
        case AttachedState.PerishSong:
          return !OnboardPokemon.HasCondition("PerishSong"); //无需判断防音 never show fail
        default:
          goto CONDITION;
      }
    FAIL:
      if (showFail) AddReport(fail);
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
      if (Tile.Field.HasCondition("Safeguard")) goto SAFEGUARD;
      goto GENERIC;
    CONDITION:
      if (OnboardPokemon.HasCondition(state.ToString())) goto FAIL;
    GENERIC:
      return ability.CanAddState(this, by, state, showFail);
    }
    public bool CanAddState(PokemonProxy by, AttachedState state, bool showFail)
    {
      return CanAddState(by, Ability, state, showFail);
    }
    public int CanChangeLv7D(PokemonProxy by, StatType stat, int change, bool showFail)
    {
      if (OnboardPokemon == NullOnboardPokemon || Hp == 0 || change == 0) return 0;
      if (change < 0 && by != this && Tile.Field.HasCondition("Mist"))
      {
        if (showFail) AddReportPm("Mist");
        return 0;
      }
      change = Ability.Lv7DChanging(this, by, stat, change, showFail);
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
      if (CanSelectMove && move.CanBeSelected)
      {
        Action = PokemonAction.WillMove;
        SelectedMove = move;
        SelectedTarget = target;
        return true;
      }
      return false;
    }
    #endregion

    #region internal
    internal void Debut()
    {
      if (Action == PokemonAction.Debuting)
      {
        Tile.Debut();
        Tile.Field.Debut(this);
        if (!CheckFaint())
        {
          Ability.Attach(this);
          Items.AirBalloon(this); Item.Attach(this);
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
        Triggers.WillAct(this);
        Action = PokemonAction.Switching;
        Tile tile = Tile;
        if (Controller.Withdraw(this, "Withdraw")) Controller.Sendout(tile);
        Action = PokemonAction.InBall;
      }
    }
    internal void Move()
    {
      if (CanMove)
      {
        LastMoveTurn = Controller.TurnNumber;
        Triggers.WillAct(this);
        switch (Action)
        {
          case PokemonAction.Stiff:
            AddReportPm("Stiff");
            Action = PokemonAction.Done;
            break;
          case PokemonAction.Moving:
            bool c = CanExecute();
            Sp.Moves.SkyDrop(AtkContext);
            if (c)
            {
              if (!AtkContext.Move.Bide()) Controller.ReportBuilder.Add(PositionChange.Reset("UseMove", this, AtkContext.Move.Id));
              AtkContext.BuildDefContext(SelectedTarget);
              AtkContext.Execute();
            }
            else Action = PokemonAction.Done;
            break;
          case PokemonAction.MoveAttached:
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
              if (AtkContext.FailAll) o.Int = 0;
              else o.Int++;
              Controller.Board.SetCondition("LastMove", o);
            }
            else
            {
              OnboardPokemon.RemoveCondition("LastMove");
              Controller.Board.RemoveCondition("LastMove");
              Action = PokemonAction.Done;
            }
            break;
        } //switch(Action)
      }
    }
    internal PokemonOutward GetOutward()
    {
      return new PokemonOutward(this);
    }
    #endregion

    #region ChangeHp
    public int MoveHurt(int damage)
    {
      if (damage >= Hp)
      {
        damage = Hp;
        if (Triggers.Remaining1HP(this))
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
      if (Action == PokemonAction.Moving && AtkContext.Move.Bide())
      {
        var o = AtkContext.GetCondition("Bide");
        o.By = def.AtkContext.Attacker;
        o.Damage += def.Damage;
      }
    }
    public void HpRecover(int changeHp, bool showFail = false, string logKey = "HpRecover", int arg1 = 0, bool consumeItem = false)
    {
      if (CanHpRecover(showFail))
      {
        if (consumeItem) ConsumeItem();
        if (changeHp == 0) changeHp = 1;
        Hp += changeHp;
        Controller.ReportBuilder.Add(new GameEvents.HpChange(this, logKey, arg1) { ConsumeItem = consumeItem });
      }
    }
    public void HpRecoverByOneNth(int n, bool showFail = false, string logKey = "HpRecover", int arg1 = 0, bool consumeItem = false)
    {
      int hp = Pokemon.Hp.Origin / n;
      HpRecover(hp, showFail, logKey, arg1, consumeItem);
    }
    public void EffectHurt(int changeHp, string logKey = "Hurt", int arg1 = 0, int arg2 = 0)
    {
      if (CanEffectHurt)
      {
        if (changeHp == 0) changeHp = 1;
        Hp -= changeHp;
        OnboardPokemon.SetTurnCondition("Assurance");
        Controller.ReportBuilder.Add(new GameEvents.HpChange(this, logKey, arg1, arg2));
      }
    }
    public void EffectHurtByOneNth(int n, string logKey = "Hurt", int arg1 = 0, int arg2 = 0)
    {
      int hp = Pokemon.Hp.Origin / n;
      EffectHurt(hp, logKey, arg1, arg2);
    }
    #endregion

    public void RemoveItem()
    {
#if DEBUG
      if (Pokemon.Item == null) System.Diagnostics.Debugger.Break();
#endif
      Pokemon.Item = null;
      if (Ability.Unburden()) OnboardPokemon.SetCondition("Unburden");
    }
    public void ConsumeItem()
    {
      OnboardPokemon.SetTurnCondition("UsedItem", Pokemon.Item);
      Tile.Field.SetCondition("UsedItem" + Id, Pokemon.Item);
      if (Pokemon.Item.Type == ItemType.Berry) Tile.Field.SetCondition("UsedBerry" + Id, Pokemon.Item);
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
        if (by.Pokemon.TeamId != Pokemon.TeamId && change < 0) Abilities.Defiant(this);
        return true;
      }
      return false;
    }
    public bool ChangeLv7D(PokemonProxy by, StatType stat, int change, bool showFail, string log = null)
    {
      if (ChangeLv7DImplement(by, stat, change, showFail, log))
      {
        Items.WhiteHerb(this);
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
      if (r) Items.WhiteHerb(this);
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
        Items.WhiteHerb(this);
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
          goto POKEMON_STATE;
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
          goto DONE;
        case AttachedState.Trap:
          {
            var move = by.AtkContext.Move;
            var c = new Condition();
            c.By = by;
            c.Turn = Controller.TurnNumber + turn - 1;
            c.Move = move;
            c.Bool = by.Item.BindingBand();
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
            c.Move = AtkContext.MoveProxy.Type;
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
            o.BattleType = by.AtkContext.Move.Id == 357 ? BattleType.Dark : BattleType.Ghost;
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
      Controller.ReportBuilder.Add(new StateChange(this, log, arg1));
      Abilities.Synchronize(this, by, state, turn);
    DONE:  
      Item.StateAdded(this, by, state);
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
        if (attachment.State == AttachedState.Trap && aer.Item.GripClaw()) turn = 8;
        else if (attachment.MinTurn != attachment.MaxTurn) turn = Controller.GetRandomInt(attachment.MinTurn, attachment.MaxTurn);
        else turn = attachment.MinTurn;
        AddStateImplement(aer, attachment.State, turn, null, 0);
        return true;
      }
      return false;
    }
  }
}
