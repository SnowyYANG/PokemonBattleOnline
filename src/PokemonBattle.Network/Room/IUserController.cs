using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using LightStudio.PokemonBattle.Game;

namespace LightStudio.PokemonBattle.Room
{
  public interface IUserController : IDisposable
  {
    event Action Quited;
    ReadOnlyObservableCollection<int> Spectators { get; }
    ReadOnlyObservableCollection<int> Players { get; }
    UserRole Role { get; }
    IPlayerController PlayerController { get; }
    GameOutward Game { get; }
    RoomState RoomState { get; }
    void Chat(string content, MessageTarget target = MessageTarget.All, int targetId = 0);
    void Quit();
  }
}
