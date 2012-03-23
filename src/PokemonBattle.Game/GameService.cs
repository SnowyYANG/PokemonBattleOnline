using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Data;

namespace LightStudio.PokemonBattle.Game
{
  public static class GameService
  {
    private static bool unlocked;
    private static IMoveE[] moves;
    private static IAbilityE[] abilities;
    private static IItemE[] items;
    private static Dictionary<int, Rule> rules;

    static GameService()
    {
      unlocked = true;
      moves = new IMoveE[DataService.Moves.Count() + 1];
      abilities = new IAbilityE[DataService.Abilities.Count() + 1];
      items = new IItemE[DataService.Items.Count() + 1];
      rules = new Dictionary<int, Rule>();
    }

    public static IMoveE GetMove(int id)
    {
      if (id < 0 || id > moves.Length) id = 0;
      return moves[id];
    }
    public static IAbilityE GetAbility(int id)
    {
      if (id < 0 || id > abilities.Length) id = 0;
      return abilities[id];
    }
    public static IItemE GetItem(Item item)
    {
      if (item == null) return items[0];
      return items[item.Id];
    }
    public static Rule GetRule(int id)
    {
      return rules.ValueOrDefault(id);
    }

    public static void Lock()
    {
      unlocked = false;
    }
    public static void RegisterMove(IMoveE move)
    {
      if (unlocked)
      {
        moves[move.Move.Id] = move;
      }
    }
    public static void RegisterAbility(IAbilityE ability)
    {
      if (unlocked)
      {
        abilities[ability.Id] = ability;
      }
    }
    public static void RegisterItem(IItemE item)
    {
      if (unlocked)
      {
        items[item.Id] = item;
      }
    }
    public static void RegisterRule(Rule rule)
    {
      if (unlocked)
      {
        rules[rule.Id] = rule;
      }
    }
  }
}
