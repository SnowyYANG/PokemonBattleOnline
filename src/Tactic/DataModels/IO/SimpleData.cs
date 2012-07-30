using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization;
using System.Xml;
using LightStudio.Tactic.Serialization;

namespace LightStudio.Tactic.DataModels.IO
{
  [DataContract(Namespace=Namespaces.DEFAULT)]
  public abstract class SimpleData
  {
    protected static T LoadFromXml<T>(string fileName) where T : SimpleData
    {
      using (XmlReader r = XmlReader.Create(fileName))
        return (T)Serializer.Deserialize(typeof(T), r);
    }
    protected static T LoadFromDat<T>(string fileName) where T : SimpleData
    {
      using (FileStream f = new FileStream(fileName, FileMode.Open, FileAccess.Read))
      using (DeflateStream s = new DeflateStream(f, CompressionMode.Decompress))
        return (T)Serializer.Deserialize<T>(s);
    }

    protected SimpleData()
    {
    }
    protected SimpleData(string key)
    {
    }
    
    protected void SaveXml(string fileName)
    {
      using (XmlWriter w = XmlWriter.Create(fileName))
        Serializer.Serialize(this, w);
    }
    protected void SaveDat(string fileName)
    {
      using (FileStream f = new FileStream(fileName, FileMode.Create))
      using (DeflateStream s = new DeflateStream(f, CompressionMode.Compress))
        Serializer.Serialize(this, s);
    }
  }
}
