﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace PokemonBattleOnline.Network.Lobby.Commands
{
  [DataContract(Namespace = PBOMarks.JSON)]
  internal class PrivateChat : P2PCommand
  {
    [DataMember(Name = "a")]
    string Info;

    public PrivateChat(string info)
    {
      Info = info;
    }

    public override void Execute(Client client, User from)
    {
      client.OnPrivateChat(Info, from);
    }
  }
  [DataContract(Namespace = PBOMarks.JSON)]
  internal class AdminChat : ClientCommand
  {
    [DataMember(Name = "a")]
    string Info;

    public AdminChat(string info)
    {
      Info = info;
    }

    public override void Execute(Client client)
    {
      client.OnAdminChat(Info);
    }
  }
}
