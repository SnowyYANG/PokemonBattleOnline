using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Data;
using LightStudio.PokemonBattle.Game.Host.Sp;
using As = LightStudio.PokemonBattle.Game.Host.Sp.Abilities;
using Is = LightStudio.PokemonBattle.Game.Host.Sp.Items;

namespace LightStudio.PokemonBattle.Game.Host.Effects.Triggers
{
  class EndTurn : Game.Host.Triggers.IEndTurn
  {
    public void Execute(Controller c)
    {
      Weather(c);
      FSDD(c);
      Wish(c);
      PropertyChange(c);
      HpRecover(c);
      PmState(c);
      Curse(c);
      Trap(c);
      PokemonCondition(c);
      FieldCondition(c);
      BoardCondition(c);
      Pokemon(c);
      ZenMode(c);
    }
    //1.0 weather ends, Sandstorm/Hail damage, Rain Dish/Dry Skin/Ice Body/SolarPower
    private void Weather(Controller c)
    {
      int turn = c.Board.GetCondition<int>("Weather");
      if (turn == c.ReportBuilder.TurnNumber) c.Weather = Game.Weather.Normal;
      else
      {
        if (c.Board.Weather == Game.Weather.Sandstorm) c.ReportBuilder.Add("Sandstorm");
        else if (c.Board.Weather == Game.Weather.Hailstorm) c.ReportBuilder.Add("Hailstorm");
        switch (c.Weather)
        {
          case Game.Weather.Sandstorm:
            foreach (var pm in c.OnboardPokemons)
            {
              int ab = pm.Ability.Id;
              if (pm.OnboardPokemon.HasType(BattleType.Rock) || pm.OnboardPokemon.HasType(BattleType.Steel) || pm.OnboardPokemon.HasType(BattleType.Ground) ||
                ab == As.OVERCOAT || ab == As.SAND_VEIL || ab == As.SAND_RUSH || ab == As.SAND_FORCE) continue;
              pm.EffectHurtByOneNth(16, "SandstormHurt");
            }
            break;
          case Game.Weather.Hailstorm:
            foreach (var pm in c.OnboardPokemons)
            {
              int ab = pm.Ability.Id;
              if (ab == As.ICE_BODY)
              {
                if (!pm.FullHp)
                {
                  pm.RaiseAbility();
                  pm.HpRecoverByOneNth(16);
                }
              }
              else
              {
                if (pm.OnboardPokemon.HasType(BattleType.Ice) || ab == As.OVERCOAT || ab == As.SNOW_CLOAK) continue;
                pm.EffectHurtByOneNth(16, "HailstormHurt");
              }
            }
            break;
          case Game.Weather.HeavyRain:
            foreach (var pm in c.OnboardPokemons)
              if (!pm.FullHp)
                if (pm.RaiseAbility(As.RAIN_DISH)) pm.HpRecoverByOneNth(16);
                else if (pm.RaiseAbility(As.DRY_SKIN)) pm.HpRecoverByOneNth(8);
            break;
          case Game.Weather.IntenseSunlight:
            foreach (var pm in c.OnboardPokemons)
              if (pm.RaiseAbility(As.SOLAR_POWER) || pm.RaiseAbility(As.DRY_SKIN)) pm.EffectHurtByOneNth(8);
            break;
        }
      }
    }
    //3.0 [unfinished] Future Sight, Doom Desire
    private void FSDD(Controller c)
    {
    }
    //4.0 [unfinished] Wish
    private void Wish(Controller c)
    {
    }
    //5.0 [pass] Fire Pledge + Grass Pledge damage
    //5.1 Shed Skin, Hydration, Healer
    //5.2 Leftovers, Black Sludge
    private void PropertyChange(Controller c)
    {
      foreach (var pm in c.OnboardPokemons)
      {
        switch (pm.Ability.Id)
        {
          case As.SHED_SKIN:
            if (pm.State != Game.PokemonState.Normal && c.RandomHappen(30))
            {
              pm.RaiseAbility();
              pm.State = Game.PokemonState.Normal;
            }
            break;
          case As.HYDRATION:
            if (pm.State != Game.PokemonState.Normal)
            {
              pm.RaiseAbility();
              pm.State = Game.PokemonState.Normal;
            }
            break;
          case As.HEALER:
            var ps = new List<PokemonProxy>();
            foreach (var p in c.Board[pm.Pokemon.TeamId].Pokemons)
              if (p != pm && p.State != Game.PokemonState.Normal) ps.Add(p);
            if (ps.Count != 0 && c.RandomHappen(30))
            {
              pm.RaiseAbility();
              ps[c.GetRandomInt(0, ps.Count - 1)].State = Game.PokemonState.Normal;
            }
            break;
        }
        switch (pm.Item.Id)
        {
          case Is.LEFTOVERS:
            pm.HpRecoverByOneNth(16, "Leftovers", Is.LEFTOVERS);
            break;
          case Is.BLACK_SLUDGE:
            if (pm.OnboardPokemon.HasType(BattleType.Poison)) pm.HpRecoverByOneNth(16, "Leftovers", Is.BLACK_SLUDGE);
            else pm.EffectHurtByOneNth(8, "ItemHurt", Is.BLACK_SLUDGE);
            break;
        }
      }
    }
    //6.0 Aqua Ring
    //7.0 Ingrain
    //8.0 [unfinished]Leech Seed
    private void HpRecover(Controller c)
    {
      foreach (var pm in c.OnboardPokemons)
        if (!pm.OnboardPokemon.HasCondition("HealBlock") && pm.OnboardPokemon.HasCondition("AquaRing"))
        {
          int hp = pm.Pokemon.Hp.Origin / 16;
          if (pm.Item.BigRoot()) hp = (int)(hp * 1.3);
          pm.HpRecover(hp, "AquaRing");
        }
      foreach (var pm in c.OnboardPokemons)
        if (!pm.OnboardPokemon.HasCondition("HealBlock") && pm.OnboardPokemon.HasCondition("Ingrain"))
        {
          int hp = pm.Pokemon.Hp.Origin / 16;
          if (pm.Item.BigRoot()) hp = (int)(hp * 1.3);
          pm.HpRecover(hp, "Ingrain");
        }
    }
    //9.0 (bad) poison damage, burn damage, Poison Heal
    //9.1 Nightmare
    private void PmState(Controller c)
    {
      foreach(var pm in c.OnboardPokemons)
        switch (pm.State)
        {
          case PokemonState.BadlyPoisoned:
            if (!pm.FullHp && pm.RaiseAbility(As.POISON_HEAL)) pm.HpRecoverByOneNth(8, "PoisonHeal");
            else pm.EffectHurtByOneNth(8, "Poisoned");
            break;
          case PokemonState.Poisoned:
            if (!pm.FullHp && pm.RaiseAbility(As.POISON_HEAL)) pm.HpRecoverByOneNth(8, "PoisonHeal");
            else
            {
              int turn = 1 + c.TurnNumber - pm.OnboardPokemon.GetCondition<int>("Poison");
              int hp = pm.Pokemon.Hp.Origin / 16;
              if (hp == 0) hp = 1;
              hp *= (turn > 15 ? 15 : turn);
              pm.EffectHurtByOneNth(8, "Poisoned");
            }
            break;
          case PokemonState.Burned:
            pm.EffectHurtByOneNth(8, "Burned");
            break;
          case PokemonState.Sleeping:
            if (pm.OnboardPokemon.HasCondition("Nightmare")) pm.EffectHurtByOneNth(4, "Nightmare");
            break;
        }
    }
    //10.0 Curse (from a Ghost-type)
    private void Curse(Controller c)
    {
      foreach (var pm in c.OnboardPokemons)
        if (pm.OnboardPokemon.HasCondition("Curse")) pm.EffectHurtByOneNth(4, "Curse");
    }
    //11.0 Bind, Wrap, Fire Spin, Clamp, Whirlpool, Sand Tomb, Magma Storm
    private void Trap(Controller c)
    {
      foreach (var pm in c.OnboardPokemons)
      {
        dynamic trap = pm.OnboardPokemon.GetCondition<dynamic>("Trap");
        if (trap != null)
          if (trap.Turn == c.TurnNumber)
          {
            pm.OnboardPokemon.RemoveCondition("Trap");
            pm.AddReportPm("TrapFree", trap.Move);
          }
          else pm.EffectHurtByOneNth(trap.BindingBand ? 4 : 8, "TrapHurt", trap.Move);
      }
    }
    //12.0 Taunt ends
    //13.0 Encore ends
    //14.0 [unfinished]Disable ends, Cursed Body ends
    //15.0 Magnet Rise ends
    //16.0 Telekinesis ends
    //17.0 Heal Block ends
    //18.0 Embargo ends
    //19.0 Yawn
    //20.0 Perish Song
    private void PokemonCondition(Controller c)
    {
      foreach (var pm in c.OnboardPokemons)
      {
        int turn = pm.OnboardPokemon.GetCondition("Taunt", -1);
        if (turn == 0)
        {
          pm.OnboardPokemon.RemoveCondition("Taunt");
          pm.AddReportPm("Encore");
        }
      } 
      foreach (var pm in c.OnboardPokemons)
      {
        dynamic obj = pm.OnboardPokemon.GetCondition<dynamic>("Encore");
        if (obj != null && obj.Turn == 0)
        {
          pm.OnboardPokemon.RemoveCondition("Encore");
          pm.AddReportPm("DeEncore");
        }
      }
      //14.0 [unfinished]Disable ends, Cursed Body ends
      foreach (var pm in c.OnboardPokemons)
      {
        int turn = pm.OnboardPokemon.GetCondition<int>("MagnetRise");
        if (turn == c.TurnNumber)
        {
          pm.OnboardPokemon.RemoveCondition("MagnetRise");
          pm.AddReportPm("DeMagnetRise");
        }
      }
      //16.0 Telekinesis ends
      //17.0 Heal Block ends
      //18.0 Embargo ends
      //19.0 Yawn
      //20.0 Perish Song
    }
    //21.0 Reflect ends
    //21.1 Light Screen ends
    //21.2 Safeguard ends
    //21.3 Mist ends
    //21.4 Tailwind ends
    //21.5 Lucky Chant ends
    //21.6 Water Pledge + Fire Pledge ends, Fire Pledge + Grass Pledge ends, Grass Pledge + Water Pledge ends
    private void FieldCondition(Controller c)
    {
    }
    //22.0 Gravity ends
    //23.0 Trick Room ends
    //24.0 Wonder Room ends
    //25.0 Magic Room ends
    private void BoardCondition(Controller c)
    {
    }
    //26.0 Uproar message
    //26.1 Speed Boost, Bad Dreams, Harvest, Moody
    //26.2 Toxic Orb activation, Flame Orb activation, Sticky Barb
    //26.3 pickup
    private void Pokemon(Controller c)
    {
    }
    //27.0 Zen Mode
    private void ZenMode(Controller c)
    {
    }
  }
}
