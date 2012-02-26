using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics.Contracts;
using System.Security.Cryptography;

namespace LightStudio.Tactic.DataModels.IO
{
    internal static class FileHelper
    {

        /// <summary>
        /// compute the hash value of a file associated with the specified filePath
        /// </summary>
        /// <returns>null, if the file does not exist</returns>
        public static string GetFileHash(string filePath)
        {
            Contract.Requires(!PathHelper.IsInvalidPath(filePath));
            if (!File.Exists(filePath))
                return null;

            byte[] hash;
            using (var cng = new MD5Cng())//for performance
            {
                using (var stream = new FileStream(filePath, FileMode.Open))
                {
                    hash = cng.ComputeHash(stream);
                }
            }
            return Convert.ToBase64String(hash);
        }

    }
}
