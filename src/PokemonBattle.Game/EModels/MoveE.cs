using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Data;
using LightStudio.PokemonBattle.Interactive.GameEvents;

namespace LightStudio.PokemonBattle.Game
{
  public abstract class MoveE : IMoveE
  {
    protected static readonly double[,] BATTLE_TYPE_EFFECT = new double[18, 18]{ { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }, { 1, 1, 2, 1, 1, 0.5, 0.5, 0.5, 0.5, 2, 1, 1, 1, 0.5, 2, 1, 0.5, 1 }, { 1, 1, 0.5, 1, 1, 0.5, 1, 1, 2, 1, 1, 1, 1, 1, 2, 1, 0.5, 1 }, { 1, 1, 1, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0.5, 1 }, { 1, 1, 1, 0.5, 0.5, 1, 1, 2, 1, 0.5, 0, 1, 1, 1, 1, 1, 1, 2 }, { 1, 0.5, 2, 1, 1, 1, 1, 0.5, 0, 1, 1, 2, 2, 0.5, 0.5, 2, 2, 1 }, { 1, 2, 1, 0.5, 1, 1, 0.5, 1, 1, 2, 1, 2, 1, 1, 1, 0.5, 2, 0.5 }, { 1, 2, 1, 1, 0.5, 2, 1, 1, 1, 2, 1, 1, 1, 1, 1, 0.5, 0.5, 1 }, { 1, 1, 0.5, 1, 1, 1, 1, 1, 2, 1, 1, 1, 0, 1, 2, 1, 0.5, 1 }, { 1, 0.5, 1, 0.5, 1, 1, 0.5, 0.5, 1, 0.5, 2, 1, 1, 0.5, 1, 2, 0.5, 2 }, { 1, 0.5, 1, 1, 2, 1, 2, 0, 1, 0.5, 1, 1, 1, 2, 1, 2, 2, 1 }, { 1, 1, 1, 2, 1, 1, 0.5, 2, 1, 2, 2, 0.5, 1, 1, 1, 1, 0.5, 0.5 }, { 1, 1, 1, 1, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1, 1, 0.5, 0.5, 1 }, { 1, 1, 1, 1, 1, 1, 1, 1, 0.5, 2, 0.5, 1, 1, 0.5, 1, 0.5, 0, 1 }, { 1, 1, 0, 1, 1, 2, 1, 1, 1, 1, 1, 1, 1, 2, 0.5, 1, 0.5, 1 }, { 1, 2, 1, 1, 1, 0.5, 2, 2, 1, 1, 0.5, 2, 1, 1, 1, 1, 0.5, 1 }, { 1, 1, 1, 1, 0.5, 1, 0.5, 1, 1, 1, 1, 2, 1, 1, 1, 2, 0.5, 0.5 }, { 1, 1, 1, 0.5, 1, 1, 2, 1, 1, 0.5, 2, 1, 1, 1, 1, 2, 1, 0.5 } };
    protected static readonly int[] TIMES25 = new int[8] { 2, 2, 2, 3, 3, 3, 4, 5 };
    protected static string ConvertMultiToString<T>(Func<T, string> convert, IList<T> args)
    {
      return DataService.GameLog.ConvertMultiObjects(convert, args);
    }

    protected MoveE(MoveType moveType)
    {
      this.Move = moveType;
    }

    public MoveType Move
    { get; private set; }

    public void Attach() { } //追击new
    public void PreAct() { } //气合拳new
    public void Act()
    {
      if (CanExecute()) Execute();
    }

    protected bool CanExecute() //梦话new
    {
      throw new NotImplementedException();
    }
    protected void Execute()
    {
    }
    protected bool CanImplement(AtkContext atk)
    {
      var report = atk.Controller.ReportBuilder;
      switch(Move.Range)
      {
        case MoveRange.Field:
        case MoveRange.EnemyField:
        case MoveRange.UserField:
          return true;
        default:
          var targets = new List<DefContext>();
          #region Check for Immunity (or Levitate) on the Ally side, position 1, then position 3. Then check Opponent side, position 1, then 2, then 3,
          {
            var ts = GetRangeTiles(atk);
            var noeffect = new List<DefContext>();
            foreach (Tile t in ts)
            if (t.Pokemon != null)
            {
              PokemonProxy pm = t.Pokemon;
              DefContext def = new DefContext(atk, pm);
              CalculateEffect(def);
              if (def.BattleTypeBoost == 0) noeffect.Add(def);
                else targets.Add(def);
            }
            if (noeffect.Count > 0)
            {
              report.Add(new SoloEvent("NoEffect", DataService.GameLog.ConvertMultiObjects((d) => d.Defender.Outward.Name, noeffect)));
              targets.Remove(noeffect);
            }
            else if (targets.Count == 0)
            {
              report.Add(new SimpleEvent("Fail"));
              return false;
            }
          }
          #endregion
          #region Check for Wide Guard in same way  
          #endregion
          #region Check for Protect
          if (Move.AdvancedFlags.Protectable)
          {
            var protect = new List<DefContext>();  
            foreach(DefContext d in targets)
              if (d.Defender.OnboardPokemon.HasCondition("Protect")) protect.Add(d);
            if (protect.Count > 0)
            {
              report.Add(new SoloEvent("ProtectSelf", DataService.GameLog.ConvertMultiObjects((d)=>d.Defender.Outward.Name, protect)));
              targets.Remove(protect);
            }
          }
          #endregion
          #region Check for Telepathy (and possibly other abilities)
          if (!atk.Attacker.Ability.IgnoreDefenderAbility)
          {
            var abnoeffect = new List<DefContext>();
            foreach (DefContext def in targets)
              if (!def.Defender.Ability.CanImplement(def)) abnoeffect.Add(def);
            targets.Remove(abnoeffect);
          }
          #endregion
          #region Check for misses
          if (atk.Attacker.Ability.Id != AbilityIds.NO_GUARD)
          {
            var miss = new List<DefContext>();  
            foreach(DefContext def in targets)
            {
              var pm = def.Defender;    
              //心眼锁定、无防御
              if (pm.OnboardPokemon.HasCondition("LOMR") || pm.Ability.Id == AbilityIds.NO_GUARD) continue;
              if (!IsYInRange(def)) miss.Add(def);
            }
            if (miss.Count > 0)
            {
              report.Add(new SoloEvent("Miss", ConvertMultiToString((d) => d.Defender.Outward.Name, miss)));
              targets.Remove(miss);
            }
          }
          #endregion
          if (targets.Count == 0) return false;
          else atk.SetTargets(targets);
          //发动属性宝石
          atk.Attacker.Item.ImplementMove(atk);
          //生成攻击次数
          if (Move.MinTimes != Move.MaxTimes) atk.Times = TIMES25[atk.Controller.GetRandomInt(0,7)];
          else atk.Times = Move.MinTimes;
          return true;
      }//switch
    }

    #region
    protected virtual void CalculateType(AtkContext atk)
    {
      atk.Type = Move.Type;
      atk.Attacker.Ability.CalculatingMoveType(ref atk.Type);
    }
    protected virtual void CalculateEffect(DefContext def)
    {
      switch (Move.Class)
      {
        case MoveInnerClass.AddState:
        case MoveInnerClass.Attack:
        case MoveInnerClass.AttackAndAbsorb:
        case MoveInnerClass.AttackWithSelfLv7DChange:
        case MoveInnerClass.AttackWithState:
        case MoveInnerClass.AttackWithTargetLv7DChange:
          def.BattleTypeBoost = BATTLE_TYPE_EFFECT[(int)def.Defender.OnboardPokemon.Type1, (int)def.AtkContext.Type] * BATTLE_TYPE_EFFECT[(int)def.Defender.OnboardPokemon.Type2, (int)def.AtkContext.Type];
          break;
        case MoveInnerClass.OHKO:
          if (def.Defender.Pokemon.Lv > def.AtkContext.Attacker.Pokemon.Lv) def.BattleTypeBoost = 0;
          else def.BattleTypeBoost = BATTLE_TYPE_EFFECT[(int)def.Defender.OnboardPokemon.Type1, (int)def.AtkContext.Type] * BATTLE_TYPE_EFFECT[(int)def.Defender.OnboardPokemon.Type2, (int)def.AtkContext.Type];
          break;
      }
      def.BattleTypeBoost = 1;
    }
    protected IEnumerable<Tile> GetRangeTiles(AtkContext atk)
    {
      Tile select = atk.Attacker.SelectedTarget;
      IEnumerable<Tile> targets = null;
      switch (Move.Range)
      {
        case MoveRange.Adjacent:
          targets = new Tile[1] { atk.Controller.GetTile(1 - atk.Attacker.Tile.Team, atk.Attacker.Tile.X) };
          break;
        case MoveRange.All:
          targets = atk.Controller.Tiles;
          break;
      }
      return targets;
    }
    protected bool CanHit(DefContext def)
    {
      if (Move.Accuracy == 0x65) return true;
      int acc;
      if (Move.Class == MoveInnerClass.OHKO)
        acc = Move.Accuracy + def.AtkContext.Attacker.Pokemon.Lv - def.Defender.Pokemon.Lv;
      else
      {
      }
    MISS:
      
      return false;
    }
    protected virtual bool IsYInRange(DefContext def)
    {
      return def.Defender.Position.Y == CoordY.Plate;
    }
    #endregion
  }
}
