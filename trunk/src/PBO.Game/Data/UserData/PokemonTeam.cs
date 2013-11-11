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
    public PokemonTeam()
    {
      Pokemons = new PokemonData[6];
      CanBattle = true;
    }
    public PokemonTeam(PokemonData[] pokemons)
    {
      Pokemons = pokemons.Length == 6 ? pokemons : new PokemonData[6];
      CanBattle = true;
    }

    [DataMember]
    public PokemonData[] Pokemons
    { get; private set; }

    [DataMember]
    public string Name
    { get; set; }

    [DataMember]
    public bool CanBattle
    { get; set; }

    public string Export()
    {
      return UserData.Export(Pokemons);
    }
  }
}
