﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Globalization;
using PokemonBattleOnline.Data;

namespace PokemonBattleOnline.PBO.Converters
{
  [ValueConversion(typeof(MoveCategory), typeof(string))]
  public class MoveCategoryText : Converter<MoveCategory>
  {
    public static readonly MoveCategoryText C = new MoveCategoryText();
    
    protected override object Convert(MoveCategory value)
    {
      return DataService.String[value.ToString()];
    }
  }
}
