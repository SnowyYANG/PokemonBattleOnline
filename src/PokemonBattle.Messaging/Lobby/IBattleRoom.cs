using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.Tactic.Messaging;
using LightStudio.Tactic.Messaging.Lobby;
using LightStudio.PokemonBattle.Messaging.Room;
using LightStudio.PokemonBattle.Data;

namespace LightStudio.PokemonBattle.Messaging
{
  public interface IBattleHost : IDisposable
  {
    Host AdminController { get; }
    void Start();
    void OnReceived(int senderId, IMessage message);
  }
  public interface IBattleClient : IDisposable
  {
    int HostId { get; }
    event Action<IRoom> EnterSucceed;
    void Start();
    void OnReceived(IMessage message);
  }
}
