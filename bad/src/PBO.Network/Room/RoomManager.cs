using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using PokemonBattleOnline.Game;

namespace PokemonBattleOnline.Network.Room
{
  internal interface IRoomManager
  {
    void JoinGame(int userId, Data.IPokemonData[] pokemons, int teamId);
    void SpectateGame(int userId);
    void Quit(int userId);
  }

  [KnownType(typeof(Data.PokemonData))]
  [DataContract(Name = "jc", Namespace = PBOMarks.JSON)]
  class JoinGameCommand : HostCommand
  {
    [DataMember(Name = "a")]
    readonly Data.IPokemonData[] Pokemons;
    
    [DataMember(Name = "b", EmitDefaultValue = false)]
    readonly int TeamId;

    public JoinGameCommand(Data.IPokemonData[] pokemons, int teamId)
    {
      Pokemons = pokemons;
      TeamId = teamId;
    }

    public override void Execute(IHost host, int userId)
    {
      host.JoinGame(userId, Pokemons, TeamId);
    }
  }

  [DataContract(Namespace = PBOMarks.JSON)]
  class SpectateGameCommand : HostCommand
  {
    public override void Execute(IHost host, int userId)
    {
      host.SpectateGame(userId);
    }
  }

  [DataContract(Namespace = PBOMarks.JSON)]
  class QuitCommand : HostCommand
  {
    public override void Execute(IHost host, int userId)
    {
      host.Quit(userId);
    }
  }
}
