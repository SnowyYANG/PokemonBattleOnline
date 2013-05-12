using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Brush = System.Windows.Media.Brush;
using ControlTemplate = System.Windows.Controls.ControlTemplate;

namespace LightStudio.PokemonBattle.PBO.UIElements
{
  public static class Buttons
  {
    public static readonly ControlTemplate GameButton;
    public static readonly Brush Luster;
    public static readonly Brush GrayBg;
    public static readonly Style Colorful;
    public static readonly Style Move;
    public static readonly Style Struggle;
    public static readonly Style Pokemon;
    public static readonly Style Text;

    static Buttons()
    {
      ResourceDictionary rd = GetDictionary("GameButton");
      GameButton = rd["GameButton"] as ControlTemplate;
      Luster = rd["Luster"] as Brush;
      GrayBg = rd["GrayBG"] as Brush;
      Colorful = GetStyle("Colorful");
      Text = GetStyle("Text");
      Pokemon = GetStyle("Pokemon");
      rd = GetDictionary("Move");
      Move = rd["MoveButton"] as Style;
      Struggle = rd["StruggleButton"] as Style;
    }

    static ResourceDictionary GetDictionary(string filename)
    {
      return Helper.GetDictionary("Buttons", filename);
    }
    static Style GetStyle(string name)
    {
      return GetDictionary(name)[name + "Button"] as Style;
    }
  }
}
