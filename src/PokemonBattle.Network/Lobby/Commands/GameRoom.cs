using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using PokemonBattleOnline.Network;

namespace PokemonBattleOnline.Network.Lobby.Commands
{
  [DataContract(Namespace = Namespaces.JSON)]
  internal class RegisterRoom : UserCommand
  {
    public override void Execute(ServerUser server)
    {
      throw new NotImplementedException();
    }
  }
  [DataContract(Namespace = Namespaces.JSON)]
  internal class RoomRegistered : ClientCommand
  {
    public override void Execute(Client client)
    {
      throw new NotImplementedException();
    }
  }
  [DataContract(Namespace = Namespaces.JSON)]
  internal class EnterRoom : UserCommand
  {
    public override void Execute(ServerUser server)
    {
      throw new NotImplementedException();
    }
  }
  [DataContract(Namespace = Namespaces.JSON)]
  internal class UserEnterRoom : ClientCommand
  {
    public override void Execute(Client client)
    {
      throw new NotImplementedException();
    }
  }
  [DataContract(Namespace = Namespaces.JSON)]
  public class SpectateRoom : UserCommand
  {
    public override void Execute(ServerUser server)
    {
      throw new NotImplementedException();
    }
  }
  [DataContract(Namespace = Namespaces.JSON)]
  public class UserSpectate : ClientCommand
  {
    public override void Execute(Client client)
    {
      throw new NotImplementedException();
    }
  }
}
