using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace PokemonBattleOnline
{
  public static class Serializer
  {
    private static readonly XmlWriterSettings WRITER_SETTINGS;
    private static readonly XmlReaderSettings READER_SETTINGS;

    static Serializer()
    {
      WRITER_SETTINGS = new XmlWriterSettings()
      {
        CloseOutput = false,
        ConformanceLevel = System.Xml.ConformanceLevel.Fragment,
        NamespaceHandling = NamespaceHandling.OmitDuplicates,
        Indent = false,
        OmitXmlDeclaration = true
      };
      READER_SETTINGS = new XmlReaderSettings()
      {
        ConformanceLevel = System.Xml.ConformanceLevel.Fragment,
        CloseInput = false,
        IgnoreWhitespace = false
      };
    }

    private static XmlReader GetXmlReader(Stream stream)
    {
      return XmlReader.Create(stream, READER_SETTINGS);
    }
    private static XmlReader GetXmlReader(TextReader reader)
    {
      return XmlReader.Create(reader, READER_SETTINGS);
    }
    
    public static DataContractSerializer GetSerializer(Type type)
    {
      return new DataContractSerializer(type);
    }

    private static void Serialize(object obj, XmlWriter writer)
    {
      var serializer = GetSerializer(obj.GetType());
      serializer.WriteStartObject(writer, obj);
      writer.WriteAttributeString("xmlns", PBOMarks.MS, null, PBOMarks.STANDARD);
      writer.WriteAttributeString("xmlns", PBOMarks.A, null, PBOMarks.ARRAY);
      serializer.WriteObjectContent(writer, obj);
      serializer.WriteEndObject(writer);
      writer.Flush();
    }
    public static void Serialize(object obj, Stream output)
    {
      Serialize(obj, XmlWriter.Create(output, WRITER_SETTINGS));
    }
    public static string SerializeToString(object obj)
    {
      var sb = new StringBuilder();
      using (var xw = XmlWriter.Create(sb, WRITER_SETTINGS))
        Serialize(obj, xw);
      return sb.ToString();
    }

    private static bool TryDeserialize(Type type, XmlReader reader, out object value)
    {
      try
      {
        value = Deserialize(type, reader);
        return true;
      }
      catch (Exception)
      {
        value = null;
        return false;
      }
    }
    public static bool TryDeserialize<T>(Stream stream, out T value)
    {
      return TryDeserialize<T>(GetXmlReader(stream), out value);
    }
    public static bool TryDeserialize<T>(XmlReader reader, out T value)
    {
      object objValue;
      if (TryDeserialize(typeof(T), reader, out objValue))
      {
        value = (T)objValue;
        return true;
      }
      else
      {
        value = default(T);
        return false;
      }
    }

    private static object Deserialize(Type type, XmlReader reader)
    {
      var serializer = GetSerializer(type);
      return serializer.ReadObject(reader);
    }
    public static object Deserialize(Type type, Stream stream)
    {
      return Deserialize(type, GetXmlReader(stream));
    }
    public static object DeserializeFromString(Type type, string content)
    {
      using (var sr = new StringReader(content))
      using (var reader = XmlReader.Create(sr, READER_SETTINGS))
        return Deserialize(type, reader);
    }
    
    public static T Deserialize<T>(Stream stream)
    {
      return (T)Deserialize(typeof(T), stream);
    }
    public static T DeserializeFromString<T>(string content)
    {
      using (var sr = new StringReader(content))
      using (var reader = XmlReader.Create(sr, READER_SETTINGS))
        return (T)Deserialize(typeof(T), reader);
    }

    public static T DeserializeFromJson<T>(byte[] bytes)
    {
      var d = new DataContractJsonSerializer(typeof(T));
      using (MemoryStream ms = new MemoryStream(bytes))
        return (T)d.ReadObject(ms);
    }
    public static T DeserializeFromJson<T>(byte[] bytes, int offset)
    {
      var d = new DataContractJsonSerializer(typeof(T));
      using (MemoryStream ms = new MemoryStream(bytes, offset, bytes.Length - offset))
        return (T)d.ReadObject(ms);
    }
    public static byte[] SerializeToJson<T>(T obj)
    {
      using (MemoryStream ms = new MemoryStream())
      {
        SerializeToJson(obj, ms);
        return ms.ToArray();
      }
    }
    public static void SerializeToJson<T>(T obj, Stream stream)
    {
      var s = new DataContractJsonSerializer(typeof(T));
      s.WriteObject(stream, obj);
    }
  }
}
