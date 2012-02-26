using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightStudio.Tactic.DataModels.IO
{
    public interface IFile
    {
        IDirectory ParentDirectory { get; }
        string Name { get; }
        string FullName { get; }
        string Identifier { get; }

        void UpdateIdentifier();
        bool ValidateFileContent();
    }
}
