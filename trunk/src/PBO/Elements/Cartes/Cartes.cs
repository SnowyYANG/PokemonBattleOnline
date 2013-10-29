using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace PokemonBattleOnline.PBO.Elements
{
  public static class Cartes
  {
    public static readonly DataTemplate Avatar;
    public static readonly DataTemplate Pokemon;
    public static readonly Brush[] Brushes;

    static Cartes()
    {
      var rd = Helper.GetDictionary("Elements/Cartes", "User");
      Avatar = (DataTemplate)rd["Avatar"];
      Pokemon = Helper.GetObject<DataTemplate>("Elements/Cartes", "Pokemon");
      Brushes = new Brush[9];
      Brushes[0] = (Brush)rd["Red"];
      Brushes[1] = (Brush)rd["Blue"];
      Brushes[2] = (Brush)rd["Yellow"];
      Brushes[3] = (Brush)rd["Green"];
      Brushes[4] = (Brush)rd["Black"];
      Brushes[5] = (Brush)rd["Brown"];
      Brushes[6] = (Brush)rd["Purple"];
      Brushes[7] = (Brush)rd["Gray"];
      Brushes[8] = (Brush)rd["Pink"];
    }

    public static Brush GetChatBrush(string username)
    {
      return Brushes[username.GetHashCode() % 9];
    }
  }
}
