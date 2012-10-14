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
    void JoinGame(int userId, Data.IPokemonData[] pokemons, int teamId);
    void SpectateGame(int userId);
    void Quit(int userId);
  }

  [KnownType(typeof(Data.PokemonData))]
  [DataContract(Namespace = Namespaces.PBO)]
  class JoinGameCommand : IHostCommand
  {
    [DataMember]
    readonly Data.IPokemonData[] Pokemons;
    
    [DataMember]
    readonly int TeamId;

    public JoinGameCommand(Data.IPokemonData[] pokemons, int teamId)
    {
      Pokemons = pokemons;
      TeamId = teamId;
    }
    
    void IHostCommand.Execute(IHost host, int userId)
    {
      host.JoinGame(userId, Pokemons, TeamId);
    }
  }

  [DataContract(Namespace = Namespaces.PBO)]
  class SpectateGameCommand : IHostCommand
  {
    void IHostCommand.Execute(IHost host, int userId)
    {
      host.SpectateGame(userId);
    }
  }

  [DataContract(Namespace = Namespaces.PBO)]
  class QuitCommand : IHostCommand
  {
    void IHostCommand.Execute(IHost host, int userId)
    {
      host.Quit(userId);
    }
  }
}
