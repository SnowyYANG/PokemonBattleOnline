using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
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
      PY = Python.CreateEngine();

      var files = PY.GetSearchPaths();
      files.Add(Environment.CurrentDirectory + "\\dll");
      PY.SetSearchPaths(files);

      PY.Runtime.IO.SetOutput(Stream.Null, new DebugTextWriter());
      PY.Runtime.IO.SetErrorOutput(Stream.Null, new DebugTextWriter());
      
      scope = PY.CreateScope();
    }
    public static void Execute(string source)
    {
      PY.Execute(source, scope);
    }
    public static void ExecuteAll(string path)
    {
      var files = Directory.EnumerateFiles(path, "*.py", SearchOption.AllDirectories);
      foreach (string f in files)
        try
        {
          PY.ExecuteFile(f, scope);
        }
        catch(Exception e)
        {
#if DEBUG
          System.Diagnostics.Debugger.Break();
#endif
        }
    }

    private class DebugTextWriter : TextWriter
    {
      public DebugTextWriter()
      {
      }

      public override Encoding Encoding
      {
        get { return Encoding.UTF8; }
      }

      public override void Write(char value)
      {
        System.Diagnostics.Debug.Write(value);
      }
    }
  }
}
