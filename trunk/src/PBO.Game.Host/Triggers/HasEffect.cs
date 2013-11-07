using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonBattleOnline.Game;

namespace PokemonBattleOnline.Game.Host.Triggers
{
  internal static class HasEffect
  {
    public static bool Execute(DefContext def)
    {
      var der = def.Defender;
      var atk = def.AtkContext;
      var move = atk.Move;

      if (move.Id == Ms.DREAM_EATER && der.State != PokemonState.SLP) return false;
      if (move.Id == Ms.SYNCHRONOISE) return Sychronoise(def);
      if (move.Id == Ms.CAPTIVATE) return Captivate(def);

      if (move.Category == MoveCategory.Status && move.Id != Ms.THUNDER_WAVE) return true;
      if (move.Class == MoveInnerClass.OHKO && (der.Pokemon.Lv > atk.Attacker.Pokemon.Lv || der.RaiseAbility(As.STURDY))) return false;
      BattleType canAtk;
      {
        var o = der.OnboardPokemon.GetCondition("CanAttack");
        canAtk = o == null ? BattleType.Invalid : o.BattleType;
      }
      return (
        canAtk != BattleType.Invalid && der.OnboardPokemon.HasType(canAtk) ||
        der.Item == Is.RING_TARGET ||
        atk.Type == BattleType.Ground ? IsGroundAffectable(der, !ATs.IgnoreDefenderAbility(atk.Attacker.Ability), true) : NonGround(def));
    }
    public static bool IsGroundAffectable(PokemonProxy pm, bool abilityAvailable, bool raiseAbility, bool ground = true)
    {
      var o = pm.OnboardPokemon;
      return
        (o.HasCondition("SmackDown") || o.HasCondition("Ingrain") || ground && pm.Controller.Board.HasCondition("Gravity")) || pm.Item == Is.IRON_BALL ||
        !
        (
          o.HasType(BattleType.Flying) ||
          o.HasCondition("MagnetRise") || o.HasCondition("Telekinesis") ||
          pm.Item == Is.AIR_BALLOON ||
          (abilityAvailable && (raiseAbility ? pm.RaiseAbility(As.LEVITATE) : pm.Ability == As.LEVITATE))
        );
    }
    private static bool NonGround(DefContext def)
    {
      var type = def.AtkContext.Type.NoEffect();
      return type == BattleType.Invalid || type == BattleType.Ghost && def.AtkContext.Attacker.Ability == As.SCRAPPY || !def.Defender.OnboardPokemon.HasType(type);
    }
    private static bool Sychronoise(DefContext def)
    {
      var types = def.AtkContext.Attacker.OnboardPokemon.Types;
      foreach (var t in types)
        if (def.Defender.OnboardPokemon.HasType(t)) return true;
      return false;
    }
    private static bool Captivate(DefContext def)
    {
      var dg = def.Defender.OnboardPokemon.Gender;
      var ag = def.AtkContext.Attacker.OnboardPokemon.Gender;
      return dg == PokemonGender.Male && ag == PokemonGender.Female || dg == PokemonGender.Female && ag == PokemonGender.Male;
    }
  }
}
