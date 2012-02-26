using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace LightStudio.PokemonBattle.PBO.Editor
{
  static class Resources
  {
    public static readonly Style PokemonHighlightAdorner;
    public static readonly Style FolderInsertionAdorner;
    public static readonly Style PokemonDragAdorner;
    public static readonly DataTemplate PokemonType;
    public static readonly DataTemplate SelectedMove;
    public static readonly DataTemplate FolderIcon;
    public static readonly DataTemplate OpenFolder;
    public static readonly DataTemplate SearchResult;

    static Resources()
    {
      ResourceDictionary rd;
      rd = GetDictionary("DragDrop");
      PokemonHighlightAdorner = (Style)rd["PokemonHighlightAdorner"];
      FolderInsertionAdorner = (Style)rd["FolderInsertionAdorner"];
      PokemonDragAdorner = (Style)rd["PokemonDragAdorner"];
      rd = GetDictionary("PokemonType");
      PokemonType = (DataTemplate)rd["PokemonType"];
      rd = GetDictionary("SelectedMove");
      SelectedMove = (DataTemplate)rd["SelectedMove"];
      rd = GetDictionary("Generic");
      FolderIcon = (DataTemplate)rd["FolderIcon"];
      OpenFolder = (DataTemplate)rd["OpenFolder"];
      SearchResult = (DataTemplate)rd["SearchResult"];
    }

    private static ResourceDictionary GetDictionary(string name)
    {
      return (ResourceDictionary)Application.LoadComponent(
        new Uri(string.Format(@"/PBO.Editor;component/Templates/{0}.xaml", name), UriKind.Relative));
    }
  }
}
