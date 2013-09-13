using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonBattleOnline.Game;

namespace PokemonBattleOnline.PBO.Editor
{
  class PokemonBTC : Converter<PokemonBT>
  {
    public static readonly PokemonBTC C = new PokemonBTC();
    
    protected override object Convert(PokemonBT value)
    {
      return value.Size == 6 ? (object)new TeamVM(value) : new BoxVM(value);
    }
  }
  class PokemonDataC : Converter<PokemonData>
  {
    public static readonly PokemonDataC C = new PokemonDataC();
    
    protected override object Convert(PokemonData value)
    {
      return new PokemonVM(value);
    }
  }
}
