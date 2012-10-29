using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Data;

namespace LightStudio.PokemonBattle.Game.Host
{
  public static class EffectsService
  {
    public readonly static AbilityE NULL_ABILITY;
    public readonly static ItemE NULL_ITEM;
    private readonly static RuleE NULL_RULE;
    private static bool unlocked;
    private static MoveE[] moves;
    private static AbilityE[] abilities;
    private static Dictionary<int, ItemE> items;
    private static Dictionary<int, IRuleE> rules;

    static EffectsService()
    {
      unlocked = true;
      NULL_ABILITY = new AbilityE(0);
      NULL_ITEM = new ItemE(0);
      NULL_RULE = new RuleE(0);
      moves = new MoveE[GameDataService.Moves.Count() + 1];
      abilities = new AbilityE[GameDataService.Abilities.Count() + 1];
      items = new Dictionary<int, ItemE>();
      rules = new Dictionary<int, IRuleE>();
      items[0] = NULL_ITEM;
      abilities[0] = NULL_ABILITY;
    }

    public static void Lock()
    {
      unlocked = false;
    }

    public static void Register<T>() where T : GameEvent
    {
      if (unlocked) ReportFragment.AddEventType(typeof(T));
    }

    #region emodels
    public static MoveE GetMove(int id)
    {
      if (id < 0 || id > moves.Length) id = 0;
      if (moves[id] == null) //不用Factory大丈夫？
      {
        MoveType move = GameDataService.GetMove(id);
        if (move == null) id = 0;
        switch (move.Class)
        {
          case MoveInnerClass.AddState:
          case MoveInnerClass.ForceToSwitch:
          case MoveInnerClass.HpRecover:
          case MoveInnerClass.Lv7DChange:
          case MoveInnerClass.ConfusionWithLv7DChange:
            moves[id] = new StatusMoveE(id);
            break;
          case MoveInnerClass.Attack:
          case MoveInnerClass.AttackAndAbsorb:
          case MoveInnerClass.AttackWithSelfLv7DChange:
          case MoveInnerClass.AttackWithState:
          case MoveInnerClass.AttackWithTargetLv7DChange:
            moves[id] = new AttackMoveE(id);
            break;
          default:
            moves[id] = new Move0(id);
            break;
        }
      }
      return moves[id];
    }
    public static AbilityE GetAbility(int id)
    {
      if (id < 0 || id > abilities.Length) id = 0;
      if (abilities[id] == null) abilities[id] = new AbilityE(id);
      return abilities[id];
    }
    public static ItemE GetItem(Item item)
    {
      if (item == null) return NULL_ITEM;
      ItemE e;
      if (!items.TryGetValue(item.Id, out e))
      {
        e = new ItemE(item.Id);
        items[item.Id] = e;
      }
      return e;
    }
    public static IRuleE GetRule(int id)
    {
      return rules.ValueOrDefault(id, NULL_RULE);
    }
    public static void Register(MoveE move)
    {
      if (unlocked)
      {
        if (move.Move.Id > 0 && move.Move.Id < moves.Length)
#if DEBUG
          if (moves[move.Move.Id] != null) System.Diagnostics.Debugger.Break();
#endif
          moves[move.Move.Id] = move;
      }
    }
    public static void Register(AbilityE ability)
    {
      if (unlocked)
      {
        if (ability.Id > 0 && ability.Id < abilities.Length)
          abilities[ability.Id] = ability;
      }
    }
    public static void Register(ItemE item)
    {
      if (unlocked)
      {
        items[item.Id] = item;
      }
    }
    public static void Register(RuleE rule)
    {
      if (unlocked)
      {
        rules.Add(rule.Id, rule);
      }
    }
    #endregion

    #region triggers
    public static IEndTurn EndTurn
    { get; private set; }
    public static ICanExecute CanExecute
    { get; private set; }
    public static IIsGroundAffectable IsGroundAffectable
    { get; private set; }
    public static void Register(ITrigger trigger)
    {
      if (unlocked && trigger != null)
      {
        if (trigger is IEndTurn) EndTurn = (IEndTurn)trigger;
        else if (trigger is ICanExecute) CanExecute = (ICanExecute)trigger;
        else if (trigger is IIsGroundAffectable) IsGroundAffectable = (IIsGroundAffectable)trigger;
      }
    }
    #endregion

    private sealed class Move0 : MoveE
    {
      public Move0(int id)
        : base(id)
      {
      }

      public override void Execute(AtkContext atk)
      {
        if (Move == null) atk.Controller.ReportBuilder.Add("error");
        else atk.Attacker.AddReportPm("unfinish", Move.Id);
        atk.Attacker.Action = PokemonAction.Done;
      }
      protected override void Act(AtkContext atk)
      {
      }
    }
  }
}
