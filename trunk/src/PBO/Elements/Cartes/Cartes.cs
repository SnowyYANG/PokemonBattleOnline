﻿using System;
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
    public static readonly DataTemplate User;
    public static readonly DataTemplate UserR;
    public static readonly SolidColorBrush[] Brushes;

    static Cartes()
    {
      var rd = Helper.GetDictionary("Elements/Cartes", "User");
      Avatar = (DataTemplate)rd["Avatar"];
      User = (DataTemplate)rd["User"];
      UserR = (DataTemplate)rd["UserR"];
      Pokemon = Helper.GetObject<DataTemplate>("Elements/Cartes", "Pokemon");
      Brushes = new SolidColorBrush[9];
      Brushes[0] = (SolidColorBrush)rd["Red"];
      Brushes[1] = (SolidColorBrush)rd["Blue"];
      Brushes[2] = (SolidColorBrush)rd["Yellow"];
      Brushes[3] = (SolidColorBrush)rd["Green"];
      Brushes[4] = (SolidColorBrush)rd["Black"];
      Brushes[5] = (SolidColorBrush)rd["Brown"];
      Brushes[6] = (SolidColorBrush)rd["Purple"];
      Brushes[7] = (SolidColorBrush)rd["Gray"];
      Brushes[8] = (SolidColorBrush)rd["Pink"];
    }

    public static SolidColorBrush GetChatBrush(string username)
    {
      return Brushes[username.GetHashCode() % 9];
    }
  }
}
