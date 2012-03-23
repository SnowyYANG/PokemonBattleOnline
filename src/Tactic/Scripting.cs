using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Scripting.Hosting;
using IronPython.Hosting;

namespace LightStudio.Tactic
{
  public static class Scripting
  {
    private static readonly ScriptEngine PY;
    private static readonly ScriptScope scope;

    static Scripting()
    {
      var files = PY.GetSearchPaths();
      files.Add(Environment.CurrentDirectory + "\\dll");
      PY.SetSearchPaths(files);
      PY = Python.CreateEngine();
      scope = PY.CreateScope();
    }
    public static void Execute(string source)
    {
      PY.Execute(source, scope);
    }
  }
}
