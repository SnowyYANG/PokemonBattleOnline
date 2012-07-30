using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using LightStudio.PokemonBattle.Game;
using LightStudio.PokemonBattle.Game.Host;

namespace LightStudio.PokemonBattle.Messaging.Room
{
  internal interface IGameManager
  {
    void RequestTie(int userId);
    void RejectTie(int userId);
    void AcceptTie(int userId);
    void Input(int userId, ActionInput action);
  }

  [DataContract(Namespace = Namespaces.DEFAULT)]
  class RequestTieCommand : IHostCommand
  {
    void IHostCommand.Execute(IHost host, int userId)
    {
      host.RequestTie(userId);
    }
  }

  [DataContract(Namespace = Namespaces.DEFAULT)]
  class RejectTieCommand : IHostCommand
  {
    void IHostCommand.Execute(IHost host, int userId)
    {
      host.RejectTie(userId);
    }
  }

  [DataContract(Namespace = Namespaces.DEFAULT)]
  class AcceptTieCommand : IHostCommand
  {
    void IHostCommand.Execute(IHost host, int userId)
    {
      host.AcceptTie(userId);
    }
  }

  [DataContract(Namespace = Namespaces.DEFAULT)]
  class InputCommand : IHostCommand
  {
    [DataMember(EmitDefaultValue = false)]
    public ActionInput Action
    { get; private set; }

    public InputCommand(ActionInput action)
    {
      this.Action = action;
    }
    void IHostCommand.Execute(IHost host, int userId)
    {
      host.Input(userId, Action);
    }
  }
}
