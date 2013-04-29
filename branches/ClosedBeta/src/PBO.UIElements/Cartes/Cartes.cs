using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace PokemonBattleOnline.PBO.UIElements
{
  public static class Cartes
  {
    public static readonly DataTemplate Avatar;
    public static readonly DataTemplate Pokemon;

    static Cartes()
    {
      var rd = Helper.GetDictionary("Cartes", "User");
      Avatar = (DataTemplate)rd["Avatar"];
      Pokemon = Helper.GetObject<DataTemplate>("Cartes", "Pokemon");
    }
  }
}
