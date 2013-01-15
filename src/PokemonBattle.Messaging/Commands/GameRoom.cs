using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using PokemonBattleOnline.Tactic.Network;
using PokemonBattleOnline.Tactic.Network.Commands;
using User = PokemonBattleOnline.Tactic.Network.User<PokemonBattleOnline.Messaging.UE>;

namespace PokemonBattleOnline.Messaging
{
  [DataContract(Namespace = Namespaces.JSON)]
  internal class RegisterRoom : UserCommand<E, UE>
  {
    public override void Execute(ServerUser<E, UE> server)
    {
      throw new NotImplementedException();
    }
  }
  [DataContract(Namespace = Namespaces.JSON)]
  internal class RoomRegistered : ClientCommand<E, UE>
  {
    public override void Execute(Client<E, UE> client)
    {
      throw new NotImplementedException();
    }
  }
  [DataContract(Namespace = Namespaces.JSON)]
  internal class EnterRoom : UserCommand<E, UE>
  {
    public override void Execute(ServerUser<E, UE> server)
    {
      throw new NotImplementedException();
    }
  }
  [DataContract(Namespace = Namespaces.JSON)]
  internal class UserEnterRoom : ClientCommand<E, UE>
  {
    public override void Execute(Client<E, UE> client)
    {
      throw new NotImplementedException();
    }
  }
  [DataContract(Namespace = Namespaces.JSON)]
  public class SpectateRoom : UserCommand<E, UE>
  {
    public override void Execute(ServerUser<E, UE> server)
    {
      throw new NotImplementedException();
    }
  }
  [DataContract(Namespace = Namespaces.JSON)]
  public class UserSpectate : ClientCommand<E, UE>
  {
    public override void Execute(Client<E, UE> client)
    {
      throw new NotImplementedException();
    }
  }
}
