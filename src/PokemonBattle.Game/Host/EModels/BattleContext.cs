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
    public bool MeFirst;
    public int ActualHits; //当前攻击次数，包含多回合攻击与连续攻击技能，从1开始数
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
      Controller.ReportBuilder.Add(new GameEvents.UseMove(Attacker, Move));
      EffectsService.GetMove(Move.Id).Execute(Attacker, null);
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
    
    internal DefContext(AtkContext a, PokemonProxy pm)
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
        dynamic c = Defender.OnboardPokemon.GetCondition<dynamic>("NoGuard");
        return c != null && c.Pm == AtkContext.Attacker && c.Turn == Defender.Controller.TurnNumber; 
      }
    }

    public IAbilityE Ability
    { get { return AtkContext.Attacker.Ability.IgnoreDefenderAbility() ? EffectsService.NULL_ABILITY : Defender.Ability; } }
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
