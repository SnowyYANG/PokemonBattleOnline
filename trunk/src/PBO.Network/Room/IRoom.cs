using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using PokemonBattleOnline.Game;

namespace PokemonBattleOnline.Network
{
  public interface IRoomEventsListener
  {
    void GameStart();
    void GameTie();
    void GameStop(GameStopReason reason, int player);
    void TimeReminder(int[] waitForWhom);
    void TimeUp(IEnumerable<KeyValuePair<int, int>> remainingTime);
    void Error(string message);
  }
}
