using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonBattleOnline.Game;
using PokemonBattleOnline.Game.GameEvents;
using PokemonBattleOnline.Game.Host.Triggers;

namespace PokemonBattleOnline.Game.Host
{
  internal static class MoveE
  {
    public static void MagicCoat(AtkContext atk)
    {
      var list = atk.GetCondition<List<PokemonProxy>>("MagicCoat");
      if (list != null)
      {
        atk.RemoveCondition("MagicCoat");
        foreach (var d in list)
        {
          var a = new AtkContext(d);
          a.SetCondition("IgnoreMagicCoat");
          a.StartExecute(atk.Move, atk.Attacker.Tile, d.RaiseAbility(As.MAGIC_BOUNCE) ? "MagicBounce" : "MagicCoat");
          if (atk.Target == null) break;
        }
      }
    }
    
    public static IEnumerable<Tile> GetRangeTiles(AtkContext atk, MoveRange range, Tile select)
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
    public static bool CanForceSwitch(PokemonProxy pm, bool abilityAvailable)
    {
      return !(pm.Hp == 0 || !pm.Controller.CanWithdraw(pm) || abilityAvailable && pm.Ability == As.SUCTION_CUPS || pm.OnboardPokemon.HasCondition("Ingrain"));
    }
    public static void ForceSwitchImplement(PokemonProxy pm, string log = "ForceWithdraw")
    {
      var c = pm.Controller;
      var sendouts = new List<int>();
      {
        var pms = pm.Pokemon.Owner.Pokemons.ToArray();
        for (int i = pm.Controller.GameSettings.Mode.OnboardPokemonsPerPlayer(); i < pms.Length; ++i)
          if (c.CanSendOut(pms[i])) sendouts.Add(i);
      }
      var tile = pm.Tile;
      c.Withdraw(pm, log, false);
      tile.WillSendOutPokemonIndex = sendouts[c.GetRandomInt(0, sendouts.Count - 1)];
      c.SendOut(tile, true, "ForceSendOut");
    }
    public static bool ForceSwitch(DefContext def)
    {
      if (CanForceSwitch(def.Defender, ATs.IgnoreDefenderAbility(def.AtkContext.Attacker.Ability)))
      {
        ForceSwitchImplement(def.Defender);
        return true;
      }
      return false;
    }

    public static void BuildDefContext(AtkContext atk, Tile select)
    {
      switch (atk.Move.Id)
      {
        case Ms.COUNTER: //68
          Counter(atk, "PhysicalDamage");
          break;
        case Ms.MIRROR_COAT: //243
          Counter(atk, "SpecialDamage");
          break;
        case Ms.METAL_BURST: //368
          Counter(atk, "Damage");
          break;
        case Ms.BIDE:
          if (atk.GetCondition("MultiTurn").Turn == 1)
          {
            var o = atk.GetCondition("Bide");
            var targets = new List<DefContext>();
            if (o.By != null)
            {
              var t = GetRangeTiles(atk, MoveRange.Single, o.By.Tile).FirstOrDefault();
              if (t != null && t.Pokemon != null) targets.Add(new DefContext(atk, t.Pokemon));
            }
            if (!targets.Any()) atk.Attacker.AddReportPm("UseMove", Ms.BIDE); //奇葩的战报
            atk.SetTargets(targets);
          }
          break;
        default:
          IEnumerable<Tile> ts = GetRangeTiles(atk, MTs.GetRange(atk.Attacker, atk.Move), select);
          if (ts != null)
          {
            var targets = new List<DefContext>();
            foreach (Tile t in ts)
              if (t.Pokemon != null) targets.Add(new DefContext(atk, t.Pokemon));
            atk.SetTargets(targets);
          }
          break;
      }
    }
    private static void Counter(AtkContext atk, string condition)
    {
      var o = atk.Attacker.OnboardPokemon.GetCondition(condition);
      if (o != null)
      {
        var pm = o.By;
        if (pm.Tile != null && pm.Pokemon.TeamId != atk.Attacker.Pokemon.TeamId)
        {
          atk.SetTargets(new DefContext[] { new DefContext(atk, pm) });
          return;
        }
      }
      atk.SetTargets(new DefContext[0]);
    }

    #region CalculateTargets
    public static void FilterDefContext(AtkContext atk)
    {
      if ((atk.Move.Id == Ms.FUTURE_SIGHT || atk.Move.Id == Ms.DOOM_DESIRE) && !atk.HasCondition("FSDD")) return;
      if (atk.Targets == null) return;
      ATs.ReTarget(atk);
      List<DefContext> targets = atk.Targets.ToList();
      var move = atk.Move;


      #region Check CoordY
      {
        var count = 0;
        foreach (DefContext def in targets.ToArray())
        {
          ++count;
          if (!(def.Defender.CoordY == CoordY.Plate || def.NoGuard))
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
        if (!HasEffect.Execute(def))
        {
          targets.Remove(def);
          def.Defender.AddReportPm("NoEffect");
        }
      #endregion
      #region Check for Wide Guard and Quick Guard in same way
      if (move.Category != MoveCategory.Status && move.Range != MoveRange.Single)
        foreach (var def in targets.ToArray())
          if (def.Defender.Tile.Field.HasCondition("WideGuard"))
          {
            def.Defender.AddReportPm("WideGuard");
            targets.Remove(def);
          }
      if (move.Priority > 0 && move.Id != Ms.FEINT)
        foreach (var def in targets.ToArray())
          if (def.Defender.Tile.Field.HasCondition("QuickGuard"))
          {
            def.Defender.AddReportPm("QuickGuard");
            targets.Remove(def);
          }
      #endregion
      #region Check for Protect
      if (move.Flags.Protectable)
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
        var mc = move.Flags.MagicCoat && !atk.HasCondition("IgnoreMagicCoat");
        var ab = !ATs.IgnoreDefenderAbility(atk.Attacker.Ability);
        foreach (DefContext def in targets.ToArray())
          if (def.Defender != atk.Attacker && (mc && STs.MagicCoat(atk, def.Defender) || ab && !CanImplement.Execute(def))) targets.Remove(def);
      }
      #endregion
      if (move.Category == MoveCategory.Status && !move.Flags.IgnoreSubstitute)
        foreach (DefContext d in targets.ToArray())
          if (d.Defender != atk.Attacker && d.Defender.OnboardPokemon.HasCondition("Substitute"))
          {
            d.Fail();
            targets.Remove(d);
          }
      if (move.Class == MoveInnerClass.ForceToSwitch)
        foreach (DefContext d in targets.ToArray())
          if (d.Defender.OnboardPokemon.HasCondition("Ingrain"))
          {
            d.Defender.AddReportPm("IngrainCantMove");
            targets.Remove(d);
          }
      #region Check for misses
      if (atk.Attacker.Ability != As.NO_GUARD && GetAccuracyBase(atk) != 0x65)
      {
        if (move.Class != MoveInnerClass.OHKO) atk.AccuracyModifier = STs.AccuracyModifier(atk);
        foreach (DefContext def in targets.ToArray())
          if (!(def.NoGuard || CanHit(def)))//心眼锁定、无防御
          {
            targets.Remove(def);
            def.Defender.AddReportPm("Miss");
          }
      }
      #endregion
      atk.SetTargets(targets);
    }
    private static bool IsYInRange(DefContext def)
    {
      var y = def.Defender.CoordY;
      var m = def.AtkContext.Move.Id;
      return
        y == CoordY.Plate ||
        y == CoordY.Water && (m == Ms.SURF || m == Ms.WHIRLPOOL) ||
        y == CoordY.Underground && (m == Ms.EARTHQUAKE || m == Ms.FISSURE) ||
        y == CoordY.Air && (m == Ms.GUST || m == Ms.TWISTER || m == Ms.THUNDER || m == Ms.HURRICANE || m == Ms.SKY_UPPERCUT);
    }
    public static bool CanHit(DefContext def)
    {
      AtkContext atk = def.AtkContext;
      Controller controller = atk.Controller;
      int acc;
      if (atk.Move.Class == MoveInnerClass.OHKO) //等级原因的“完全没有效果”已经判断过了
        acc = GetAccuracyBase(atk) + atk.Attacker.Pokemon.Lv - def.Defender.Pokemon.Lv;
      else
      {
        int lv;
        if (def.Ability == As.UNAWARE) lv = 0;
        else lv = def.AtkContext.Attacker.OnboardPokemon.AccuracyLv;
        //如果攻击方是天然特性，防御方的回避等级按0计算。 
        //循序渐进无视防御方回避等级。
        //将攻击方的命中等级减去防御方的回避等级。 
        if (!(atk.Attacker.Ability == As.UNAWARE || atk.Move.IgnoreDefenderLv7D())) lv -= def.Defender.OnboardPokemon.EvasionLv;
        if (lv < -6) lv = -6;
        else if (lv > 6) lv = 6;
        //用技能基础命中乘以命中等级修正，向下取整。
        int numerator = 3, denominator = 3;
        if (lv > 0) numerator += lv;
        else denominator -= lv;
        acc = (GetAccuracyBase(atk) * numerator / denominator) * AccuracyModifier.Execute(def);
      }
      //产生1～100的随机数，如果小于等于命中，判定为命中，否则判定为失误。
      return controller.RandomHappen(acc);
    }
    public static int GetAccuracyBase(AtkContext atk)
    {
      var m = atk.Move.Id;
      var w = atk.Controller.Weather;
      bool thunder = m == Ms.THUNDER || m == Ms.HURRICANE;
      return w == Weather.HeavyRain && thunder || w == Weather.Hailstorm && m == Ms.BLIZZARD ? 0x65 : w == Weather.IntenseSunlight && thunder ? 50 : atk.Move.Accuracy;
    }
    #endregion

    public static void MoveEnding(AtkContext atk)
    {
      var aer = atk.Attacker;

      if (atk.Move.Id == Ms.SPIT_UP || atk.Move.Id == Ms.SWALLOW)
      {
        int i = aer.OnboardPokemon.GetCondition<int>("Stockpile");
        aer.ChangeLv7D(atk.Attacker, false, 0, -i, 0, -i);
        aer.OnboardPokemon.RemoveCondition("Stockpile");
        aer.AddReportPm("DeStockpile");
      }

      MagicCoat(atk);

      atk.SetAttackerAction(atk.Move.Flags.StiffOneTurn ? PokemonAction.Stiff : PokemonAction.Done);
      if (atk.Targets != null)
        foreach (var d in atk.Targets)
        {
          ITs.Attach(d.Defender);
          ATs.RecoverAfterMoldBreaker(d.Defender);
        }
      ITs.Attach(atk.Attacker); //先树果汁后PP果

      var c = aer.Controller;
      {
        var o = atk.GetCondition("MultiTurn");
        if (o != null)
        {
          o.Turn--;
          if (o.Turn != 0) atk.SetAttackerAction(PokemonAction.Moving);
          else if (o.Bool) aer.AddState(aer, AttachedState.Confuse, false, 0, "EnConfuse2");
        }
      }
      {
        var o = atk.GetCondition<Tile>("EjectButton");
        if (o != null)
        {
          c.PauseForSendOutInput(o);
          return;
        }
      }
      {
        var tile = aer.Tile;
        if (atk.Move.Switch() && tile != null)
        {
          c.Withdraw(aer, "SelfWithdraw", true);
          c.PauseForSendOutInput(tile);
        }
      }
    }
  }
}
