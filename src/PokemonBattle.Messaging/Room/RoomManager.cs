using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using LightStudio.PokemonBattle.Game;

namespace LightStudio.PokemonBattle.Messaging.Room
{
  internal interface IRoomManager
  {
    void JoinGame(int userId, Data.PokemonCustomInfo[] pokemons, int teamId);
    void SpectateGame(int userId);
    void Quit(int userId);
  }

  [DataContract(Namespace = Namespaces.LIGHT)]
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

  [DataContract(Namespace = Namespaces.LIGHT)]
  class SpectateGameCommand : IHostCommand
  {
    void IHostCommand.Execute(IHost host, int userId)
    {
      host.SpectateGame(userId);
    }
  }

  [DataContract(Namespace = Namespaces.LIGHT)]
  class QuitCommand : IHostCommand
  {
    void IHostCommand.Execute(IHost host, int userId)
    {
      host.Quit(userId);
    }
  }
}
