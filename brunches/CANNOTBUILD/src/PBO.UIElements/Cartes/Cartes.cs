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
    public static readonly DataTemplate User;
    public static readonly DataTemplate Pokemon;

    static Cartes()
    {
      var rd = Helper.GetDictionary("Cartes", "User");
      Avatar = (DataTemplate)rd["Avatar"];
      User = (DataTemplate)rd["User"];
      Pokemon = Helper.GetObject<DataTemplate>("Cartes", "Pokemon");
    }
  }
}
