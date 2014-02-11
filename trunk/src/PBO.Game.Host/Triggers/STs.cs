using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonBattleOnline.Game;
using PokemonBattleOnline.Game.GameEvents;

namespace PokemonBattleOnline.Game.Host.Triggers
{
  internal static class STs
  {
    public static PokemonProxy HasAbility(this IEnumerable<PokemonProxy> pms, int ability)
    {
      foreach (var p in pms)
        if (p.AbilityE(ability)) return p;
      return null;
    }
    public static PokemonProxy RaiseAbility(this IEnumerable<PokemonProxy> pms, int ability)
    {
      foreach (var p in pms)
        if (p.RaiseAbility(ability)) return p;
      return null;
    }
    #region gen5
    public static void KOed(DefContext def, OnboardPokemon o)
    {
      var der = def.Defender;
      var aer = def.AtkContext.Attacker;
      if (o.HasCondition("DestinyBond"))
      {
        aer.ShowLogPm("DestinyBond"); //战报顺序已测
        aer.Faint();
      }
      var mp = def.AtkContext.MoveProxy;
      if (o.HasCondition("Grudge") && mp != null && mp.PP != 0)
      {
        mp.PP = 0;
        aer.ShowLogPm("Grudge");
      }
      if (def.AtkContext.Move.Id == Ms.FELL_STINGER) aer.ChangeLv7D(aer, StatType.Atk, 2, false);
      if (aer.AbilityE(As.MOXIE)) aer.ChangeLv7D(aer, StatType.Atk, 1, false, true);
    }
    public static void WillAct(PokemonProxy pm)
    {
      pm.OnboardPokemon.RemoveCondition("DestinyBond");
      pm.OnboardPokemon.RemoveCondition("Grudge");
      pm.OnboardPokemon.RemoveCondition("Rage");
      var i = pm.OnboardPokemon.GetCondition<int>("Taunt");
      if (i != 0) pm.OnboardPokemon.SetCondition("Taunt", i - 1);
      var o = pm.OnboardPokemon.GetCondition("Encore");
      if (o != null) o.Turn--;
    }
    public static void SendingOut(PokemonProxy pm)
    {
      pm.Reset();
      var o = pm.OnboardPokemon;
      if (pm.State == PokemonState.BadlyPSN) o.SetCondition("PSN", pm.Controller.TurnNumber);
      var pass = pm.Tile.GetCondition<OnboardPokemon>("BatonPass");
      if (pass != null)
      {
        o.SetLv7D(pass.Lv5D.Atk, pass.Lv5D.Def, pass.Lv5D.SpAtk, pass.Lv5D.SpDef, pass.Lv5D.Speed, pass.AccuracyLv, pass.EvasionLv);
        pm.Tile.RemoveCondition("BatonPass");
        object c;
        //混乱状态 
        c = pass.GetCondition<object>("Confuse");
        if (c != null) o.SetCondition("Confuse", c);
        //寄生种子状态 
        c = pass.GetCondition<object>("LeechSeed");
        if (c != null) o.SetCondition("LeechSeed", c);
        //扣押状态
        c = pass.GetCondition<object>("Embargo");
        if (c != null) o.SetCondition("Embargo", c);
        //回复封印状态 
        c = pass.GetCondition<object>("HealBlock");
        if (c != null) o.SetCondition("HealBlock", c);
        //念动力状态
        c = pass.GetCondition<object>("Telekinesis");
        if (c != null) o.SetCondition("Telekinesis", c);
        //胃液状态
        if (pass.HasCondition("GastroAcid")) o.SetCondition("GastroAcid");
        //扎根状态
        if (pass.HasCondition("Ingrain")) o.SetCondition("Ingrain");
        //液态圈状态
        if (pass.HasCondition("AquaRing")) o.SetCondition("AquaRing");
        //蓄气状态 
        if (pass.HasCondition("FocusEnergy")) o.SetCondition("FocusEnergy");
        //替身状态
        c = pass.GetCondition<object>("Substitute");
        if (c != null) o.SetCondition("Substitute", c);
        //电磁浮游状态
        c = pass.GetCondition<object>("MagnetRise");
        if (c != null) o.SetCondition("MagnetRise", c);
        //灭亡之歌状态
        c = pass.GetCondition<object>("PerishSong");
        if (c != null) o.SetCondition("PerishSong", c);
      }
      ATs.Illusion(pm);//幻影特性以交换前的队伍顺序决定
    }
    public static void Withdrawing(PokemonProxy pm, bool canPursuit)
    {
      if (canPursuit && pm.Hp != 0)
      {
        var tile = pm.Tile;
        foreach (var p in pm.Controller.Board[1 - tile.Team].GetPokemons(tile.X - 1, tile.X + 1).ToArray())
          if (p.SelectedMove != null && p.SelectedMove.Type.Id == Ms.PURSUIT && p.CanMove)
          {
            p.OnboardPokemon.SetCondition("Pursuiting");
            p.Move();
            p.OnboardPokemon.RemoveCondition("Pursuiting");
            if (pm.Hp == 0) return;
          }
      }
      foreach (var p in pm.Controller.OnboardPokemons)
        if (p != pm)
        {
          var op = p.OnboardPokemon;
          {
            var o = op.GetCondition("CanAttack");
            if (o != null && o.By == pm) op.RemoveCondition("CanAttack");
          }
          {
            if (op.GetCondition<PokemonProxy>("CantSelectWithdraw") == pm) op.RemoveCondition("CantSelectWithdraw");
          }
          {
            var o = op.GetCondition("HealBlock");
            if (o != null && o.By == pm) op.RemoveCondition("HealBlock");
          }
          {
            if (op.GetCondition<PokemonProxy>("Attract") == pm) op.RemoveCondition("Attract");
          }
          {
            if (op.GetCondition<PokemonProxy>("Torment") == pm) op.RemoveCondition("Torment");
          }
          {
            var o = op.GetCondition("Trap");
            if (o != null && o.By == pm) op.RemoveCondition("Trap");
          }
        }
    }
    public static bool SetWeather(PokemonProxy pm, Weather weather, bool ability)
    {
      var c = pm.Controller;
      if (c.Board.Weather == weather) return false;
      c.Board.Weather = weather;
      if (ability) pm.RaiseAbility();
      c.ReportBuilder.ShowWeather(c);
      if (!ATs.IgnoreWeather(c)) ATs.WeatherChanged(c);
      c.Board.SetCondition("Weather", (c.TurnNumber == 0 ? 1 : c.TurnNumber) + (pm.ItemE(weather.Item()) ? 7 : 4));
      return true;
    }
    public static bool Remaining1HP(PokemonProxy pm, bool ability)
    {
      if (pm.OnboardPokemon.HasCondition("Endure"))
      {
        pm.ShowLogPm("Endure");
        return true;
      }
      if (ability && pm.Hp == pm.Pokemon.MaxHp && pm.RaiseAbility(As.STURDY))
      {
        pm.ShowLogPm("Endure");
        return true;
      }
      if (pm.ItemE(Is.FOCUS_BAND) && pm.Controller.OneNth(10) || pm.ItemE(Is.FOCUS_SASH) && pm.Hp == pm.Pokemon.MaxHp)
      {
        pm.ShowLogPm("FocusItem", pm.Pokemon.Item);
        if (pm.Pokemon.Item == Is.FOCUS_SASH) pm.ConsumeItem();
        return true;
      }
      return false;
    }
    public static bool MagicCoat(AtkContext atk, PokemonProxy der)
    {
      //atk.Move.AdvancedFlags.MagicCoat is already checked
      if (der.OnboardPokemon.HasCondition("MagicCoat") || !atk.IgnoreDefenderAbility() && der.AbilityE(As.MAGIC_BOUNCE))
      {
        var o = atk.GetCondition<List<PokemonProxy>>("MagicCoat");
        if (o == null)
        {
          o = new List<PokemonProxy>();
          atk.SetCondition("MagicCoat", o);
        }
        o.Add(der);
        return true;
      }
      return false;
    }
    public static bool CanExecuteMove(PokemonProxy pm, MoveType move)
    {
      //重力
      if (move.UnavailableWithGravity() && pm.Controller.Board.HasCondition("Gravity"))
      {
        pm.ShowLogPm("GravityCantUseMove", move.Id);
        return false;
      }
      //回复封印
      if (move.Heal() && pm.OnboardPokemon.HasCondition("HealBlock"))
      {
        pm.ShowLogPm("HealBlockCantUseMove", move.Id);
        return false;
      }
      return true;
    }
    public static Modifier AccuracyModifier(AtkContext atk)
    {
      var aer = atk.Attacker;
      int ab = aer.Ability;
      Modifier m = (Modifier)(ab == As.COMPOUNDEYES ? 0x14cc : ab == As.HUSTLE && atk.Move.Category == MoveCategory.Physical ? 0xccc : 0x1000);
      foreach (PokemonProxy pm in atk.Controller.GetOnboardPokemons(aer.Pokemon.TeamId))
        if (pm.AbilityE(As.VICTORY_STAR)) m *= 0x1199;
      if (aer.OnboardPokemon.RemoveCondition("MicleBerry")) m *= 0x1199;
      if (aer.ItemE(Is.WIDE_LENS)) m *= 0x1199;
      return m;
    }
    #endregion
    public static void Lv7DDown(PokemonProxy pm)
    {
      switch (pm.Ability)
      {
        case As.DEFIANT:
          pm.ChangeLv7D(pm, StatType.Atk, 2, false, true);
          break;
        case As.COMPETITIVE:
          pm.ChangeLv7D(pm, StatType.SpAtk, 2, false, true);
          break;
      }
    }
    public static bool TypeCalculated(AtkContext atk)
    {
      var aer = atk.Attacker;
      if (atk.Type == BattleType.Fire && aer.OnboardPokemon.HasCondition("Powder"))
      {
        aer.EffectHurtByOneNth(4, "m_Powder");
        return true;
      }
      if (aer.AbilityE(As.PROTEAN) && aer.OnboardPokemon.SetTypes(atk.Type))
      {
        aer.RaiseAbility();
        aer.ShowLogPm("TypeChange", (int)atk.Type);
      }
      return false;
    }
  }
  internal static class SubstituteTriggers
  {
    public static bool IgnoreSubstitute(this AtkContext atk)
    {
      return atk.Move.IgnoreSubstitute() || atk.Attacker.AbilityE(As.INFILTRATOR);
    }
    private static int Generic(DefContext def)
    {
      int hp = def.Defender.OnboardPokemon.GetCondition<int>("Substitute");
      def.HitSubstitute = hp > 0;
      return hp;
    }
    private static void Disappear(PokemonProxy pm)
    {
      pm.Controller.ReportBuilder.DeSubstitute(pm);
      pm.OnboardPokemon.RemoveCondition("Substitute");
    }
    public static bool Hurt(DefContext def)
    {
      int hp = Generic(def);
      if (hp > 0)
      {
        Controller c = def.Defender.Controller;
        if (def.Damage > hp) def.Damage = hp;
        hp -= def.Damage;
        def.Defender.ShowLogPm("HurtSubstitute");
        if (def.EffectRevise > 0) c.ReportBuilder.ShowLog("SuperHurt0");
        else if (def.EffectRevise < 0) c.ReportBuilder.ShowLog("WeakHurt0");
        if (def.IsCt) c.ReportBuilder.ShowLog("CT0");
        if (def.Defender.ItemE(Is.AIR_BALLOON)) ITs.AirBalloon(def);
        if (hp == 0) Disappear(def.Defender);
        else def.Defender.OnboardPokemon.SetCondition("Substitute", hp);
        return true;
      }
      return false;
    }
    public static bool OHKO(DefContext def)
    {
      int hp = Generic(def);
      if (hp > 0)
      {
        def.Damage = hp;
        Disappear(def.Defender);
        return true;
      }
      return false;
    }
  }
  internal static class EHTs
  {
    private static List<Condition> GetHazards(Field field)
    {
      return field.GetCondition<List<Condition>>("Hazards");
    }
    public static bool En(Field field, MoveType move)
    {
      var hazards = GetHazards(field);
      if (hazards == null)
      {
        hazards = new List<Condition>();
        field.AddCondition("Hazards", hazards);
      }
      foreach (var eh in hazards)
        if (eh.Move == move) return En(eh);
      var newh = new Condition() { Move = move };
      if (move.Id == Ms.SPIKES) newh.Int = 8;
      hazards.Add(newh);
      return true;
    }
    private static bool En(Condition hazard)
    {
      switch (hazard.Move.Id)
      {
        case Ms.SPIKES:
          if (hazard.Int == 4) return false;
          hazard.Int = hazard.Int == 8 ? 6 : 4;
          return true;
        case Ms.TOXIC_SPIKES:
          if (hazard.Bool) return false;
          hazard.Bool = true;
          return true;
        default:
          return false;
      }
    }
    public static void De(ReportBuilder report, Field field)
    {
      var hazards = GetHazards(field);
      if (hazards != null)
      {
        foreach (var eh in hazards.ToArray()) DeReport(report, eh, field);
        hazards.Clear();
      }
    }
    public static void De(ReportBuilder report, Field field, MoveType move)
    {
      var hazards = GetHazards(field);
      if (hazards != null)
        foreach (var eh in hazards)
          if (eh.Move == move)
          {
            hazards.Remove(eh);
            DeReport(report, eh, field);
            break;
          }
    }
    private static void DeReport(ReportBuilder report, Condition hazard, Field field)
    {
      var m = hazard.Move.Id;
      report.ShowLog(m == Ms.SPIKES ? "DeSpikes" : m == Ms.TOXIC_SPIKES ? "DeToxicSpikes" : m == Ms.STEALTH_ROCK ? "DeStealthRock" : "DeStickyWeb", field.Team);
    }
    public static void Debut(PokemonProxy pm) //欢迎登场，口耐的精灵们（笑
    {
      var hazards = GetHazards(pm.Field);
      if (hazards != null)
        foreach (var eh in hazards.ToArray())
        {
          Debut(eh, pm);
          if (pm.CheckFaint()) return;
        }
    }
    private static void Debut(Condition hazard, PokemonProxy pm)
    {
      switch (hazard.Move.Id)
      {
        case Ms.SPIKES:
          if (HasEffect.IsGroundAffectable(pm, true, false)) pm.EffectHurtByOneNth(hazard.Int, "m_Spikes");
          break;
        case Ms.TOXIC_SPIKES:
          if (HasEffect.IsGroundAffectable(pm, true, false))
            if (pm.OnboardPokemon.HasType(BattleType.Poison)) De(pm.Controller.ReportBuilder, pm.Field, hazard.Move);
            else if (pm.CanAddState(pm, AttachedState.PSN, false)) pm.AddState(pm, AttachedState.PSN, false, hazard.Bool ? 15 : 0);
          break;
        case Ms.STEALTH_ROCK:
          int revise = BattleType.Rock.EffectRevise(pm.OnboardPokemon.Types);//羽栖有效无效都无所谓
          int hp = (revise > 0 ? pm.Pokemon.MaxHp << revise : pm.Pokemon.MaxHp >> -revise) >> 3;
          pm.EffectHurt(hp, "m_StealthRock");
          break;
        case Ms.STICKY_WEB:
          if (HasEffect.IsGroundAffectable(pm, true, false))
          {
            pm.ShowLogPm("StickyWeb");
            pm.ChangeLv7D(null, StatType.Speed, -1, false, false);
          }
          break;
      }
    }
  }
}
