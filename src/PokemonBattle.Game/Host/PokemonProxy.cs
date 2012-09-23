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
      moves = pokemon.Moves.Select((m) => new MoveProxy(m, this)).ToArray();
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
      if (State == PokemonState.Sleeping) OnboardPokemon.SetCondition("Sleeping", Controller.GetRandomInt(2, 4));
      else if (State == PokemonState.BadlyPoisoned) OnboardPokemon.SetCondition("Poison", Controller.TurnNumber);
      foreach (var m in Moves) m.HasUsed = false;

      Controller.OnboardPokemons.Insert(0, this);
      Abilities.Illusion(this);
    }
    internal void Withdraw()
    {
      var ability = Ability.Id;
      Action = PokemonAction.InBall;
      OnboardPokemon = nullOnboard;
      Tile.Pokemon = null;
      Tile = null;
      Controller.OnboardPokemons.Remove(this);
      Abilities.Withdrawn(this, ability);
    }

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
    { get; internal set; }
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
    { 
      get { return _atkContext; }
      private set
      {
#if DEBUG
        if (_atkContext != null) throw new Exception("AtkContext != null");
#endif
          _atkContext = value;
      }
    }
    public DefContext DefContext
    { get; private set; }
    public AbilityE Ability
    { get { return Tile == null || OnboardPokemon.HasCondition("GastroAcid") ? EffectsService.NULL_ABILITY : EffectsService.GetAbility(OnboardPokemon.Ability); } }
    public ItemE Item
    { get { return Tile == null || OnboardPokemon.HasCondition("Detain") || Controller.Board.HasCondition("MagicRoom") || Ability.Klutz() ? EffectsService.NULL_ITEM : EffectsService.GetItem(Pokemon.Item); } }
    private MoveProxy[] moves;
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
        if (Controller.Board[Pokemon.TeamId].HasCondition("Tailwind")) speed <<= 1;
        if (Controller.Board[Pokemon.TeamId].HasCondition("Swamp")) speed = (speed + 1) >> 2; //小数点是0.5以下就舍去，如果是0.75就四舍五入
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
    public void ChangeAbility(int ab)
    {
      Ability.Detach(this);
      int oldAb = OnboardPokemon.Ability;
      OnboardPokemon.Ability = ab;
      AddReport(new AbilityEvent(this, oldAb, ab));
      Ability.Attach(this);
    }
    public void ChangeItem(int item, string log, PokemonProxy itemLoser = null, bool attach = true) //lost and found
    {
      UsingItem = false;
      Pokemon.Item = DataService.GetItem(item);
      Controller.ReportBuilder.Add(new GetItem(this, log, itemLoser));
      if (attach) Item.Attach(this);
      OnboardPokemon.RemoveCondition("Unburden");
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
    public int LastActTurn
    { get; private set; }
    public bool CanActMove
    {
      get
      {
        return Hp > 0 && LastActTurn != Controller.TurnNumber &&
          (Action == PokemonAction.MoveAttached || Action == PokemonAction.Stiff || Action == PokemonAction.Moving);
      }
    }
    public bool CanHpRecover(bool showFail = false)
    {
      if (Tile == null || Hp == 0) return false;
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
      get { return !(Tile == null || Hp == 0 || Ability.MagicGuard()); }
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
    internal bool CanExecute()
    {
      OnboardPokemon.CoordY = CoordY.Plate;
      return EffectsService.CanExecute.Execute(this);
    }
    public void DeAbnormalState(bool item = false)
    {
      if (State != PokemonState.Normal && Hp > 0)
      {
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
      if (Tile == null || Hp == 0) return false;
      string fail = Controller.Game.Settings.Mode.NeedTarget() ? "Fail" : "Fail0";
      switch (state)
      {
        case AttachedState.Burn:
          if (State == PokemonState.Burned) goto BEENSTATE;
          if (OnboardPokemon.HasType(BattleType.Fire)) goto NOEFFECT;
          goto STATE;
        case AttachedState.Freeze:
          if (State == PokemonState.Frozen) goto BEENSTATE;
          if (OnboardPokemon.HasType(BattleType.Ice)) goto NOEFFECT;
          goto STATE;
        case AttachedState.Paralysis:
          if (State == PokemonState.Paralyzed) goto BEENSTATE;
          goto STATE;
        case AttachedState.Poison:
          if (State == PokemonState.Poisoned || State == PokemonState.BadlyPoisoned)
          {
            if (showFail) AddReportPm("BeenPoisoned");
            return false;
          }
          if (OnboardPokemon.HasType(BattleType.Poison) || OnboardPokemon.HasType(BattleType.Steel)) goto FAIL;
          goto STATE;
        case AttachedState.Sleep:
          if (State == PokemonState.Sleeping) goto BEENSTATE;
          goto STATE;
        case AttachedState.Infatuation:
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
      if (showFail) AddReportPm("Been" + State.ToString());
      return false;
    STATE:
      if (State != PokemonState.Normal) goto FAIL;
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
      if (Tile == null || Hp == 0 || change == 0) return 0;
      if (change < 0 && by != this && Controller.Board[Pokemon.TeamId].HasCondition("Mist"))
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
        Controller.Board[Pokemon.TeamId].Debut(this);
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
    internal void ActMove()
    {
      if (!CanActMove) return;
      LastActTurn = Controller.TurnNumber;
      Triggers.WillActMove(this);
      switch (Action)
      {
        case PokemonAction.Stiff:
          AddReportPm("Stiff");
          Action = PokemonAction.Done;
          break;
        case PokemonAction.Moving:
          bool c = CanExecute();
          Sp.Moves.SkyDrop(AtkContext);
          if (c) AtkContext.Execute();
          else Action = PokemonAction.Done;
          break;
        case PokemonAction.MoveAttached:
          if (CanExecute() && SelectedMove.CanExecute())
          {
            _atkContext = null; //考虑下要不要拿到花括号外面或者删掉
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
          }
          else
          {
            OnboardPokemon.RemoveCondition("LastMove");
            Action = PokemonAction.Done;
          }
          break;
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
        if (Abilities.Sturdy(this) || Items.Remain1Hp(this))
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
      var o = new Condition();
      o.Damage = def.Damage;
      o.By = def.AtkContext.Attacker;
      string c = def.AtkContext.Move.Category == MoveCategory.Physical ? "PhysicalDamage" : "SpecialDamage";
      OnboardPokemon.SetTurnCondition(c, o);
      OnboardPokemon.SetTurnCondition("Damage", o);
      OnboardPokemon.SetTurnCondition("Hurt" + o.By.Id.ToString());
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
      Controller.Board[Pokemon.TeamId].SetCondition("UsedItem" + Id, Pokemon.Item);
      if (Pokemon.Item.Type == ItemType.Berry) Controller.Board[Pokemon.TeamId].SetCondition("UsedBerry" + Id, Pokemon.Item);
      RemoveItem();
    }
    public bool CheckFaint()
    {
      if (Hp == 0)
      {
        Controller.Withdraw(this);
        Pokemon.State = PokemonState.Faint;
        Controller.Board[Pokemon.TeamId].SetCondition("FaintTurn", Controller.TurnNumber);
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
    public bool ChangeLv7D(AtkContext atk)
    {
      bool r = false;
      bool showFail = atk.Move.Category == MoveCategory.Status;
      foreach (MoveLv7DChange c in atk.Move.Lv7DChanges)
      {
        if (c.Probability == 0 || atk.RandomHappen(c.Probability))
          if (c.Type == StatType.All)
          {
            r |= ChangeLv7DImplement(atk.Attacker, StatType.Atk, c.Change, showFail, null);
            r |= ChangeLv7DImplement(atk.Attacker, StatType.Def, c.Change, showFail, null);
            r |= ChangeLv7DImplement(atk.Attacker, StatType.SpAtk, c.Change, showFail, null);
            r |= ChangeLv7DImplement(atk.Attacker, StatType.SpDef, c.Change, showFail, null);
            r |= ChangeLv7DImplement(atk.Attacker, StatType.Speed, c.Change, showFail, null);
          }
          else r |= ChangeLv7DImplement(atk.Attacker, c.Type, c.Change, showFail, null);
      }
      Items.WhiteHerb(this);
      return r;
    }
    private void AddStateImplement(PokemonProxy by, AttachedState state, int turn, string log, int arg1)
    {
      switch (state)
      {
        case AttachedState.Burn:
          Pokemon.State = PokemonState.Burned;
          goto POKEMON_STATE;
        case AttachedState.Freeze:
          Pokemon.State = PokemonState.Frozen;
          goto POKEMON_STATE;
        case AttachedState.Paralysis:
          Pokemon.State = PokemonState.Paralyzed;
          goto POKEMON_STATE;
        case AttachedState.Poison:
          if (turn == 0) Pokemon.State = PokemonState.Poisoned;
          else
          {
            Pokemon.State = PokemonState.BadlyPoisoned;
            OnboardPokemon.SetCondition("Poison", Controller.TurnNumber);
          }
          goto POKEMON_STATE;
        case AttachedState.Sleep:
          Pokemon.State = PokemonState.Sleeping;
          OnboardPokemon.SetCondition("Sleeping", turn == 0 ? Controller.GetRandomInt(2, 4) : turn);
          goto POKEMON_STATE;
        case AttachedState.Confusion:
          OnboardPokemon.SetCondition("Confused", turn == 0 ? Controller.GetRandomInt(2, 5) : turn);
          AddReportPm(log ?? "EnConfused");
          goto DONE;
        case AttachedState.Infatuation:
          OnboardPokemon.SetCondition("Infatuation", by);
          AddReportPm(log ?? "EnInfatuation", arg1);
          goto DONE;
        case AttachedState.Trapped:
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
      var atk = def.AtkContext;
      MoveAttachment attachment = atk.Move.Attachment;
      if ((attachment.Probability == 0 || atk.RandomHappen(attachment.Probability)) && CanAddState(atk.Attacker, def.Ability, attachment.State, atk.Move.Category == MoveCategory.Status))
      {
        int turn;
        if (attachment.State == AttachedState.Trapped && atk.Attacker.Item.GripClaw()) turn = 8;
        else if (attachment.MinTurn != attachment.MaxTurn) turn = Controller.GetRandomInt(attachment.MinTurn, attachment.MaxTurn);
        else turn = attachment.MinTurn;
        AddStateImplement(atk.Attacker, attachment.State, turn, null, 0);
        return true;
      }
      return false;
    }
  }
}
