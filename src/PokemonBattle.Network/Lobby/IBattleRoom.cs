using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.Tactic.Messaging;
using LightStudio.PokemonBattle.Room;
using LightStudio.PokemonBattle.Data;

namespace LightStudio.PokemonBattle.Messaging
{
  public interface IBattleHost : IBattleRoom
  {
    Host AdminController { get; }
    void Start();
    void OnReceived(int senderId, IMessage message);
  }
  public interface IBattleClient : IBattleRoom
  {
    int HostId { get; }
    event Action<IUserController> EnterSucceed;
    void Start(PokemonCustomInfo[] pokemons);
    void OnReceived(IMessage message);
  }
  public interface IBattleRoom : IDisposable
  {
    event EventHandler<MessageSentEventArgs> MessageSent;
  }
}
