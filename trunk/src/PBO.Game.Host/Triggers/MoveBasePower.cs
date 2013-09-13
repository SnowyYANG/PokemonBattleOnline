using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokemonBattleOnline.Game.Host.Triggers
{
  internal static class MoveBasePower
  {
    public static void Execute(DefContext def)
    {
      var der = def.Defender;
      var atk = def.AtkContext;
      var aer = atk.Attacker;
      var move = atk.Move;

      switch (move.Id)
      {
        case Ms.GUST: //16
        case Ms.TWISTER: //239
          def.BasePower = def.Defender.OnboardPokemon.CoordY == CoordY.Air ? 80 : 40;
          break;
        case Ms.TRIPLE_KICK: //167
          break;
        case Ms.ROLLOUT: //205
        case Ms.ICE_BALL: //301
          def.BasePower = 30 * (1 << ((aer.OnboardPokemon.HasCondition("DefenseCurl") ? 6 : 5) - atk.GetCondition("MultiTurn").Turn));
          break;
        case Ms.PRESENT: //217
          def.BasePower = atk.GetCondition<int>("Present");
          break;
        case Ms.PURSUIT: //228
          def.BasePower = atk.IgnoreSwitchItem ? 80 : 40;
          break;
        case Ms.HIDDEN_POWER: //237
          HiddenPower(def);
          break;
        case Ms.BEAT_UP: //251:
          def.BasePower = def.AtkContext.GetCondition<int>("BeatUpAtk") / 10 + 5;
          break;
        case Ms.SPIT_UP: //255
          def.BasePower = 100 * aer.OnboardPokemon.GetCondition<int>("Stockpile");
          break;
        case Ms.REVENGE: //279
        case Ms.AVALANCHE:
          Revenge(def);
          break;
        case Ms.ERUPTION: //284
        case Ms.WATER_SPOUT: //323
          def.BasePower = 150 * aer.Hp / aer.Pokemon.Hp.Origin;
          break;
        case Ms.WEATHER_BALL: //311
          def.BasePower = der.Controller.Weather == Weather.Normal ? 50 : 100;
          break;
        case Ms.NATURAL_GIFT: //363
          {
            var i = Is.BerryNumber(def.AtkContext.Attacker.Pokemon.Item.Id);
            def.BasePower = i < 17 ? 60 : i < 33 ? 70 : i < 36 ? 80 : i < 53 ? 60 : 80;
          }
          break;
        case Ms.PAYBACK: //371
          def.BasePower = der.LastMoveTurn == der.Controller.TurnNumber ? 100 : 50;
          break;
        case Ms.ASSURANCE: //372
          def.BasePower = der.OnboardPokemon.HasCondition("Assurance") ? 100 : 50;
          break;
        case Ms.FLING: //374
          def.BasePower = aer.Pokemon.Item.FlingPower;
          break;
        case Ms.TRUMP_CARD: //376
          TrumpCard(def);
          break;
        case Ms.WRING_OUT: //378
        case Ms.CRUSH_GRIP: //462
          def.BasePower = 120 * der.Hp / der.Pokemon.Hp.Origin;
          break;
        case Ms.PUNISHMENT: //386
          SLevel(def, 60);
          break;
        case Ms.STORED_POWER: //500
          SLevel(def, 20);
          break;
        case Ms.HEX: //506
          def.BasePower = der.Pokemon.State == PokemonState.Normal ? 50 : 100;
          break;
        case Ms.MAGNITUDE:
          def.BasePower = 10 + 20 * atk.GetCondition<int>("Magnitude");
          break;
        case Ms.HEAVY_SLAM:
        case Ms.HEAT_CRASH:
          HeavySlam(def);
          break;
        case Ms.LOW_KICK:
        case Ms.GRASS_KNOT:
          GrassKnot(def);
          break;
        case Ms.FURY_CUTTER:
          {
            var c = atk.Attacker.OnboardPokemon.GetCondition("LastMove");
            if (c != null && c.Move == move) def.BasePower = 20 * (1 << (c.Int > 3 ? 3 : c.Int));
            else def.BasePower = 20;
          }
          break;
        case Ms.FLAIL:
        case Ms.REVERSAL:
          Flail(def);
          break;
        case Ms.ECHOED_VOICE:
          EchoedVoice(def);
          break;
        case Ms.ELECTRO_BALL:
          ElectroBall(def);
          break;
        case Ms.GYRO_BALL:
          GyroBall(def);
          break;
        case Ms.ACROBATICS:
          def.BasePower = aer.Pokemon.Item == null ? 110 : 55;
          break;
        case Ms.RETURN:
          def.BasePower = aer.Pokemon.Happiness * 4 / 10;
          break;
        case Ms.FRUSTRATION:
          def.BasePower = (255 - aer.Pokemon.Happiness) * 4 / 10;
          break;
        case Ms.SMELLINGSALT: //265
          DeAbnormalState(def, PokemonState.PAR);
          break;
        case Ms.WAKEUP_SLAP: //358
          DeAbnormalState(def, PokemonState.SLP);
          break;
        default:
          def.BasePower = move.Power;
          break;
      }
      if (def.BasePower == 0) def.BasePower = 1;
    }

    private static void TrumpCard(DefContext def)
    {
      if (def.AtkContext.MoveProxy == null || def.AtkContext.MoveProxy.Type.Id != Ms.TRUMP_CARD) def.BasePower = 40;
      else
      {
        int pwa = def.AtkContext.MoveProxy.PP;
        if (pwa >= 5) def.BasePower = 40;
        else if (pwa == 4) def.BasePower = 50;
        else if (pwa == 3) def.BasePower = 60;
        else if (pwa == 2) def.BasePower = 80;
        else def.BasePower = 200;
      }
    }

    private static int Positive(int x)
    {
      return x > 0 ? x : 0;
    }
    private static void SLevel(DefContext def, int @const)
    {
      var l5 = def.Defender.OnboardPokemon.Lv5D;
      int sst = l5.Atk;
      sst += Positive(l5.Def);
      sst += Positive(l5.SpAtk);
      sst += Positive(l5.SpDef);
      sst += Positive(l5.Speed);
      sst += Positive(def.Defender.OnboardPokemon.AccuracyLv);
      sst += Positive(def.Defender.OnboardPokemon.EvasionLv);
      def.BasePower = sst * 20 + @const;
    }

    private static void HeavySlam(DefContext def)
    {
      double w = def.AtkContext.Attacker.Weight / def.Defender.Weight;
      if (w >= 5) def.BasePower = 120;
      else if (w >= 4) def.BasePower = 100;
      else if (w >= 3) def.BasePower = 80;
      else if (w >= 2) def.BasePower = 60;
      else def.BasePower = 40;
    }

    private static void GrassKnot(DefContext def)
    {
      double w = def.Defender.Weight;
      if (w >= 200) def.BasePower = 120;
      else if (w >= 100) def.BasePower = 100;
      else if (w >= 50) def.BasePower = 80;
      else if (w >= 25) def.BasePower = 60;
      else if (w >= 10) def.BasePower = 40;
      else def.BasePower = 20;
    }

    private static void Flail(DefContext def)
    {
      int pwd = def.AtkContext.Attacker.Hp * 48 / def.AtkContext.Attacker.Pokemon.Hp.Origin;

      if (pwd == 1) def.BasePower = 200;
      else if (pwd <= 4) def.BasePower = 150;
      else if (pwd <= 9) def.BasePower = 100;
      else if (pwd <= 16) def.BasePower = 80;
      else if (pwd <= 32) def.BasePower = 40;
      else def.BasePower = 20;
    }

    private static void EchoedVoice(DefContext def)
    {
      var c = def.AtkContext.Attacker.OnboardPokemon.GetCondition("LastMove");
      if (c != null && c.Move == def.AtkContext.Move)
      {
        def.BasePower = 40 * (c.Int + 1);
        if (def.BasePower > 200) def.BasePower = 200;
      }
      else def.BasePower = 40;
    }

    private static void ElectroBall(DefContext def)
    {
      int pwb = (int)(def.AtkContext.Attacker.Speed / def.Defender.Speed);
      if (pwb >= 4) def.BasePower = 150;
      else if (pwb >= 3) def.BasePower = 120;
      else if (pwb >= 2) def.BasePower = 80;
      else def.BasePower = 60;
    }

    private static void GyroBall(DefContext def)
    {
      int pwb = (int)(25 * def.Defender.Speed / def.AtkContext.Attacker.Speed);
      if (pwb > 150) def.BasePower = 150;
      else if (pwb < 1) def.BasePower = 1;
      else def.BasePower = pwb;
    }

    private static void DeAbnormalState(DefContext def, PokemonState state)
    {
      def.BasePower = def.Defender.State == state ? 120 : 60;
    }

    private static void HiddenPower(DefContext def)
    {
      var iv = def.AtkContext.Attacker.Pokemon.Iv;
      int pI = (iv.Hp & 2) >> 1;
      pI |= (iv.Atk & 2);
      pI |= (iv.Def & 2) << 1;
      pI |= (iv.Speed & 2) << 2;
      pI |= (iv.SpAtk & 2) << 3;
      pI |= (iv.SpDef & 2) << 4;
      def.BasePower = (int)(pI * 40 / 63 + 30);
    }

    private static void Revenge(DefContext def)
    {
      var o = def.AtkContext.Attacker.OnboardPokemon.GetCondition("Damage");
      if (o != null && o.By == def.Defender) def.BasePower = 120;
      else def.BasePower = 60;
    }
  }
}
