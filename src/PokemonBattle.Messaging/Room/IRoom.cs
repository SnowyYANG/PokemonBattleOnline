using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using LightStudio.PokemonBattle.Game;

namespace LightStudio.PokemonBattle.Messaging.Room
{
  public interface IRoom : IDisposable
  {
    event Action Quited;
    ReadOnlyObservableCollection<int> Spectators { get; }
    ReadOnlyObservableCollection<Player> Players { get; }
    IPlayerController PlayerController { get; }
    GameOutward Game { get; }
    RoomState RoomState { get; }
    void Quit();
  }
}
