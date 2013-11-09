using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonBattleOnline.Game;
using PokemonBattleOnline.Game.GameEvents;

namespace PokemonBattleOnline.Game.Host.Triggers
{
  internal static class AbilityAttach
  {
    public static void Execute(PokemonProxy pm)
    {
      switch (pm.Ability)
      {
        case As.LIMBER: //7
          CantAddState(pm, PokemonState.PAR);
          break;
        case As.INSOMNIA: //15
        case As.VITAL_SPIRIT: //72
          CantAddState(pm, PokemonState.SLP);
          break;
        case As.MAGMA_ARMOR: //40
          CantAddState(pm, PokemonState.FRZ);
          break;
        case As.WATER_VEIL: //41
          CantAddState(pm, PokemonState.BRN);
          break;
        case As.OBLIVIOUS: //12
          if (pm.OnboardPokemon.RemoveCondition("Attract"))
          {
            pm.RaiseAbility();
            pm.ShowLogPm("DeAttract");
          }
          break;
        case As.IMMUNITY: //17
          if (pm.State == PokemonState.PSN || pm.State == PokemonState.BadlyPSN)
          {
            pm.RaiseAbility();
            pm.DeAbnormalState();
          }
          break;
        case As.OWN_TEMPO: //20
          if (pm.OnboardPokemon.RemoveCondition("Confuse"))
          {
            pm.RaiseAbility();
            pm.ShowLogPm("DeConfuse");
          }
          break;
        case As.IMPOSTER:
          Imposter(pm);
          break;
        case As.FRISK:
          Frisk(pm);
          break;
        case As.FLOWER_GIFT:
          WeatherObserver(pm, 421, pm.Controller.Weather == Weather.IntenseSunlight ? 1 : 0);
          break;
        case As.FORECAST:
          {
            var form = (int)pm.Controller.Weather;
            WeatherObserver(pm, 351, form > 3 ? 0 : form);
          }
          break;
        case As.FOREWARN:
          Forewarn(pm);
          break;
        case As.FLASH_FIRE:
          pm.OnboardPokemon.RemoveCondition("FlashFire");
          break;
        case As.AIR_LOCK:
          pm.RaiseAbility();
          pm.Controller.ReportBuilder.ShowLog("AirLock");
          if (pm.Controller.Board.Weather != Weather.Normal) ATs.WeatherChanged(pm.Controller);
          break;
        case As.DRIZZLE:
          WeatherAbility(pm, Weather.HeavyRain);
          break;
        case As.DROUGHT:
          WeatherAbility(pm, Weather.IntenseSunlight);
          break;
        case As.SAND_STREAM:
          WeatherAbility(pm, Weather.Sandstorm);
          break;
        case As.SNOW_WARNING:
          WeatherAbility(pm, Weather.Hailstorm);
          break;
        case As.INTIMIDATE:
          pm.RaiseAbility();
          foreach (var p in pm.Controller.Board[1 - pm.Pokemon.TeamId].GetPokemons(pm.OnboardPokemon.X - 1, pm.OnboardPokemon.X + 1))
            if (p.OnboardPokemon.HasCondition("Substitute")) p.ShowLogPm("NoEffect");
            else p.ChangeLv7D(pm, StatType.Atk, -1, true);
          break;
        case As.TRACE:
          Trace(pm);
          break;
        case As.PRESSURE:
          SimpleAttachRaise(pm, "Pressure");
          break;
        case As.MOLD_BREAKER:
          SimpleAttachRaise(pm, "MoldBreaker");
          break;
        case As.TURBOBLAZE:
          SimpleAttachRaise(pm, "Turboblaze");
          break;
        case As.TERAVOLT:
          SimpleAttachRaise(pm, "Teravolt");
          break;
        case As.DOWNLOAD:
          Download(pm);
          break;
        case As.ANTICIPATION:
          Anticipation(pm);
          break;
        case As.SLOW_START:
          pm.OnboardPokemon.SetCondition("SlowStart", pm.Controller.TurnNumber + 5);
          pm.RaiseAbility();
          pm.ShowLogPm("EnSlowStart");
          break;
      }
    }
    private static void Imposter(PokemonProxy pm)
    {
      var t = pm.Controller.Board[1 - pm.Pokemon.TeamId][pm.OnboardPokemon.X].Pokemon;
      if (pm.CanTransform(t))
      {
        pm.RaiseAbility();
        pm.Transform(t);
      }
    }
    private static void Frisk(PokemonProxy pm)
    {
      var pms = pm.Controller.GetOnboardPokemons(1 - pm.Pokemon.TeamId);
      var items = new List<int>();
      foreach (var p in pms)
        if (p.Pokemon.Item != 0) items.Add(p.Pokemon.Item);
      if (items.Count == 0) return;
      int i = pm.Controller.GetRandomInt(0, items.Count - 1);
      pm.RaiseAbility();
      pm.ShowLogPm("Frisk", items[i]);
    }
    private static void WeatherObserver(PokemonProxy pm, int number, int form)
    {
      if (pm.CanChangeForm(number, form))
      {
        //pm.OnboardPokemon.SetCondition("ObserveWeather"); Sp.Abilities.AttachWeatherObserver
        pm.RaiseAbility();
        pm.ChangeForm(form);
      }
    }
    private static void Forewarn(PokemonProxy pm)
    {
      List<KeyValuePair<PokemonProxy, MoveType>> moves = new List<KeyValuePair<PokemonProxy, MoveType>>();
      {
        int maxPower = 0;
        foreach (PokemonProxy p in pm.Controller.Board[1 - pm.Pokemon.TeamId].GetPokemons(pm.OnboardPokemon.X - 1, pm.OnboardPokemon.X + 1))
          foreach (MoveProxy m in p.Moves)
          {
            int power = m.Type.Power;
            if (power == 1)
            {
              if (m.Type.Class == MoveInnerClass.OHKO) power = 160;
              else
              {
                var mid = m.Type.Id;
                if (mid == 382) power = 0; //先取
                else if (mid == 68 || mid == 243 || mid == 368) power = 120;
              }
            }
            if (power > maxPower)
            {
              moves.Clear();
              maxPower = power;
            }
            if (power == maxPower) moves.Add(new KeyValuePair<PokemonProxy, MoveType>(p, m.Type));
          }
      }
      if (moves.Count != 0)
      {
        KeyValuePair<PokemonProxy, MoveType> pair = moves[pm.Controller.GetRandomInt(0, moves.Count - 1)];
        pm.RaiseAbility();
        pm.Controller.ReportBuilder.ShowLog("ReadMove", pair.Key.Id, pair.Value.Id);
      }
    }
    private static void WeatherAbility(PokemonProxy pm, Weather weather)
    {
      var c = pm.Controller;
      if (c.Board.Weather != weather)
      {
        pm.RaiseAbility();
        c.Weather = weather;
        c.Board.SetCondition("Weather", pm.Controller.TurnNumber + 4);
      }
    }
    private static void Trace(PokemonProxy pm)
    {
      var pms = new List<PokemonProxy>();
      foreach (var p in pm.Controller.Board[1 - pm.Pokemon.TeamId].GetPokemons(pm.OnboardPokemon.X - 1, pm.OnboardPokemon.X + 1))
        if (ATs.Trace(p.OnboardPokemon.Ability)) pms.Add(p);
      var n = pms.Count;
      if (n != 0)
      {
        pm.RaiseAbility();
        var target = pms[pm.Controller.GetRandomInt(0, n - 1)];
        pm.ChangeAbility(target.OnboardPokemon.Ability);
        pm.Controller.ReportBuilder.ShowLog("Trace", target.Id, target.OnboardPokemon.Ability);
      }
    }
    private static void SimpleAttachRaise(PokemonProxy pm, string log)
    {
      pm.RaiseAbility();
      pm.ShowLogPm(log);
    }
    private static void Download(PokemonProxy pm)
    {
      var sa = pm.CanChangeLv7D(pm, StatType.SpAtk, 1, false) != 0;
      var a = pm.CanChangeLv7D(pm, StatType.Atk, 1, false) != 0;
      if (sa || a)
      {
        var stats = new List<StatType>();
        foreach (var p in pm.Controller.Board[1 - pm.Pokemon.TeamId].GetPokemons(pm.OnboardPokemon.X - 1, pm.OnboardPokemon.X + 1))
          if (p.OnboardPokemon.FiveD.Def > p.OnboardPokemon.FiveD.SpDef)
          {
            if (sa) stats.Add(StatType.SpAtk);
          }
          else if (a) stats.Add(StatType.Atk);
        var n = stats.Count;
        if (n != 0)
        {
          pm.RaiseAbility();
          pm.ChangeLv7D(pm, stats[pm.Controller.GetRandomInt(0, n - 1)], 1, false);
        }
      }
    }
    private static void Anticipation(PokemonProxy pm)
    {
      foreach (var p in pm.Controller.Board[1 - pm.Pokemon.TeamId].GetPokemons(pm.OnboardPokemon.X - 1, pm.OnboardPokemon.X + 1))
        foreach (var m in p.Moves)
          if (m.Type.Class == MoveInnerClass.OHKO || BattleTypeHelper.EffectRevise(m.Type.Type, pm.OnboardPokemon.Types) > 0)
          {
            pm.RaiseAbility();
            pm.ShowLogPm("Anticipation");
            return;
          }
    }
    private static void CantAddState(PokemonProxy pm, PokemonState state)
    {
      if (pm.State == state)
      {
        pm.RaiseAbility();
        pm.DeAbnormalState();
      }
    }
  }
}
