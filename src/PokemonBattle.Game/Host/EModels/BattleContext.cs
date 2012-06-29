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
    public BattleType Type;
    public Modifier AccuracyModifier = 0x1000;
    public bool MultiTargets;
    public int CTLv;
    public int AtkRaw;
    public int AtkLv;
    public int Times;
    private bool? _sheerForceActive;
    public bool SheerForceActive
    { 
      get
      {
        if (_sheerForceActive == null)
          _sheerForceActive =
            Attacker.Ability.SheerForce() && (
            Move.Class == MoveInnerClass.AttackWithTargetLv7DChange ||
            Move.FlinchProbability > 0 ||
            (Move.Attachment != null && Move.Attachment.Probability > 0) ||
            (Move.Class == MoveInnerClass.AttackWithSelfLv7DChange && Move.Lv7DChanges.First().Change > 0));
        return _sheerForceActive.Value;
      }
    }
    public Modifier AtkModifier = 0x1000;
    public Modifier WeatherModifier = 0x1000;
    public Modifier STAB = 0x1000; //本属性修正/适应力
    public Modifier PowerModifier_Item = 0x1000;
    public Modifier PowerModifier_Board = 0x1000;
    
    internal AtkContext(PokemonProxy pm, MoveType move)
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

    internal void Execute()
    {
      Controller.ReportBuilder.Add(Interactive.GameEvents.PositionChange.Reset("UseMove", Attacker, Move.GetLocalizedName()));
      GameService.GetMove(Move.Id).Execute(Attacker);
    }
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
    public int BasePower;
    public bool IsCt;
    public bool HitSubstitute;
    /// <summary>
    /// bit operation, -2, -1, 0, 1, 2, -0x7f for no effect
    /// </summary>
    public sbyte EffectRevise;
    
    internal DefContext(AtkContext a, PokemonProxy pm)
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
      return
        !IsCt &&
        Defender.Controller.Board[Defender.Tile.Team].HasCondition(condition) &&
        (Defender.Tile.Team == a.Tile.Team || !AtkContext.Attacker.Ability.Infiltrator());
    }
    public void ModifyDamage(Int16 modifier)
    {
      Damage = (Damage * modifier + 0x800) >> 12;
    }
  }
}
