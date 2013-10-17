using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokemonBattleOnline.Network
{
  internal class ServerController
  {
    public readonly Server Server;
    
    public ServerController(Server server)
    {
      Server = server;
    }
  }
}
