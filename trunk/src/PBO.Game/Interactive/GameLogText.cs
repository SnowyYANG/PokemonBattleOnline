using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Reflection;
using PokemonBattleOnline.Data;

namespace PokemonBattleOnline.Game
{
  [DataContract(Namespace = PBOMarks.PBO)]
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
    private IFormatProvider _formatter;
    protected IFormatProvider Formatter
    {
      get { return _formatter; }
      private set
      {
        _formatter = value;
        if (Contents != null)
          foreach (var c in Contents)
            if (c is LogText) ((LogText)c).Formatter = value;
      }
    }

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

    public override void SetData(params object[] data)
    {
      if (Data == null) base.SetData(data);
    }

    public IText Clone(IFormatProvider formatter)
    {
      return new LogText()
        {
          Alignment = this.Alignment,
          Background = this.Background,
          Contents = this.Contents == null ? null : this.Contents.Select((c) => c.Clone(formatter)).ToArray(),
          Foreground = this.Foreground,
          Formatter = formatter,
          FontSize = this.FontSize,
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
