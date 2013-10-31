using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonBattleOnline.Game;

namespace PokemonBattleOnline.PBO.Editor
{
  class PokemonTeamC : Converter<PokemonTeam>
  {
    public static readonly PokemonTeamC C = new PokemonTeamC();
    
    protected override object Convert(PokemonTeam value)
    {
      return new TeamVM(value);
    }
  }
}
