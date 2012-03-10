﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Reflection;
using LightStudio.Tactic.DataModels;

namespace LightStudio.PokemonBattle.Data
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

    [DataMember]
    public override string Text
    {
      get
      {
        return string.Format(base.Text, Data ?? NODATA);
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
