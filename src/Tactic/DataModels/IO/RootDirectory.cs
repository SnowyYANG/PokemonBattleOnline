using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics.Contracts;

namespace LightStudio.Tactic.DataModels.IO
{
    public class RootDirectory : DirectoryBase
    {
        public RootDirectory(string path)
            : base(null, path)
        { }

        public static RootDirectory FromDirectory(string dir)
        {
            if (!Directory.Exists(dir))
                throw new ArgumentException("the specified directory does not exist");

            var root = new RootDirectory(dir);
            AppendChildren(root);
            return root;
        }

        private static void AppendChildren(IDirectory dir)
        {
            foreach (var subdir in Directory.GetDirectories(dir.FullName))
            {
                AppendChildren(dir.AddDirectory(Path.GetFileName(subdir)));
            }
            foreach (var file in Directory.GetFiles(dir.FullName))
            {
                dir.AddFile(Path.GetFileName(file));
            }
        }
    }
}
