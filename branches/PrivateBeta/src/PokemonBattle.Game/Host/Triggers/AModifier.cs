﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Data;

namespace LightStudio.PokemonBattle.Game.Host.Triggers
{
  internal static class AModifier
  {
    public static Modifier Execute(DefContext def)
    {
      var der = def.Defender;
      var atk = def.AtkContext;
      var aer = atk.Attacker;
      var cat = atk.Move.Category;

      Modifier m = ThickFat(def);

      switch (aer.Ability)
      {
        case As.FLASH_FIRE:
          if (atk.Type == BattleType.Fire && aer.OnboardPokemon.HasCondition("FlashFire")) m *= 0x1800;
          break;
        case As.HUGE_POWER:
        case As.PURE_POWER:
          if (cat == MoveCategory.Physical) m *= 0x2000;
          break;
        case As.PLUS:
        case As.MINUS:
          m *= PlusMinus(atk);
          break;
        case As.GUTS:
          if (aer.State != PokemonState.Normal && cat == MoveCategory.Physical) m *= 0x1800;
          break;
        case As.OVERGROW:
          m *= One3rdHp(atk, BattleType.Grass);
          break;
        case As.BLAZE:
          m *= One3rdHp(atk, BattleType.Fire);
          break;
        case As.TORRENT:
          m *= One3rdHp(atk, BattleType.Water);
          break;
        case As.SWARM:
          m *= One3rdHp(atk, BattleType.Bug);
          break;
        case As.SOLAR_POWER:
          if (aer.Controller.Weather == Weather.IntenseSunlight && cat == MoveCategory.Special) m *= 0x1800;
          break;
        case As.DEFEATIST:
          if (aer.Hp * 2 < aer.Pokemon.Hp.Origin) m *= 0x800;
          break;
      }
      
      if (cat == MoveCategory.Physical && atk.Controller.Weather == Weather.IntenseSunlight)
        foreach (PokemonProxy pm in atk.Controller.GetOnboardPokemons(atk.Attacker.Pokemon.Id))
          if (pm.Pokemon.Form.Type.Number == 421 && pm.Ability == As.FLOWER_GIFT) return m *= 0x1800;

      var n = aer.Pokemon.Form.Type.Number;
      switch (aer.Item)
      {
        case Is.CHOICE_BAND:
          if (cat == MoveCategory.Physical) m *= 0x1800;
          break;
        case Is.CHOICE_SPECS:
          if (cat == MoveCategory.Special) m *= 0x1800;
          break;
        case Is.SOUL_DEW:
          if ((n == 380 || n == 381) && cat == MoveCategory.Special) m *= 0x1800;
          break;
        case Is.DEEPSEATOOTH:
          if (n == 366 && cat == MoveCategory.Special) m *= 0x2000;
          break;
        case Is.LIGHT_BALL:
          if (n == 25) m *= 0x2000;
          break;
        case Is.THICK_CLUB:
          if ((n == 104 || n == 105) && cat == MoveCategory.Physical) m *= 0x2000;
          break;
      }

      return m;
    }
    private static Modifier ThickFat(DefContext def)
    {
      BattleType type = def.AtkContext.Move.Type;
      return (Modifier)((type == BattleType.Ice || type == BattleType.Fire) && def.Ability == As.THICK_FAT ? 0x800 : 0x1000);
    }
    private static Modifier PlusMinus(AtkContext atk)
    {
      if (atk.Move.Category == MoveCategory.Special)
        foreach (var p in atk.Attacker.Tile.Field.Pokemons)
          if (p != atk.Attacker)
          {
            var a = p.Ability;
            if (a == 57 || a == 58) return 0x1800;
          }
      return 0x1000;
    }
    private static Modifier One3rdHp(AtkContext atk, BattleType type)
    {
      return (Modifier)(atk.Type == type && atk.Attacker.Hp * 3 <= atk.Attacker.Pokemon.Hp.Origin ? 0x1800 : 0x1000);
    }
  }
}
