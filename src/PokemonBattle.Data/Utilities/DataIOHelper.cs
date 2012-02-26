using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace LightStudio.PokemonBattle.Data
{
    internal static class DataIOHelper
    {
        public static FileStream OpenFile(string relativePath, FileMode mode)
        {
            string fullPath = GetFullPath(relativePath);
            return new FileStream(fullPath, mode);
        }

        public static FileStream CreateFile(string relativePath, FileMode mode)
        {
            string fullPath = GetFullPath(relativePath);
            string directory = Path.GetDirectoryName(fullPath);
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);
            return new FileStream(fullPath, mode);
        }

        public static string GetFullPath(string relativePath)
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativePath);
        }
    }
}
