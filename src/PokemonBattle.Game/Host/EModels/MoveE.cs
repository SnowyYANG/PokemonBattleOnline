using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Data;
using LightStudio.PokemonBattle.Interactive.GameEvents;

namespace LightStudio.PokemonBattle.Game
{
  public abstract class MoveE : IMoveE
  {
    protected static readonly double[,] BATTLE_TYPE_EFFECT = new double[18, 18]{ { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }, { 1, 1, 2, 1, 1, 0.5, 0.5, 0.5, 0.5, 2, 1, 1, 1, 0.5, 2, 1, 0.5, 1 }, { 1, 1, 0.5, 1, 1, 0.5, 1, 1, 2, 1, 1, 1, 1, 1, 2, 1, 0.5, 1 }, { 1, 1, 1, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0.5, 1 }, { 1, 1, 1, 0.5, 0.5, 1, 1, 2, 1, 0.5, 0, 1, 1, 1, 1, 1, 1, 2 }, { 1, 0.5, 2, 1, 1, 1, 1, 0.5, 0, 1, 1, 2, 2, 0.5, 0.5, 2, 2, 1 }, { 1, 2, 1, 0.5, 1, 1, 0.5, 1, 1, 2, 1, 2, 1, 1, 1, 0.5, 2, 0.5 }, { 1, 2, 1, 1, 0.5, 2, 1, 1, 1, 2, 1, 1, 1, 1, 1, 0.5, 0.5, 1 }, { 1, 1, 0.5, 1, 1, 1, 1, 1, 2, 1, 1, 1, 0, 1, 2, 1, 0.5, 1 }, { 1, 0.5, 1, 0.5, 1, 1, 0.5, 0.5, 1, 0.5, 2, 1, 1, 0.5, 1, 2, 0.5, 2 }, { 1, 0.5, 1, 1, 2, 1, 2, 0, 1, 0.5, 1, 1, 1, 2, 1, 2, 2, 1 }, { 1, 1, 1, 2, 1, 1, 0.5, 2, 1, 2, 2, 0.5, 1, 1, 1, 1, 0.5, 0.5 }, { 1, 1, 1, 1, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1, 1, 0.5, 0.5, 1 }, { 1, 1, 1, 1, 1, 1, 1, 1, 0.5, 2, 0.5, 1, 1, 0.5, 1, 0.5, 0, 1 }, { 1, 1, 0, 1, 1, 2, 1, 1, 1, 1, 1, 1, 1, 2, 0.5, 1, 0.5, 1 }, { 1, 2, 1, 1, 1, 0.5, 2, 2, 1, 1, 0.5, 2, 1, 1, 1, 1, 0.5, 1 }, { 1, 1, 1, 1, 0.5, 1, 0.5, 1, 1, 1, 1, 2, 1, 1, 1, 2, 0.5, 0.5 }, { 1, 1, 1, 0.5, 1, 1, 2, 1, 1, 0.5, 2, 1, 1, 1, 1, 2, 1, 0.5 } };
    protected static readonly int[] TIMES25 = new int[8] { 2, 2, 2, 3, 3, 3, 4, 5 };
    protected static readonly double[] LV_ACC = { 0.33, 0.36, 0.43, 0.5, 0.6, 0.75, 1, 1.33, 1.66, 2, 2.5, 2.66, 3 };
    protected static string ConvertMultiToString<T>(Func<T, string> convert, IList<T> args)
    {
      return GameService.Logs.ConvertMultiObjects(convert, args);
    }

    protected MoveE(MoveType moveType)
    {
      this.Move = moveType;
    }

    public MoveType Move
    { get; private set; }
    public virtual bool AvailableEvenSleeping
    { get { return false; } }

    public virtual void Attach() { } //追击new

    public abstract void Execute(PokemonProxy pm); //变化技能和攻击技能分开写吧
    protected void BuildTargets(AtkContext atk)
    {
      var report = atk.Controller.ReportBuilder;
      List<DefContext> targets;
      #region Check CoordY
      {
        var ts = GetRangeTiles(atk);
        if (ts == null) return;
        targets = new List<DefContext>();
        var miss = new List<DefContext>();
        foreach (Tile t in ts)
          if (t.Pokemon != null)
          {
            var pm = t.Pokemon;
            DefContext def = new DefContext(atk, pm);
            if (IsYInRange(def) || def.NoGuard) targets.Add(def);
            else miss.Add(def);
          }
        if (miss.Count > 0)
        {
          report.Add(new SoloEvent("Miss", ConvertMultiToString((d) => d.Defender.Outward.Name, miss)));
          targets.Remove(miss);
        }
        else if (targets.Count == 0)
        {
          report.Add(new SimpleEvent("Fail"));
        }
      }
      #endregion
      #region Check for Immunity (or Levitate) on the Ally side, position 1, then position 3. Then check Opponent side, position 1, then 2, then 3,
      var noeffect = new List<DefContext>();
      foreach (DefContext def in targets)
      {
        CalculateEffect(def);
        if (def.BattleTypeRevise == 0) noeffect.Add(def);
        else targets.Add(def);
      }
      if (noeffect.Count > 0)
      {
        report.Add(new SoloEvent("NoEffect", GameService.Logs.ConvertMultiObjects((d) => d.Defender.Outward.Name, noeffect)));
        targets.Remove(noeffect);
      }
      #endregion
      #region [unfinished] Check for Wide Guard in same way
      #endregion
      #region Check for Protect
      if (Move.AdvancedFlags.Protectable)
      {
        var protect = new List<DefContext>();
        foreach (DefContext d in targets)
          if (d.Defender.OnboardPokemon.HasCondition("Protect")) protect.Add(d);
        if (protect.Count > 0)
        {
          report.Add(new SoloEvent("ProtectSelf", GameService.Logs.ConvertMultiObjects((d) => d.Defender.Outward.Name, protect)));
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
        if (SpItems.CheckMicleBerry(atk)) return;
        atk.Attacker.Ability.CalculatingAccuracy(atk); //计算攻击方命中修正
        atk.Attacker.Item.CalculatingAccuracy(atk);
        var miss = new List<DefContext>();
        foreach (DefContext def in targets)
        {
          //心眼锁定、无防御
          if (def.NoGuard) continue;
          if (!CanHit(def)) miss.Add(def);
        }
        if (miss.Count > 0)
        {
          report.Add(new SoloEvent("Miss", ConvertMultiToString((d) => d.Defender.Outward.Name, miss)));
          targets.Remove(miss);
        }
      }
      #endregion
    }

    #region
    protected virtual void CalculateType(AtkContext atk)
    {
      atk.Type = Move.Type;
      atk.Attacker.Ability.CalculatingMoveType(ref atk.Type);
    }
    protected virtual void CalculateEffect(DefContext def)
    {
      switch (Move.Class)
      {
        case MoveInnerClass.AddState:
        case MoveInnerClass.Attack:
        case MoveInnerClass.AttackAndAbsorb:
        case MoveInnerClass.AttackWithSelfLv7DChange:
        case MoveInnerClass.AttackWithState:
        case MoveInnerClass.AttackWithTargetLv7DChange:
          def.BattleTypeRevise = BATTLE_TYPE_EFFECT[(int)def.Defender.OnboardPokemon.Type1, (int)def.AtkContext.Type] * BATTLE_TYPE_EFFECT[(int)def.Defender.OnboardPokemon.Type2, (int)def.AtkContext.Type];
          break;
        case MoveInnerClass.OHKO:
          if (def.Defender.Pokemon.Lv > def.AtkContext.Attacker.Pokemon.Lv) def.BattleTypeRevise = 0;
          else def.BattleTypeRevise = BATTLE_TYPE_EFFECT[(int)def.Defender.OnboardPokemon.Type1, (int)def.AtkContext.Type] * BATTLE_TYPE_EFFECT[(int)def.Defender.OnboardPokemon.Type2, (int)def.AtkContext.Type];
          break;
        default:
          def.BattleTypeRevise = 1;
          break;
      }
    }
    protected virtual IEnumerable<Tile> GetRangeTiles(AtkContext atk)
    {
      Tile select = atk.Attacker.SelectedTarget;
      IEnumerable<Tile> targets = null;
      Controller controller = atk.Controller;
      int team = atk.Attacker.Tile.Team;
      int rTeam = 1 - team;
      int x = atk.Attacker.Tile.X;
      switch (Move.Range)
      {
        case MoveRange.UserField: //do nothing
        case MoveRange.EnemyField: //do nothing
        case MoveRange.Field: //do nothing
        case MoveRange.Varies: //override
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
          if (select == null) goto case MoveRange.RandomEnemy; //非鬼系选诅咒后变诅咒随机对方一个精灵
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
      }
      return targets;
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
        //将攻击方的命中等级减去防御方的回避等级。 
        if (atk.Attacker.Ability.Unaware())
          lv -= def.Defender.OnboardPokemon.EvasionLv;
        if (lv < -6) lv = -6;
        else if (lv > 6) lv = 6;
        //用技能基础命中乘以命中等级修正，向下取整。
        acc = (int)(Move.Accuracy * LV_ACC[lv + 6]);

        def.Ability.CalculatingAccuracy(def);
        def.Defender.Item.CalculatingAccuracy(def);
        acc = (int)(acc * atk.AccuracyRevise);
        acc = (int)(acc * def.AccuracyRevise);
        
        //如果场上存在重力，命中×5/3。
        if (controller.Board.HasCondition("Gravity"))
        {
          acc *= 5;
          acc /= 3;
        }
      }
      //产生1～100的随机数，如果小于等于命中，判定为命中，否则判定为失误。
      return controller.GetRandomInt(1, 100) <= acc;
    }
    protected virtual bool IsYInRange(DefContext def)
    {
      return def.Defender.Position.Y == CoordY.Plate;
    }
    #endregion
  }
}
