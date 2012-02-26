using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Scripting.Hosting;
using IronPython.Hosting;

namespace LightStudio.Tactic.DataModels
{
  /// <summary>
  /// support three mode: implementation, delegate or dynamic
  /// </summary>
  public interface IExecutableElement<in T>
  {
    void Attached(T arg);
    void Unattached(T arg);
  }

  public class DelegateExecutableElement<T> : IExecutableElement<T>
  {
    private readonly Action<T> attached;
    private readonly Action<T> unattached;
    public DelegateExecutableElement(Action<T> a, Action<T> ua)
    {
      attached = a;
      unattached = ua;
    }
    public void Attached(T arg)
    {
      attached(arg);
    }
    public void Unattached(T arg)
    {
      unattached(arg);
    }
  }

  /// <summary>
  /// currently python only
  /// </summary>
  public class DynamicExecutableElement : IExecutableElement<object>
  {
    private static readonly ScriptEngine ENGINE = Python.CreateEngine();
    
    dynamic _dynamic;

    public DynamicExecutableElement(ScriptScope scope)
    {
      _dynamic = scope;
    }
    public DynamicExecutableElement(ScriptSource source)
    {
      ScriptScope scope = ENGINE.CreateScope();
      source.Execute(scope);
      _dynamic = scope;
    }
    public DynamicExecutableElement(string source)
      : this(ENGINE.CreateScriptSourceFromString(source))
    {
    }
    public void Attached(object arg)
    {
      _dynamic.Attached(arg);
    }
    public void Unattached(object arg)
    {
      _dynamic.Unattached(arg);
    }
  }
}
