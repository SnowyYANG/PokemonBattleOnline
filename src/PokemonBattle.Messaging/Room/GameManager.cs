using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using PokemonBattleOnline.Game;
using PokemonBattleOnline.Game.Host;

namespace PokemonBattleOnline.Messaging.Room
{
  internal interface IGameManager
  {
    void RequestTie(int userId);
    void RejectTie(int userId);
    void AcceptTie(int userId);
    void Input(int userId, ActionInput action);
  }

  [DataContract(Namespace = Namespaces.JSON)]
  class RequestTieCommand : HostCommand
  {
    public override void Execute(IHost host, int userId)
    {
      host.RequestTie(userId);
    }
  }

  [DataContract(Namespace = Namespaces.JSON)]
  class RejectTieCommand : HostCommand
  {
    public override void Execute(IHost host, int userId)
    {
      host.RejectTie(userId);
    }
  }

  [DataContract(Namespace = Namespaces.JSON)]
  class AcceptTieCommand : HostCommand
  {
    public override void Execute(IHost host, int userId)
    {
      host.AcceptTie(userId);
    }
  }

  [DataContract(Namespace = Namespaces.JSON)]
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
