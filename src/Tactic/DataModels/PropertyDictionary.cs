using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace LightStudio.Tactic.DataModels
{
    [CollectionDataContract(KeyName = "Name", ValueName = "Value", Namespace = Namespaces.DEFAULT)]
    [Serializable]
    public class PropertyDictionary : Dictionary<string, object>
    {
        public PropertyDictionary()
        { }

        public PropertyDictionary(IDictionary<string, object> dict)
            : base(dict)
        { }

        protected PropertyDictionary(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }
    }
}
