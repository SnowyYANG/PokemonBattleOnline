using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;

namespace LightStudio.Tactic.DataModels.IO
{
    public interface IDirectory
    {

        IDirectory ParentDirectory { get; }
        Dictionary<string, IDirectory> Directories { get; }
        Dictionary<string, IFile> Files { get; }
        string Name { get; }
        string FullName { get; }

        IFile AddFile(string fileName);
        IDirectory AddDirectory(string dirName);
        IFile GetFile(string fileName);
        IDirectory GetDirectory(string dirName);
        bool ValidateFiles();
        string GetAbsolutePath(string relativePath);
    }
}
