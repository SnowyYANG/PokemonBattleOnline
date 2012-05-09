using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Data;
using LightStudio.PokemonBattle.Game.Sp;

namespace LightStudio.PokemonBattle.Game
{
  public class AtkContext
  {
    public readonly MoveProxy MoveProxy; //压力、诅咒身躯，针对一开始选的技能
    public readonly MoveType Move; //生成技能在后期
    public readonly bool SheerForceActive;
    public BattleType Type;
    public bool MultiTargets;
    public int Power;
    public int CTLv;
    public int AtkRaw;
    public int AtkLv;
    public int Times;
    public double AccuracyRevise = 1d;
    /// <summary>
    /// 
    /// </summary>
    public Queue<double> DamageRevise1 = new Queue<double>();
    /// <summary>
    /// 宝石、节拍器、生命玉
    /// </summary>
    public double DamageRevise2 = 1d;
    /// <summary>
    /// 本属性修正/适应力
    /// </summary>
    public double DamageRevise3 = 1d;
    
    public AtkContext(PokemonProxy pm, MoveType move)
    {
      MoveProxy = pm.SelectedMove;
      Move = move;
      SheerForceActive = Attacker.Ability.SheerForce() && (
        move.Class == MoveInnerClass.AttackWithTargetLv7DChange ||
        move.FlinchProbability > 0 ||
        (move.Attachment != null && move.Attachment.Probability > 0) ||
        (move.Class == MoveInnerClass.AttackWithSelfLv7DChange && move.Lv7DChanges.First().Change > 0));
    }

    public Controller Controller
    { get { return MoveProxy.Owner.Controller; } }
    public PokemonProxy Attacker
    { get { return MoveProxy.Owner; } }
    public IEnumerable<DefContext> Targets
    { get; private set; }
    public DefContext Target
    { get; private set; }

    public void SetTargets(IEnumerable<DefContext> targets)
    {
      if (Targets == null)
      {
        Targets = targets;
        Target = targets.First();
      }
    }
    public bool RandomHappen(int percentage, bool isFlinch = false)
    {
      if (!isFlinch && percentage == 0) return true;
      if (SheerForceActive) return false;
      if (Attacker.Ability.SereneGrace()) percentage *= 3;
      return Controller.RandomHappen(percentage);
    }
  }
  public class DefContext
  {
    public readonly AtkContext AtkContext;
    public readonly PokemonProxy Defender;
    public int Damage;
    public bool IsCt;
    public bool HitSubstitute;
    public double PowerRevise = 1d;
    public double EffectRevise = 1d;
    public double AccuracyRevise = 1d;
    
    public DefContext(AtkContext a, PokemonProxy pm)
    {
      AtkContext = a;
      Defender = pm;
      NoGuard = a.Attacker.Ability.NoGuard() || pm.Ability.NoGuard() || pm.OnboardPokemon.HasCondition("NoGuard");
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
        return AtkContext.Attacker.Ability.IgnoreDefenderAbility() ?
          GameService.NULL_ABILITY :
          Defender.Ability;
      }
    }
    public bool HasInfiltratableCondition(string condition)
    {
      PokemonProxy a = AtkContext.Attacker;
      return Defender.Controller.Board[Defender.Tile.Team].HasCondition(condition) &&
        (Defender.Tile.Team == a.Tile.Team || !AtkContext.Attacker.Ability.Infiltrator());
    }
  }
}
