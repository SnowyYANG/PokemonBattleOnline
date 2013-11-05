using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace PokemonBattleOnline.PBO.Editor
{
  static class R
  {
    public static readonly DropShadowEffect MagentaShadow;
    public static readonly DropShadowEffect OrangeShadow;
    public static readonly DataTemplate PokemonSpecies;
    public static readonly DataTemplate SelectedMove;
    public static readonly ImageSource P00000;

    static R()
    {
      ResourceDictionary rd;
      rd = GetDictionary("R");
      MagentaShadow = (DropShadowEffect)rd["MagentaShadow"];
      OrangeShadow = (DropShadowEffect)rd["OrangeShadow"];
      PokemonSpecies = (DataTemplate)rd["PokemonSpecies"];
      SelectedMove = (DataTemplate)rd["LearnedMove"];
      P00000 = Helper.GetImage(@"Game/00000.png");
    }

    private static ResourceDictionary GetDictionary(string name)
    {
      return (ResourceDictionary)Application.LoadComponent(
        new Uri(string.Format(@"/PBO;component/Editor/{0}.xaml", name), UriKind.Relative));
    }
  }
}
