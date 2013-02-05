using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Win32;
using System.Diagnostics.Contracts;

namespace PokemonBattleOnline.PBO.Editor
{
  internal static class FileHelper
  {
    /// <summary>
    /// get a file name selected by user to open 
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    public static string OpenFile(string filter)
    {
      var dialog = new OpenFileDialog();
      dialog.Filter = filter;
      if (dialog.ShowDialog() ?? false)
      {
        return dialog.FileName;
      }
      return null;
    }

    /// <summary>
    /// get a file name selected by user to save
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    public static string SaveFile(string filter, string defaultName)
    {
      var dialog = new SaveFileDialog();
      dialog.FileName = defaultName;
      dialog.Filter = filter;
      dialog.AddExtension = true;
      if (dialog.ShowDialog() ?? false)
      {
        return dialog.FileName;
      }
      return null;
    }

    /// <summary>
    /// open an existing file and process
    /// </summary>
    /// <returns>return the file name</returns>
    public static string OpenFile(string filter, Action<string, Stream> action)
    {
      string fileName = OpenFile(filter);
      if (!string.IsNullOrEmpty(fileName))
        ProcessFile(fileName, FileMode.Open, action);
      return fileName;
    }

    /// <summary>
    /// create a file and process
    /// </summary>
    /// <returns>return the file name</returns>
    public static string SaveFile(string filter, string defaultName, Action<string, Stream> action)
    {
      string fileName = SaveFile(filter, defaultName);
      if (!string.IsNullOrEmpty(fileName))
        ProcessFile(fileName, FileMode.Create, action);
      return fileName;
    }

    private static void ProcessFile(string fileName, FileMode fileMode, Action<string, Stream> action)
    {
      if (!string.IsNullOrEmpty(fileName))
      {
        using (var stream = new FileStream(fileName, fileMode))
        {
          action(Path.GetFileNameWithoutExtension(fileName), stream);
        }
      }
    }

  }
}
