using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Reflection;
using LightStudio.Tactic.DataModels;
using LightStudio.PokemonBattle.Data;
using LightStudio.PokemonBattle.Game;

namespace LightStudio.PokemonBattle.Interactive
{
  [DataContract(Namespace = Namespaces.DEFAULT)]
  public class LogLine : TextBase
  {
    private static readonly LogText LineBreak = new LogText("\n");

    public LogLine(string text)
      : base(text)
    {
    }
    public LogLine(params IText[] contents)
      : base(contents)
    {
    }

    public override string Text
    {
      get
      {
        return base.Text + "\n";
      }
      protected set
      {
        base.Text = value;
      }
    }
    public override IText[] Contents
    {
      get
      {
        var r = base.Contents.ToList();
        r.Add(LineBreak);
        return r.ToArray();
      }
      protected set
      {
        base.Contents = value;
      }
    }
  }

  [DataContract(Namespace = Namespaces.DEFAULT)]
  public class LogText : TextBase
  {
    private static readonly object[] NODATA = new object[0];

    public override string Text
    {
      get
      {
        string[] textData = null;
        if (Data != null)
        {
          textData = new string[Data.Length];
          for (int i = 0; i < textData.Length; ++i)
          {
            object o = Data[i];
            if (o is GameElement) textData[i] = ((GameElement)o).Name;
            else if (o is PokemonOutward) textData[i] = ((PokemonOutward)o).Name;
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
    
    public LogText(string text)
      : base(text)
    {
    }
    public LogText(params IText[] contents)
      : base(contents)
    {
    }
  }

  [DataContract(Namespace = Namespaces.DEFAULT)]
  public class LogObject : TextBase
  {
    [DataMember(EmitDefaultValue = false)]
    string PropertyName;
    [DataMember(EmitDefaultValue = false)]
    Dictionary<string, string> Triggers;

    object Value;

    public LogObject(string propertyName)
    {
      PropertyName = propertyName;
    }

    public override string Text
    {
      get
      {
        if (Value == null) return null;
        string s = Value.ToString();
        if (Triggers != null) s = Triggers.ValueOrDefault(s);
        return s;
      }
      protected set
      {
      }
    }

    public void AddTrigger(string key, string value)
    {
      if (Triggers == null) Triggers = new Dictionary<string, string>();
      Triggers[key] = value;
    }

    public override void SetData(params object[] data)
    {
      if (data.Length < 1) return;
      if (string.IsNullOrWhiteSpace(PropertyName)) Value = data[0];
      else
      {
        PropertyInfo p = data[0].GetType().GetProperty(PropertyName);
        if (p != null) Value = p.GetValue(data[0], null);
      }
      base.SetData(data[0]);
    }
  }
}
