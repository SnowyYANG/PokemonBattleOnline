using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonBattleOnline.Game;
using PokemonBattleOnline.Game.GameEvents;

namespace PokemonBattleOnline.Game.Host.Triggers
{
  internal static class EndTurn
  {
    public static void Execute(Controller c)
    {
      Weather(c); if (!c.CanContinue) return;
      FSDD(c); if (!c.CanContinue) return;
      Wish(c); if (!c.CanContinue) return;
      PropertyChange(c); if (!c.CanContinue) return;
      HpRecover(c); if (!c.CanContinue) return;
      PmState(c); if (!c.CanContinue) return;
      Curse(c); if (!c.CanContinue) return;
      Trap(c); if (!c.CanContinue) return;
      PokemonCondition(c); if (!c.CanContinue) return;
      FieldCondition(c); if (!c.CanContinue) return;
      BoardCondition(c); if (!c.CanContinue) return;
      Pokemon(c); if (!c.CanContinue) return;
      ZenMode(c);
    }
    //1.0 weather ends, Sandstorm/Hail damage, Rain Dish/Dry Skin/Ice Body/SolarPower
    private static void Weather(Controller c)
    {
      if (c.Board.GetCondition<int>("Weather") == c.TurnNumber)
      {
        c.Weather = Game.Weather.Normal;
        c.Board.RemoveCondition("Weather");
      }
      else
      {
        if (c.Board.Weather == Game.Weather.Sandstorm) c.ReportBuilder.ShowLog("Sandstorm");
        else if (c.Board.Weather == Game.Weather.Hailstorm) c.ReportBuilder.ShowLog("Hailstorm");
        switch (c.Weather)
        {
          case Game.Weather.Sandstorm:
            foreach (var pm in c.OnboardPokemons.ToArray())
            {
              var types = pm.OnboardPokemon.Types;
              if (types.Contains(BattleType.Rock) || types.Contains(BattleType.Steel) || types.Contains(BattleType.Ground) || pm.Item == Is.SAFETY_GOGGLES) continue;
              int ab = pm.Ability;
              if (ab == As.OVERCOAT || ab == As.SAND_VEIL || ab == As.SAND_RUSH || ab == As.SAND_FORCE) continue;
              pm.EffectHurtByOneNth(16, "m_SandstormHurt");
              pm.CheckFaint();
            }
            break;
          case Game.Weather.Hailstorm:
            foreach (var pm in c.OnboardPokemons.ToArray())
            {
              int ab = pm.Ability;
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
                if (ab == As.OVERCOAT || ab == As.SNOW_CLOAK || pm.OnboardPokemon.HasType(BattleType.Ice)) continue;
                pm.EffectHurtByOneNth(16, "m_HailstormHurt");
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
    //3.0 Future Sight, Doom Desire
    private static void FSDD(Controller c)
    {
      foreach (var t in c.Board.Tiles)
      {
        var o = t.GetCondition("FSDD");
        if (o != null && o.Turn == c.TurnNumber)
        {
          t.RemoveCondition("FSDD");
          if (t.Pokemon != null) o.Atk.StartExecute(o.Atk.GetCondition<MoveType>("FSDD"), t, "FSDD");
        }
      }
    }
    //4.0 Wish
    private static void Wish(Controller c)
    {
      foreach (var t in c.Tiles)
      {
        var o = t.GetCondition("Wish");
        if (o != null && o.Turn == c.TurnNumber)
        {
          t.RemoveCondition("Wish");
          if (t.Pokemon != null) t.Pokemon.HpRecover(o.Int, false, "m_Wish");
        }
      }
    }
    //5.0 [pass] Fire Pledge + Grass Pledge damage
    //5.1 Shed Skin, Hydration, Healer
    //5.2 Leftovers, Black Sludge
    private static void PropertyChange(Controller c)
    {
      if (c.Board.HasCondition("GrassyTerrain"))
      {
        foreach (var pm in c.OnboardPokemons)
          if (HasEffect.IsGroundAffectable(pm, true, false)) pm.HpRecoverByOneNth(16);
      }
      bool? hydration = null;
      foreach (var pm in c.OnboardPokemons.ToArray())
      {
        switch (pm.Ability)
        {
          case As.SHED_SKIN:
            if (pm.State != Game.PokemonState.Normal && c.RandomHappen(30))
            {
              pm.RaiseAbility();
              pm.DeAbnormalState();
            }
            break;
          case As.HYDRATION:
            if (pm.State != Game.PokemonState.Normal)
            {
              if (hydration == null) hydration = c.Weather == Game.Weather.HeavyRain;
              if (hydration == true)
              {
                pm.RaiseAbility();
                pm.DeAbnormalState();
              }
            }
            break;
          case As.HEALER:
            var ps = new List<PokemonProxy>();
            foreach (var p in pm.Field.Pokemons)
              if (p != pm && p.State != Game.PokemonState.Normal) ps.Add(p);
            if (ps.Count != 0 && c.RandomHappen(30))
            {
              pm.RaiseAbility();
              ps[c.GetRandomInt(0, ps.Count - 1)].DeAbnormalState();
            }
            break;
        }
        switch (pm.Item)
        {
          case Is.LEFTOVERS:
            pm.HpRecoverByOneNth(16, false, "m_ItemRecover2", Is.LEFTOVERS);
            break;
          case Is.BLACK_SLUDGE:
            if (pm.OnboardPokemon.HasType(BattleType.Poison))
            {
              if (pm.CanHpRecover()) pm.HpRecoverByOneNth(16, false, "m_ItemRecover2", Is.BLACK_SLUDGE);
            }
            else pm.EffectHurtByOneNth(8, "m_ItemHurt", Is.BLACK_SLUDGE);
            break;
        }
        pm.CheckFaint();
      }
    }
    //6.0 Aqua Ring
    //7.0 Ingrain
    //8.0 Leech Seed
    private static void HpRecover(Controller c)
    {
      foreach (var pm in c.OnboardPokemons)
        if (pm.OnboardPokemon.HasCondition("AquaRing"))
        {
          int hp = pm.Pokemon.MaxHp;
          if (pm.Item == Is.BIG_ROOT) hp = (int)(hp * 1.3);
          hp /= 16;
          pm.HpRecover(hp, false, "m_AquaRing");
        }
      foreach (var pm in c.OnboardPokemons)
        if (pm.OnboardPokemon.HasCondition("Ingrain"))
        {
          int hp = pm.Pokemon.MaxHp;
          if (pm.Item == Is.BIG_ROOT) hp = (int)(hp * 1.3);
          hp /= 16;
          pm.HpRecover(hp, false, "m_Ingrain");
        }
      foreach (var pm in c.OnboardPokemons.ToArray())
      {
        var tile = pm.OnboardPokemon.GetCondition<Tile>("LeechSeed");
        if (tile != null && tile.Pokemon != null)
        {
          var hp = pm.Hp;
          pm.EffectHurtByOneNth(8, "m_LeechSeed");
          hp -= pm.Hp;
          var recover = tile.Pokemon;
          if (hp > 0 && recover.CanHpRecover())
          {
            if (recover.Item == Is.BIG_ROOT) hp = (int)(hp * 1.3);
            if (recover.Ability != As.MAGIC_GUARD && pm.RaiseAbility(As.LIQUID_OOZE))
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
    private static void PmState(Controller c)
    {
      foreach (var pm in c.OnboardPokemons.ToArray())
      {
        switch (pm.State)
        {
          case PokemonState.BadlyPSN:
          case PokemonState.PSN:
            var id = pm.Ability;
            if (id == As.POISON_HEAL)
            {
              if (pm.CanHpRecover())
              {
                pm.RaiseAbility();
                pm.HpRecoverByOneNth(8, false, "m_PoisonHeal");
              }
            }
            else if (pm.State == PokemonState.BadlyPSN)
            {
              int turn = 1 + c.TurnNumber - pm.OnboardPokemon.GetCondition<int>("PSN");
              int hp = pm.Pokemon.MaxHp * (turn > 15 ? 15 : turn) / 16;
              pm.EffectHurt(hp, "m_PSN");
            }
            else pm.EffectHurtByOneNth(8, "m_PSN");
            break;
         case PokemonState.BRN:
            pm.EffectHurtByOneNth(8, "m_BRN");
            break;
          case PokemonState.SLP:
            if (pm.OnboardPokemon.HasCondition("Nightmare")) pm.EffectHurtByOneNth(4, "m_Nightmare");
            break;
        }
        pm.CheckFaint();
      }
    }
    //10.0 Curse (from a Ghost-type)
    private static void Curse(Controller c)
    {
      foreach (var pm in c.OnboardPokemons)
        if (pm.OnboardPokemon.HasCondition("Curse"))
        {
          pm.EffectHurtByOneNth(4, "m_Curse");
          pm.CheckFaint();
        }
    }
    //11.0 Bind, Wrap, Fire Spin, Clamp, Whirlpool, Sand Tomb, Magma Storm
    private static void Trap(Controller c)
    {
      foreach (var pm in c.OnboardPokemons.ToArray())
      {
        var trap = pm.OnboardPokemon.GetCondition("Trap");
        if (trap != null)
          if (trap.Turn == c.TurnNumber)
          {
            pm.OnboardPokemon.RemoveCondition("Trap");
            pm.ShowLogPm("TrapFree", trap.Move.Id);
          }
          else
          {
            pm.EffectHurtByOneNth(trap.Bool ? 6 : 8, "m_TrapHurt", trap.Move.Id);
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
    private static void PokemonCondition(Controller c)
    {
      foreach (var pm in c.OnboardPokemons)
      {
        int turn = pm.OnboardPokemon.GetCondition("Taunt", -1);
        if (turn == 0)
        {
          pm.OnboardPokemon.RemoveCondition("Taunt");
          pm.ShowLogPm("DeTaunt");
        }
      }
      foreach (var pm in c.OnboardPokemons)
      {
        var o = pm.OnboardPokemon.GetCondition("Encore");
        if (o != null && o.Turn == 0)
        {
          pm.OnboardPokemon.RemoveCondition("Encore");
          pm.ShowLogPm("DeEncore");
        }
      }
      foreach (var pm in c.OnboardPokemons)
      {
        var o = pm.OnboardPokemon.GetCondition("Disable");
        if (o != null && o.Turn == c.TurnNumber)
        {
          pm.OnboardPokemon.RemoveCondition("Disable");
          pm.ShowLogPm("DeDisable");
        }
      }
      foreach (var pm in c.OnboardPokemons)
        if (pm.OnboardPokemon.GetCondition<int>("MagnetRise") == c.TurnNumber)
        {
          pm.OnboardPokemon.RemoveCondition("MagnetRise");
          pm.ShowLogPm("DeMagnetRise");
        }
      foreach (var pm in c.OnboardPokemons)
        if (pm.OnboardPokemon.GetCondition<int>("Telekinesis") == c.TurnNumber)
        {
          pm.OnboardPokemon.RemoveCondition("Telekinesis");
          pm.ShowLogPm("DeTelekinesis");
        }
      foreach (var pm in c.OnboardPokemons)
      {
        var o = pm.OnboardPokemon.GetCondition("HealBlock");
        if (o != null && o.Turn == c.TurnNumber)
        {
          pm.OnboardPokemon.RemoveCondition("HealBlock");
          pm.ShowLogPm("DeHealBlock");
        }
      }
      foreach (var pm in c.OnboardPokemons)
        if (pm.OnboardPokemon.GetCondition<int>("Embargo") == c.TurnNumber)
        {
          pm.OnboardPokemon.RemoveCondition("Embargo");
          pm.ShowLogPm("DeEmbargo");
          ITs.Attach(pm);
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
        if (turn != -1)
        {
          pm.ShowLogPm("PerishSong", turn);
          if (turn == 0) pm.Faint();
          else pm.OnboardPokemon.SetCondition("PerishSong", turn - 1);
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
    private static void FieldCondition(Controller c)
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
    private static void FieldCondition(string condition, Field f, Controller c)
    {
      if (f.GetCondition<int>(condition) == c.TurnNumber)
      {
        f.RemoveCondition(condition);
        c.ReportBuilder.ShowLog("De" + condition, f.Team);
      }
    }
    //22.0 Gravity ends
    //23.0 Trick Room ends
    //24.0 Wonder Room ends
    //25.0 Magic Room ends
    private static void BoardCondition(Controller c)
    {
      var board = c.Board;
      int turn = c.TurnNumber;
      if (board.GetCondition<int>("Gravity") == turn)
      {
        board.RemoveCondition("Gravity");
        c.ReportBuilder.ShowLog("DeGravity");
      }
      if (board.GetCondition<int>("TrickRoom") == turn)
      {
        board.RemoveCondition("TrickRoom");
        c.ReportBuilder.ShowLog("DeTrickRoom");
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
        c.ReportBuilder.ShowLog("DeWonderRoom");
      }
      if (board.GetCondition<int>("MagicRoom") == turn)
      {
        board.RemoveCondition("MagicRoom");
        c.ReportBuilder.ShowLog("DeMagicRoom");
        foreach (var pm in c.OnboardPokemons) ITs.Attach(pm);
      }
      var t = board.GetCondition<int>("GrassyTerrain"); 
      if (t == turn)
      {
        board.RemoveCondition("GrassyTerrain");
        c.ReportBuilder.ShowLog("DeGrassyTerrain");
      }
      else if (t == 0)
      {
        t = board.GetCondition<int>("ElectricTerrain");
        if (t == turn)
        {
          board.RemoveCondition("ElectricTerrain");
          c.ReportBuilder.ShowLog("DeElectricTerrain");
        }
        else if (t == 0 && board.GetCondition<int>("MistyTerrain") == turn)
        {
          board.RemoveCondition("MistyTerrain");
          c.ReportBuilder.ShowLog("DeMistyTerrain");
        }
      }
    }
    //26.0 Uproar message
    //26.1 Speed Boost, Bad Dreams, Harvest, Moody
    //26.2 Toxic Orb activation, Flame Orb activation, Sticky Barb
    //26.3 pickup
    private static void Pokemon(Controller c)
    {
      foreach (var pm in c.OnboardPokemons.ToArray())
      {
        int ab = pm.Ability;
        if (pm.AtkContext != null && pm.AtkContext.Move.Id == Ms.UPROAR)
        {
          if (pm.Action == PokemonAction.Moving)  pm.ShowLogPm("Uproar");
          else if (pm.AtkContext.GetCondition("MultiTurn").Turn == 0)
          {
            pm.AtkContext.RemoveCondition("MultiTurn");
            pm.ShowLogPm("DeUproar");
          }
        }
        switch (ab)
        {
          case As.SPEED_BOOST:
            if (pm.LastMoveTurn != 0) pm.ChangeLv7D(pm, StatType.Speed, 1, false, true);
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
                  target.EffectHurtByOneNth(8, "m_BadDreams");
                }
            }
            break;
          case As.HARVEST:
            if (pm.Pokemon.Item == 0)
            {
              var i = pm.Field.GetCondition<int>("UsedBerry" + pm.Id);
              if (i != 0 && c.Weather == Game.Weather.IntenseSunlight || c.RandomHappen(50))
              {
                pm.RaiseAbility();
                pm.SetItem(i);
                pm.ShowLogPm("Harvest", i);
                ITs.Attach(pm);
              }
            }
            break;
          case As.MOODY:
            {
              var up = new List<StatType>(7);
              var down = new List<StatType>(7);
              foreach (var s in StatHelper.SEVEN_D)
              {
                if (pm.CanChangeLv7D(pm, s, 2, false) != 0) up.Add(s);
                if (pm.CanChangeLv7D(pm, s, -1, false) != 0) down.Add(s);
              }
              if (up.Count != 0 && down.Count != 0)
              {
                pm.RaiseAbility();
                if (up.Count != 0) pm.ChangeLv7D(pm, up[c.GetRandomInt(0, up.Count - 1)], 2, false);
                if (down.Count != 0) pm.ChangeLv7D(pm, down[c.GetRandomInt(0, down.Count - 1)], -1, false);
              }
            }
            break;
        }
        switch (pm.Item)
        {
          case Is.TOXIC_ORB:
            pm.AddState(pm, AttachedState.PSN, false, 15, "ItemEnBadlyPSN", Is.TOXIC_ORB);
            break;
          case Is.FLAME_ORB:
            pm.AddState(pm, AttachedState.BRN, false, 0, "ItemEnBRN", Is.FLAME_ORB);
            break;
          case Is.STICKY_BARB:
            pm.EffectHurtByOneNth(8, "m_ItemHurt", Is.STICKY_BARB);
            break;
        }
        if (ab == As.PICKUP && pm.Pokemon.Item == 0)
        {
          var items = new List<int>();
          var owners = new List<OnboardPokemon>();
          foreach (var p in c.Board[1 - pm.Pokemon.TeamId].Pokemons)
          {
            var i = p.OnboardPokemon.GetCondition<int>("UsedItem");
            if (i != 0)
            {
              items.Add(i);
              owners.Add(p.OnboardPokemon);
            }
          }
          if (!items.Any())
            foreach (var p in pm.Field.Pokemons)
              if (p != pm)
              {
                var i = p.OnboardPokemon.GetCondition<int>("UsedItem");
                if (i != 0)
                {
                  items.Add(i);
                  owners.Add(p.OnboardPokemon);
                }
              }
          if (items.Any())
          {
            var i = c.GetRandomInt(0, items.Count - 1);
            owners[i].RemoveCondition("UsedItem");
            pm.RaiseAbility();
            pm.SetItem(items[i]);
            pm.ShowLogPm("Pickup", items[i]);
            ITs.Attach(pm);
          }
        }
        pm.CheckFaint();
      }
    }
    //27.0 Zen Mode
    private static void ZenMode(Controller c)
    {
      foreach (var pm in c.OnboardPokemons)
      {
        var form = pm.Hp << 1 <= pm.Pokemon.MaxHp ? 1 : 0;
        if (pm.CanChangeForm(555, form) && pm.RaiseAbility(As.ZEN_MODE)) pm.ChangeForm(form, false, form == 0 ? "DnZenMode" : "EnZenMode");
      }
    }
  }
}
