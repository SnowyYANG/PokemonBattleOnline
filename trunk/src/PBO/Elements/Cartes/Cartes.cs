using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace PokemonBattleOnline.PBO.Elements
{
  public static class Cartes
  {
    public static readonly DataTemplate Avatar;
    public static readonly DataTemplate Pokemon;

    static Cartes()
    {
      var rd = Helper.GetDictionary("Elements/Cartes", "User");
      Avatar = (DataTemplate)rd["Avatar"];
      Pokemon = Helper.GetObject<DataTemplate>("Elements/Cartes", "Pokemon");
    }
  }
}
