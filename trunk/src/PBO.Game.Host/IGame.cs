using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonBattleOnline.Game;

namespace PokemonBattleOnline.Game.Host
{
  /// <summary>
  /// 本接口给Host使用
  /// </summary>
  public interface IGame
  {
    event Action<ReportFragment, IDictionary<int, InputRequest>> ReportUpdated;

    IEnumerable<Team> Teams { get; }
    IGameSettings Settings { get; }
    int Turn { get; }

    void Start();
    void TryContinue();
    Player GetPlayer(int id);
    bool InputAction(int player, ActionInput action);
    ReportFragment GetLastLeapFragment();
  }
}
