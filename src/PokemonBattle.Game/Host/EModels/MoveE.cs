using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Data;
using LightStudio.PokemonBattle.Game.GameEvents;
using LightStudio.PokemonBattle.Game.Host.Sp;

namespace LightStudio.PokemonBattle.Game.Host
{
  public abstract class MoveE : IMoveE
  {
    protected MoveE(int moveId)
    {
      this.Move = DataService.GetMove(moveId);
    }

    public MoveType Move
    { get; private set; }

    protected abstract void Act(AtkContext atk);
    public virtual void Execute(PokemonProxy pm)
    {
      if (pm.AtkContext == null) pm.BuildAtkContext(Move);
      if (PrepareOneTurn(pm) && !Sp.Items.PowerHerb(pm)) return;
      AtkContext atk = pm.AtkContext;
      if (!Abilities.CalculateType(atk)) CalculateType(atk);
      CalculateTargets(atk);
      if (atk.Targets == null || atk.Target != null) Act(atk);
      pm.Action = Move.AdvancedFlags.StiffOneTurn ? PokemonAction.Stiff : PokemonAction.Done;
    }

    protected virtual bool PrepareOneTurn(PokemonProxy pm)
    {
      if (Move.AdvancedFlags.PrepareOneTurn && pm.Action == PokemonAction.MoveAttached)
      {
        pm.AddReportPm("Prepare" + Move.Id.ToString());
        pm.Action = PokemonAction.Moving;
        return true;
      }
      return false;
    }

    #region CalculateTargets
    protected void CalculateTargets(AtkContext atk)
    {
      var report = atk.Controller.ReportBuilder;
      IEnumerable<Tile> ts = GetRangeTiles(atk);
      if (ts == null) return; //no target needed

      List<DefContext> targets = new List<DefContext>();
      if (ts.Count() == 0)
      {
        Fail(atk);
        goto DONE;
      }

      #region Check CoordY
      {
        var miss = new List<DefContext>();
        int count = 0;
        foreach (Tile t in ts)
          if (t.Pokemon != null)
          {
            //从压力无视破格来看，严格来说miss的DefContext不应建立，不过为了NoGuard省心
            var pm = t.Pokemon;
            DefContext def = new DefContext(atk, pm);
            ++count;
            Abilities.CheckPressure(def);
            if (IsYInRange(def) || def.NoGuard) targets.Add(def);
            else report.Add("Miss", pm);
          }
        if (count > 1) atk.MultiTargets = true;
      }
      #endregion
      #region Check for Immunity (or Levitate) on the Ally side, position 1, then position 3. Then check Opponent side, position 1, then 2, then 3,
      var noeffect = new List<DefContext>();
      foreach (DefContext def in targets)
        if (!HasEffect(def))
        {
          noeffect.Add(def);
          report.Add("NoEffect", def.Defender);
        }
      if (noeffect.Count > 0) targets.Remove(noeffect);
      #endregion
      #region [unfinished] Check for Wide Guard in same way
      #warning
      #endregion
      #region Check for Protect
      if (Move.AdvancedFlags.Protectable)
      {
        var protect = new List<DefContext>();
        foreach (DefContext d in targets)
          if (d.Defender.OnboardPokemon.HasCondition("Protect")) protect.Add(d);
        foreach (DefContext d in protect)
        {
          report.Add("Protect", d.Defender);
          targets.Remove(protect);
        }
      }
      #endregion
      #region Check for Telepathy (and possibly other abilities)
      if (!atk.Attacker.Ability.IgnoreDefenderAbility()) //为了性能
      {
        var abnoeffect = new List<DefContext>();
        foreach (DefContext def in targets)
          if (!def.Defender.Ability.CanImplement(def)) abnoeffect.Add(def);
        targets.Remove(abnoeffect);
      }
      #endregion
      #region Check for misses
      if (!atk.Attacker.Ability.NoGuard() && GetAccuracyBase(atk) < 0x65)
      {
        if (Move.Class != MoveInnerClass.OHKO)
        {
          if (Items.MicleBerry(atk)) goto DONE;
          atk.AccuracyModifier = Abilities.AccuracyModifier(atk) * Items.WideLens(atk);
        }
        var miss = new List<DefContext>();
        foreach (DefContext def in targets)
          if (!(def.NoGuard || CanHit(def)))//心眼锁定、无防御
          {
            miss.Add(def);
            report.Add("Miss", def.Defender);
          }
        if (miss.Count > 0) targets.Remove(miss);
      }
      #endregion
    DONE:
      atk.SetTargets(targets);
    }
    protected bool CanHit(DefContext def)
    {
      AtkContext atk = def.AtkContext;
      Controller controller = atk.Controller;
      int acc;
      if (Move.Class == MoveInnerClass.OHKO) //等级原因的“完全没有效果”已经判断过了
        acc = GetAccuracyBase(atk) + atk.Attacker.Pokemon.Lv - def.Defender.Pokemon.Lv;
      else
      {
        int lv;
        if (def.Ability.Unaware()) lv = 0;
        else lv = def.AtkContext.Attacker.OnboardPokemon.AccuracyLv;
        //如果攻击方是天然特性，防御方的回避等级按0计算。 
        //循序渐进无视防御方回避等级。
        //将攻击方的命中等级减去防御方的回避等级。 
        if (!(atk.Attacker.Ability.Unaware() || Move.ChipAway()))
          lv -= def.Defender.OnboardPokemon.EvasionLv;
        if (lv < -6) lv = -6;
        else if (lv > 6) lv = 6;
        //用技能基础命中乘以命中等级修正，向下取整。
        int numerator = 3, denominator = 3;
        if (lv > 0) numerator += lv;
        else denominator -= lv;
        acc = (int)(GetAccuracyBase(atk) * numerator / denominator);

        Modifier m = def.Ability.AccuracyModifier(def);
        m *= atk.AccuracyModifier;
        m *= Items.AccuracyModifier(def);
        m *= Items.ZoomLens(def);
        if (controller.Board.HasCondition("Gravity")) m *= 0x1AAA;//如果场上存在重力，命中×5/3。

        acc *= m;
      }
      //产生1～100的随机数，如果小于等于命中，判定为命中，否则判定为失误。
      return controller.RandomHappen(acc);
    }
    public virtual int GetAccuracyBase(AtkContext def)
    { return Move.Accuracy == 0 ? 0x65 : Move.Accuracy; }
    #endregion
    #region CalculateType
    protected virtual void CalculateType(AtkContext atk) //觉醒力量
    {
      atk.Type = Move.Type;
    }
    private bool HasEffect_Ground(DefContext def)
    {
      var pm = def.Defender.OnboardPokemon;
      return
        (pm.HasCondition("SmackDown") || pm.HasCondition("Ingrain") || def.Defender.Item.IronBall() || def.Defender.Controller.Board.HasCondition("Gravity")) ||
        !(
          pm.HasType(BattleType.Flying) ||
          pm.HasCondition("Suspension") || pm.HasCondition("Telekinesis") ||
          def.Defender.Item.AirBalloon() ||
          def.Defender.RaiseAbility(Abilities.LEVITATE));
    }
    private bool HasEffect_NonGround(BattleType atk, BattleType def)
    {
      return !(
        ((atk == BattleType.Normal || atk == BattleType.Fighting) && def == BattleType.Ghost) ||
        (atk == BattleType.Electric && def == BattleType.Ground) ||
        (atk == BattleType.Poison && def == BattleType.Steel) ||
        (atk == BattleType.Psychic && def == BattleType.Dark) ||
        (atk == BattleType.Ghost && def == BattleType.Normal));
    }
    protected virtual bool HasEffect(DefContext def)
    {
      switch (Move.Class)
      {
        case MoveInnerClass.Attack:
        case MoveInnerClass.AttackAndAbsorb:
        case MoveInnerClass.AttackWithSelfLv7DChange:
        case MoveInnerClass.AttackWithState:
        case MoveInnerClass.AttackWithTargetLv7DChange:
        case MoveInnerClass.OHKO:
          BattleType atk = def.AtkContext.Type;
          BattleType canAtk = def.Defender.OnboardPokemon.GetCondition<BattleType>("CanAttack");
          BattleType def1 = def.Defender.OnboardPokemon.Type1, def2 = def.Defender.OnboardPokemon.Type2;
          return
            ((canAtk != BattleType.Invalid && (canAtk == def1 || canAtk == def2)) || def.Defender.Item.RingTarget() || atk == BattleType.Ground ? HasEffect_Ground(def) : HasEffect_NonGround(atk, def1) && HasEffect_NonGround(atk, def2)) &&
            (Move.Class != MoveInnerClass.OHKO || (def.Defender.Pokemon.Lv <= def.AtkContext.Attacker.Pokemon.Lv && !def.Defender.RaiseAbility(Abilities.STURDY)));
        default:
          if (Move.ThunderWave()) goto case MoveInnerClass.OHKO;
          return true;
      }
    }
    protected virtual IEnumerable<Tile> GetRangeTiles(AtkContext atk)
    {
      Tile select = atk.Attacker.SelectedTarget;
      IEnumerable<Tile> targets = null;
      Controller controller = atk.Controller;
      int team = atk.Attacker.Pokemon.TeamId;
      int rTeam = 1 - team;
      int x = atk.Attacker.OnboardPokemon.X;
      switch (Move.Range)
      {
        case MoveRange.UserField: //do nothing
        case MoveRange.EnemyField: //do nothing
        case MoveRange.Field: //do nothing
          break;
        case MoveRange.Adjacent:
          {
            var ts = new List<Tile>();
            Tile t;
            t = controller.GetTile(team, x - 1); if (t != null) ts.Add(t);
            t = controller.GetTile(team, x + 1); if (t != null) ts.Add(t);
            t = controller.GetTile(rTeam, x - 1); if (t != null) ts.Add(t);
            t = controller.GetTile(rTeam, x); if (t != null) ts.Add(t);
            t = controller.GetTile(rTeam, x + 1); if (t != null) ts.Add(t);
            targets = ts;
          }
          break;
        case MoveRange.AdjacentEnemies:
          {
            var ts = new List<Tile>();
            Tile t;
            t = controller.GetTile(rTeam, x - 1); if (t != null) ts.Add(t);
            t = controller.GetTile(rTeam, x); if (t != null) ts.Add(t);
            t = controller.GetTile(rTeam, x + 1); if (t != null) ts.Add(t);
            targets = ts;
          }
          break;
        case MoveRange.All:
          targets = atk.Controller.Board.Tiles;
          break;
        case MoveRange.Partner:
          if (select == null) goto case MoveRange.UserOrParner;
          else targets = new Tile[] { select };
          break;
        case MoveRange.RandomEnemy:
          {
            int min = 0, max = controller.Game.Board.XBound - 1;
            if (!Move.AdvancedFlags.IsRemote)
            {
              if (x - 1 > min) min = x - 1;
              if (x + 1 < max) max = x + 1;
            }
            targets = new Tile[] { controller.GetTile(rTeam, controller.GetRandomInt(min, max)) };
          }
          break;
        case MoveRange.Single:
        case MoveRange.SingleEnemy:
          if (select == null || (!Move.AdvancedFlags.IsRemote && (select.X < x - 1 || select.X > x + 1)))
            goto case MoveRange.RandomEnemy; //非鬼系选诅咒后变诅咒随机对方一个精灵
          targets = new Tile[] { select };
          break;
        case MoveRange.User: //done?
          targets = new Tile[] { atk.Attacker.Tile };
          break;
        case MoveRange.UserOrParner:
          {
            int min = 0, max = controller.Game.Board.XBound - 1;
            if (!Move.AdvancedFlags.IsRemote)
            {
              if (x - 1 > min) min = x - 1;
              if (x + 1 < max) max = x + 1;
            }
            targets = new Tile[] { controller.GetTile(team, controller.GetRandomInt(min, max)) };
          }
          break;
        case MoveRange.UserParty: //场下队友请另写特效
          {
            var ts = new Tile[controller.Game.Board.XBound];
            for (int i = 0; i < ts.Length; ++i)
              ts[i] = controller.GetTile(team, i);
            targets = ts;
          }
          break;
        default:
          System.Diagnostics.Debugger.Break();
          break;
      }
      return targets;
    }
    protected virtual bool IsYInRange(DefContext def)
    {
      return def.Defender.OnboardPokemon.CoordY == CoordY.Plate;
    }

    #endregion

    protected void Fail(AtkContext atk, PokemonProxy pm = null)
    {
      if (pm == null) atk.Controller.ReportBuilder.Add("Fail_s");
      else atk.Controller.ReportBuilder.Add("Fail", pm);
    }
    protected virtual void MoveEnding(AtkContext atk)
    {
    }
  }
}
