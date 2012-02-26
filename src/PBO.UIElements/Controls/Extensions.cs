using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Interactivity;

namespace LightStudio.PokemonBattle.PBO.UIElements
{
  static class Extensions
  {
    public static T GetParent<T>(this DependencyObject element) where T : DependencyObject
    {
      do
      {
        element = VisualTreeHelper.GetParent(element);
      }
      while ((element != null) && !(element is T));
      return (element as T);
    }
  }
}
