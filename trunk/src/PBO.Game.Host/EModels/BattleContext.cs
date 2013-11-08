using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonBattleOnline.Game;
using PokemonBattleOnline.Game.Host.Triggers;

namespace PokemonBattleOnline.Game.Host
{
  internal class AtkContext : ConditionalObject
  {
    public readonly PokemonProxy Attacker;
    public readonly MoveProxy MoveProxy; //压力、诅咒身躯，针对一开始选的技能
    public BattleType Type;
    public int Pressure;
    public Modifier AccuracyModifier;
    public int CTLv;
    public int TotalDamage;
    public bool Fail;
    public bool IgnoreSwitchItem;

    public AtkContext(MoveProxy mp)
    {
      MoveProxy = mp;
      Attacker = mp.Owner;
    }
    public AtkContext(PokemonProxy pm)
    {
      Attacker = pm;
    }

    public MoveType Move
    { get; internal set; }
    public Controller Controller
    { get { return Attacker.Controller; } }
    public IEnumerable<DefContext> Targets
    { get; private set; }
    public DefContext Target
    { get; private set; }
    public bool MultiTargets
    { get; internal set; }

    public void SetAttackerAction(PokemonAction action)
    {
      if (Attacker.AtkContext == this) Attacker.Action = action;
    }

    public void StartExecute(MoveType move, Tile selectTile = null, string log = "UseMove")
    {
      Move = move;
      if (STs.CanExecuteMove(Attacker, move))
      {
        if (log != null) Attacker.AddReportPm(log, move.Id);
        InitAtkContext.Execute(this);
        MoveE.BuildDefContext(this, selectTile);
        if (MoveProxy != null) ATs.Pressure(this, MTs.GetRange(Attacker, Move));
        MoveExecute.Execute(this);
      }
      else FailAll(null);
    }
    public void ContinueExecute(Tile selectTile)
    {
      TotalDamage = 0;
      MoveE.BuildDefContext(this, selectTile);
      MoveExecute.Execute(this);
    }
    public void SetTargets(IEnumerable<DefContext> targets)
    {
      Targets = targets;
      Target = targets.FirstOrDefault();
    }
    public void ImplementPressure()
    {
      if (MoveProxy != null)
      {
        MoveProxy.PP -= Pressure;
        Pressure = 0;
      }
    }
    public bool RandomHappen(int percentage)
    {
      if (percentage == 0) return true;
      var a = Attacker.Ability;
      return a != As.SHEER_FORCE && Controller.RandomHappen(a == As.SERENE_GRACE ? percentage *= 3 : percentage);
    }
    public void FailAll(string log = "Fail0", int arg0 = 0, int arg1 = 0)
    {
      Fail = true;
      SetAttackerAction(PokemonAction.Done);
      if (log != null) Controller.ReportBuilder.ShowLog(log, arg0, arg1);
    }
  }
  internal class DefContext : ConditionalObject
  {
    public readonly AtkContext AtkContext;
    public readonly PokemonProxy Defender;
    public int Damage;
    public int BasePower;
    public bool IsCt;
    public bool HitSubstitute;
    /// <summary>
    /// bit operation, -2, -1, 0, 1, 2
    /// </summary>
    public int EffectRevise;
    
    public DefContext(AtkContext a, PokemonProxy pm)
    {
      AtkContext = a;
      Defender = pm;
    }

    /// <summary>
    /// 无防御、心眼、锁定
    /// </summary>
    public bool NoGuard
    { 
      get
      {
        if (AtkContext.Attacker.Ability == As.NO_GUARD || Defender.Ability == As.NO_GUARD) return true;
        Condition c = Defender.OnboardPokemon.GetCondition("NoGuard");
        return c != null && c.By == AtkContext.Attacker && c.Turn == Defender.Controller.TurnNumber; 
      }
    }

    public int Ability
    { get { return ATs.IgnoreDefenderAbility(AtkContext.Attacker.Ability) ? 0 : Defender.Ability; } }
    
    public bool RandomHappen(int percentage)
    {
      return percentage == 0 || Ability != As.SHIELD_DUST && AtkContext.RandomHappen(percentage);
    }
    public bool HasInfiltratableCondition(string condition)
    {
      PokemonProxy a = AtkContext.Attacker;
      return
        !IsCt &&
        Defender.Tile.Field.HasCondition(condition) &&
        (Defender.Tile.Team == a.Tile.Team || a.Ability != As.INFILTRATOR);
    }
    public void MoveHurt()
    {
      Damage = Defender.MoveHurt(Damage);
      {
        var o = new Condition();
        o.Damage = Damage;
        o.By = AtkContext.Attacker;
        string c = AtkContext.Move.Category == MoveCategory.Physical ? "PhysicalDamage" : "SpecialDamage";
        Defender.OnboardPokemon.SetTurnCondition(c, o);
        Defender.OnboardPokemon.SetTurnCondition("Damage", o);
      }
      if (Defender.Action == PokemonAction.Moving && AtkContext.Move.Id == Ms.BIDE)
      {
        var o = AtkContext.GetCondition("Bide");
        o.By = AtkContext.Attacker;
        o.Damage += Damage;
      }
    }
    public void ModifyDamage(Modifier modifier)
    {
      Damage *= modifier;
    }
    public void Fail()
    {
      Defender.AddReportPm("Fail");
    }
  }
}
