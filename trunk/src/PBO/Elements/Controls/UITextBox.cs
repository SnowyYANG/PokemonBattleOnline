using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PokemonBattleOnline.PBO.Elements
{
  public partial class UITextBox : TextBox
  {
    static UITextBox()
    {
      DefaultStyleKeyProperty.OverrideMetadata(typeof(UITextBox), new FrameworkPropertyMetadata(typeof(UITextBox)));
    }

    #region EndEditCommand
    public static readonly DependencyProperty EndEditCommandProperty = DependencyProperty.Register("EndEditCommand", typeof(ICommand), typeof(UITextBox));
    public ICommand EndEditCommand
    {
      get { return (ICommand)GetValue(EndEditCommandProperty); }
      set { SetValue(EndEditCommandProperty, value); }
    }
    #endregion

    #region IsEditing
    public static readonly DependencyProperty IsEditingProperty = DependencyProperty.Register("IsEditing", typeof(bool), typeof(UITextBox), new PropertyMetadata(false, new PropertyChangedCallback(IsEditingChanged)));
    public bool IsEditing
    {
      get { return (bool)GetValue(IsEditingProperty); }
      set { SetValue(IsEditingProperty, value); }
    }
    private static void IsEditingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      UITextBox t = d as UITextBox;
      if ((bool)e.NewValue)
      {
        t.Visibility = System.Windows.Visibility.Visible;
        t.Focus();
        t.SelectAll();
      }
      else
      {
        t.Visibility = System.Windows.Visibility.Hidden;
      }
    }
    #endregion

    public UITextBox() : base()
    {
      LostFocus += (sender, e) => EndEdit();
      KeyDown += (sender, e) =>
        {
          if (e.Key == Key.Enter) EndEdit();
        };
      Visibility = System.Windows.Visibility.Hidden;
    }

    void EndEdit()
    {
      IsEditing = false;
      if (EndEditCommand != null) EndEditCommand.Execute(Text);
    }
  }
}
