using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Data;
using LightStudio.PokemonBattle.Game.Host.Sp;
using LightStudio.PokemonBattle.Game.GameEvents;
using As = LightStudio.PokemonBattle.Game.Host.Sp.Abilities;
using Is = LightStudio.PokemonBattle.Game.Host.Sp.Items;

namespace LightStudio.PokemonBattle.Game.Host.Effects.Triggers
{
  class EndTurn : IEndTurn
  {
    private static readonly StatType[] SEVEN_D = { StatType.Atk, StatType.Def, StatType.SpAtk, StatType.SpDef, StatType.Speed, StatType.Accuracy, StatType.Evasion };

    public void Execute(Controller c)
    {
      Weather(c); if (!c.CanContinue) goto GAMEEND;
      FSDD(c); if (!c.CanContinue) goto GAMEEND;
      Wish(c); if (!c.CanContinue) goto GAMEEND;
      PropertyChange(c); if (!c.CanContinue) goto GAMEEND;
      HpRecover(c); if (!c.CanContinue) goto GAMEEND;
      PmState(c); if (!c.CanContinue) goto GAMEEND;
      Curse(c); if (!c.CanContinue) goto GAMEEND;
      Trap(c); if (!c.CanContinue) goto GAMEEND;
      PokemonCondition(c); if (!c.CanContinue) goto GAMEEND;
      FieldCondition(c); if (!c.CanContinue) goto GAMEEND;
      BoardCondition(c); if (!c.CanContinue) goto GAMEEND;
      Pokemon(c); if (!c.CanContinue) goto GAMEEND;
      ZenMode(c);
    GAMEEND: ;
    }
    //1.0 weather ends, Sandstorm/Hail damage, Rain Dish/Dry Skin/Ice Body/SolarPower
    private void Weather(Controller c)
    {
      int turn = c.Board.GetCondition<int>("Weather");
      if (turn == c.TurnNumber) c.Weather = Game.Weather.Normal;
      else
      {
        if (c.Board.Weather == Game.Weather.Sandstorm) c.ReportBuilder.Add("Sandstorm");
        else if (c.Board.Weather == Game.Weather.Hailstorm) c.ReportBuilder.Add("Hailstorm");
        switch (c.Weather)
        {
          case Game.Weather.Sandstorm:
            foreach (var pm in c.OnboardPokemons.ToArray())
            {
              int ab = pm.Ability.Id;
              if (pm.OnboardPokemon.HasType(BattleType.Rock) || pm.OnboardPokemon.HasType(BattleType.Steel) || pm.OnboardPokemon.HasType(BattleType.Ground) ||
                ab == As.OVERCOAT || ab == As.SAND_VEIL || ab == As.SAND_RUSH || ab == As.SAND_FORCE) continue;
              pm.EffectHurtByOneNth(16, "SandstormHurt");
              pm.CheckFaint();
            }
            break;
          case Game.Weather.Hailstorm:
            foreach (var pm in c.OnboardPokemons.ToArray())
            {
              int ab = pm.Ability.Id;
              if (ab == As.ICE_BODY)
              {
                if (pm.CanHpRecover())
                {
                  pm.RaiseAbility();
                  pm.HpRecoverByOneNth(16);
                }
              }
              else
              {
                if (pm.OnboardPokemon.HasType(BattleType.Ice) || ab == As.OVERCOAT || ab == As.SNOW_CLOAK) continue;
                pm.EffectHurtByOneNth(16, "HailstormHurt");
                pm.CheckFaint();
              }
            }
            break;
          case Game.Weather.HeavyRain:
            foreach (var pm in c.OnboardPokemons)
              if (pm.CanHpRecover())
                if (pm.RaiseAbility(As.RAIN_DISH)) pm.HpRecoverByOneNth(16);
                else if (pm.RaiseAbility(As.DRY_SKIN)) pm.HpRecoverByOneNth(8);
            break;
          case Game.Weather.IntenseSunlight:
            foreach (var pm in c.OnboardPokemons.ToArray())
              if (pm.RaiseAbility(As.SOLAR_POWER) || pm.RaiseAbility(As.DRY_SKIN))
              {
                pm.EffectHurtByOneNth(8);
                pm.CheckFaint();
              }
            break;
        }
      }
    }
    //3.0 [unfinished] Future Sight, Doom Desire
    private void FSDD(Controller c)
    {
    }
    //4.0 Wish
    private void Wish(Controller c)
    {
      foreach (var pm in c.OnboardPokemons)
      {
        var o = pm.Tile.GetCondition("Wish");
        if (o != null && o.Turn == c.TurnNumber)
        {
          pm.Tile.RemoveCondition("Wish");
          pm.HpRecover(o.Int, false, "Wish");
        }
      }
    }
    //5.0 [pass] Fire Pledge + Grass Pledge damage
    //5.1 Shed Skin, Hydration, Healer
    //5.2 Leftovers, Black Sludge
    private void PropertyChange(Controller c)
    {
      bool hydration = c.Weather == Game.Weather.HeavyRain;
      foreach (var pm in c.OnboardPokemons.ToArray())
      {
        switch (pm.Ability.Id)
        {
          case As.SHED_SKIN:
            if (pm.State != Game.PokemonState.Normal && c.RandomHappen(30))
            {
              pm.RaiseAbility();
              pm.DeAbnormalState();
            }
            break;
          case As.HYDRATION:
            if (hydration && pm.State != Game.PokemonState.Normal)
            {
              pm.RaiseAbility();
              pm.DeAbnormalState();
            }
            break;
          case As.HEALER:
            var ps = new List<PokemonProxy>();
            foreach (var p in c.Board[pm.Pokemon.TeamId].Pokemons)
              if (p != pm && p.State != Game.PokemonState.Normal) ps.Add(p);
            if (ps.Count != 0 && c.RandomHappen(30))
            {
              pm.RaiseAbility();
              ps[c.GetRandomInt(0, ps.Count - 1)].DeAbnormalState();
            }
            break;
        }
        switch (pm.Item.Id)
        {
          case Is.LEFTOVERS:
            pm.HpRecoverByOneNth(16, false, "ItemRecover2", Is.LEFTOVERS);
            break;
          case Is.BLACK_SLUDGE:
            if (pm.OnboardPokemon.HasType(BattleType.Poison))
            {
              if (pm.CanHpRecover()) pm.HpRecoverByOneNth(16, false, "ItemRecover2", Is.BLACK_SLUDGE);
            }
            else pm.EffectHurtByOneNth(8, "ItemHurt", Is.BLACK_SLUDGE);
            break;
        }
        pm.CheckFaint();
      }
    }
    //6.0 Aqua Ring
    //7.0 Ingrain
    //8.0 Leech Seed
    private void HpRecover(Controller c)
    {
      foreach (var pm in c.OnboardPokemons)
        if (pm.OnboardPokemon.HasCondition("AquaRing"))
        {
          int hp = pm.Pokemon.Hp.Origin;
          if (pm.Item.BigRoot()) hp = (int)(hp * 1.3);
          hp /= 16;
          pm.HpRecover(hp, false, "AquaRing");
        }
      foreach (var pm in c.OnboardPokemons)
        if (pm.OnboardPokemon.HasCondition("Ingrain"))
        {
          int hp = pm.Pokemon.Hp.Origin;
          if (pm.Item.BigRoot()) hp = (int)(hp * 1.3);
          hp /= 16;
          pm.HpRecover(hp, false, "Ingrain");
        }
      foreach (var pm in c.OnboardPokemons.ToArray())
      {
        var tile = pm.OnboardPokemon.GetCondition<Tile>("LeechSeed");
        if (tile != null && tile.Pokemon != null)
        {
          var hp = pm.Hp;
          pm.EffectHurtByOneNth(8, "LeechSeed");
          hp -= pm.Hp;
          var recover = tile.Pokemon;
          if (hp > 0 && recover.CanHpRecover())
          {
            if (recover.Item.BigRoot()) hp = (int)(hp * 1.3);
            if (!recover.Ability.MagicGuard() && pm.RaiseAbility(As.LIQUID_OOZE))
            {
              recover.EffectHurt(hp);
              recover.CheckFaint();
            }
            else recover.HpRecover(hp);
          }
        }
        pm.CheckFaint();
      }
    }
    //9.0 (bad) poison damage, burn damage, Poison Heal
    //9.1 Nightmare
    private void PmState(Controller c)
    {
      foreach (var pm in c.OnboardPokemons.ToArray())
      {
        switch (pm.State)
        {
          case PokemonState.BadlyPSN:
            if (pm.CanHpRecover() && pm.RaiseAbility(As.POISON_HEAL)) pm.HpRecoverByOneNth(8, false, "PoisonHeal");
            else
            {
              int turn = 1 + c.TurnNumber - pm.OnboardPokemon.GetCondition<int>("PSN");
              int hp = pm.Pokemon.Hp.Origin * (turn > 15 ? 15 : turn) / 16;
              pm.EffectHurt(hp, "PSN");
            }
            break;
          case PokemonState.PSN:
            if (pm.CanHpRecover() && pm.RaiseAbility(As.POISON_HEAL)) pm.HpRecoverByOneNth(8, false, "PoisonHeal");
            else pm.EffectHurtByOneNth(8, "PSN");
            break;
         case PokemonState.BRN:
            pm.EffectHurtByOneNth(8, "BRN");
            break;
          case PokemonState.SLP:
            if (pm.OnboardPokemon.HasCondition("Nightmare")) pm.EffectHurtByOneNth(4, "Nightmare");
            break;
        }
        pm.CheckFaint();
      }
    }
    //10.0 Curse (from a Ghost-type)
    private void Curse(Controller c)
    {
      foreach (var pm in c.OnboardPokemons)
        if (pm.OnboardPokemon.HasCondition("Curse"))
        {
          pm.EffectHurtByOneNth(4, "Curse");
          pm.CheckFaint();
        }
    }
    //11.0 Bind, Wrap, Fire Spin, Clamp, Whirlpool, Sand Tomb, Magma Storm
    private void Trap(Controller c)
    {
      foreach (var pm in c.OnboardPokemons.ToArray())
      {
        var trap = pm.OnboardPokemon.GetCondition("Trap");
        if (trap != null)
          if (trap.Turn == c.TurnNumber)
          {
            pm.OnboardPokemon.RemoveCondition("Trap");
            pm.AddReportPm("TrapFree", trap.Move.Id);
          }
          else
          {
            pm.EffectHurtByOneNth((bool)trap.Bool ? 4 : 8, "TrapHurt", trap.Move.Id);
            pm.CheckFaint();
          }
      }
    }
    //12.0 Taunt ends
    //13.0 Encore ends
    //14.0 Disable ends, Cursed Body ends
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
          pm.AddReportPm("DeTaunt");
        }
      }
      foreach (var pm in c.OnboardPokemons)
      {
        var o = pm.OnboardPokemon.GetCondition("Encore");
        if (o != null && o.Turn == 0)
        {
          pm.OnboardPokemon.RemoveCondition("Encore");
          pm.AddReportPm("DeEncore");
        }
      }
      foreach (var pm in c.OnboardPokemons)
      {
        var d = pm.OnboardPokemon.GetCondition("Disable");
        if (d != null && d.Turn == c.TurnNumber)
        {
          pm.OnboardPokemon.RemoveCondition("Disable");
          pm.AddReportPm("DeDisable");
        }
      }
      foreach (var pm in c.OnboardPokemons)
        if (pm.OnboardPokemon.GetCondition<int>("MagnetRise") == c.TurnNumber)
        {
          pm.OnboardPokemon.RemoveCondition("MagnetRise");
          pm.AddReportPm("DeMagnetRise");
        }
      foreach (var pm in c.OnboardPokemons)
        if (pm.OnboardPokemon.GetCondition<int>("Telekinesis") == c.TurnNumber)
        {
          pm.OnboardPokemon.RemoveCondition("Telekinesis");
          pm.AddReportPm("DeTelekinesis");
        }
      foreach (var pm in c.OnboardPokemons)
      {
        var o = pm.OnboardPokemon.GetCondition("HealBlock");
        if (o != null && o.Turn == c.TurnNumber)
        {
          pm.OnboardPokemon.RemoveCondition("HealBlock");
          pm.AddReportPm("DeHealBlock");
        }
      }
      foreach (var pm in c.OnboardPokemons)
        if (pm.OnboardPokemon.GetCondition<int>("Embargo") == c.TurnNumber)
        {
          pm.OnboardPokemon.RemoveCondition("Embargo");
          pm.AddReportPm("DeEmbargo");
          pm.Item.Attach(pm);
        }
      foreach (var pm in c.OnboardPokemons)
      {
        var o = pm.OnboardPokemon.GetCondition("Yawn");
        if (o != null && o.Turn == c.TurnNumber)
        {
          pm.AddState(o.By, AttachedState.SLP, false);
          pm.OnboardPokemon.RemoveCondition("Yawn");
        }
      }
      foreach (var pm in c.OnboardPokemons.ToArray())
      {
        int turn = pm.OnboardPokemon.GetCondition<int>("PerishSong", -1);
        if (turn == 0)
        {
          pm.OnboardPokemon.RemoveCondition("PerishSong");
          pm.Pokemon.SetHp(0);
          c.ReportBuilder.Add(new HpChange(pm, "DePerishSong"));
          pm.CheckFaint();
        }
        else if (turn != -1)
        {
          pm.AddReportPm("PerishSong", turn);
          pm.OnboardPokemon.SetCondition("PerishSong", turn - 1);
        }
      }
    }
    //21.0 Reflect ends
    //21.1 Light Screen ends
    //21.2 Safeguard ends
    //21.3 Mist ends
    //21.4 Tailwind ends
    //21.5 Lucky Chant ends
    //21.6 [pass] Water Pledge + Fire Pledge ends, Fire Pledge + Grass Pledge ends, Grass Pledge + Water Pledge ends
    private void FieldCondition(Controller c)
    {
      foreach (var f in c.Board.Fields)
      {
        FieldCondition("Reflect", f, c);
        FieldCondition("LightScreen", f, c);
        FieldCondition("Safeguard", f, c);
        FieldCondition("Mist", f, c);
        FieldCondition("Tailwind", f, c);
        FieldCondition("LuckyChant", f, c);
      }
    }
    private void FieldCondition(string condition, Field f, Controller c)
    {
      if (f.GetCondition<int>(condition) == c.TurnNumber)
      {
        f.RemoveCondition(condition);
        c.ReportBuilder.Add("De" + condition, f.TeamId);
      }
    }
    //22.0 Gravity ends
    //23.0 Trick Room ends
    //24.0 Wonder Room ends
    //25.0 Magic Room ends
    private void BoardCondition(Controller c)
    {
      var board = c.Board;
      int turn = c.TurnNumber;
      if (board.GetCondition<int>("Gravity") == turn)
      {
        board.RemoveCondition("Gravity");
        c.ReportBuilder.Add("DeGravity");
      }
      if (board.GetCondition<int>("TrickRoom") == turn)
      {
        board.RemoveCondition("TrickRoom");
        c.ReportBuilder.Add("DeTrickRoom");
      }
      if (board.GetCondition<int>("WonderRoom") == turn)
      {
        foreach (var pm in c.OnboardPokemons)
        {
          var stats = pm.OnboardPokemon.FiveD;
          var d = stats.Def;
          stats.Def = stats.SpDef;
          stats.SpDef = d;
        }
        board.RemoveCondition("WonderRoom");
        c.ReportBuilder.Add("DeWonderRoom");
      }
      if (board.GetCondition<int>("MagicRoom") == turn)
      {
        board.RemoveCondition("MagicRoom");
        c.ReportBuilder.Add("DeMagicRoom");
        foreach (var pm in c.OnboardPokemons) pm.Item.Attach(pm);
      }
    }
    //26.0 [unfinished] Uproar message
    //26.1 Speed Boost, Bad Dreams, Harvest, Moody
    //26.2 Toxic Orb activation, Flame Orb activation, Sticky Barb
    //26.3 pickup
    private void Pokemon(Controller c)
    {
      foreach (var pm in c.OnboardPokemons.ToArray())
      {
        int ab = pm.Ability.Id;
        switch (ab)
        {
          case As.SPEED_BOOST:
            if (pm.LastMoveTurn != 0 && pm.CanChangeLv7D(pm, StatType.Speed, 1, false) != 0)
            {
              pm.RaiseAbility();
              pm.ChangeLv7D(pm, StatType.Speed, 1, false);
            }
            break;
          case As.BAD_DREAMS:
            {
              bool first = true;
              foreach (var target in c.Board[1 - pm.Pokemon.TeamId].Pokemons)
                if (target.State == PokemonState.SLP)
                {
                  if (first)
                  {
                    pm.RaiseAbility();
                    first = false;
                  }
                  target.EffectHurtByOneNth(8, "BadDreams");
                }
            }
            break;
          case As.HARVEST:
            if (pm.Pokemon.Item == null)
            {
              var i = c.Board[pm.Pokemon.TeamId].GetCondition<Item>("UsedBerry" + pm.Id);
              if (i != null && c.Weather == Game.Weather.IntenseSunlight || c.RandomHappen(50))
              {
                pm.RaiseAbility();
                pm.ChangeItem(i.Id, "Harvest");
              }
            }
            break;
          case As.MOODY:
            {
              StatType[] up = SEVEN_D.Where((s) => pm.CanChangeLv7D(pm, s, 2, false) != 0).ToArray();
              StatType[] down = SEVEN_D.Where((s) => pm.CanChangeLv7D(pm, s, -1, false) != 0).ToArray();
              pm.RaiseAbility();
              if (up.Length != 0) pm.ChangeLv7D(pm, up[c.GetRandomInt(0, up.Length)], 2, false);
              if (down.Length != 0) pm.ChangeLv7D(pm, down[c.GetRandomInt(0, down.Length)], -1, false);
            }
            break;
        }
        switch (pm.Item.Id)
        {
          case Is.TOXIC_ORB:
            pm.AddState(pm, AttachedState.PSN, false, 15, "ItemEnBadlyPSN", Is.TOXIC_ORB);
            break;
          case Is.FLAME_ORB:
            pm.AddState(pm, AttachedState.BRN, false, 0, "ItemEnBRN", Is.FLAME_ORB);
            break;
          case Is.STICKY_BARB:
            pm.EffectHurtByOneNth(8, "ItemHurt", Is.STICKY_BARB);
            break;
        }
        if (ab == As.PICKUP && pm.Pokemon.Item == null)
        {
          var items = new List<Item>();
          foreach (var p in c.Board[1 - pm.Pokemon.TeamId].Pokemons)
          {
            var i = p.OnboardPokemon.GetCondition<Item>("UsedItem");
            if (i != null) items.Add(i);
          }
          if (!items.Any())
            foreach (var p in c.Board[pm.Pokemon.TeamId].Pokemons)
              if (p != pm)
              {
                var i = p.OnboardPokemon.GetCondition<Item>("UsedItem");
                if (i != null) items.Add(i);
              }
          if (items.Any())
          {
            var item = items[c.GetRandomInt(0, items.Count - 1)].Id;
            pm.RaiseAbility();
            pm.ChangeItem(item, "Pickup");
          }
        }
        pm.CheckFaint();
      }
    }
    //27.0 Zen Mode
    private void ZenMode(Controller c)
    {
    }
  }
}
