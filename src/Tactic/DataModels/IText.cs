using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace LightStudio.Tactic.DataModels
{
  public enum Alignment : byte
  {
    Left = 0,
    Center,
    Right
  }
  public interface IText
  {
    UInt32 Background { get; }
    UInt32 Foreground { get; }
    double FontSize { get; }
    bool IsBold { get; }
    bool IsItalic { get; }
    bool IsUnderlined { get; }
    Alignment Alignment { get; }

    void SetData(params object[] data);
    void ClearData();
    /// <summary>
    /// Text works only when Contents is null
    /// </summary>
    string Text { get; }
    /// <summary>
    /// Text works only when Contents is null
    /// </summary>
    IText[] Contents { get; }
  }

  [DataContract(Namespace = Namespaces.DEFAULT)]
  public abstract class TextBase : IText
  {
    [DataMember(EmitDefaultValue = false)]
    private string text;

    [DataMember(EmitDefaultValue = false)]
    public UInt32 Background { get; protected set; }
    [DataMember]
    public UInt32 Foreground { get; protected set; }
    [DataMember]
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
    public virtual IText[] Contents { get; protected set; }

    private TextBase(string text, IText[] content, UInt32 fg = 0xff000000, bool isBold = false, bool isItalic = false, bool isUnderlined = false, double fontSize = 15, Alignment alignment = Alignment.Left, UInt32 bg = 0)
    {
      Foreground = fg;
      IsBold = isBold;
      IsItalic = isItalic;
      IsUnderlined = isUnderlined;
      FontSize = fontSize;
      Alignment = alignment;
      Background = bg;
      this.text = text;
      Contents = content;
    }
    protected TextBase(params IText[] contents)
      : this(null, contents)
    {
    }
    protected TextBase(string text)
      : this(text, null)
    {
    }

    public virtual string Text
    {
      get { return text; }
      protected set
      {
        text = value;
        if (value != null) Contents = null;
      }
    }

    protected object[] Data
    { get; private set; }
    public virtual void SetData(params object[] data)
    {
      Data = data;
      if (Contents != null)
        foreach (IText t in Contents)
          t.SetData(data);
    }
    public void ClearData()
    {
      Data = null;
    }
  }
}
