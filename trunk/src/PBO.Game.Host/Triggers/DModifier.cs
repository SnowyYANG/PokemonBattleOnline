using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonBattleOnline.Game;

namespace PokemonBattleOnline.Game.Host.Triggers
{
  internal static class DModifier
  {
    public static Modifier Execute(DefContext def)
    {
      var der = def.Defender;
      var atk = def.AtkContext;
      var aer = atk.Attacker;
      var cat = atk.Move.Category;
      var n = der.Pokemon.Form.Species.Number;
      
      Modifier m = 0x1000;
      if (def.Ability == As.MARVEL_SCALE && der.State != PokemonState.Normal) m *= 0x1800;
      if (cat == MoveCategory.Special && der.Controller.Weather == Weather.IntenseSunlight)
        foreach (PokemonProxy pm in der.Controller.GetOnboardPokemons(der.Pokemon.TeamId))
          if (pm.Pokemon.Form.Species.Number == 421 && pm.Ability == As.FLOWER_GIFT) m *= 0x1800;
      switch (der.Item)
      {
        case Is.EVIOLITE:
          if (Data.GameDataService.GetEvolutions(der.Pokemon.Form.Species.Number).Any()) m = 0x1800;
          break;
        case Is.SOUL_DEW:
          if (cat == MoveCategory.Special && (n == 380 || n == 381)) m *= 0x1800;
          break;
        case Is.DEEPSEASCALE:
          if (cat == MoveCategory.Special && n == 366) m *= 0x2000;
          break;
        case Is.METAL_POWDER:
          if (cat == MoveCategory.Physical && n == 132 && !der.OnboardPokemon.HasCondition("Transform")) m *= 0x2000;
          break;
      }
      return m;
    }

    public static Modifier Sandstorm(DefContext def)
    {
      return (Modifier)(def.AtkContext.Move.Category == MoveCategory.Special && def.Defender.Controller.Weather == Weather.Sandstorm && def.Defender.OnboardPokemon.HasType(BattleType.Rock) ? 0x1800 : 0x1000);
    }
  }
}
