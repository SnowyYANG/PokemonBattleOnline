using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Win32;
using System.Diagnostics.Contracts;

namespace LightStudio.PokemonBattle.PBO.Editor
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
        public static string SaveFile(string filter)
        {
            var dialog = new SaveFileDialog();
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
        public static string OpenFile(string filter, Action<Stream> action)
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
        public static string SaveFile(string filter, Action<Stream> action)
        {
            string fileName = SaveFile(filter);
            if (!string.IsNullOrEmpty(fileName))
                ProcessFile(fileName, FileMode.Create, action);
            return fileName;
        }

        public static void ProcessFile(string fileName, FileMode fileMode, 
            Action<Stream> action)
        {
            Contract.Requires(!string.IsNullOrEmpty(fileName));
            Contract.Requires(action != null);

            if (!string.IsNullOrEmpty(fileName))
            {
                using (var stream = new FileStream(fileName, fileMode))
                {
                    action(stream);
                }
            }
        }

    }
}
