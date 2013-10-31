using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace PokemonBattleOnline.Game
{
	[DataContract(Namespace = PBOMarks.PBO)]
  public class PokemonTeam
	{
    [DataMember]
    public readonly PokemonData[] Pokemons;

    internal PokemonTeam()
    {
      Pokemons = new PokemonData[6];
    }
    internal PokemonTeam(PokemonData[] pokemons)
    {
      Pokemons = pokemons.Length == 6 ? pokemons : new PokemonData[6];
    }

    [DataMember]
    public string Name
    { get; set; }

    public string Export()
    {
      return Helper.Export(Pokemons);
    }
  }
}
