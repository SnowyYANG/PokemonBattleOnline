using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonBattleOnline.Game.Host.Triggers;

namespace PokemonBattleOnline.Game.Host
{
  internal static partial class PTs
  {
    public static bool CanAddState(this PokemonProxy pm, PokemonProxy by, AttachedState state, bool showFail)
    {
      return CanAddState(pm, by, true, state, showFail);
    }
    private static bool CanAddState(this PokemonProxy pm, PokemonProxy by, bool ability, AttachedState state, bool showFail)
    {
      if (!pm.AliveOnboard) return false;
      string fail = pm.Controller.GameSettings.Mode.NeedTarget() ? "Fail" : "Fail0";
      switch (state)
      {
        case AttachedState.BRN:
          if (pm.State == PokemonState.BRN) goto FAIL_BEENSTATE;
          if (pm.OnboardPokemon.HasType(BattleType.Fire)) goto FAIL_NOEFFECT;
          goto STATE;
        case AttachedState.FRZ:
          if (pm.Controller.Weather == Weather.IntenseSunlight) goto FAIL_FAIL;//战报顺序未测
          if (pm.State == PokemonState.FRZ) goto FAIL_BEENSTATE;
          if (pm.OnboardPokemon.HasType(BattleType.Ice)) goto FAIL_NOEFFECT;
          goto STATE;
        case AttachedState.PAR:
          if (pm.State == PokemonState.PAR) goto FAIL_BEENSTATE;
          if (pm.OnboardPokemon.HasType(BattleType.Electric)) goto FAIL_NOEFFECT;
          goto STATE;
        case AttachedState.PSN:
          if (pm.State == PokemonState.PSN || pm.State == PokemonState.BadlyPSN)
          {
            if (showFail) pm.ShowLogPm("BeenPSN");
            return false;
          }
          if (pm.OnboardPokemon.HasType(BattleType.Poison) || pm.OnboardPokemon.HasType(BattleType.Steel)) goto FAIL_NOEFFECT;
          goto STATE;
        case AttachedState.SLP:
          if (!(ability && pm.AbilityE(As.SOUNDPROOF)))
            foreach (var p in pm.Controller.ActingPokemons)
              if (p.Action == PokemonAction.Moving && pm.AtkContext.Move.Id == Ms.UPROAR)
              {
                if (showFail)
                  if (p == pm) pm.ShowLogPm("UproarCantSLP2");
                  else pm.ShowLogPm("UproarCantSLP");
                return false;
              }
          if (pm.State == PokemonState.SLP) goto FAIL_BEENSTATE;
          goto STATE;
        case AttachedState.Confuse:
          if (pm.OnboardPokemon.HasCondition("Confuse")) goto FAIL_BEENSTATE;
          return !(Safeguard(pm, by, showFail) || ability && CheckAbility(As.OWN_TEMPO, pm, by, state, showFail));
        case AttachedState.Attract:
          if (pm.OnboardPokemon.Gender == PokemonGender.None || by.OnboardPokemon.Gender == PokemonGender.None || pm.OnboardPokemon.Gender == by.OnboardPokemon.Gender) goto FAIL_NOEFFECT;
          if (pm.OnboardPokemon.HasCondition("Attract")) goto FAIL_FAIL;
          return !(ability && CheckAbility(As.OBLIVIOUS, pm, by, state, showFail));
        case AttachedState.Nightmare:
          if (pm.State == PokemonState.SLP) goto CONDITION;
          goto FAIL_NOEFFECT;//战报已测
        case AttachedState.LeechSeed:
          if (pm.OnboardPokemon.HasType(BattleType.Grass)) goto FAIL_NOEFFECT;
          goto CONDITION;
        case AttachedState.Embargo:
          if (pm.OnboardPokemon.Ability == As.MULTITYPE) goto FAIL_NOEFFECT;
          goto CONDITION;
        case AttachedState.PerishSong:
          return !pm.OnboardPokemon.HasCondition("PerishSong"); //无需判断防音 never show fail
        case AttachedState.Disable:
          if (pm.LastMove == null) goto FAIL_FAIL;
          goto CONDITION;
        case AttachedState.Yawn:
          if (pm.OnboardPokemon.HasCondition("Yawn")) goto FAIL_FAIL;
          return CanAddXXX(pm, by, ability, AttachedState.SLP, showFail);
        default:
          goto CONDITION;
      }
    FAIL_FAIL:
      if (showFail) pm.Controller.ReportBuilder.ShowLog(fail);
      return false;
    FAIL_NOEFFECT:
      if (showFail) pm.NoEffect();
      return false;
    FAIL_BEENSTATE:
      if (showFail) pm.ShowLogPm("Been" + state);
      return false;
    CONDITION:
      if (pm.OnboardPokemon.HasCondition(state.ToString())) goto FAIL_FAIL;
      return true;
    STATE:
      if (pm.State != PokemonState.Normal) goto FAIL_FAIL;
      return CanAddXXX(pm, by, ability, state, showFail) && Rules.CanAddState(pm, state, by, showFail);
    }
    /// <summary>
    /// par, slp, psn, frz, brn
    /// </summary>
    /// <param name="pm"></param>
    /// <param name="by"></param>
    /// <param name="ability"></param>
    /// <param name="state"></param>
    /// <param name="showFail"></param>
    /// <returns></returns>
    public static bool CanAddXXX(PokemonProxy pm, PokemonProxy by, bool ability, AttachedState state, bool showFail)
    {
      if (state == AttachedState.SLP && pm.State == PokemonState.SLP)
      {
        if (showFail) pm.Controller.ReportBuilder.ShowLog("Fail0");
        return false;
      }
      if (Safeguard(pm, by, showFail)
          || MistyTerrain(pm, showFail)
          || state == AttachedState.SLP && ETSV(pm, showFail)
          || ability
          && (pm.Controller.Weather == Weather.IntenseSunlight && CheckAbility(As.LEAF_GUARD, pm, by, state, showFail)
              || state == AttachedState.PAR && CheckAbility(As.LIMBER, pm, by, state, showFail)
              || state == AttachedState.SLP && (CheckAbility(As.INSOMNIA, pm, by, state, showFail) || CheckAbility(As.VITAL_SPIRIT, pm, by, state, showFail))
              || state == AttachedState.PSN && CheckAbility(As.IMMUNITY, pm, by, state, showFail)
              || state == AttachedState.FRZ && CheckAbility(As.MAGMA_ARMOR, pm, by, state, showFail)
              || state == AttachedState.BRN && CheckAbility(As.WATER_VEIL, pm, by, state, showFail))) return false;
      return true;
    }
    /// <summary>
    /// true if mistyterrain is available
    /// </summary>
    /// <param name="pm"></param>
    /// <param name="showFail"></param>
    /// <returns></returns>
    private static bool MistyTerrain(PokemonProxy pm, bool showFail)
    {
      if (pm.Controller.Board.HasCondition("MistyTerrain") && HasEffect.IsGroundAffectable(pm, true, false))
      {
        if (showFail) pm.ShowLogPm("MistyTerrain");
        return true;
      }
      return false;
    }
    /// <summary>
    /// true if either is available
    /// </summary>
    /// <param name="pm"></param>
    /// <param name="showFail"></param>
    /// <returns></returns>
    private static bool ETSV(PokemonProxy pm, bool showFail)
    {
      if (pm.Controller.Board.HasCondition("ElectricTerrain") && HasEffect.IsGroundAffectable(pm, true, false))
      {
        if (showFail) pm.ShowLogPm("ElectricTerrain");
        return true;
      }
      foreach (var p in pm.Field.Pokemons)
        if (p.AbilityE(As.SWEET_VEIL)) //not checkability
        {
          if (showFail)
          {
            p.RaiseAbility();
            if (pm == p) pm.ShowLogPm("SweetVeil0");
            else pm.ShowLogPm("SweetVeil1", p.Id);
          }
          return true;
        }
      return false;
    }
    private static bool Safeguard(PokemonProxy pm, PokemonProxy by, bool showFail)
    {
      if (pm.Field.HasCondition("Safeguard") && pm != by && !by.AbilityE(As.INFILTRATOR))
      {
        if (showFail) pm.ShowLogPm("Safeguard");
        return true;
      }
      return false;
    }
    private static bool CheckAbility(int ability, PokemonProxy pm, PokemonProxy by, AttachedState state, bool showFail)
    {
      if (pm.AbilityE(ability))
      {
        if (showFail)
        {
          pm.RaiseAbility();
          if (pm == by || state == AttachedState.Confuse) pm.ShowLogPm("Cant" + state);
          else pm.NoEffect();
        }
        return true;
      }
      return false;
    }

    private static void AddStateImplement(this PokemonProxy pm, PokemonProxy by, AttachedState state, int turn, string log, int arg1)
    {
      switch (state)
      {
        case AttachedState.BRN:
          pm.Pokemon.State = PokemonState.BRN;
          goto POKEMON_STATE;
        case AttachedState.FRZ:
          pm.Pokemon.State = PokemonState.FRZ;
          pm.ShowLogPm(log ?? "EnFRZ", arg1);
          if (pm.CanChangeForm(492, 0)) pm.ChangeForm(0, true);
          goto DONE;
        case AttachedState.PAR:
          pm.Pokemon.State = PokemonState.PAR;
          goto POKEMON_STATE;
        case AttachedState.PSN:
          if (turn == 0) pm.Pokemon.State = PokemonState.PSN;
          else
          {
            pm.Pokemon.State = PokemonState.BadlyPSN;
            pm.OnboardPokemon.SetCondition("PSN", pm.Controller.TurnNumber);
          }
          goto POKEMON_STATE;
        case AttachedState.SLP:
          pm.Pokemon.State = PokemonState.SLP;
          pm.Pokemon.SLPTurn = turn == 0 ? pm.Controller.GetRandomInt(2, 4) : turn;
          goto POKEMON_STATE;
        case AttachedState.Confuse:
          pm.OnboardPokemon.SetCondition("Confuse", turn == 0 ? pm.Controller.GetRandomInt(2, 5) : turn);
          pm.ShowLogPm(log ?? "Confuse");
          goto DONE;
        case AttachedState.Attract:
          pm.OnboardPokemon.SetCondition("Attract", by);
          pm.ShowLogPm(log ?? "EnAttract", arg1);
          ITs.DestinyKnot(pm, by);
          goto DONE;
        case AttachedState.Trap:
          {
            var move = by.AtkContext.Move;
            var c = new Condition();
            c.By = by;
            c.Turn = pm.Controller.TurnNumber + turn - 1;
            c.Move = move;
            c.Bool = by.Item == Is.BINDING_BAND;
            pm.OnboardPokemon.SetCondition("Trap", c);
            pm.ShowLogPm("EnTrap" + move.Id.ToString(), by.Id);
          }
          goto DONE;
        case AttachedState.Nightmare:
          pm.OnboardPokemon.SetCondition("Nightmare");
          pm.ShowLogPm("EnNightmare");
          goto DONE;
        case AttachedState.Torment:
          pm.OnboardPokemon.SetCondition("Torment", by);
          pm.ShowLogPm("EnTorment");
          goto DONE;
        case AttachedState.Disable:
          {
            var c = new Condition();
            c.Move = pm.LastMove;
            c.Turn = pm.Controller.TurnNumber + turn - 1;
            pm.OnboardPokemon.SetCondition("Disable", c);
            pm.ShowLogPm("EnDisable", c.Move.Id);
          }
          goto DONE;
        case AttachedState.Yawn:
          {
            var o = new Condition();
            o.Turn = pm.Controller.TurnNumber + 1;
            o.By = by; //睡眠规则
            pm.OnboardPokemon.AddCondition("Yawn", o);
          }
          pm.ShowLogPm("EnYawn");
          goto DONE;
        case AttachedState.HealBlock:
          {
            var o = new Condition();
            o.Turn = pm.Controller.TurnNumber + turn;
            o.By = by;
            pm.OnboardPokemon.SetCondition("HealBlock", o);
          }
          pm.ShowLogPm("EnHealBlock");
          goto DONE;
        case AttachedState.CanAttack:
          {
            var o = new Condition();
            o.BattleType = by.AtkContext.Move.Id == Ms.MIRACLE_EYE ? BattleType.Dark : BattleType.Ghost;
            o.By = by;
            pm.OnboardPokemon.SetCondition("CanAttack", o);
            pm.ShowLogPm("CanAttack");
          }
          goto DONE;
        case AttachedState.LeechSeed:
          pm.OnboardPokemon.SetCondition("LeechSeed", by.Tile);
          pm.ShowLogPm("EnLeechSeed");
          goto DONE;
        case AttachedState.Embargo:
          pm.OnboardPokemon.SetCondition("Embargo");
          pm.ShowLogPm("EnEmbargo");
          goto DONE;
        case AttachedState.PerishSong:
          pm.OnboardPokemon.SetCondition("PerishSong", 3);
          goto DONE;
        case AttachedState.Ingrain:
          pm.OnboardPokemon.SetCondition("Ingrain");
          pm.ShowLogPm("EnIngrain");
          goto DONE;
#if DEBUG
        default:
          System.Diagnostics.Debugger.Break();
          return;
#endif
      }
    POKEMON_STATE:
      pm.ShowLogPm(log ?? "En" + state.ToString(), arg1);
      if (state != AttachedState.FRZ && state != AttachedState.SLP) ATs.Synchronize(pm, by, state, turn);
    DONE:
      StateAdded.Execute(pm);
    }
    /// <summary>
    /// null log to show default log
    /// </summary>
    /// <param name="by"></param>
    /// <param name="state"></param>
    /// <param name="showFail"></param>
    /// <param name="turn"></param>
    /// <param name="log"></param>
    /// <param name="arg1"></param>
    /// <returns></returns>
    public static bool AddState(this PokemonProxy pm, PokemonProxy by, AttachedState state, bool showFail, int turn = 0, string log = null, int arg1 = 0)
    {
      if (CanAddState(pm, by, state, showFail))
      {
        AddStateImplement(pm, by, state, turn, log, arg1);
        return true;
      }
      return false;
    }
    public static bool AddState(this PokemonProxy pm, DefContext def)
    {
      var aer = def.AtkContext.Attacker;
      MoveAttachment attachment = def.AtkContext.Move.Attachment;
      if (def.RandomHappen(attachment.Probability) && CanAddState(pm, aer, !def.AtkContext.IgnoreDefenderAbility(), attachment.State, def.AtkContext.Move.Category == MoveCategory.Status))
      {
        int turn;
        if (attachment.State == AttachedState.Trap && aer.Item == Is.GRIP_CLAW) turn = 8;
        else if (attachment.MinTurn != attachment.MaxTurn) turn = pm.Controller.GetRandomInt(attachment.MinTurn, attachment.MaxTurn);
        else turn = attachment.MinTurn;
        AddStateImplement(pm, aer, attachment.State, turn, null, 0);
        return true;
      }
      return false;
    }
  }
}
