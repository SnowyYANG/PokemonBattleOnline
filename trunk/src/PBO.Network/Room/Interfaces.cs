//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Runtime.Serialization;
//using PokemonBattleOnline.Game;

//namespace PokemonBattleOnline.Network
//{
//  /// <summary>
//  /// isn't it sth from host to user?
//  /// </summary>
//  internal interface IRoomUser : IDisposable
//  {
//    void ExecuteInformation(UserInformation info);
//    void InformUserSpectateGame(int userId);
//    void InformUserJoinGame(int userId, int teamId);
//    void InformUserQuit(int userId);
//    void InformEnterFailed(string message);//Join or Observe Game
//    void InformEnterSucceed(GameInitSettings settings, Player[] players, int[] spectators);
//    void InformGameStop(GameStopReason reason, int player);
//    void InformTimeUp(IEnumerable<KeyValuePair<int, int>> remainingTime);
//    void InformWaitingForInput(int[] players);
//    /// <summary>
//    /// for 4P or bandwidthsave mode
//    /// </summary>
//    /// <param name="teamIndex"></param>
//    /// <param name="pokemons"></param>
//    void InformPlayerInfo(int userId, PokemonData[] pokemons);
//    void InformReportUpdate(ReportFragment fragment);
//    void InformRequireInput(InputRequest pms, int time);
//  }
//}
