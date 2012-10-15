using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization;
using System.Xml;
using LightStudio.Tactic.Serialization;

namespace LightStudio.Tactic.DataModels
{
  [DataContract(Namespace=Namespaces.PBO)]
  public abstract class SimpleData
  {
#if DEBUG
    public
#else
    protected
#endif
      static T LoadFromXml<T>(string fileName) where T : SimpleData
    {
      using (XmlReader r = XmlReader.Create(fileName))
        return (T)Serializer.Deserialize(typeof(T), r);
    }
    protected static T LoadFromDat<T>(string fileName) where T : SimpleData
    {
      using (FileStream f = new FileStream(fileName, FileMode.Open, FileAccess.Read))
        using (DeflateStream s = new DeflateStream(f, CompressionMode.Decompress))
      {
          return (T)Serializer.Deserialize<T>(s);
      }
    }

    protected SimpleData()
    {
    }
    protected SimpleData(string key)
    {
    }
    
#if DEBUG
    public
#else
    protected
#endif
      void SaveXml(string fileName)
    {
      using (XmlWriter w = XmlWriter.Create(fileName))
        Serializer.Serialize(this, w);
    }
#if DEBUG
    public
#else
    protected
#endif
      void SaveDat(string fileName)
    {
      using (FileStream f = new FileStream(fileName, FileMode.Create))
      using (DeflateStream s = new DeflateStream(f, CompressionMode.Compress))
        Serializer.Serialize(this, s);
    }
  }
}
