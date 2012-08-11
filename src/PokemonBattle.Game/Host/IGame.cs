using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Data;

namespace LightStudio.PokemonBattle.Game.Host
{
  /// <summary>
  /// 本接口给Host使用
  /// </summary>
  public interface IGame
  {
    event Action<int, int> GameEnd;
    event Action<ReportFragment, IEnumerable<KeyValuePair<int, InputRequest>>> ReportUpdated;

    bool Prepared { get; }
    IGameSettings Settings { get; }

    bool Start();
    void TryContinue();
    bool SetPlayer(int teamId, int userId, PokemonCustomInfo[] pokemons);
    Player GetPlayer(int id);
    bool InputAction(int player, ActionInput action);
    ReportFragment GetLastLeapFragment();

    int Turn { get; }
  }
  public static class GameFactory
  {
    public static IGame CreateGame(IGameSettings settings, Func<int> nextId)
    {
      return new GameContext(settings, nextId);
    }
  }
}
