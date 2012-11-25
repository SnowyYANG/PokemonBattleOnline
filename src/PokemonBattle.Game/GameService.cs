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

    static GameService()
    {
      Logs = GameLogs.Load(DataService.CurrentLanguage);
    }
  }
}
