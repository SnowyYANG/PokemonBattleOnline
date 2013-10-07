using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using PokemonBattleOnline.Game;

namespace PokemonBattleOnline.Network.Room
{
  [DataContract(Namespace = PBOMarks.JSON)]
  internal abstract class HostCommand //knowntype
  {
    public abstract void Execute(IHost host, int senderId);
  }

  [DataContract(Namespace = PBOMarks.JSON)]
  internal abstract class UserInformation //knowntype
  {
    public abstract void Execute(IRoomUser user);
  }

  /// <summary>
  /// user to host
  /// </summary>
  internal interface IHost : IDisposable
  {
    event Action Closed;
    void ExecuteCommand(HostCommand command, int userId);
    void StartGame();
    void CloseRoom();
    void Input(int userId, ActionInput action);
    void JoinGame(int userId, PokemonData[] pokemons, int teamId);//成为玩家
    void SpectateGame(int userId);//不成为玩家
    void Enter(int userId);
    void Quit(int userId);
  }

  /// <summary>
  /// isn't it sth from host to user?
  /// </summary>
  internal interface IRoomUser : IDisposable
  {
    void ExecuteInformation(UserInformation info);
    void InformUserSpectateGame(int userId);
    void InformUserJoinGame(int userId, int teamId);
    void InformUserQuit(int userId);
    void InformEnterFailed(string message);//Join or Observe Game
    void InformEnterSucceed(GameInitSettings settings, Player[] players, int[] spectators);
    void InformGameStop(GameStopReason reason, int player);
    void InformTimeUp(IEnumerable<KeyValuePair<int, int>> remainingTime);
    void InformWaitingForInput(int[] players);
    /// <summary>
    /// for 4P or bandwidthsave mode
    /// </summary>
    /// <param name="teamIndex"></param>
    /// <param name="pokemons"></param>
    void InformPlayerInfo(int userId, PokemonData[] pokemons);
    void InformReportUpdate(ReportFragment fragment);
    void InformRequireInput(InputRequest pms, int time);
  }
}
