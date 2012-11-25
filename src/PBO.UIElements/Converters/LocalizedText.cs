using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Data;

namespace LightStudio.PokemonBattle.PBO.Converters
{
  public class LocalizedText : Converter<string>
  {
    public static readonly LocalizedText C = new LocalizedText();
    
    protected override object Convert(string value)
    {
      return DataService.String[value];
    }
  }
}
