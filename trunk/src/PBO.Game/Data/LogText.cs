using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Reflection;

namespace PokemonBattleOnline.Game
{
  public enum Alignment : byte
  {
    Left = 0,
    Center,
    Right
  }
  [DataContract(Namespace = PBOMarks.PBO)]
  public class LogText
  {
    private static readonly string[] NODATA = new string[] { "{0}", "{1}", "{2}", "{3}", "{4}", "{5}" };

    public const UInt32 DEFAULT_FOREGROUND = 0xff000000;

    [DataMember(EmitDefaultValue = false)]
    public UInt32 Background { get; protected set; }
    [DataMember(EmitDefaultValue = false)]
    private UInt32 foreground;
    public UInt32 Foreground
    {
      get { return DEFAULT_FOREGROUND ^ foreground; }
      protected set { foreground = DEFAULT_FOREGROUND ^ value; }
    }
    [DataMember(EmitDefaultValue = false)]
    public double FontSize { get; protected set; }
    [DataMember(EmitDefaultValue = false)]
    public bool IsBold { get; protected set; }
    [DataMember(EmitDefaultValue = false)]
    public bool IsItalic { get; protected set; }
    [DataMember(EmitDefaultValue = false)]
    public bool IsUnderlined { get; protected set; }
    [DataMember(EmitDefaultValue = false)]
    public Alignment Alignment { get; protected set; }
    [DataMember(EmitDefaultValue = false)]
    public virtual LogText[] Contents { get; protected set; }

    private LogText(string text, LogText[] content, UInt32 fg = DEFAULT_FOREGROUND, bool isBold = false, bool isItalic = false, bool isUnderlined = false, double fontSize = 0, Alignment alignment = Alignment.Left, UInt32 bg = 0)
    {
      Foreground = fg;
      IsBold = isBold;
      IsItalic = isItalic;
      IsUnderlined = isUnderlined;
      FontSize = fontSize;
      Alignment = alignment;
      Background = bg;
      this.RawText = text;
      Contents = content;
    }
    protected LogText(params LogText[] contents)
      : this(null, contents)
    {
    }
    protected LogText(string text)
      : this(text, null)
    {
    }

    [DataMember(Name = "text", EmitDefaultValue = false)]
    private string RawText;
    //private string _text;
    //[DataMember(Name = "text", EmitDefaultValue = false)]
    //private string RawText
    //{
    //  get { return _text; }
    //  set
    //  {
    //    if (value == null) _text = null;
    //    else _text = value.Replace("\n", Environment.NewLine);
    //  }
    //}
    public string Text
    {
      get
      {
        if (RawText == null) return null;
        return string.Format(Formatter, RawText, Data ?? NODATA);
      }
    }

    [DataMember(EmitDefaultValue = false)]
    public bool HiddenInBattle
    { get; internal set; }
    [DataMember(EmitDefaultValue = false)]
    public bool HiddenAfterBattle
    { get; internal set; }


    protected object[] Data
    { get; private set; }

    public override string ToString()
    {
      if (Contents == null) return Text;
      else
      {
        StringBuilder sb = new StringBuilder();
        foreach (LogText content in Contents) sb.Append(content.ToString());
        return sb.ToString();
      }
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

    public void SetData(params object[] data)
    {
      Data = data;
      if (Contents != null)
        foreach (LogText t in Contents) t.SetData(data);
    }

    public LogText Clone(IFormatProvider formatter)
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
          RawText = RawText
        };
    }
  }
}
