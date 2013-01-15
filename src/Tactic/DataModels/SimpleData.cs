using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization;
using System.Xml;
using PokemonBattleOnline.Tactic;

namespace PokemonBattleOnline.Tactic.DataModels
{
  [DataContract(Namespace=Namespaces.PBO)]
  public abstract class SimpleData
  {
#if EDITING
    public
#else
    protected
#endif
      static T LoadFromXml<T>(string fileName) where T : SimpleData
    {
      using (FileStream f = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
        return (T)Serializer.Deserialize(typeof(T), f);
    }
    protected static T LoadFromDat<T>(string fileName) where T : SimpleData
    {
      using (FileStream f = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
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
    
#if EDITING
    public
#else
    protected
#endif
      void SaveXml(string fileName)
    {
      using (FileStream f = new FileStream(fileName, FileMode.Create))
        Serializer.Serialize(this, f);
    }
#if EDITING
    public
#else
    protected
#endif
      void SaveDat(string fileName)
    {
      var dir = Path.GetDirectoryName(fileName);
      if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
      using (FileStream f = new FileStream(fileName, FileMode.Create))
      using (DeflateStream s = new DeflateStream(f, CompressionMode.Compress))
        Serializer.Serialize(this, s);
    }
  }
}
