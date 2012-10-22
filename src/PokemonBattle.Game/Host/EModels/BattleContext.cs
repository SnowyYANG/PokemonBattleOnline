﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Data;
using LightStudio.PokemonBattle.Game.Host.Sp;

namespace LightStudio.PokemonBattle.Game.Host
{
  [Flags]
  public enum AtkContextFlag
  {
    None = 0,
    IgnorePostEffectItem,
    MeFirst
  }
  public class AtkContext
  {
    public AtkContextFlag Flag;
    public readonly MoveProxy MoveProxy; //压力、诅咒身躯，针对一开始选的技能
    public readonly MoveType Move; //生成技能在后期
    public BattleType Type;
    public Modifier AccuracyModifier;
    public bool MultiTargets
    { get; internal set; }
    public int CTLv;
    public int TotalDamage;
    public bool Gem;
    public bool MeFirst;
    public Tile EjectButton;
    public bool FailAll;
    public dynamic Attachment;

    public AtkContext(PokemonProxy pm, MoveType move)
    {
      MoveProxy = pm.SelectedMove;
      Move = move;
    }

    public Controller Controller
    { get { return MoveProxy.Owner.Controller; } }
    public PokemonProxy Attacker
    { get { return MoveProxy.Owner; } }
    public IEnumerable<DefContext> Targets
    { get; private set; }
    public DefContext Target
    { get; private set; }

    internal void Execute(AtkContextFlag flag)
    {
      TotalDamage = 0;
      Flag = flag;
      Controller.ReportBuilder.Add(new GameEvents.UseMove(Attacker, Move));
      EffectsService.GetMove(Move.Id).Execute(Attacker, null, Flag);
    }
    public void SetTargets(IEnumerable<DefContext> targets)
    {
      Targets = targets;
      Target = targets.FirstOrDefault();
    }
    public bool RandomHappen(int percentage, bool isFlinch = false)
    {
      if (!isFlinch && percentage == 0) return true;
      if (Move.HasProbabilitiedAdditonalEffects() && Attacker.Ability.SereneGrace()) percentage *= 3;
      return Controller.RandomHappen(percentage);
    }
  }
  public class DefContext
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
    public bool HasInfiltratableCondition(string condition)
    {
      PokemonProxy a = AtkContext.Attacker;
      return
        !IsCt &&
        Defender.Tile.Field.HasCondition(condition) &&
        (Defender.Tile.Team == a.Tile.Team || !AtkContext.Attacker.Ability.Infiltrator());
    }
    public void ModifyDamage(UInt16 modifier)
    {
      Damage = (Damage * modifier + 0x800) >> 12;
    }
  }
}
