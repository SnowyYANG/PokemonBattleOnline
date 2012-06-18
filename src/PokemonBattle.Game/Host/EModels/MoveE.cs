using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Data;
using LightStudio.PokemonBattle.Interactive.GameEvents;
using LightStudio.PokemonBattle.Game.Sp;

namespace LightStudio.PokemonBattle.Game
{
  public abstract class MoveE : IMoveE
  {
    protected static readonly sbyte[,] BATTLE_TYPE_EFFECT = new sbyte[18, 18] { { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 1, 0, 0, -1, -1, -1, -1, 1, 0, 0, 0, -1, 1, 0, -1, 0 }, { 0, 0, -1, 0, 0, -1, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, -1, 0 }, { 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0 }, { 0, 0, 0, -1, -1, 0, 0, 1, 0, -1, -128, 0, 0, 0, 0, 0, 0, 1 }, { 0, -1, 1, 0, 0, 0, 0, -1, -128, 0, 0, 1, 1, -1, -1, 1, 1, 0 }, { 0, 1, 0, -1, 0, 0, -1, 0, 0, 1, 0, 1, 0, 0, 0, -1, 1, -1 }, { 0, 1, 0, 0, -1, 1, 0, 0, 0, 1, 0, 0, 0, 0, 0, -1, -1, 0 }, { 0, 0, -1, 0, 0, 0, 0, 0, 1, 0, 0, 0, -128, 0, 1, 0, -1, 0 }, { 0, -1, 0, -1, 0, 0, -1, -1, 0, -1, 1, 0, 0, -1, 0, 1, -1, 1 }, { 0, -1, 0, 0, 1, 0, 1, -128, 0, -1, 0, 0, 0, 1, 0, 1, 1, 0 }, { 0, 0, 0, 1, 0, 0, -1, 1, 0, 1, 1, -1, 0, 0, 0, 0, -1, -1 }, { 0, 0, 0, 0, 0, 0, 0, 0, -128, 0, 0, 0, 0, 0, 0, -1, -1, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, -1, 1, -1, 0, 0, -1, 0, -1, -128, 0 }, { 0, 0, -128, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 1, -1, 0, -1, 0 }, { 0, 1, 0, 0, 0, -1, 1, 1, 0, 0, -1, 1, 0, 0, 0, 0, -1, 0 }, { 0, 0, 0, 0, -1, 0, -1, 0, 0, 0, 0, 1, 0, 0, 0, 1, -1, -1 }, { 0, 0, 0, -1, 0, 0, 1, 0, 0, -1, 1, 0, 0, 0, 0, 1, 0, -1 } };
    protected static readonly int[] TIMES25 = new int[8] { 2, 2, 2, 3, 3, 3, 4, 5 };
    protected static readonly double[] LV_ACC = { 0.33, 0.36, 0.43, 0.5, 0.6, 0.75, 1, 1.33, 1.66, 2, 2.5, 2.66, 3 };
    protected static readonly int[] LV_CT = { 16, 8, 4, 3, 2, 0 };

    protected MoveE(int moveId)
    {
      this.Move = DataService.GetMoveType(moveId);
    }

    public MoveType Move
    { get; private set; }
    public virtual void Attach() { } //追击
    public abstract void Execute(PokemonProxy pm); //变化技能和攻击技能分开写吧

    protected void BuildDefContexts(AtkContext atk, params Tile[] ts)
    {
      var report = atk.Controller.ReportBuilder;
      List<DefContext> targets = new List<DefContext>();
      #region Check CoordY
      {
        if (ts.Length == 0) return;
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
            else miss.Add(def);
          }
        if (miss.Count > 0)
          report.Add(new MultiPmEvent("Miss", miss));
        if (count > 1) atk.MultiTargets = true;
      }
      #endregion
      #region Check for Immunity (or Levitate) on the Ally side, position 1, then position 3. Then check Opponent side, position 1, then 2, then 3,
      var noeffect = new List<DefContext>();
      foreach (DefContext def in targets)
      {
        CalculateEffect(def);
        if (def.EffectRevise == -128) noeffect.Add(def);
      }
      if (noeffect.Count > 0)
      {
        report.Add(new MultiPmEvent("NoEffect", noeffect));
        targets.Remove(noeffect);
      }
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
          report.Add("ProtectWork", d.Defender);
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
      if (!atk.Attacker.Ability.NoGuard() && Move.Accuracy < 0x65)
      {
        if (Move.Class != MoveInnerClass.OHKO)
        {
          if (Items.CheckMicleBerry(atk)) goto DONE;
          Abilities.AccuracyModifier(atk);
          Items.WideLens(atk);
        }
        var miss = new List<DefContext>();
        foreach (DefContext def in targets)
        {
          //心眼锁定、无防御
          if (def.NoGuard) continue;
          if (!CanHit(def)) miss.Add(def);
        }
        if (miss.Count > 0)
        {
          report.Add(new MultiPmEvent("Miss", miss));
          targets.Remove(miss);
        }
      }
    DONE:
      if (targets.Count > 0)
        atk.SetTargets(targets);
      #endregion
    }
    protected bool CanHit(DefContext def)
    {
      AtkContext atk = def.AtkContext;
      Controller controller = atk.Controller;
      int acc;
      if (Move.Class == MoveInnerClass.OHKO) //等级原因的“完全没有效果”已经判断过了
        acc = Move.Accuracy + atk.Attacker.Pokemon.Lv - def.Defender.Pokemon.Lv;
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
        acc = (int)(Move.Accuracy * LV_ACC[lv + 6]);

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

    #region
    protected virtual void CalculateType(AtkContext atk) //觉醒力量
    {
      atk.Type = Move.Type;
    }
    private sbyte CalculateEffect(DefContext def, BattleType defType)
    {
      sbyte e = BATTLE_TYPE_EFFECT[(int)def.AtkContext.Type, (int)defType];
      if (e == -128 && def.Defender.OnboardPokemon.GetCondition<BattleType>("CanAttack") == defType)
        e = 0;
      return e;
    }
    protected virtual void CalculateEffect(DefContext def)
    {
      switch (Move.Class)
      {
        case MoveInnerClass.AddState:
          def.EffectRevise = (sbyte)((CalculateEffect(def, def.Defender.OnboardPokemon.Type1) == -128 || CalculateEffect(def, def.Defender.OnboardPokemon.Type2) == -128) ? -128 : 0);
          break;
        case MoveInnerClass.Attack:
        case MoveInnerClass.AttackAndAbsorb:
        case MoveInnerClass.AttackWithSelfLv7DChange:
        case MoveInnerClass.AttackWithState:
        case MoveInnerClass.AttackWithTargetLv7DChange:
          def.EffectRevise = (sbyte)(CalculateEffect(def, def.Defender.OnboardPokemon.Type1) + CalculateEffect(def, def.Defender.OnboardPokemon.Type2));
          break;
        case MoveInnerClass.OHKO:
          def.EffectRevise =(sbyte)(
            (CalculateEffect(def, def.Defender.OnboardPokemon.Type1) == -128 || CalculateEffect(def, def.Defender.OnboardPokemon.Type2) == -128 ||
            def.Defender.Pokemon.Lv > def.AtkContext.Attacker.Pokemon.Lv ||
            def.Defender.RaiseAbility(Abilities.STURDY)) ?
            -128 : 0);
          break;
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
          targets = atk.Controller.Tiles;
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
  }
}
