using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using LightStudio.PokemonBattle.Game;

namespace LightStudio.PokemonBattle.Messaging.Room
{
  public interface IRoomEventsListener
  {
    void GameStart();
    void GameResult(int team0, int team1);
    void GameTie();
    void GameStop(GameStopReason reason, int player);
    void TimeReminder(int[] waitForWhom);
    void TimeUp(IEnumerable<KeyValuePair<int, int>> remainingTime);
    void Error(string message);
  }
  public interface IRoom : IDisposable
  {
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
