using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Reflection;
using LightStudio.PokemonBattle.Data;
using LightStudio.PokemonBattle.Game;

namespace LightStudio.PokemonBattle.Interactive
{
  [DataContract(Namespace = Namespaces.DEFAULT)]
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

    [DataMember(EmitDefaultValue = false)]
    public bool HiddenInBattle
    { get; protected set; }
    [DataMember(EmitDefaultValue = false)]
    public bool HiddenAfterBattle
    { get; protected set; }

    public override string Text
    {
      get
      {
        if (base.Text == null) return null;
        string[] textData = null;
        if (Data != null)
        {
          textData = new string[Data.Length];
          for (int i = 0; i < textData.Length; ++i)
          {
            object o = Data[i];
            if (o == null) continue;
            if (o is PokemonOutward) textData[i] = ((PokemonOutward)o).Name;
            else if (o is Tactic.DataModels.GameElement) textData[i] = ((Tactic.DataModels.GameElement)o).GetLocalizedName();
            else textData[i] = o.ToString();
          }
        }
        return string.Format(base.Text, textData ?? NODATA);
      }
      protected set
      {
        base.Text = value;
      }
    }

    public IText Clone()
    {
      return new LogText()
        {
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
