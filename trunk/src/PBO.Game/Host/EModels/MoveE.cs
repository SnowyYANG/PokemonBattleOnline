﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonBattleOnline.Data;
using PokemonBattleOnline.Game.GameEvents;
using PokemonBattleOnline.Game.Host.Sp;

namespace PokemonBattleOnline.Game.Host
{
  public abstract class MoveE
  {
    protected static IEnumerable<Tile> GetRangeTiles(AtkContext atk, MoveRange range, Tile select)
    {
      var aer = atk.Attacker;
      IEnumerable<Tile> targets = null;
      Board b = aer.Controller.Board;
      bool remote = atk.Move.Flags.IsRemote;
      int team = aer.Pokemon.TeamId;
      int rTeam = 1 - team;
      int x = aer.OnboardPokemon.X;
      switch (range)
      {
        case MoveRange.UserField: //do nothing
        case MoveRange.EnemyField: //do nothing
        case MoveRange.Field: //do nothing
        case MoveRange.UserParty: //防音防不住治愈铃铛，所以这只是个摆设
          break;
        case MoveRange.Adjacent:
          {
            var ts = new List<Tile>();
            Tile t;
            t = b[team][x - 1]; if (t != null) ts.Add(t);
            t = b[team][x + 1]; if (t != null) ts.Add(t);
            t = b[rTeam][x - 1]; if (t != null) ts.Add(t);
            t = b[rTeam][x]; if (t != null) ts.Add(t);
            t = b[rTeam][x + 1]; if (t != null) ts.Add(t);
            targets = ts;
          }
          break;
        case MoveRange.AdjacentEnemies:
          {
            var ts = new List<Tile>();
            Tile t;
            t = b[rTeam][x - 1]; if (t != null) ts.Add(t);
            t = b[rTeam][x]; if (t != null) ts.Add(t);
            t = b[rTeam][x + 1]; if (t != null) ts.Add(t);
            targets = ts;
          }
          break;
        case MoveRange.All:
          targets = b.Tiles;
          break;
        case MoveRange.Partner:
          if (select == null) goto case MoveRange.UserOrParner;
          else targets = new Tile[] { select };
          break;
        case MoveRange.RandomEnemy:
          {
            int min = 0, max = b.XBound - 1;
            if (!remote)
            {
              if (x - 1 > min) min = x - 1;
              if (x + 1 < max) max = x + 1;
            }
            var ts = new List<Tile>();
            for (int i = min; i <= max; ++i)
              if (b[rTeam][i].Pokemon != null) ts.Add(b[rTeam][i]);
            if (ts.Count == 0) targets = new Tile[] { };
            else targets = new Tile[] { ts[aer.Controller.GetRandomInt(0, ts.Count - 1)] };
          }
          break;
        case MoveRange.Single: 
          goto case MoveRange.SingleEnemy;
        case MoveRange.SingleEnemy:
          if (select == null || (!remote && (select.X < x - 1 || select.X > x + 1)))
            goto case MoveRange.RandomEnemy; //非鬼系选诅咒后变诅咒随机对方一个精灵
          targets = new Tile[] { select };
          break;
        case MoveRange.User: //done?
          targets = new Tile[] { aer.Tile };
          break;
        case MoveRange.UserOrParner:
          {
            int min = 0, max = b.XBound - 1;
            if (!remote)
            {
              if (x - 1 > min) min = x - 1;
              if (x + 1 < max) max = x + 1;
            }
            targets = new Tile[] { b[team][aer.Controller.GetRandomInt(min, max)] };
          }
          break;
      }
      return targets;
    }
    internal static bool CanForceSwitch(PokemonProxy pm, bool abilityAvailable)
    {
      return !(pm.Hp == 0 || !pm.Controller.CanWithdraw(pm) || abilityAvailable && pm.Ability.SuctionCups() || pm.OnboardPokemon.HasCondition("Ingrain"));
    }
    internal static void ForceSwitchImplement(PokemonProxy pm, string log = "ForceWithdraw")
    {
      var c = pm.Controller;
      var sendouts = new List<int>();
      {
        var pms = pm.Pokemon.Owner.Pokemons.ToArray();
        for (int i = pm.Controller.GameSettings.Mode.OnboardPokemonsPerPlayer(); i < pms.Length; ++i)
          if (c.CanSendout(pms[i])) sendouts.Add(i);
      }
      var tile = pm.Tile;
      c.Withdraw(pm, log, false);
      tile.WillSendoutPokemonIndex = sendouts[c.GetRandomInt(0, sendouts.Count - 1)];
      c.Sendout(tile, true, "ForceSendout");
    }
    protected static bool ForceSwitch(DefContext def)
    {
      if (def.AtkContext.Attacker.Tile != null && CanForceSwitch(def.Defender, def.AtkContext.Attacker.Ability.IgnoreDefenderAbility()))
      {
        ForceSwitchImplement(def.Defender);
        return true;
      }
      return false;
    }
    
    public readonly MoveType Move;

    protected MoveE(int moveId)
    {
      Move = GameDataService.GetMove(moveId);
    }

    protected abstract void Act(AtkContext atk);
    public virtual void InitAtkContext(AtkContext atk)
    {
    }
    protected internal virtual void BuildDefContext(AtkContext atk, Tile select)
    {
      IEnumerable<Tile> ts = GetRangeTiles(atk, Moves.GetRange(atk.Attacker, atk.Move), select);
      if (ts != null)
      {
        var targets = new List<DefContext>();
        foreach (Tile t in ts)
          if (t.Pokemon != null) targets.Add(new DefContext(atk, t.Pokemon));
        atk.SetTargets(targets);
      }
    }
    public virtual void Execute(AtkContext atk)
    {
      if (NotFail(atk))
      {
        CalculateType(atk);
        FilterDefContext(atk);
        if (atk.Targets == null || atk.Target != null)
        {
          atk.ImplementPressure();
          Act(atk);
          MoveEnding(atk);
        }
        else atk.FailAll(null);
      }
      else atk.FailAll();
    }

    protected virtual bool PrepareOneTurn(AtkContext atk)
    {
      if (Move.Flags.PrepareOneTurn && atk.Attacker.Action == PokemonAction.MoveAttached)
      {
        atk.Attacker.AddReportPm("Prepare" + Move.Id.ToString());
        atk.SetAttackerAction(PokemonAction.Moving);
        return true;
      }
      return false;
    }

    #region CalculateTargets
    protected virtual bool NotFail(AtkContext atk)
    {
      return atk.Targets == null || atk.Target != null;
    }
    protected internal virtual void FilterDefContext(AtkContext atk)
    {
      if (atk.Targets == null) return;
      Abilities.ReTarget(atk);
      List<DefContext> targets = atk.Targets.ToList();
      #region Check CoordY
      {
        var count = 0;
        foreach (DefContext def in targets.ToArray())
        {
          ++count;
          if (!(IsYInRange(def) || def.NoGuard))
          {
            def.Defender.AddReportPm("Miss");
            targets.Remove(def);
          }
        }
        if (count > 1) atk.MultiTargets = true;
      }
      #endregion
      #region Attack Move and Thunder Wave: Check for Immunity (or Levitate) on the Ally side, position 1, then position 3. Then check Opponent side, position 1, then 2, then 3,
      foreach (DefContext def in targets.ToArray())
        if (!HasEffect(def))
        {
          targets.Remove(def);
          def.Defender.AddReportPm("NoEffect");
        }
      #endregion
      #region Check for Wide Guard in same way
      if (Move.Range == MoveRange.Adjacent || Move.Range == MoveRange.AdjacentEnemies)
        foreach (var def in targets.ToArray())
          if (def.Defender.Tile.Field.HasCondition("WideGuard"))
          {
            def.Defender.AddReportPm("WideGuard");
            targets.Remove(def);
          }
      if (Move.Priority > 0)
        foreach (var def in targets.ToArray())
          if (def.Defender.Tile.Field.HasCondition("QuickGuard"))
          {
            def.Defender.AddReportPm("QuickGuard");
            targets.Remove(def);
          }
      #endregion
      #region Check for Protect
      if (Move.Flags.Protectable)
      {
        foreach (DefContext d in targets.ToArray())
          if (d.Defender.OnboardPokemon.HasCondition("Protect"))
          {
            d.Defender.AddReportPm("Protect");
            targets.Remove(d);
          }
      }
      #endregion
      #region Check for Telepathy (and possibly other abilities)
      {
        var mc = Move.Flags.MagicCoat && !atk.HasCondition("IgnoreMagicCoat");
        var ab = !atk.Attacker.Ability.IgnoreDefenderAbility();
        foreach (DefContext def in targets.ToArray())
          if (def.Defender != atk.Attacker && (mc && Triggers.MagicCoat(atk, def.Defender) || ab && !def.Defender.Ability.CanImplement(def))) targets.Remove(def);
      }
      #endregion
      #region Check for misses
      if (!atk.Attacker.Ability.NoGuard() && GetAccuracyBase(atk) != 0x65)
      {
        if (Move.Class != MoveInnerClass.OHKO)
        {
          if (Items.MicleBerry(atk)) goto DONE;
          atk.AccuracyModifier = Abilities.AccuracyModifier(atk) * Items.WideLens(atk);
        }
        foreach (DefContext def in targets.ToArray())
          if (!(def.NoGuard || CanHit(def)))//心眼锁定、无防御
          {
            targets.Remove(def);
            def.Defender.AddReportPm("Miss");
          }
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
        if (!(atk.Attacker.Ability.Unaware() || Move.IgnoreDefenderLv7D()))
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
    { return Move.Accuracy; }
    #endregion
    #region CalculateType
    protected virtual void CalculateType(AtkContext atk)
    {
      atk.Type = atk.Attacker.Ability.Normalize() ? BattleType.Normal : Move.Type;
    }
    private bool HasEffect_Ground(DefContext def)
    {
      return EffectsService.IsGroundAffectable.Execute(def.Defender, !def.AtkContext.Attacker.Ability.IgnoreDefenderAbility(), true);
    }
    private bool HasEffect_NonGround(DefContext def)
    {
      var type = def.AtkContext.Type.NoEffect();
      return type == BattleType.Invalid || type == BattleType.Ghost && def.AtkContext.Attacker.Ability.Scrappy() || !def.Defender.OnboardPokemon.HasType(type);
    }
    protected virtual bool HasEffect(DefContext def)
    {
      if (Move.Category == MoveCategory.Status && !Move.ThunderWave()) return true;
      if (Move.Class == MoveInnerClass.OHKO && (def.Defender.Pokemon.Lv > def.AtkContext.Attacker.Pokemon.Lv || def.Defender.RaiseAbility(Abilities.STURDY))) return false;
      BattleType canAtk;
      {
        var o = def.Defender.OnboardPokemon.GetCondition("CanAttack");
        canAtk = o == null ? BattleType.Invalid : o.BattleType;
      }
      return
        (
        canAtk != BattleType.Invalid && def.Defender.OnboardPokemon.HasType(canAtk) ||
        def.Defender.Item.RingTarget() ||
        def.AtkContext.Type == BattleType.Ground ? HasEffect_Ground(def) : HasEffect_NonGround(def));
    }
    protected virtual bool IsYInRange(DefContext def)
    {
      return def.Defender.OnboardPokemon.CoordY == CoordY.Plate;
    }
    #endregion

    protected void Fail(DefContext def)
    {
      Fail(def.Defender);
    }
    protected void Fail(PokemonProxy der)
    {
      if (der.Controller.Game.Settings.Mode.NeedTarget()) der.AddReportPm("Fail");
      else der.Controller.ReportBuilder.Add("Fail0");
    }
    protected virtual void MoveEnding(AtkContext atk)
    {
      atk.SetAttackerAction(Move.Flags.StiffOneTurn ? PokemonAction.Stiff : PokemonAction.Done);
      if (atk.Targets != null)
        foreach (var d in atk.Targets)
        {
          d.Defender.Item.Attach(d.Defender);
          Abilities.RecoverAfterMoldBreaker(d.Defender);
        }
      atk.Attacker.Item.Attach(atk.Attacker); //先树果汁后PP果
    }
  }
}