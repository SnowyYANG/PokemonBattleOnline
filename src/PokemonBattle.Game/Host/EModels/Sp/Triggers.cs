using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Data;
using LightStudio.PokemonBattle.Game.GameEvents;

namespace LightStudio.PokemonBattle.Game.Host.Sp
{
  public static class Triggers
  {
    internal static Modifier PowerModifier(DefContext def, Func<DefContext, Modifier> movePowerModifier)
    {
      var atk = def.AtkContext;
      var c = def.Defender.Controller;

      Modifier m = atk.Attacker.Ability.PowerModifier(def);
      m *= Abilities.PowerModifier(def);
      if (def.AtkContext.HasCondition("Gem")) m *= 0x1800;
      else m *= atk.Attacker.Item.PowerModifier(atk);
      m *= movePowerModifier(def);
      if (atk.HasCondition("MeFirst")) m *= 0x1800;
      m *= Moves.SolarBeam(def);
      if (atk.Type == BattleType.Electric && atk.Attacker.OnboardPokemon.GetCondition<int>("Charge") == c.TurnNumber) m *= 0x2000;
      //helpinghand
      if ((atk.Type == BattleType.Fire && c.OnboardPokemons.Any((p) => p.OnboardPokemon.HasCondition("WaterSport"))) ||
        (atk.Type == BattleType.Electric && c.OnboardPokemons.Any((p) => p.OnboardPokemon.HasCondition("MudSport"))))
        m *= 0x800;

      return m;
    }
    internal static Modifier DamageFinalModifier(DefContext def, Modifier move)
    {
      //If the target's side is affected by Reflect, the move used was physical, the user's ability isn't Infiltrator and the critical hit flag isn't set. 
      //The value of the modificator is 0xA8F if there is more than one Pokemon per side of the field and 0x800 otherwise.
      //Same as above with Light Screen and special moves.
      Modifier m = (Modifier)(
        def.AtkContext.Move.Category == MoveCategory.Physical && def.HasInfiltratableCondition("Reflect") ||
        def.AtkContext.Move.Category == MoveCategory.Special && def.HasInfiltratableCondition("LightScreen") ?
        def.AtkContext.MultiTargets ? 0xA8F : 0x800 : 0x1000);
      //multiscale tinedlens friendguard sniper filter solidrock
      m *= Abilities.DamageFinalModifier(def);
      //metronome expertbelt lifeorb
      m *= Items.DamageFinalModifier(def);
      //If the target is holding a damage lowering berry of the attack's type.
      m *= def.Defender.Item.DamageFinalModifier(def);
      return m * move;
    }
    public static void KOed(DefContext def, OnboardPokemon o)
    {
      var der = def.Defender;
      var aer = def.AtkContext.Attacker;
      if (o.HasCondition("DestinyBond"))
      {
        der.AddReportPm("DestinyBond"); //战报顺序已测
        aer.Faint();
      }
      var mp = def.AtkContext.MoveProxy;
      if (o.HasCondition("Grudge") && mp != null && mp.PP != 0)
      {
        aer.Controller.ReportBuilder.Add(new PPChange("Grudge", mp, mp.PP));
        mp.PP = 0;
      }
      if (aer.CanChangeLv7D(aer, StatType.Atk, 1, false) != 0 && aer.RaiseAbility(Abilities.MOXIE)) aer.ChangeLv7D(aer, false, 1);
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
      pm.ResetMoves();
      var o = pm.OnboardPokemon;
      if (pm.State == PokemonState.SLP) o.SetCondition("SLP", pm.Tile.Field.HasCondition("Rest" + pm.Id) ? 3 : pm.Controller.GetRandomInt(2, 4));
      else
      {
        pm.Tile.Field.RemoveCondition("Rest" + pm.Id);
        if (pm.State == PokemonState.BadlyPSN) o.SetCondition("PSN", pm.Controller.TurnNumber);
      }
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
      Abilities.Illusion(pm);//幻影特性以交换前的队伍顺序决定
    }
    public static void Withdrawing(PokemonProxy pm, bool canPursuit)
    {
      if (canPursuit && pm.Hp != 0)
      {
        var tile = pm.Tile;
        foreach (var p in pm.Controller.Board[1 - tile.Team].GetPokemons(tile.X - 1, tile.X + 1).ToArray())
          if (p.SelectedMove != null && p.SelectedMove.Type.Id == Moves.PURSUIT && p.CanMove)
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
    public static bool Remaining1HP(PokemonProxy pm)
    {
      if (pm.OnboardPokemon.HasCondition("Endure"))
      {
        pm.AddReportPm("Endure");
        return true;
      }
      if (pm.Hp == pm.Pokemon.Hp.Origin && pm.RaiseAbility(Abilities.STURDY))
      {
        pm.AddReportPm("Endure");
        return true;
      }
      const int FOCUS_BAND = 15, FOCUS_SASH = 52;
      if ((pm.Item.Id == FOCUS_BAND && pm.Controller.OneNth(10)) || (pm.Item.Id == FOCUS_SASH && pm.Hp == pm.Pokemon.Hp.Origin))
      {
        pm.RaiseItem("FocusItem");
        return true;
      }
      return false;
    }
    public static bool MagicCoat(AtkContext atk, PokemonProxy der)
    {
      //atk.Move.AdvancedFlags.MagicCoat is already checked
      if (der.OnboardPokemon.HasCondition("MagicCoat") || !atk.Attacker.Ability.IgnoreDefenderAbility() && der.Ability.Id == Abilities.MAGIC_BOUNCE)
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
      if (move.Flags.UnavailableWithGravity && pm.Controller.Board.HasCondition("Gravity"))
      {
        pm.AddReportPm("GravityCantUseMove", move.Id);
        return false;
      }
      //回复封印
      if (move.Flags.IsHeal && pm.OnboardPokemon.HasCondition("HealBlock"))
      {
        pm.AddReportPm("HealBlockCantUseMove", move.Id);
        return false;
      }
      return true;
    }
  }
}
