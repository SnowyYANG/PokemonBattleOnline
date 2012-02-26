using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using LightStudio.PokemonBattle.Interactive;

namespace LightStudio.PokemonBattle.Room
{
  internal interface IRoomManager
  {
    void Chat(int userId, MessageTarget target, int targetId, string content);
    void JoinGame(int userId, Data.PokemonCustomInfo[] pokemons, int teamId);
    void SpectateGame(int userId);
    void Quit(int userId);
  }

  [DataContract(Namespace = Namespaces.DEFAULT)]
  class ChatCommand : IHostCommand
  {
    [DataMember(EmitDefaultValue = false)]
    public MessageTarget Target
    { get; set; }

    [DataMember(EmitDefaultValue = false)]
    public int TargetId
    { get; set; }

    [DataMember]
    public string Content
    { get; set; }

    public ChatCommand(string content, MessageTarget target, int targetId)
    {
      Content = content;
      Target = target;
      TargetId = targetId;
    }
    void IHostCommand.Execute(IHost host, int userId)
    {
      host.Chat(userId, Target, TargetId, Content);
    }
  }

  [DataContract(Namespace = Namespaces.DEFAULT)]
  class JoinGameCommand : IHostCommand
  {
    [DataMember]
    readonly Data.PokemonCustomInfo[] Pokemons;
    
    [DataMember]
    readonly int TeamId;

    public JoinGameCommand(Data.PokemonCustomInfo[] pokemons, int teamId)
    {
      Pokemons = pokemons;
      TeamId = teamId;
    }
    
    void IHostCommand.Execute(IHost host, int userId)
    {
      host.JoinGame(userId, Pokemons, TeamId);
    }
  }

  [DataContract(Namespace = Namespaces.DEFAULT)]
  class SpectateGameCommand : IHostCommand
  {
    void IHostCommand.Execute(IHost host, int userId)
    {
      host.SpectateGame(userId);
    }
  }

  [DataContract(Namespace = Namespaces.DEFAULT)]
  class QuitCommand : IHostCommand
  {
    void IHostCommand.Execute(IHost host, int userId)
    {
      host.Quit(userId);
    }
  }
}
