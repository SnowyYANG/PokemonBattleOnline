using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Data;

namespace LightStudio.PokemonBattle.Game
{
  public static class GameService
  {
    public readonly static GameLogs Logs;
    private static Dictionary<int, Rule> rules;

    static GameService()
    {
      Logs = GameLogs.Load(DataService.CurrentLanguage);
      rules = new Dictionary<int, Rule>();
    }

    public static Rule GetRule(int id)
    {
      return rules.ValueOrDefault(id);
    }
  }
}
