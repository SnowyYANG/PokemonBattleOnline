using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Diagnostics.Contracts;
using System.Runtime.Serialization;

namespace LightStudio.Tactic.Serialization
{
  public static class Serializer
  {
    private static readonly HashSet<Type> knownTypes = new HashSet<Type>();

    public static DataContractSerializer GetSerializer(Type type)
    {
      return new DataContractSerializer(type, knownTypes, Int32.MaxValue,
          false, false, null, new Resolver());
    }
    private static XmlReader GetXmlReader(Stream stream)
    {
      return XmlReader.Create(stream, GetReaderSettings());
    }
    private static XmlReader GetXmlReader(TextReader reader)
    {
      return XmlReader.Create(reader, GetReaderSettings());
    }
    private static XmlWriterSettings GetWriterSettings()
    {
      var settings = new XmlWriterSettings();
      settings.NamespaceHandling = NamespaceHandling.OmitDuplicates;
#if DEBUG
      settings.Indent = true;
#else
            settings.Indent = false;
#endif
      settings.CloseOutput = false;
      settings.OmitXmlDeclaration = true;
      return settings;
    }
    private static XmlReaderSettings GetReaderSettings()
    {
      var settings = new XmlReaderSettings();
      settings.CloseInput = false;
      return settings;
    }

    public static void AddKnownType(Type type)
    {
      knownTypes.Add(type);
    }
    public static string SerializeToString(object obj)
    {
      Contract.Requires(obj != null);
      var sb = new StringBuilder();
      Serialize(obj, sb);
      return sb.ToString();
    }
    public static T DeserializeFromString<T>(string content)
    {
      using (var reader = new StringReader(content))
      {
        return Deserialize<T>(reader);
      }
    }
    public static object DeserializeFromString(Type type, string content)
    {
      using (var reader = new StringReader(content))
      {
        return Deserialize(type, reader);
      }
    }
    
    public static void Serialize(object obj, Stream output)
    {
      Contract.Requires(obj != null);
      Contract.Requires(output != null);

      Serialize(obj, XmlWriter.Create(output, GetWriterSettings()));
    }
    public static void Serialize(object obj, StringBuilder output)
    {
      Contract.Requires(obj != null);
      Contract.Requires(output != null);
      using (XmlWriter writer = XmlWriter.Create(output, GetWriterSettings()))
      {
        Serialize(obj, writer);
      }
    }
    public static void Serialize(object obj, TextWriter output)
    {
      Contract.Requires(obj != null);
      Contract.Requires(output != null);

      Serialize(obj, XmlWriter.Create(output, GetWriterSettings()));
    }
    public static void Serialize(object obj, XmlWriter writer)
    {
      Contract.Requires(obj != null);
      Contract.Requires(writer != null);

      var serializer = GetSerializer(obj.GetType());
      serializer.WriteStartObject(writer, obj);
      writer.WriteAttributeString("xmlns", Namespaces.MS, null, Namespaces.STANDARD);
      writer.WriteAttributeString("xmlns", Namespaces.A, null, Namespaces.ARRAY);
      serializer.WriteObjectContent(writer, obj);
      serializer.WriteEndObject(writer);
      writer.Flush();
    }

    public static bool TryDeserialize<T>(Stream stream, out T value)
    {
      Contract.Requires(stream != null);

      return TryDeserialize<T>(GetXmlReader(stream), out value);
    }
    public static bool TryDeserialize<T>(TextReader reader, out T value)
    {
      Contract.Requires(reader != null);

      return TryDeserialize<T>(GetXmlReader(reader), out value);
    }
    public static bool TryDeserialize<T>(XmlReader reader, out T value)
    {
      Contract.Requires(reader != null);

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
    public static bool TryDeserialize(Type type, XmlReader reader, out object value)
    {
      Contract.Requires(type != null);
      Contract.Requires(reader != null);

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

    public static T Deserialize<T>(Stream stream)
    {
      Contract.Requires(stream != null);

      return (T)Deserialize(typeof(T), stream);
    }
    public static T Deserialize<T>(TextReader reader)
    {
      Contract.Requires(reader != null);

      return (T)Deserialize(typeof(T), reader);
    }
    public static T Deserialize<T>(XmlReader reader)
    {
      Contract.Requires(reader != null);

      return (T)Deserialize(typeof(T), reader);
    }

    public static object Deserialize(Type type, Stream stream)
    {
      Contract.Requires(type != null);
      Contract.Requires(stream != null);

      return Deserialize(type, GetXmlReader(stream));
    }
    public static object Deserialize(Type type, TextReader reader)
    {
      Contract.Requires(type != null);
      Contract.Requires(reader != null);

      return Deserialize(type, GetXmlReader(reader));
    }
    public static object Deserialize(Type type, XmlReader reader)
    {
      Contract.Requires(type != null);
      Contract.Requires(reader != null);

      var serializer = GetSerializer(type);
      return serializer.ReadObject(reader);
    }
  }
}
