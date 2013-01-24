using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace PokemonBattleOnline.Network.Room
{
  [DataContract(Namespace = Namespaces.JSON)]
  internal abstract class HostCommand //knowntype
  {
    public abstract void Execute(IHost host, int senderId);
  }

  [DataContract(Namespace = Namespaces.JSON)]
  internal abstract class UserInformation //knowntype
  {
    public abstract void Execute(IRoomUser user);
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
    void ExecuteCommand(HostCommand command, int userId);
  }

  /// <summary>
  /// isn't it sth from host to user?
  /// </summary>
  internal interface IRoomUser : IRoomInformer, IGameInformer, IDisposable
  {
    void ExecuteInformation(UserInformation info);
  }
}
