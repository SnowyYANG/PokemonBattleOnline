using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics.Contracts;

namespace LightStudio.Tactic.DataModels.IO
{
    internal static class PathHelper
    {
        [Pure]
        public static bool IsInvalidFileNameChar(char c)
        {
            return Path.GetInvalidFileNameChars().Contains(c);
        }

        [Pure]
        public static bool IsInvalidFileName(string fileName)
        {
            return string.IsNullOrWhiteSpace(fileName) || fileName.Any(c => IsInvalidFileNameChar(c));
        }

        [Pure]
        public static bool IsInvalidPathChar(char c)
        {
            return Path.GetInvalidPathChars().Contains(c);
        }

        [Pure]
        public static bool IsInvalidPath(string path)
        {
            return string.IsNullOrWhiteSpace(path) || path.Any(c => IsInvalidPathChar(c));
        }

    }
}
