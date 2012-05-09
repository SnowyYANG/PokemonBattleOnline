using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Data;

namespace LightStudio.PokemonBattle.PBO.Converters
{
  public class IdMoveConverter : Converter<int>
  {
    public static readonly IdMoveConverter I = new IdMoveConverter();
    
    protected override object Convert(int value)
    {
      return DataService.GetMoveType(value).GetLocalizedName();
    }
  }
}
