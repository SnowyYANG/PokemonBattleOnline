using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonBattleOnline.Game;

namespace PokemonBattleOnline.Game.Host.Triggers
{
  internal static class DamageModifier
  {
    public static Modifier Execute(DefContext def)
    {
      var der = def.Defender;
      var atk = def.AtkContext;
      var move = atk.Move;
      PokemonProxy aer = atk.Attacker;

      //If the target's side is affected by Reflect, the move used was physical, the user's ability isn't Infiltrator and the critical hit flag isn't set. 
      //The value of the modificator is 0xA8F if there is more than one Pokemon per side of the field and 0x800 otherwise.
      //Same as above with Light Screen and special moves.
      Modifier m = (Modifier)(
        move.Category == MoveCategory.Physical && def.HasInfiltratableCondition("Reflect") ||
        move.Category == MoveCategory.Special && def.HasInfiltratableCondition("LightScreen") ?
        atk.MultiTargets ? 0xA8F : 0x800 : 0x1000);

      {
        //multiscale tinedlens friendguard sniper filter solidrock
        int aa = def.AtkContext.Attacker.Ability, da = def.Ability;
        //If the target's ability is Multiscale and the target is at full health.
        m *= (Modifier)(da == As.MULTISCALE && der.Hp == der.Pokemon.Hp.Origin ? 0x800 : 0x1000);
        //If the user's ability is Tinted Lens and the move wasn't very effective.
        if (def.EffectRevise < 0 && aa == As.TINTED_LENS) m *= 0x2000;
        //If one of the target's allies' ability is Friend Guard.
        foreach (PokemonProxy pm in der.Controller.GetOnboardPokemons(der.Pokemon.TeamId))
          if (pm != der && pm.Ability == As.FRIEND_GUARD) m *= 0xC00;
        //If user has ability Sniper and move was a critical hit.
        if (def.IsCt && aa == As.SNIPER) m *= 0x1800;
        //If the target's ability is Solid Rock or Filter and the move was super effective.
        if (def.EffectRevise > 0 && (da == As.FILTER || da == As.SOLID_ROCK)) m *= 0xC00;
      }

      switch (aer.Item)
      {
        //If the user is holding an expert belt and the move was super effective.
        case Is.EXPERT_BELT:
          if (def.EffectRevise > 0) m *= 0x1333;
          break;
        //If the user is holding a Life Orb.
        case Is.LIFE_ORB:
          m *= 0x14cc;
          break;
        //If the user is holding the item Metronome. If n is the number of time the current move was used successfully and successively, the value of the modifier is 0x1000+n*0x333 if n≤4 and 0x2000 otherwise.
        case Is.METRONOME:
          var c = aer.OnboardPokemon.GetCondition("LastMove");
          if (c != null && move == c.Move)
          {
            if (c.Int < 5) m *= (ushort)(0x1000 + c.Int * 0x333);
            else m *= 0x2000;
          }
          break;
      }

      //If the target is holding a damage lowering berry of the attack's type.
      {
        var item = der.Item;
        if (
          item == 180 && atk.Type == BattleType.Normal ||
          164 <= item && item <= 179 && atk.Type == BattleTypeHelper.GetItemType(item, 164) && def.EffectRevise > 0
          )
        {
          def.SetCondition("Antiberry");
          m *= 0x800;
        }
      }

      switch (move.Id)
      {
        case Ms.STOMP: //23
        case Ms.STEAMROLLER: //537
          if (der.OnboardPokemon.HasCondition("Minimize")) m *= 0x2000;
          break;
        case 57:
        case Ms.WHIRLPOOL:
          if (der.OnboardPokemon.CoordY == CoordY.Water) m *= 0x2000;
          break;
        case Ms.EARTH_POWER: //89
        case Ms.MAGNITUDE: //222
          if (der.OnboardPokemon.CoordY == CoordY.Underground) m *= 0x2000;
          break;
      }

      return m;
    }
  }
}
