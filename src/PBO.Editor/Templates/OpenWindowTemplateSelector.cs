using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace LightStudio.PokemonBattle.PBO.Editor
{
  class OpenWindowTS : DataTemplateSelector
  {
    public static readonly OpenWindowTS I = new OpenWindowTS();

    public override DataTemplate SelectTemplate(object item, DependencyObject container)
    {
      var element = container as FrameworkElement;
      if (item is IFolderViewModel) return Resources.OpenFolder;
      else if (item is SearchResultViewModel) return Resources.SearchResult;
      else return null;
    }
  }
}
