using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Data;
using LightStudio.PokemonBattle.Game;

namespace LightStudio.PokemonBattle.Effects.Abilities
{
  class FlashFire : AbilityE
  {
    public FlashFire()
      : base(34)
    {
    }
    
    public override void Attach(PokemonProxy pm)
    {
      pm.OnboardPokemon.RemoveCondition("FlashFire");
    }
    public override bool CanImplement(DefContext def)
    {
      if (def.AtkContext.Type == Data.BattleType.Fire)
      {
        PokemonProxy pm = def.Defender;
        pm.OnboardPokemon.SetCondition("FlashFire");
        pm.Controller.ReportBuilder.Add(new Interactive.GameEvents.AbilityEvent(pm));
        pm.Controller.ReportBuilder.Add("FlashFire", pm);
        return false;
      }
      return true;
    }
    public override Modifier ADSModifier(PokemonProxy pm, StatType stat)
    {
      if (stat == StatType.Atk || stat == StatType.SpAtk)
      {
        AtkContext atk = pm.AtkContext;
        if (atk != null && atk.Type == BattleType.Fire && pm.OnboardPokemon.HasCondition("FlashFire"))
          return 0x1800;
      }
      return 0x1000;
    }
  }
}
