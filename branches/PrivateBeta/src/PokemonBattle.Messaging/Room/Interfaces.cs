using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightStudio.PokemonBattle.Messaging.Room
{
  internal interface IHostCommand: LightStudio.Tactic.Messaging.IMessagable
  {
    void Execute(IHost host, int senderId);
  }

  internal interface IUserInformation : LightStudio.Tactic.Messaging.IMessagable
  {
    void Execute(IRoomUser user);
  }

  /// <summary>
  /// user to host
  /// </summary>
  internal interface IHost : IRoomManager, IGameManager, IDisposable
  {
    event Action Closed;
    void Kick(int targetId);
    void StartGame();
    void CloseRoom();
    void ExecuteCommand(IHostCommand command, int userId);
  }

  /// <summary>
  /// isn't it sth from host to user?
  /// </summary>
  internal interface IRoomUser : IRoomInformer, IGameInformer, IDisposable
  {
    void ExecuteInformation(IUserInformation info);
  }
}
