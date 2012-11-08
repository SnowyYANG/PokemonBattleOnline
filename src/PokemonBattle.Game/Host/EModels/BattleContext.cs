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
    public Modifier AccuracyModifier;
    public int CTLv;
    public int TotalDamage;
    public bool FailAll;
    public bool IgnoreSwitchItem;

    internal AtkContext(MoveProxy mp)
    {
      MoveProxy = mp;
      Attacker = mp.Owner;
      Move = mp.Type;
    }
    public AtkContext(PokemonProxy pm, MoveType move)
    {
      Attacker = pm;
      Move = move;
    }

    public MoveType Move
    { get; set; }
    public Controller Controller
    { get { return Attacker.Controller; } }
    public IEnumerable<DefContext> Targets
    { get; private set; }
    public DefContext Target
    { get; private set; }
    public bool MultiTargets
    { get; internal set; }

    public void BuildDefContext(Tile selectTile)
    {
      EffectsService.GetMove(Move.Id).BuildDefContext(this, selectTile);
    }
    public void Execute()
    {
      TotalDamage = 0;
      EffectsService.GetMove(Move.Id).Execute(this);
    }
    public void SetTargets(IEnumerable<DefContext> targets)
    {
      Targets = targets;
      Target = targets.FirstOrDefault();
    }
    public bool RandomHappen(int percentage)
    {
      if (percentage == 0) return true;
      var a = Attacker.Ability;
      return !a.SheerForce() && Controller.RandomHappen(a.SereneGrace() ? percentage *= 3 : percentage);
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
