using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.IO;

namespace LightStudio.Tactic.DataModels.IO
{
    public abstract class DirectoryBase : DataItem, IDirectory
    {

        public Dictionary<string, IDirectory> Directories
        { get; private set; }

        public Dictionary<string, IFile> Files
        { get; private set; }

        protected DirectoryBase(IDirectory parent, string name)
            : base(parent, name)
        {
            this.Directories = new Dictionary<string, IDirectory>();
            this.Files = new Dictionary<string, IFile>();
        }

        public bool ValidateFiles()
        {
            foreach (var file in Files.Values)
            {
                if (!file.ValidateFileContent())
                    return false;
            }
            return true;
        }

        public IFile AddFile(string fileName)
        {
            if (Files.ContainsKey(fileName))
                return null;

            var file = new DataFile(this, fileName);
            file.UpdateIdentifier();
            Files.Add(fileName, file);
            return file;
        }

        public IDirectory AddDirectory(string dirName)
        {
            if (Directories.ContainsKey(dirName))
                return null;
            var dir = new DataDirectory(this, dirName);
            Directories.Add(dirName, dir);
            return dir;
        }

        public IFile GetFile(string fileName)
        {
            return Files.ValueOrDefault(fileName);
        }

        public IDirectory GetDirectory(string dirName)
        {
            return Directories.ValueOrDefault(dirName);
        }

        public string GetAbsolutePath(string relativePath)
        {
            return Path.Combine(FullName, relativePath);
        }

    }
}
