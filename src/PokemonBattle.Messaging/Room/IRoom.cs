using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using PokemonBattleOnline.Game;

namespace PokemonBattleOnline.Messaging.Room
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
  public interface IRoom : IDisposable
  {
    event Action GameEnd;
    event Action Quited;
    ReadOnlyObservableCollection<int> Spectators { get; }
    ReadOnlyObservableCollection<Player> Players { get; }
    IPlayerController PlayerController { get; }
    GameOutward Game { get; }
    RoomState RoomState { get; }
    void Quit();
    void AddListener(IRoomEventsListener listener);
  }
}
