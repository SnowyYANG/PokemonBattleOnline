using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Data;
using LightStudio.PokemonBattle.Game.Host.Sp;

namespace LightStudio.PokemonBattle.Game.Host
{
  public class AtkContext : ConditionalObject
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

    internal AtkContext(MoveProxy mp)
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
      if (Triggers.CanExecuteMove(Attacker, move))
      {
        if (log != null) Attacker.AddReportPm(log, move.Id);
        var e = EffectsService.GetMove(move.Id);
        e.InitAtkContext(this);
        BuildDefContext(selectTile);
        e.Execute(this);
      }
      else FailAll(null);
    }
    internal void BuildDefContext(Tile selectTile)
    {
      EffectsService.GetMove(Move.Id).BuildDefContext(this, selectTile);
      if (MoveProxy != null) Abilities.Pressure(this, Moves.GetRange(Attacker, Move));
    }
    internal void ContinueExecute(Tile selectTile)
    {
      TotalDamage = 0;
      BuildDefContext(selectTile);
      EffectsService.GetMove(Move.Id).Execute(this);
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
      return !a.SheerForce() && Controller.RandomHappen(a.SereneGrace() ? percentage *= 3 : percentage);
    }
    public void FailAll(string log = "Fail0", int arg0 = 0, int arg1 = 0)
    {
      ImplementPressure();
      Fail = true;
      SetAttackerAction(PokemonAction.Done);
      if (log != null) Controller.ReportBuilder.Add(log, arg0, arg1);
    }
  }
  public class DefContext : ConditionalObject
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
        if (AtkContext.Attacker.Ability.NoGuard() || Defender.Ability.NoGuard()) return true;
        Condition c = Defender.OnboardPokemon.GetCondition("NoGuard");
        return c != null && c.By == AtkContext.Attacker && c.Turn == Defender.Controller.TurnNumber; 
      }
    }

    public AbilityE Ability
    { get { return AtkContext.Attacker.Ability.IgnoreDefenderAbility() ? EffectsService.NULL_ABILITY : Defender.Ability; } }
    public bool RandomHappen(int percentage)
    {
      return percentage == 0 || !Ability.ShieldDust() && AtkContext.RandomHappen(percentage);
    }
    public bool HasInfiltratableCondition(string condition)
    {
      PokemonProxy a = AtkContext.Attacker;
      return
        !IsCt &&
        Defender.Tile.Field.HasCondition(condition) &&
        (Defender.Tile.Team == a.Tile.Team || !AtkContext.Attacker.Ability.Infiltrator());
    }
    public void ModifyDamage(Modifier modifier)
    {
      Damage *= modifier;
    }
  }
}
