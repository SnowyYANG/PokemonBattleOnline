using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonBattleOnline.Data;

namespace PokemonBattleOnline.Game.Host.Triggers
{
  internal static class MoveExecute
  {
    public static void Execute(AtkContext atk)
    {
      var move = atk.Move.Id;
      var aer = atk.Attacker;

      if (move == Ms.TELEPORT || move == Ms.SKETCH || move == Ms.SKY_DROP)
      {
        atk.FailAll("bad", aer.Id, move);
        return;
      }

      if (MoveNotFail.Execute(atk))
        switch (move)
        {
          case Ms.SNATCH:
            Snatch(atk);
            break;
          case Ms.METRONOME: //118
            Metronome(atk);
            break;
          case Ms.MIRROR_MOVE: //119
            MirrorMove(atk);
            break;
          case Ms.SLEEP_TALK: //214
            SleepTalk(atk);
            break;
          case Ms.NATURE_POWER: //267
            NaturePower(atk);
            break;
          case Ms.ASSIST: //274
            Assist(atk);
            break;
          case Ms.ME_FIRST: //382
            MeFirst(atk);
            break;
          case Ms.COPYCAT: //383
            Copycat(atk);
            break;
          default:
            atk.ImplementPressure();
            Generic(atk);
            if (atk.Fail)
              switch (move)
              {
                case 26:
                case 136:
                  aer.EffectHurtByOneNth(2, "FailSelfHurt");
                  break;
                case 120:
                case 153:
                  aer.Faint();
                  break;
                case Ms.NATURAL_GIFT: //363
                case Ms.FLING:
                  aer.ConsumeItem();
                  aer.Controller.ReportBuilder.Add(new GameEvents.RemoveItem(null, aer));
                  break;
              }
            break;
        }
      else atk.ImplementPressure();
      if (atk.Move.Id == Ms.RAGE && aer != null) aer.OnboardPokemon.SetCondition("Rage");
    }

    private static void Generic(AtkContext atk)
    {
      var move = atk.Move;
      var aer = atk.Attacker;

      if (move.Flags.PrepareOneTurn && PrepareOneTurn(atk)) return;

      if (move.Flags.Snatchable)
        foreach (var pm in atk.Controller.ActingPokemons)
          if (pm.OnboardPokemon.HasCondition("Snatch"))
          {
            pm.OnboardPokemon.RemoveCondition("Snatch");
            pm.AddReportPm("Snatch", aer);
            var s = new AtkContext(pm) { Move = move };
            MoveInitAtkContext.Execute(s);
            MoveE.BuildDefContext(s, null);
            if (MoveNotFail.Execute(s)) MoveAct.Execute(s);
            else s.FailAll();
            atk.SetAttackerAction(PokemonAction.Done);
            return;
          }

      if (move.Flags.MagicCoat && atk.Targets == null)
      {
        var pm = aer.Controller.GetOnboardPokemons(1 - aer.Pokemon.TeamId).FirstOrDefault((p) => STs.MagicCoat(atk, p));
        if (pm != null)
        {
          atk.SetCondition("MagicCoat", new List<PokemonProxy>() { pm });
          atk.FailAll(null);
          MoveE.MagicCoat(atk);
          return;
        }
      }

      MoveCalculateType.Execute(atk);
      MoveE.FilterDefContext(atk);
      if (atk.Targets != null && atk.Target == null) atk.FailAll(null);
      else
      {
        MoveAct.Execute(atk);
        MoveE.MoveEnding(atk);
      }

      if (move.Flags.MagicCoat && atk.Targets != null) MoveE.MagicCoat(atk);
    }

    private static void Snatch(AtkContext atk)
    {
      atk.Attacker.OnboardPokemon.SetTurnCondition("Snatch");
      atk.Attacker.AddReportPm("EnSnatch");
      atk.SetAttackerAction(PokemonAction.Done);
    }

    private static readonly int[] ASSIST_BLOCK = { 166, 274, 118, 165, 271, 415, 214, 382, 448, 68, 243, 194, 182, 197, 203, 364, 264, 266, 476, 270, 383, 119, 289, 525, 509, 144, Ms.NATURE_POWER, Ms.THIEF, Ms.COVET, Ms.MIMIC };
    private static void Assist(AtkContext atk)
    {
      var aer = atk.Attacker;
      var moves = new List<MoveType>();
      foreach (var pm in aer.Tile.Field.Pokemons)
        if (pm != aer && pm.Pokemon.Owner == aer.Pokemon.Owner)
          foreach (var m in pm.Moves)
            if (!ASSIST_BLOCK.Contains(m.Type.Id)) moves.Add(m.Type);
      for (int i = aer.Controller.GameSettings.Mode.OnboardPokemonsPerPlayer(); i < aer.Pokemon.Owner.Pokemons.Count(); ++i)
        foreach (var m in aer.Pokemon.Owner.GetPokemon(i).Moves)
          if (!ASSIST_BLOCK.Contains(m.Type.Id)) moves.Add(m.Type);
      if (moves.Count == 0) atk.FailAll();
      else atk.StartExecute(moves[aer.Controller.GetRandomInt(0, moves.Count - 1)]);
    }

    private static void NaturePower(AtkContext atk)
    {
      Data.MoveType m;
      switch (atk.Controller.GameSettings.Terrain)
      {
        case Terrain.Path:
          m = Data.GameDataService.GetMove(Ms.EARTHQUAKE);
          break;
        default:
          atk.Controller.ReportBuilder.Add("error");
          return;
      }
      atk.StartExecute(m, null, "NaturePower");
    }

    private static readonly int[] SLEEPTALK_BLOCK = new int[] { 214, 117, 448, 253, 383, 119, 382, 264 };
    private static void SleepTalk(AtkContext atk)
    {
      var aer = atk.Attacker;
      var moves = new List<MoveType>();
      foreach (var m in aer.Moves)
        if (!(m.Type.Flags.PrepareOneTurn || SLEEPTALK_BLOCK.Contains(m.Type.Id))) moves.Add(m.Type);
      var n = moves.Count;
      if (n == 0) atk.FailAll();
      else atk.StartExecute(moves[aer.Controller.GetRandomInt(0, moves.Count - 1)]);
    }

    private static readonly int[] METRONOME_BLOCK = new int[] { 68, 102, 118, 119, 144, 165, 166, 168, 173, 182, 194, 197, 203, 214, 243, 264, 266, 267, 270, 271, 274, 289, 343, 364, 382, 383, 415, 448, 469, 476, 495, 501, 511, 516, };
    private static void Metronome(AtkContext atk)
    {
    LOOP:
      var m = GameDataService.GetMove(atk.Controller.GetRandomInt(1, GameDataService.Moves.Count()));
      if (METRONOME_BLOCK.Contains(m.Id)) goto LOOP;
      atk.StartExecute(m, null, "Metronome");
    }

    private static readonly int[] COPYCAT_BLOCK = new int[] { 182, 197, 383, 102, 166, 271, 415, 509, 525, 194, 364, 264, 165 };
    private static void Copycat(AtkContext atk)
    {
      var o = atk.Controller.Board.GetCondition("LastMove");
      if (o == null || COPYCAT_BLOCK.Contains(o.Move.Id)) atk.FailAll();
      else atk.StartExecute(o.Move);
    }

    private static void MeFirst(AtkContext atk)
    {
      var der = atk.Target.Defender;
      var m = der.SelectedMove.Type;
      if (der.OnboardPokemon.HasCondition("SkyDrop") || m.Id == Ms.STRUGGLE || m.Id == Ms.FOCUS_PUNCH) atk.FailAll();
      else
      {
        atk.SetTurnCondition("MeFirst");
        atk.StartExecute(m, der.Tile);
      }
    }

    private static void MirrorMove(AtkContext atk)
    {
      var last = atk.Target.Defender.AtkContext;
      if (last != null && last.Move.Flags.Mirrorable) atk.StartExecute(last.Move, atk.Target.Defender.Tile);
      else atk.FailAll();
    }

    private static bool PrepareOneTurn(AtkContext atk)
    {
      var aer = atk.Attacker;
      var m = atk.Move.Id;
      if (aer.Action == PokemonAction.MoveAttached)
      {
        switch (m)
        {
          case Ms.FLY: //19
          case Ms.BOUNCE: //340
            PrepareOneTurn(atk, CoordY.Air);
            break;
          case Ms.DIG: //91
            PrepareOneTurn(atk, CoordY.Underground);
            break;
          case Ms.DIVE: //291
            PrepareOneTurn(atk, CoordY.Water);
            break;
          case Ms.SHADOW_FORCE: //467
            PrepareOneTurn(atk, CoordY.Another);
            break;
          default:
            aer.AddReportPm("Prepare" + m.ToString());
            break;
        }
        if (m == Ms.SKULL_BASH) aer.ChangeLv7D(atk.Attacker, StatType.Def, 1, false);
        atk.SetAttackerAction(PokemonAction.Moving);
        return !(m == Ms.SOLARBEAM && aer.Controller.Weather == Weather.IntenseSunlight || Is.PowerHerb(aer));
      }
      return false;
    }
    private static void PrepareOneTurn(AtkContext atk, CoordY y)
    {
      atk.Attacker.Controller.ReportBuilder.Add(GameEvents.PositionChange.Leap("Prepare" + atk.Move.Id.ToString(), atk.Attacker, y));
      atk.Attacker.OnboardPokemon.CoordY = y;
    }
  }
}
