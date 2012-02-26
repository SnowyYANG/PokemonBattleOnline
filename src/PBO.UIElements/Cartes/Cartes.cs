using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace LightStudio.PokemonBattle.PBO.UIElements
{
  public static class Cartes
  {
    public static readonly DataTemplate DetailedAvatar;
    public static readonly DataTemplate User;
    public static readonly DataTemplate Move;

    static Cartes()
    {
      ResourceDictionary rd = Helper.GetDictionary("Cartes", "Move");
      Move = rd["MoveCarte"] as DataTemplate;
      rd = Helper.GetDictionary("Cartes", "User");
      DetailedAvatar = (DataTemplate)rd["DetailedAvatar"];
      User = (DataTemplate)rd["User"];
    }
  }
}
