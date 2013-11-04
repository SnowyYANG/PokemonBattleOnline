using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization;
using System.Xml;

namespace PokemonBattleOnline
{
  [DataContract(Namespace = PBOMarks.PBO)]
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
        return Serializer.Deserialize<T>(f);
    }
#if EDITING
    public
#else
    protected
#endif
      static T LoadFromXml<T>(Stream stream) where T : SimpleData
    {
      return Serializer.Deserialize<T>(stream);
    }
    protected static T LoadFromDat<T>(string fileName) where T : SimpleData
    {
      using (FileStream f = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
      using (DeflateStream s = new DeflateStream(f, CompressionMode.Decompress))
        return Serializer.Deserialize<T>(s);
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
