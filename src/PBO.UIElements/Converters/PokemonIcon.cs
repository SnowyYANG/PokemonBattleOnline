using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Data;
using LightStudio.PokemonBattle.Game;

namespace LightStudio.PokemonBattle.PBO.Converters
{
  public class PokemonIcon : Converter<Pokemon>
  {
    public static readonly PokemonIcon C = new PokemonIcon();
    
    protected override object Convert(Pokemon value)
    {
      return GameDataService.GetPokemonIcon(value.Form, value.Gender);
    }
  }
  public class PokemonDataIcon : Converter<IPokemonData>
  {
    public static readonly PokemonDataIcon C = new PokemonDataIcon();
    
    protected override object Convert(IPokemonData value)
    {
      return GameDataService.GetPokemonIcon(value.Form, value.Gender);
    }
  }
  public class PokemonTypeIcon : Converter<PokemonType>
  {
    public static readonly PokemonTypeIcon C = new PokemonTypeIcon();
    
    protected override object Convert(PokemonType value)
    {
      return GameDataService.GetPokemonIcon(value.GetForm(0), value.Genders.First());
    }
  }
  public class PokemonFormIcon : Converter<PokemonForm>
  {
    public static readonly PokemonFormIcon C = new PokemonFormIcon();

    protected override object Convert(PokemonForm value)
    {
      return GameDataService.GetPokemonIcon(value, value.Type.Genders.First());
    }
  }
}
