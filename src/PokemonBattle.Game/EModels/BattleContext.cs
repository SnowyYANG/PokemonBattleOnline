using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Data;

namespace LightStudio.PokemonBattle.Game
{
  public class AtkContext
  {
    public readonly Controller Controller;
    public readonly PokemonProxy Attacker;
    public readonly MoveType MoveType;
    public readonly Tile SelectTarget;
    public IEnumerable<DefContext> Targets
    { get; private set; }
    public BattleType Type;
    public int Power;
    public int AtkValue; //攻击力 × （攻击方等级 × 2 ÷ 5 + 2）
    public int Times;
    public double AccuracyRevise;
    public double PowerRevise;
    public double DamageRevise;
    
    public AtkContext(PokemonProxy pm)
    {
      Controller = pm.Controller;
      Attacker = pm;
      MoveType = pm.SelectedMove.Move.Type;
      SelectTarget = pm.SelectedTarget;
      AccuracyRevise = 1d;
    }

    public void SetTargets(IEnumerable<DefContext> targets)
    {
      if (Targets == null)
        Targets = targets;
    }
  }
  public class DefContext
  {
    public readonly AtkContext AtkContext;
    public readonly PokemonProxy Defender;
    public int DefValue; // 防御力 ÷ 50 
    public int Damage;
    public double BattleTypeRevise;
    public double DamageRevise;
    public double AccuracyRevise;
    
    public DefContext(AtkContext a, PokemonProxy pm)
    {
      AtkContext = a;
      Defender = pm;
      NoGuard = a.Attacker.Ability.Id == AbilityIds.NO_GUARD || pm.OnboardPokemon.HasCondition("NoGuard") || pm.Ability.Id == AbilityIds.NO_GUARD;
      AccuracyRevise = 1d;
    }

    /// <summary>
    /// 无防御、心眼、锁定
    /// </summary>
    public bool NoGuard
    { get; private set; }

    public IAbilityE Ability
    {
      get
      {
        if (AtkContext.Attacker.Ability.IgnoreDefenderAbility)
          return GameService.GetAbility(0);
        return Defender.Ability;
      }
    }
  }
}
