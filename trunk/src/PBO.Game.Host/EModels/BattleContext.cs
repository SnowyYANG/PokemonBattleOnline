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
    public int TotalDamage;
    public bool Fail;
    public bool IgnoreSwitchItem;
    public MoveType Move;
    public bool MultiTargets;
    public int Hits;
    public int Hit;

    public AtkContext(MoveProxy mp)
    {
      MoveProxy = mp;
      Attacker = mp.Owner;
    }
    public AtkContext(PokemonProxy pm)
    {
      Attacker = pm;
    }

    public Controller Controller
    { get { return Attacker.Controller; } }
    public IEnumerable<DefContext> Targets
    { get; private set; }
    public DefContext Target
    { get; private set; }

    public void SetAttackerAction(PokemonAction action)
    {
      if (Attacker.AtkContext == this) Attacker.Action = action;
    }

    public void StartExecute(MoveType move, Tile selectTile = null, string log = "UseMove")
    {
      Move = move;
      if (STs.CanExecuteMove(Attacker, move))
      {
        if (log != null) Attacker.ShowLogPm(log, move.Id);
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
        if (AtkContext.Attacker.AbilityE(As.NO_GUARD) || Defender.AbilityE(As.NO_GUARD)) return true;
        Condition c = Defender.OnboardPokemon.GetCondition("NoGuard");
        return c != null && c.By == AtkContext.Attacker && c.Turn == Defender.Controller.TurnNumber; 
      }
    }

    public int Ability
    { get { return AtkContext.IgnoreDefenderAbility() ? 0 : Defender.Ability; } }
    public bool AbilityE(int ability)
    {
      return Defender.AbilityE(ability) && !AtkContext.IgnoreDefenderAbility();
    }
    
    public bool RandomHappen(int percentage)
    {
      return percentage == 0 || !AbilityE(As.SHIELD_DUST) && AtkContext.RandomHappen(percentage);
    }
    
    public void MoveHurt()
    {
      Damage = Defender.MoveHurt(Damage, !AtkContext.IgnoreDefenderAbility());
      {
        var o = new Condition();
        o.Damage = Damage;
        o.By = AtkContext.Attacker;
        string c = AtkContext.Move.Category == MoveCategory.Physical ? "PhysicalDamage" : "SpecialDamage";
        Defender.OnboardPokemon.SetTurnCondition(c, o);
        Defender.OnboardPokemon.SetTurnCondition("Damage", o);
      }
    }
    public void ModifyDamage(Modifier modifier)
    {
      Damage *= modifier;
    }
    public void Fail()
    {
      Defender.ShowLogPm("Fail");
    }
  }
}
