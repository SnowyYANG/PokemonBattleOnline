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
        case MoveRange.SelfField: //do nothing
        case MoveRange.FoeField: //do nothing
        case MoveRange.Board: //do nothing
        case MoveRange.SelfPokemons: //防音防不住治愈铃铛，所以这只是个摆设
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
        case MoveRange.FoePokemons:
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
        case MoveRange.SingleAlly:
          if (select == null) goto case MoveRange.RandomSelfPokemon;
          else targets = new Tile[] { select };
          break;
        case MoveRange.RandomFoePokemon:
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
          goto case MoveRange.SingleFoe;
        case MoveRange.SingleFoe:
          if (select == null || (!remote && (select.X < x - 1 || select.X > x + 1)))
            goto case MoveRange.RandomFoePokemon; //非鬼系选诅咒后变诅咒随机对方一个精灵
          targets = new Tile[] { select };
          break;
        case MoveRange.Self: //done?
          targets = new Tile[] { aer.Tile };
          break;
        case MoveRange.RandomSelfPokemon:
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
    /// <summary>
    /// check canwithdraw first, null log to show nothing
    /// </summary>
    /// <param name="pm"></param>
    /// <param name="ability"></param>
    /// <param name="log"></param>
    /// <param name="arg1"></param>
    /// <returns></returns>
    public static bool ForceSwitchImplement(PokemonProxy pm, bool ability)
    {
      if (ability && pm.RaiseAbility(As.SUCTION_CUPS))
      {
        pm.ShowLogPm("SuctionCups");
        return false;
      }
      if (pm.OnboardPokemon.HasCondition("Ingrain"))
      {
        pm.ShowLogPm("IngrainCantMove");
        return false;
      }
      var c = pm.Controller;
      var sendouts = new List<int>();
      {
        var pms = pm.Pokemon.Owner.Pokemons.ToArray();
        for (int i = pm.Controller.GameSettings.Mode.OnboardPokemonsPerPlayer(); i < pms.Length; ++i)
          if (c.CanSendOut(pms[i])) sendouts.Add(i);
      }
      var tile = pm.Tile;
      c.Withdraw(pm, "forcewithdraw", 0, false);
      tile.WillSendOutPokemonIndex = sendouts[c.GetRandomInt(0, sendouts.Count - 1)];
      c.SendOut(tile, true, "ForceSendOut");
      return true;
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
            if (!targets.Any()) atk.Attacker.ShowLogPm("UseMove", Ms.BIDE); //奇葩的战报
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
      var aer = atk.Attacker;

      #region Check CoordY
      {
        var count = 0;
        foreach (DefContext def in targets.ToArray())
        {
          ++count;
          if (!(def.Defender.CoordY == CoordY.Plate || def.NoGuard))
          {
            def.Defender.ShowLogPm("Miss");
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
          def.Defender.ShowLogPm("NoEffect");
        }
      #endregion
      #region WideGuard QuickGuard CraftyShield MatBlock
      if (move.Category != MoveCategory.Status && move.Range != MoveRange.Single)
        foreach (var def in targets.ToArray())
          if (def.Defender.Field.HasCondition("WideGuard"))
          {
            def.Defender.ShowLogPm("WideGuard");
            targets.Remove(def);
          }
      if (move.Priority > 0 && move.Id != Ms.FEINT)
        foreach (var def in targets.ToArray())
          if (def.Defender.Field.HasCondition("QuickGuard"))
          {
            def.Defender.ShowLogPm("QuickGuard");
            targets.Remove(def);
          }
      if (move.Category == MoveCategory.Status)
      {
        foreach (var def in targets.ToArray())
          if (def.Defender.Field.HasCondition("CraftyShield"))
          {
            def.Defender.ShowLogPm("CraftyShield");
            targets.Remove(def);
          }
      }
      else
      {
        var d0 = targets.FirstOrDefault();
        if (d0 != null && d0.Defender.Field.HasCondition("MatBlock"))
        {
          d0.Defender.Controller.ReportBuilder.ShowLog("MatBlock", move.Id);
          var td = d0.Defender.Pokemon.TeamId;
          foreach (var d in targets.ToArray())
            if (d.Defender.Pokemon.TeamId == td) targets.Remove(d);
          d0 = targets.FirstOrDefault();
          if (d0 != null && d0.Defender.Field.HasCondition("MatBlock")) targets.Clear();
        }
      }
      #endregion
      #region Protect KingsShield SpikyShield
      if (move.Flags.Protectable)
      {
        foreach (DefContext d in targets.ToArray())
          if (d.Defender.OnboardPokemon.HasCondition("Protect"))
          {
            d.Defender.ShowLogPm("Protect");
            targets.Remove(d);
          }
      }
      if (move.Category != MoveCategory.Status)
      {
        foreach(var d in targets.ToArray())
          if (d.Defender.OnboardPokemon.HasCondition("SpikyShield"))
          {
            d.Defender.ShowLogPm("Protect");
            if (move.Flags.NeedTouch) aer.EffectHurtByOneNth(8);
            targets.Remove(d);
          }
        foreach(var d in targets.ToArray())
          if (d.Defender.OnboardPokemon.HasCondition("KingsShield"))
          {
            d.Defender.ShowLogPm("Protect");
            if (move.Flags.NeedTouch) aer.ChangeLv7D(d.Defender, StatType.Atk, -2, false);
            targets.Remove(d);
          }
      }
      #endregion
      #region Check for Telepathy (and possibly other abilities)
      {
        var mc = move.Flags.MagicCoat && !atk.HasCondition("IgnoreMagicCoat");
        var ab = !ATs.IgnoreDefenderAbility(aer.Ability);
        foreach (DefContext def in targets.ToArray())
          if (def.Defender != atk.Attacker && (mc && STs.MagicCoat(atk, def.Defender) || ab && !CanImplement.Execute(def))) targets.Remove(def);
      }
      #endregion
      if (move.Category == MoveCategory.Status && !atk.IgnoreSubstitute())
        foreach (DefContext d in targets.ToArray())
          if (d.Defender != aer && d.Defender.OnboardPokemon.HasCondition("Substitute"))
          {
            d.Fail();
            targets.Remove(d);
          }
      #region Check for misses
      if (!(MustHit(atk) || aer.Ability == As.NO_GUARD))
      {
        if (move.Class != MoveInnerClass.OHKO) atk.AccuracyModifier = STs.AccuracyModifier(atk);
        foreach (DefContext def in targets.ToArray())
          if (!(def.NoGuard || CanHit(def)))//心眼锁定、无防御
          {
            targets.Remove(def);
            def.Defender.ShowLogPm("Miss");
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
      Controller c = atk.Controller;
      var move = atk.Move;
      int acc;
      if (move.Class == MoveInnerClass.OHKO) acc = move.Accuracy + atk.Attacker.Pokemon.Lv - def.Defender.Pokemon.Lv;
      else
      {
        int lv;
        if (def.Ability == As.UNAWARE) lv = 0;
        else lv = def.AtkContext.Attacker.OnboardPokemon.AccuracyLv;
        //如果攻击方是天然特性，防御方的回避等级按0计算。 
        //循序渐进无视防御方回避等级。
        //将攻击方的命中等级减去防御方的回避等级。 
        if (!move.IgnoreDefenderLv7D())
        {
          var aa = atk.Attacker.Ability;
          if (aa == As.UNAWARE || aa == As.KEEN_EYE) lv -= def.Defender.OnboardPokemon.EvasionLv;
        }
        if (lv < -6) lv = -6;
        else if (lv > 6) lv = 6;
        //用技能基础命中乘以命中等级修正，向下取整。
        int numerator = 3, denominator = 3;
        if (lv > 0) numerator += lv;
        else denominator -= lv;
        acc = (c.Weather == Weather.IntenseSunlight && (move.Id == Ms.THUNDER || move.Id == Ms.HURRICANE) ? 50 : atk.Move.Accuracy) * numerator / denominator;
        acc *= AccuracyModifier.Execute(def);
      }
      //产生1～100的随机数，如果小于等于命中，判定为命中，否则判定为失误。
      return c.RandomHappen(acc);
    }
    public static bool MustHit(AtkContext atk)
    {
      var m = atk.Move.Id;
      return atk.Move.Accuracy == 0 || (m == Ms.THUNDER || m == Ms.HURRICANE) && atk.Controller.Weather == Weather.HeavyRain || m == Ms.BLIZZARD && atk.Controller.Weather == Weather.Hailstorm || atk.Attacker.OnboardPokemon.HasType(BattleType.Poison) && m == Ms.TOXIC;
    }
    #endregion

    public static void MoveEnding(AtkContext atk)
    {
      var aer = atk.Attacker;

      if (atk.Move.Id == Ms.SPIT_UP || atk.Move.Id == Ms.SWALLOW)
      {
        int i = aer.OnboardPokemon.GetCondition<int>("Stockpile");
        aer.ChangeLv7D(atk.Attacker, false, false, 0, -i, 0, -i);
        aer.OnboardPokemon.RemoveCondition("Stockpile");
        aer.ShowLogPm("DeStockpile");
      }

      MagicCoat(atk);

      atk.SetAttackerAction(atk.Move.StiffOneTurn() ? PokemonAction.Stiff : PokemonAction.Done);
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
          c.Withdraw(aer, "SelfWithdraw", 0, true);
          c.PauseForSendOutInput(tile);
        }
      }
    }
  }
}
