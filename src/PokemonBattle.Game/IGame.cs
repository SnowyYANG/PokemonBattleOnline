using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Data;
using LightStudio.PokemonBattle.Interactive;

namespace LightStudio.PokemonBattle.Game
{
  /// <summary>
  /// 本接口给Host使用
  /// </summary>
  public interface IGame
  {
    event Action<int, int> GameEnd;
    event Action<ReportFragment, int[]> ReportUpdated;
    event Action<Player> InputSucceed;

    bool Prepared { get; }
    GameSettings Settings { get; }

    bool Start();
    bool SetPlayer(int teamId, int userId, PokemonCustomInfo[] pokemons);
    Player GetPlayer(int id);
    bool InputAction(int player, ActionInput action);
    ReportFragment GetLastLeapFragment();
  }
  public static class GameFactory
  {
    public static IGame CreateGame(GameSettings settings)
    {
      return new GameContext(settings);
    }
  }
}
