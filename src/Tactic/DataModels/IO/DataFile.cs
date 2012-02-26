using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics.Contracts;

namespace LightStudio.Tactic.DataModels.IO
{
    internal class DataFile : DataItem, IFile
    {

        /// <summary>
        /// a string that identifies the content of the file
        /// </summary>
        public string Identifier
        { get; private set; }

        public DataFile(IDirectory parent, string name)
            : base(parent, name)
        {
            Contract.Requires(parent != null);
        }

        public DataFile(IDirectory parent, string name, string identifier)
            : this(parent, name)
        {
            this.Identifier = identifier;
        }

        public void UpdateIdentifier()
        {
            Identifier = FileHelper.GetFileHash(FullName);
        }

        /// <summary>
        /// check whether the file was changed using Identifier
        /// </summary>
        public bool ValidateFileContent()
        {
            return FileHelper.GetFileHash(FullName) == Identifier;
        }

    }
}
