using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Data;
using LightStudio.PokemonBattle.Game.Host.Triggers;

namespace LightStudio.PokemonBattle.Game.Host
{
  public static partial class EffectsService
  {
    public readonly static IAbilityE NULL_ABILITY;
    public readonly static IItemE NULL_ITEM;
    private readonly static IRuleE NULL_RULE;
    private static bool unlocked;
    private static IMoveE[] moves;
    private static IAbilityE[] abilities;
    private static IItemE[] items;
    private static Dictionary<int, IRuleE> rules;

    static EffectsService()
    {
      unlocked = true;
      NULL_ABILITY = new AbilityE0();
      NULL_ITEM = new ItemE0();
      NULL_RULE = new RuleE(0);
      moves = new IMoveE[DataService.Moves.Count() + 1];
      abilities = new IAbilityE[DataService.Abilities.Count() + 1];
      items = new IItemE[DataService.Items.Count() + 1];
      rules = new Dictionary<int, IRuleE>();
      items[0] = NULL_ITEM;
      abilities[0] = NULL_ABILITY;
    }

    public static void Lock()
    {
      unlocked = false;
    }

    #region emodels
    public static IMoveE GetMove(int id)
    {
      if (id < 0 || id > moves.Length) id = 0;
      if (moves[id] == null) //不用Factory大丈夫？
      {
        MoveType move = DataService.GetMove(id);
        if (move == null) id = 0;
        switch (move.Class)
        {
          case MoveInnerClass.AddState:
          case MoveInnerClass.ForceToShift:
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
    public static IAbilityE GetAbility(int id)
    {
      if (id < 0 || id > abilities.Length) id = 0;
      if (abilities[id] == null) abilities[id] = new AbilityE(id);
      return abilities[id];
    }
    public static IItemE GetItem(Item item)
    {
      if (item == null || item.Id < 0 || item.Id > items.Length) return items[0];
      if (items[item.Id] == null) items[item.Id] = new ItemE(item.Id);
      return items[item.Id];
    }
    public static IRuleE GetRule(int id)
    {
      return rules.ValueOrDefault(id, NULL_RULE);
    }
    public static void Register(IMoveE move)
    {
      if (unlocked)
      {
        if (move.Move.Id > 0 && move.Move.Id < moves.Length)
          moves[move.Move.Id] = move;
      }
    }
    public static void Register(IAbilityE ability)
    {
      if (unlocked)
      {
        if (ability.Id > 0 && ability.Id < abilities.Length)
          abilities[ability.Id] = ability;
      }
    }
    public static void Register(IItemE item)
    {
      if (unlocked)
      {
        if (item.Id > 0 && item.Id < items.Length)
          items[item.Id] = item;
      }
    }
    public static void Register(IRuleE rule)
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
    public static void Register(ITrigger trigger)
    {
      if (unlocked && trigger != null)
      {
        if (trigger is IEndTurn) EndTurn = (IEndTurn)trigger;
        else if (trigger is ICanExecute) CanExecute = (ICanExecute)trigger;
      }
    }
    #endregion
  }
}
