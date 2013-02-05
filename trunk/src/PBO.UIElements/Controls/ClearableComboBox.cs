using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Controls.Primitives;

namespace PokemonBattleOnline.PBO.UIElements
{
  [TemplatePart(Name = PART_CancelButton, Type = typeof(Button))]
  [TemplatePart(Name = PART_Selector, Type = typeof(ComboBox))]
  public class ClearableComboBox : ComboBox
  {
    private const string PART_CancelButton = "PART_CancelButton";
    private const string PART_Selector = "PART_Selector";

    #region CanClear
    public bool CanClear
    {
      get { return (bool)GetValue(CanClearProperty); }
      set { SetValue(CanClearProperty, value); }
    }
    public static readonly DependencyProperty CanClearProperty =
      DependencyProperty.Register("CanClear", typeof(bool), typeof(ClearableComboBox));
    #endregion

    static ClearableComboBox()
    {
      DefaultStyleKeyProperty.OverrideMetadata(typeof(ClearableComboBox),
          new FrameworkPropertyMetadata(typeof(ClearableComboBox)));
    }

    public override void OnApplyTemplate()
    {
      base.OnApplyTemplate();
      var cancelButton = GetTemplateChild(PART_CancelButton) as Button;
      cancelButton.Click += (sender, e) => SelectedItem = null;
    }
    protected override void OnSelectionChanged(SelectionChangedEventArgs e)
    {
      base.OnSelectionChanged(e);
    }
  }
}
