using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using PokemonBattleOnline.Game;
using PokemonBattleOnline.Game.Host;

namespace PokemonBattleOnline.Network.Room
{
  [DataContract(Namespace = PBOMarks.JSON)]
  class InputCommand : HostCommand
  {
    [DataMember(EmitDefaultValue = false)]
    public ActionInput Action
    { get; private set; }

    public InputCommand(ActionInput action)
    {
      this.Action = action;
    }
    public override void Execute(IHost host, int userId)
    {
      host.Input(userId, Action);
    }
  }
}
