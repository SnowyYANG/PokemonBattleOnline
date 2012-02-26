using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightStudio.Tactic.DataModels.IO
{
    public abstract class DataItem
    {

        public IDirectory ParentDirectory
        { get; private set; }

        public string Name
        { get; private set; }

        public string FullName
        { get; protected set; }

        protected DataItem(IDirectory parent, string name)
        {
            this.ParentDirectory = parent;
            this.Name = name;
            this.FullName = parent != null ? parent.GetAbsolutePath(name) : name;
        }

    }
}
