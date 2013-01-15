using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace PokemonBattleOnline.Tactic.Network
{
  public interface INetworkServer : IDisposable
  {
    event Action<INetworkUser> NewUser;
    
    IPEndPoint ListenerEndPoint { get; }
    IEnumerable<INetworkUser> Users { get; }
    bool CanAddUser { get; set; }
    
    INetworkUser GetUser(int id);
    void RemoveUser(int id);
  }
}
