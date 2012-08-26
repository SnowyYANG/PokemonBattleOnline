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
      Controller.OnboardPokemons.Add(this);
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
      set
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
      set
      {
        if (_atkContext == null)
          _atkContext = value;
      }
    }
    public DefContext DefContext
    { get; private set; }
    public IAbilityE Ability
    { get { return Tile == null || OnboardPokemon.HasCondition("GastroAcid") ? EffectsService.NULL_ABILITY : EffectsService.GetAbility(OnboardPokemon.Ability); } }
    public IItemE Item
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
    public void ChangeAbility(int ab)
    {
      Ability.UnAttach(this);
      int oldAb = OnboardPokemon.Ability;
      OnboardPokemon.Ability = ab;
      AddReport(new AbilityEvent(this, oldAb, ab));
      Ability.Attach(this);
    }
    public void ChangeItem(int item)
    {
      UsingItem = false;
      Pokemon.Item = DataService.GetItem(item);
      Item.Attach(this);
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
        if (!Item.ShedShell())
        {
          if (OnboardPokemon.HasCondition("Ingrain") || OnboardPokemon.HasCondition("CantWithdraw")) return false;
          bool arenaTrap, magnetPull, shadowTag;
          {
            int ab = Ability.Id;
            arenaTrap = !OnboardPokemon.HasType(BattleType.Flying) && ab != Abilities.LEVITATE;
            magnetPull = OnboardPokemon.HasType(BattleType.Steel);
            shadowTag = ab != Abilities.SHADOW_TAG;
          }
          if (arenaTrap || magnetPull || shadowTag)
            foreach (var pm in Controller.GetOnboardPokemons(1 - Pokemon.TeamId))
            {
              int ab = pm.Ability.Id;
              if ((ab == Abilities.SHADOW_TAG && shadowTag) || (ab == Abilities.ARENA_TRAP && arenaTrap) || (ab == Abilities.MAGNET_PULL && magnetPull))
                return false;
            }
        }
        return true;
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
    public bool CanHpRecover
    { get { return !(Tile == null || Hp == 0 || Hp == Pokemon.Hp.Origin); } }
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
          i.Mail() ||
          Ability.StickyHold()
          );
#warning TODO: more
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
        int i = item ? Pokemon.Item.Id : 0;
        AddReport(new GameEvents.StateChange(this) { Arg1 = i });
      }
    }
    private bool CanAddState(PokemonProxy by, IAbilityE ability, AttachedState state, bool showFail)
    {
      if (Tile == null || Hp == 0) return false;
      string fail = Controller.Game.Settings.Mode.NeedTarget() ? "Fail" : "Fail0";
      switch (state)
      {
        case AttachedState.Burn:
        case AttachedState.Freeze:
        case AttachedState.Paralysis:
        case AttachedState.Poison:
        case AttachedState.Sleep:
          if (showFail && State != PokemonState.Normal)
          {
            string key;
            if (State == PokemonState.BadlyPoisoned) key = "BeenPoisoned";
            else key = "Been" + State.ToString();
            AddReportPm(key);
          }
          return State == PokemonState.Normal && ability.CanAddState(this, by, state, showFail);
        default:
          if
            (
            OnboardPokemon.HasCondition(state.ToString()) ||
            (state == AttachedState.Infatuation && (OnboardPokemon.Gender == null || by.OnboardPokemon.Gender == null || OnboardPokemon.Gender == by.OnboardPokemon.Gender))
            )
          {
            if (showFail) AddReport(fail);
            return false;
          }
          else
          {
            return ability.CanAddState(this, by, state, showFail);
          }
      }
    }
    public bool CanAddState(PokemonProxy by, AttachedState state, bool showFail)
    {
      return CanAddState(by, Ability, state, showFail);
    }
    public int CanChangeLv7D(PokemonProxy by, StatType stat, int change, bool showFail)
    {
      if (Tile == null || Hp == 0 || change == 0) return 0;
      change = Ability.Lv7DChanging(this, by, stat, change, showFail);
      if (change != 0)
      {
        int oldValue = stat == StatType.Accuracy ? OnboardPokemon.AccuracyLv : stat == StatType.Evasion ? OnboardPokemon.EvasionLv : OnboardPokemon.Lv5D.GetStat(stat);
        if (oldValue == 6 && change > 0)
        {
          if (showFail) AddReportPm("Lv7DMax", stat);
          return 0;
        }
        else if (oldValue == -6 && change < 0)
        {
          if (showFail) AddReportPm("Lv7DMin", stat);
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
        Ability.Attach(this);//特性
        Items.AirBalloon(this); Item.Attach(this);//道具
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
      LastActTurn = Controller.TurnNumber;
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
          break;
        case PokemonAction.MoveAttached:
          if (CanExecute() && SelectedMove.CanExecute())
          {
            _atkContext = null;
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
      var o = new Dictionary<string, object>();
      o["Damage"] = def.Damage;
      o["By"] = def.AtkContext.Attacker;
      string c = def.AtkContext.Move.Category == MoveCategory.Physical ? "PhysicalDamage" : "SpecialDamage";
      OnboardPokemon.SetTurnCondition(c, o);
      OnboardPokemon.SetTurnCondition("Damage", o);
    }
    public void HpRecover(int changeHp, string logKey = "HpRecover", int arg1 = 0, bool removeItem = false)
    {
      if (CanHpRecover)
      {
        if (removeItem) ConsumeItem();
        Hp += changeHp;
        Controller.ReportBuilder.Add(new GameEvents.HpChange(this, logKey, arg1) { RemoveItem = removeItem });
      }
    }
    public void HpRecoverByOneNth(int n, string logKey = "HpRecover", int arg1 = 0, bool removeItem = false)
    {
      int hp = Pokemon.Hp.Origin / n;
      if (hp == 0) hp = 1;
      HpRecover(hp, logKey, arg1, removeItem);
    }
    public void EffectHurt(int changeHp, string logKey = "Hurt", int arg1 = 0, int arg2 = 0)
    {
      if (CanEffectHurt)
      {
        Hp -= changeHp;
        Controller.ReportBuilder.Add(new GameEvents.HpChange(this, logKey, arg1, arg2));
      }
    }
    public void EffectHurtByOneNth(int n, string logKey = "Hurt", int arg1 = 0, int arg2 = 0)
    {
      int hp = Pokemon.Hp.Origin / n;
      if (hp == 0) hp = 1;
      EffectHurt(hp, logKey, arg1, arg2);
    }
    public void DamagePercentage(DefContext def, sbyte percentage)
    {
      int v = def.Damage * percentage / 100;
      if (percentage > 0)
      {
        if (Item.BigRoot()) v = (int)(v * 1.3);
        if (!Ability.MagicGuard() && Abilities.RaiseAbility(def.Defender, Abilities.LIQUID_OOZE)) EffectHurt(v);
        else HpRecover(v);
      }
      else //ReHurt
      {
        if (Ability.RockHead()) return;
        Hp += v;
        Controller.ReportBuilder.Add(new GameEvents.HpChange(this, "ReHurt"));
      }
    }
    #endregion

    public void ConsumeItem()
    {
#if DEBUG
      if (Pokemon.Item == null) System.Diagnostics.Debugger.Break();
#endif
      OnboardPokemon.SetTurnCondition("UsedItem", Pokemon.Item);
      if (Ability.Unburden()) OnboardPokemon.SetCondition("Unburden");
      if (Pokemon.Item.Type == ItemType.Berry) Controller.Board[Pokemon.TeamId].SetCondition("UsedBerry" + Id, Pokemon.Item);
      Pokemon.Item = null;
    }
    public bool CheckFaint()
    {
      if (Hp == 0)
      {
        Controller.Withdraw(this, false);
        Pokemon.State = PokemonState.Faint;
        Controller.Board[Pokemon.TeamId].SetCondition("FaintTurn", Controller.TurnNumber);
        return true;
      }
      return false;
    }
    private bool ChangeLv7DImplement(PokemonProxy by, StatType stat, int change, bool showFail)
    {
      change = CanChangeLv7D(by, stat, change, showFail);
      if (change != 0)
      {
        if (stat == StatType.Accuracy) OnboardPokemon.AccuracyLv += change;
        else if (stat == StatType.Evasion) OnboardPokemon.EvasionLv += change;
        else OnboardPokemon.ChangeLv7D(stat, change);
        string logKey;
        switch (change)
        {
          case 1:
            logKey = "7DUp1";
            break;
          case 2:
            logKey = "7DUp2";
            break;
          case -1:
            logKey = "7DDown1";
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
        if (by.Pokemon.TeamId != Pokemon.TeamId && change < 0) Abilities.Defiant(this);
        return true;
      }
      return false;
    }
    public bool ChangeLv7D(PokemonProxy by, StatType stat, int change, bool showFail)
    {
      if (ChangeLv7DImplement(by, stat, change, showFail))
      {
        Items.WhiteHerb(this);
        return true;
      }
      return false;
    }
    public bool ChangeLv7D(PokemonProxy by, bool showFail, int a, int d = 0, int sa = 0, int sd = 0, int s = 0, int ac = 0, int e = 0)
    {
      bool r = false;
      r |= ChangeLv7DImplement(by, StatType.Atk, a, showFail);
      r |= ChangeLv7DImplement(by, StatType.Def, d, showFail);
      r |= ChangeLv7DImplement(by, StatType.SpAtk, sa, showFail);
      r |= ChangeLv7DImplement(by, StatType.SpDef, sd, showFail);
      r |= ChangeLv7DImplement(by, StatType.Speed, s, showFail);
      r |= ChangeLv7DImplement(by, StatType.Accuracy, ac, showFail);
      r |= ChangeLv7DImplement(by, StatType.Evasion, e, showFail);
      if (r) Items.WhiteHerb(this);
      return r;
    }
    public bool ChangeLv7D(AtkContext atk)
    {
      bool r = false;
      foreach (MoveLv7DChange c in atk.Move.Lv7DChanges)
      {
        if (c.Probability == 0 || atk.RandomHappen(c.Probability))
          r |= ChangeLv7DImplement(atk.Attacker, c.Type, c.Change, atk.Move.Category == MoveCategory.Status);
      }
      Items.WhiteHerb(this);
      return r;
    }
    private void AddStateImplement(PokemonProxy by, AttachedState state, int turn)
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
            OnboardPokemon.SetCondition("BadlyPoison", Controller.TurnNumber);
          }
          goto POKEMON_STATE;
        case AttachedState.Sleep:
          Pokemon.State = PokemonState.Sleeping;
          OnboardPokemon.SetCondition("Sleeping", turn == 0 ? Controller.GetRandomInt(2, 4) : turn);
          goto POKEMON_STATE;
        case AttachedState.Confusion:
          OnboardPokemon.SetCondition("Confused", turn);
          AddReportPm("EnConfused");
          goto DONE;
        case AttachedState.Infatuation:
          OnboardPokemon.SetCondition("Infatuation", by);
          AddReportPm("EnInfatuation");
          goto DONE;
        case AttachedState.Trapped:
          {
            int move = by.AtkContext.Move.Id;
            var c = new Dictionary<string, object>();
            c["Pm"] = by;
            c["Turn"] = Controller.TurnNumber + turn - 1;
            c["Move"] = move;
            c["Band"] = by.Item.BindingBand();
            OnboardPokemon.SetCondition("Trap", c);
            AddReportPm("EnTrap" + move, by);
          }
          goto DONE;
        case AttachedState.Nightmare:
          OnboardPokemon.SetCondition("Nightmare");
          AddReportPm("EnNightmare");
          goto DONE;
        case AttachedState.Torment:
          goto DONE;
        case AttachedState.Disable:
          {
            var c = new Dictionary<string, object>();
            int move = AtkContext.MoveProxy.Type.Id;
            c["Move"] = move;
            c["Turn"] = Controller.TurnNumber + turn - 1;
            OnboardPokemon.SetCondition("Disable", c);
            AddReportPm("EnDisable", move);
          }
          goto DONE;
        case AttachedState.Yawn:
          goto DONE;
        case AttachedState.HealBlock:
          goto DONE;
        case AttachedState.CanAttack:
          goto DONE;
        case AttachedState.LeechSeed:
          goto DONE;
        case AttachedState.Embargo:
          goto DONE;
        case AttachedState.PerishSong:
          goto DONE;
        case AttachedState.Ingrain:
          OnboardPokemon.SetCondition("Ingrain");
          goto DONE;
#if DEBUG
        default:
          System.Diagnostics.Debugger.Break();
          return;
#endif
      }
    POKEMON_STATE:
      Controller.ReportBuilder.Add(new StateChange(this));
      Abilities.Synchronize(this, by, state, turn);
    DONE:  
      Item.StateAdded(this, by, state);
    }
    public bool AddState(PokemonProxy by, AttachedState state, bool showFail, int turn = 0)
    {
      if (CanAddState(by, state, showFail))
      {
        AddStateImplement(by, state, turn);
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
        #warning 特性道具对回合影响
        if (attachment.State == AttachedState.Trapped && atk.Attacker.Item.GripClaw()) turn = 8;
        else if (attachment.MinTurn != attachment.MaxTurn) turn = Controller.GetRandomInt(attachment.MinTurn, attachment.MaxTurn);
        else turn = attachment.MinTurn;
        AddStateImplement(atk.Attacker, attachment.State, turn);
        return true;
      }
      return false;
    }
  }
}
