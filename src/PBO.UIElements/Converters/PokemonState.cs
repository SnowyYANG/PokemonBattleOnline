using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Controls;
using System.Windows.Media;
using System.Globalization;
using LightStudio.PokemonBattle.Data;

namespace LightStudio.PokemonBattle.PBO.Converters
{
  public class PokemonStateText : Converter<PokemonState>
  {
    protected override object Convert(PokemonState value)
    {
      switch (value)
      {
        case PokemonState.Burned:
          return "烧伤";
        case PokemonState.Faint:
          return "濒死";
        case PokemonState.Frozen:
          return "冻结";
        case PokemonState.Paralyzed:
          return "麻痹";
        case PokemonState.Poisoned:
          return "中毒";
        case PokemonState.BadlyPoisoned:
          return "剧毒";
        case PokemonState.Sleeping:
          return "睡眠";
      }
      return string.Empty;
    }
  }
}