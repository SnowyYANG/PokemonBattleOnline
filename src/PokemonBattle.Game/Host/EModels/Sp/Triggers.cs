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
    public static Modifier PowerModifier(DefContext def, Func<DefContext, Modifier> movePowerModifier)
    {
      var atk = def.AtkContext;
      var c = def.Defender.Controller;

      Modifier m = atk.Attacker.Ability.PowerModifier(def);
      m *= Abilities.PowerModifier(def);
      if (def.AtkContext.Gem) m *= 0x1800;
      else m *= atk.Attacker.Item.PowerModifier(atk);
      m *= movePowerModifier(def);
      if (atk.MeFirst) m *= 0x1800;
      m *= Moves.SolarBeam(def);
      if (atk.Type == BattleType.Electric && atk.Attacker.OnboardPokemon.GetCondition<int>("Charge") == c.TurnNumber) m *= 0x2000;
      //helpinghand
      if ((atk.Type == BattleType.Fire && c.OnboardPokemons.Any((p) => p.OnboardPokemon.HasCondition("WaterSport"))) ||
        (atk.Type == BattleType.Electric && c.OnboardPokemons.Any((p) => p.OnboardPokemon.HasCondition("MudSport"))))
        m *= 0x800;

      return m;
    }
    public static void KOed(DefContext def)
    {
      var der = def.Defender;
      var aer = def.AtkContext.Attacker;
      if (der.OnboardPokemon.HasCondition("DestinyBond"))
      {
        der.AddReportPm("DestinyBond"); //战报顺序已测
        aer.Pokemon.SetHp(0);
        aer.CheckFaint();
      }
      if (der.OnboardPokemon.HasCondition("Grudge"))
      {
        int formerPP = def.AtkContext.MoveProxy.PP;
        def.AtkContext.MoveProxy.PP = 0;
        aer.Controller.ReportBuilder.Add(new PPChange("Grudge", def.AtkContext.MoveProxy, formerPP));
      }
      if (aer.CanChangeLv7D(aer, StatType.Atk, 1, false) != 0 && aer.RaiseAbility(Abilities.MOXIE)) aer.ChangeLv7D(aer, false, 1);
    }
    public static void WillAct(PokemonProxy pm)
    {
      pm.Tile.RemoveCondition("DestinyBond");
      pm.Tile.RemoveCondition("Grudge");
    }
    public static void Withdrawing(PokemonProxy pm, bool canPursuit)
    {
      if (pm.Hp > 0 && canPursuit)
      {
        var tile = pm.Tile;
        foreach (var p in pm.Controller.GetOnboardPokemons(1 - pm.Pokemon.TeamId).ToArray())
          if (Math.Abs(tile.X - p.OnboardPokemon.X) <= 1 && p.SelectedMove != null && p.SelectedMove.Type.Id == Moves.PERSUIT)
          {
            p.Move(AtkContextFlag.IgnorePostEffectItem);
            if (pm.CheckFaint()) return;
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
  }
}
