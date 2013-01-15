using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace PokemonBattleOnline.Tactic.Network.Commands
{
  [DataContract(Namespace = Namespaces.JSON)]
  internal class PrivateChat<TE, TUE> : P2PCommand<TE, TUE>
  {
    [DataMember(Name = "a")]
    string Info;

    public PrivateChat(string info)
    {
      Info = info;
    }

    public override void Execute(Client<TE, TUE> client, User<TUE> from)
    {
      client.OnPrivateChat(from, Info);
    }
  }
  [DataContract(Namespace = Namespaces.JSON)]
  internal class AdminChat<TE, TUE> : ClientCommand<TE, TUE>
  {
    [DataMember(Name = "a")]
    string Info;

    public AdminChat(string info)
    {
      Info = info;
    }

    public override void Execute(Client<TE, TUE> client)
    {
      client.OnAdminChat(Info);
    }
  }
}
