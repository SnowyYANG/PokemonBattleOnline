using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media.Effects;

namespace LightStudio.PokemonBattle.PBO.Editor
{
  static class Resources
  {
    public static readonly Style PokemonHighlightAdorner;
    public static readonly Style FolderInsertionAdorner;
    public static readonly Style PokemonDragAdorner;
    public static readonly DataTemplate PokemonType;
    public static readonly DataTemplate PokemonForm;
    public static readonly DataTemplate SelectedMove;
    public static readonly DataTemplate Collection;
    public static readonly DataTemplate OpenCollection;
    public static readonly DropShadowEffect MagentaShadow;
    public static readonly DropShadowEffect OrangeShadow;

    static Resources()
    {
      ResourceDictionary rd;
      rd = GetDictionary("DragDrop");
      PokemonHighlightAdorner = (Style)rd["PokemonHighlightAdorner"];
      FolderInsertionAdorner = (Style)rd["FolderInsertionAdorner"];
      PokemonDragAdorner = (Style)rd["PokemonDragAdorner"];
      rd = GetDictionary("PokemonType");
      PokemonType = (DataTemplate)rd["PokemonType"];
      PokemonForm = (DataTemplate)rd["PokemonForm"];
      rd = GetDictionary("SelectedMove");
      SelectedMove = (DataTemplate)rd["SelectedMove"];
      rd = GetDictionary("Generic");
      Collection = (DataTemplate)rd["Collection"];
      OpenCollection = (DataTemplate)rd["OpenCollection"];
      MagentaShadow = (DropShadowEffect)rd["MagentaShadow"];
      OrangeShadow = (DropShadowEffect)rd["OrangeShadow"];
    }

    private static ResourceDictionary GetDictionary(string name)
    {
      return (ResourceDictionary)Application.LoadComponent(
        new Uri(string.Format(@"/PBO.Editor;component/Templates/{0}.xaml", name), UriKind.Relative));
    }
  }
}
