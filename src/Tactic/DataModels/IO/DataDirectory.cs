using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.IO;
using System.Diagnostics.Contracts;

namespace LightStudio.Tactic.DataModels.IO
{
    internal class DataDirectory : DirectoryBase
    {

        public DataDirectory(IDirectory parent, string name)
            : base(parent, name)
        {
            Contract.Requires(parent != null);
        }

    }
}
