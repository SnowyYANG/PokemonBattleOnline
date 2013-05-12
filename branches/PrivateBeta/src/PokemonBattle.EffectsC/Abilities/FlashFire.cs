using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Data;
using LightStudio.PokemonBattle.Game.Host.Sp;

namespace LightStudio.PokemonBattle.Game.Host.Effects.Abilities
{
  class FlashFire : AbilityE
  {
    public FlashFire(int id)
      : base(id)
    {
    }
    
    public override void Attach(PokemonProxy pm)
    {
      pm.OnboardPokemon.RemoveCondition("FlashFire");
    }
    public override bool CanImplement(DefContext def)
    {
      if (def.AtkContext.Type == BattleType.Fire)
      {
        PokemonProxy pm = def.Defender;
        pm.OnboardPokemon.SetCondition("FlashFire");
        pm.RaiseAbility();
        pm.AddReportPm("FlashFire");
        return false;
      }
      return true;
    }
    public override Modifier AModifier(AtkContext atk)
    {
      return (ushort)(atk.Type == BattleType.Fire && atk.Attacker.OnboardPokemon.HasCondition("FlashFire") ? 0x1800 : 0x1000);
    }
  }
}
