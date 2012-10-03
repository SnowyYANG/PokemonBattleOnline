using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Reflection;
using LightStudio.PokemonBattle.Data;

namespace LightStudio.PokemonBattle.Game
{
  [DataContract(Namespace = Namespaces.PBO)]
  public class LogText : Tactic.DataModels.TextBase<IText>, IText
  {
    private static readonly string[] NODATA = new string[] { "{0}", "{1}", "{2}", "{3}", "{4}", "{5}" };

    public LogText(string text)
      : base(text)
    {
    }
    public LogText(params IText[] contents)
      : base(contents)
    {
    }

    /// <summary>
    /// it's ok to be null
    /// </summary>
    protected IFormatProvider Formatter
    { get; private set; }

    [DataMember(EmitDefaultValue = false)]
    public bool HiddenInBattle
    { get; internal set; }
    [DataMember(EmitDefaultValue = false)]
    public bool HiddenAfterBattle
    { get; internal set; }

    public override string Text
    {
      get
      {
        if (base.Text == null) return null;
        return string.Format(Formatter, base.Text, Data ?? NODATA);
      }
      protected set
      {
        base.Text = value;
      }
    }
    public IText Clone(IFormatProvider formatter)
    {
      return new LogText()
        {
          Formatter = formatter,
          Alignment = this.Alignment,
          Background = this.Background,
          Contents = this.Contents,
          FontSize = this.FontSize,
          Foreground = this.Foreground,
          HiddenAfterBattle = this.HiddenAfterBattle,
          HiddenInBattle = this.HiddenInBattle,
          IsBold = this.IsBold,
          IsItalic = this.IsItalic,
          IsUnderlined = this.IsUnderlined,
          Text = base.Text
        };
    }
  }
}
