using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Data;
using LightStudio.PokemonBattle.Game.Host.Sp;

namespace LightStudio.PokemonBattle.Game.Host
{
  public class AtkContext
  {
    public readonly MoveProxy MoveProxy; //压力、诅咒身躯，针对一开始选的技能
    public readonly MoveType Move; //生成技能在后期
    public BattleType Type;
    public Modifier AccuracyModifier;
    public bool MultiTargets
    { get; internal set; }
    public int CTLv;
    private bool? _sheerForceAvailable;
    private bool SheerForceAvailable
    { 
      get
      {
        if (_sheerForceAvailable == null)
          _sheerForceAvailable = 
            (
            Move.Class == MoveInnerClass.AttackWithTargetLv7DChange ||
            Move.FlinchProbability > 0 ||
            (Move.Attachment != null && Move.Attachment.Probability > 0) ||
            (Move.Class == MoveInnerClass.AttackWithSelfLv7DChange && Move.Lv7DChanges.First().Change > 0)
            );
        return _sheerForceAvailable.Value;
      } 
    }
    public bool SheerForceActive
    { get { return SheerForceAvailable && Attacker.Ability.SheerForce(); } }
    public bool MeFirst;
    public int ActualHits; //当前攻击次数，包含多回合攻击与连续攻击技能，从1开始数
    public bool RaiseItem;
    public Tile EjectButton;
    public dynamic Attachment;

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
      //压力逆鳞压力摇手指逆鳞？
      var um = new GameEvents.UseMove(Attacker, Move);
      Controller.ReportBuilder.Add(um);
      EffectsService.GetMove(Move.Id).Execute(Attacker, um);
    }
    public void SetTargets(IEnumerable<DefContext> targets)
    {
      Targets = targets;
      Target = targets.FirstOrDefault();
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
    /// bit operation, -2, -1, 0, 1, 2
    /// </summary>
    public int EffectRevise;
    public bool RaiseItem;
    
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
          EffectsService.NULL_ABILITY :
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
    public void ModifyDamage(UInt16 modifier)
    {
      Damage = (Damage * modifier + 0x800) >> 12;
    }
  }
}
