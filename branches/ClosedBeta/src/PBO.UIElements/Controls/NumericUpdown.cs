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
using System.Windows.Controls.Primitives;

namespace PokemonBattleOnline.PBO.UIElements
{
  public class LimitedValueRule : ValidationRule
  {
    static readonly ValidationResult VALID = new ValidationResult(true, null);
    static readonly ValidationResult INVALID = new ValidationResult(false, null);

    public LimitedValueRule(double min, double max)
    {
      Min = min;
      Max = max;
    }

    public double Max { get; set; }
    public double Min { get; set; }
    public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
    {
      try
      {
        double d = double.Parse((string)value);
        if (d >= Min && d <= Max) return VALID;
        return INVALID;
      }
      catch
      {
        return INVALID;
      }
    }
  }
  
  [TemplatePart(Name = PART_IncreaseButton, Type = typeof(ButtonBase))]
  [TemplatePart(Name = PART_DecreaseButton, Type = typeof(ButtonBase))]
  [TemplatePart(Name=PART_TextBox,Type=typeof(TextBox))]
  [TemplatePart(Name=PART_Drag,Type=typeof(Thumb))]
  public class NumericUpdown : RangeBase
  {
    const string PART_IncreaseButton = "PART_IncreaseButton";
    const string PART_DecreaseButton = "PART_DecreaseButton";
    const string PART_TextBox = "PART_TextBox";
    const string PART_Drag = "PART_Drag";
    static NumericUpdown()
    {
      DefaultStyleKeyProperty.OverrideMetadata(typeof(NumericUpdown), new FrameworkPropertyMetadata(typeof(NumericUpdown)));
    }

    #region DragScale
    public static readonly DependencyProperty DragScaleProperty =
      DependencyProperty.Register("DragScale", typeof(double), typeof(NumericUpdown));
    public double DragScale
    {
      get { return (double)GetValue(DragScaleProperty); }
      set { SetValue(DragScaleProperty, value); }
    }
    #endregion

    #region DragMax
    static readonly DependencyPropertyKey DragMaxPropertyKey =
      DependencyProperty.RegisterReadOnly("DragMax", typeof(double), typeof(NumericUpdown), new PropertyMetadata());
    public static readonly DependencyProperty DragMaxProperty = DragMaxPropertyKey.DependencyProperty;
    public double DragMax
    {
      get { return (double)GetValue(DragMaxProperty); }
      private set { SetValue(DragMaxPropertyKey, value); }
    }
    #endregion

    #region DragMin
    static readonly DependencyPropertyKey DragMinPropertyKey =
      DependencyProperty.RegisterReadOnly("DragMin", typeof(double), typeof(NumericUpdown), new PropertyMetadata());
    public static readonly DependencyProperty DragMinProperty = DragMinPropertyKey.DependencyProperty;
    public double DragMin
    {
      get { return (double)GetValue(DragMinProperty); }
      private set { SetValue(DragMinPropertyKey, value); }
    }
    #endregion

    #region DragValue
    static readonly DependencyPropertyKey DragValuePropertyKey =
      DependencyProperty.RegisterReadOnly("DragValue",typeof(double), typeof(NumericUpdown), new PropertyMetadata());
    public static readonly DependencyProperty DragValueProperty = DragValuePropertyKey.DependencyProperty;
    public double DragValue
    { get { return (double)GetValue(DragValueProperty); } }
    #endregion

    TextBox textBox;
    Thumb drag;
    double dragStartedPosition;
    LimitedValueRule validationRule;

    public NumericUpdown()
    {
      validationRule = new LimitedValueRule(Minimum, Maximum);
      ValueChanged += (sender, e) => ValueRefreshDrag();
      MouseWheel += (sender, e) => {
        if (e.Delta > 0)
          if (Value + SmallChange < DragMax * LargeChange) Value += SmallChange;
          else Value = DragMax * LargeChange;
        else
          if (Value - SmallChange > DragMin * LargeChange) Value -= SmallChange;
          else Value = DragMin * LargeChange;
      };
    }
    private void ValueRefreshDrag()
    {
      if (drag != null && !drag.IsDragging)
      {
        double dv = Value * DragScale;
        if (dv > DragMax) dv = DragMax;
        else if (dv < DragMin) dv = DragMin;
        SetValue(DragValuePropertyKey, dv);
        Canvas.SetLeft(drag, DragValue - 3);
      }
    }
    private void DragRefreshValue(double dragValue)
    {
      if (DragScale != 0 && dragValue >= DragMin && dragValue <= DragMax)
      {
        Value = dragValue / DragScale;
        SetValue(DragValuePropertyKey, Value * DragScale);
      }
    }
    private void SetNearestValue(double value)
    {
      sbyte t = 60;
      while ((Value > value ? Value - value : value - Value) >= 2)
      {
        value = (Value + value) / 2;
        Value = value;
        if (--t < 0) break;
      }
      if (Value < value) Value += 1;
      else if (Value > value) Value -= 1;
    }
    protected override void OnMinimumChanged(double oldMinimum, double newMinimum)
    {
      base.OnMinimumChanged(oldMinimum, newMinimum);
      validationRule.Min = newMinimum;
      DragMin = Math.Ceiling(newMinimum * DragScale);
    }
    protected override void OnMaximumChanged(double oldMaximum, double newMaximum)
    {
      base.OnMaximumChanged(oldMaximum, newMaximum);
      validationRule.Max = newMaximum;
      DragMax = Math.Floor(newMaximum * DragScale);
    }

    public override void OnApplyTemplate()
    {
      base.OnApplyTemplate();
      var b = GetTemplateChild(PART_IncreaseButton) as ButtonBase;
      if (b != null)
        b.Click += (sender, e) =>
        {
          if (Value + LargeChange < Maximum) Value += LargeChange;
          else Value = Maximum;
        };
      b = GetTemplateChild(PART_DecreaseButton) as ButtonBase;
      if (b != null)
        b.Click += (sender, e) =>
        {
          if (Value - LargeChange > Minimum) Value -= LargeChange;
          else Value = Minimum;
        };

      textBox = GetTemplateChild(PART_TextBox) as TextBox;
      if (textBox != null)
      {
        Binding text = new Binding("Value");
        text.RelativeSource = new RelativeSource(RelativeSourceMode.TemplatedParent);
        text.Mode = BindingMode.TwoWay;
        text.ValidationRules.Add(validationRule);
        text.NotifyOnValidationError = true;
        textBox.SetBinding(TextBox.TextProperty, text);
        Validation.AddErrorHandler(textBox, (sender, e) =>
          {
            var binding = ((TextBox)sender).GetBindingExpression(TextBox.TextProperty);
            Application.Current.Dispatcher.BeginInvoke(new Action(binding.UpdateTarget));
          });
      }//if (textBox != null)

      drag = GetTemplateChild(PART_Drag) as Thumb;
      if (drag != null)
      {
        drag.DragStarted += (sender, e) =>
          {
            var v = Math.Floor(Canvas.GetLeft(drag) + 3);
            if (!double.IsNaN(v)) dragStartedPosition = v;
          };
        drag.DragDelta += (sender, e) =>
          {
            double dv = dragStartedPosition + e.HorizontalChange;
            if (dv < DragMin) dv = DragMin;
            else if (dv > DragMax) dv = DragMax;
            DragRefreshValue(dv);
            if (DragValue != dv) SetNearestValue(dv / DragScale);
          };
        drag.DragCompleted += (sender, e) => Canvas.SetLeft(drag, DragValue - 3);
      }//if (drag != null)
    }
  }
}
