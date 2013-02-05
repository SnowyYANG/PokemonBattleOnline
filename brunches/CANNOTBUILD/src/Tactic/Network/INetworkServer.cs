using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace PokemonBattleOnline.Tactic.Network
{
  public interface INetworkServer : IDisposable
  {
    event Action<INetworkUser> NewComingUser;
    IPEndPoint ListenerEndPoint { get; }
    bool CanAddUser { get; set; }
  }
}
