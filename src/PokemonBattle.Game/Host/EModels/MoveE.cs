using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Data;
using LightStudio.PokemonBattle.Game.GameEvents;
using LightStudio.PokemonBattle.Game.Host.Sp;

namespace LightStudio.PokemonBattle.Game.Host
{
  public abstract class MoveE
  {
    protected static bool ForceSwitch(DefContext def)
    {
      var der = def.Defender;
      var c = der.Controller;
      if (!(def.AtkContext.Attacker.Tile == null || der.Hp == 0 || !c.CanWithdraw(der) || def.Ability.SuctionCups() || der.OnboardPokemon.HasCondition("Ingrain")))
      {
        var sendouts = new List<int>();
        {
          var pms = der.Pokemon.Owner.Pokemons.ToArray();
          for (int i = 0; i < pms.Length; ++i)
            if (c.CanSendout(pms[i])) sendouts.Add(i);
        }
        if (sendouts.Count != 0)
        {
          var tile = der.Tile;
          c.Withdraw(der, false, "ForceWithdraw");
          tile.WillSendoutPokemonIndex = sendouts[c.GetRandomInt(0, sendouts.Count - 1)];
          c.Sendout(tile, true, "ForceSendout");
          return true;
        }
      }
      return false;
    }
    
    protected MoveE(int moveId)
    {
      this.Move = DataService.GetMove(moveId);
    }

    public MoveType Move
    { get; private set; }

    protected abstract void Act(AtkContext atk);
    public virtual void Execute(PokemonProxy pm, UseMove eventForPP)
    {
      if (pm.AtkContext == null) pm.BuildAtkContext(Move);
      AtkContext atk = pm.AtkContext;
      int oldPP = atk.MoveProxy.PP;
      {
        if (atk.Attacker.Ability.Normalize()) atk.Type = BattleType.Normal;
        else CalculateType(atk);
        if (NotFail(atk))
        {
          CalculateTargets(atk);
          if (atk.Targets == null || atk.Target != null)
          {
            Act(atk);
            MoveEnding(atk);
          }
          else
          {
            atk.FailAll = true;
            atk.Attacker.Action = PokemonAction.Done;
          }
        }
        else FailAll(atk);
      }
      if (eventForPP != null) eventForPP.PP = oldPP - atk.MoveProxy.PP;
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
    protected virtual bool NotFail(AtkContext atk)
    {
      return true;
    }
    protected internal virtual void CalculateTargets(AtkContext atk)
    {
      IEnumerable<Tile> ts = GetRangeTiles(atk);
      if (ts == null) return; //no target needed
      List<DefContext> targets = new List<DefContext>();
      if (ts.Count() != 0)
      {
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
              Abilities.Pressure(def);
              if (IsYInRange(def) || def.NoGuard) targets.Add(def);
              else pm.AddReportPm("Miss");
            }
          if (count > 1) atk.MultiTargets = true;
        }
        #endregion
        #region Attack Move and Thunder Wave: Check for Immunity (or Levitate) on the Ally side, position 1, then position 3. Then check Opponent side, position 1, then 2, then 3,
        if (Move.Category != MoveCategory.Status || Move.ThunderWave())
          foreach (DefContext def in targets.ToArray())
            if (!HasEffect(def))
            {
              targets.Remove(def);
              def.Defender.AddReportPm("NoEffect");
            }
        #endregion
        #region Check for Wide Guard in same way
        {
          var board = atk.Controller.Board;
          if (Move.Range != MoveRange.Single)
            foreach (var def in targets.ToArray())
              if (board[def.Defender.Pokemon.TeamId].HasCondition("WideGuard"))
              {
                def.Defender.AddReportPm("WideGuard");
                targets.Remove(def);
              }
          if (Move.Priority > 0)
            foreach (var def in targets.ToArray())
              if (board[def.Defender.Pokemon.TeamId].HasCondition("QuickGuard"))
              {
                def.Defender.AddReportPm("QuickGuard");
                targets.Remove(def);
              }
        }
        #endregion
        #region Check for Protect
        if (Move.AdvancedFlags.Protectable)
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
        if (!atk.Attacker.Ability.IgnoreDefenderAbility()) //为了性能
          foreach (DefContext def in targets.ToArray())
            if (def.Defender != atk.Attacker && !def.Defender.Ability.CanImplement(def)) targets.Remove(def);
        #endregion
        #region Check for misses
        if (!atk.Attacker.Ability.NoGuard() && GetAccuracyBase(atk) < 0x65)
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
      }
      else FailAll(atk);
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
    { return Move.Accuracy == 0 ? 0x65 : Move.Accuracy; }
    #endregion
    #region CalculateType
    protected virtual void CalculateType(AtkContext atk)
    {
      atk.Type = Move.Id == Moves.STRUGGLE ? BattleType.Invalid : Move.Type;
    }
    private bool HasEffect_Ground(DefContext def)
    {
      return EffectsService.IsGroundAffectable.Execute(def.Defender, def.AtkContext.Attacker.Ability.IgnoreDefenderAbility(), true);
    }
    private bool HasEffect_NonGround(DefContext def)
    {
      var type = def.AtkContext.Type.NoEffect();
      return type == BattleType.Invalid || !(type == BattleType.Ghost ? def.AtkContext.Attacker.Ability.Scrappy() : def.Defender.OnboardPokemon.HasType(type));
    }
    protected virtual bool HasEffect(DefContext def)
    {
      BattleType canAtk;
      {
        var o = def.Defender.OnboardPokemon.GetCondition("CanAttack");
        canAtk = o == null ? BattleType.Invalid : o.BattleType;
      }
      return
        (canAtk != BattleType.Invalid && def.Defender.OnboardPokemon.HasType(canAtk) || def.Defender.Item.RingTarget() || (def.AtkContext.Type == BattleType.Ground ? HasEffect_Ground(def) : HasEffect_NonGround(def))) &&
        (Move.Class != MoveInnerClass.OHKO || (def.Defender.Pokemon.Lv <= def.AtkContext.Attacker.Pokemon.Lv && !def.Defender.RaiseAbility(Abilities.STURDY)));
    }
    protected virtual MoveRange GetRange(AtkContext atk)
    {
      return Move.Range;
    }
    protected virtual IEnumerable<Tile> GetRangeTiles(AtkContext atk)
    {
      Tile select = atk.Attacker.SelectedTarget;
      IEnumerable<Tile> targets = null;
      Controller controller = atk.Controller;
      int team = atk.Attacker.Pokemon.TeamId;
      int rTeam = 1 - team;
      int x = atk.Attacker.OnboardPokemon.X;
      switch (GetRange(atk))
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
          Abilities.ReTarget(atk, ref select);
          goto case MoveRange.SingleEnemy;
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
      }
      return targets;
    }
    protected virtual bool IsYInRange(DefContext def)
    {
      return def.Defender.OnboardPokemon.CoordY == CoordY.Plate;
    }
    #endregion

    protected void FailAll(AtkContext atk)
    {
      atk.FailAll = true;
      atk.Attacker.Action = PokemonAction.Done;
      atk.Controller.ReportBuilder.Add("Fail0");
    }
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
      atk.Attacker.Action = Move.AdvancedFlags.StiffOneTurn ? PokemonAction.Stiff : PokemonAction.Done;
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
