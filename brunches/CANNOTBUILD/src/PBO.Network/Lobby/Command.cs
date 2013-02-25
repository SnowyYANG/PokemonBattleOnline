using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace PokemonBattleOnline.Network.Lobby
{
  [DataContract(Namespace = PBOMarks.JSON)]
  public abstract class ClientCommand
  {
    public abstract void Execute(Client client);
  }
  [DataContract(Namespace = PBOMarks.JSON)]
  public abstract class P2PCommand
  {
    public abstract void Execute(Client client, User from);
  }
  [DataContract(Namespace = PBOMarks.JSON)]
  public abstract class UserCommand
  {
    public abstract void Execute(ServerUser server);
  }
}
