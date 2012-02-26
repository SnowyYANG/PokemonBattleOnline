using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Compression;
using System.Diagnostics.Contracts;
using System.Xml;
using System.Xml.Linq;
using LightStudio.Tactic.Serialization;
using LightStudio.Tactic.Security;

namespace LightStudio.Tactic.DataModels.IO
{
  public class DataCollection
  {
    public static DataCollection Create(string name, string baseDir)
    {
      Contract.Requires(!PathHelper.IsInvalidPath(baseDir));
      Contract.Requires(!PathHelper.IsInvalidFileName(name));

      var collection = new DataCollection(name);
      collection.rootDirectory = RootDirectory.FromDirectory(baseDir);
      return collection;
    }
    public static DataCollection Load(Stream stream, string baseDir)
    {
      Contract.Requires(stream != null);
      Contract.Requires(!string.IsNullOrWhiteSpace(baseDir));

      //verification
      XmlDocument xmlDoc = new XmlDocument { PreserveWhitespace = true };
      xmlDoc.Load(stream);
      if (!XmlVerifier.CheckUsingBuiltinKey(xmlDoc))
      {
        throw new InvalidDataException("the DataCollection file has been damaged");
      }

      using (var xmlReader = new XmlNodeReader(xmlDoc))
      {
        XElement element = XElement.Load(xmlReader);
        var collection = new DataCollection(element.GetName());
        collection.rootDirectory =
            element.Element(XmlFormatter.RootDirectoryString).ToRootDirectory(baseDir);
        return collection;
      }
    }
    public static DataCollection Load(string indexFile, string baseDir)
    {
      using (var stream = File.Open(indexFile, FileMode.Open))
        return Load(stream, baseDir);
    }

    private RootDirectory rootDirectory;

    private DataCollection(string name)
    {
      this.Name = name;
    }
    
    public string Name { get; private set; }
    public string BaseDirectory
    {
      get
      {
        return rootDirectory.Name;
      }
    }

    public string GetAbsolutePath(string relativePath)
    {
      return rootDirectory.GetAbsolutePath(relativePath);
    }

    /// <param name="validate">whether to validate the relativePath and 
    /// the associated file before opening</param>
    /// <returns>null, if the specified relativePath or the associated file is invalid, 
    /// or the specified file does not exist</returns>
    public Stream OpenFile(string relativePath, bool validate)
    {
      if (validate && !ValidateFile(relativePath))
        return null;
      string path = rootDirectory.GetAbsolutePath(relativePath);
      if (File.Exists(path))
        return File.Open(path, FileMode.Open);
      return null;
    }

    /// <summary>
    /// validates the existence and the content of the file associated with the 
    /// speicified relativePath
    /// </summary>
    public bool ValidateFile(string relativePath)
    {
      return true;
      if (PathHelper.IsInvalidPath(relativePath))
        return false;

      var parts = relativePath.Split(new[] { Path.PathSeparator },
          StringSplitOptions.RemoveEmptyEntries);
      IDirectory dir = rootDirectory;
      for (int i = 0; i < parts.Length - 1; i++)
      {
        dir = dir.GetDirectory(parts[i]);
        if (dir == null)
          return false;
      }
      var file = dir.GetFile(parts[parts.Length - 1]);
      return file != null && file.ValidateFileContent();
    }
    public bool ValidateFiles()
    {
      return true;
      return rootDirectory.ValidateFiles();
    }

    /// <summary>
    /// deserialize the content of a file associated with the specified relativePath, 
    /// using the Serializer class
    /// </summary>
    /// <param name="validate">whether to validate the relativePath and 
    /// the associated file before opening</param>
    /// <returns>default(T), if the specified relativePath or the associated file is invalid, 
    /// or the specified file does not exist</returns>
    public T ReadFile<T>(string relativePath, bool validate)
    {
      if (string.IsNullOrWhiteSpace(relativePath))
        return default(T);

      using (var stream = OpenFile(relativePath, validate))
      {
        if (stream == null)
          return default(T);
        return Serializer.Deserialize<T>(stream);
      }
    }
    public T ReadCompressedFile<T>(string relativePath, bool validate)
    {
      if (string.IsNullOrWhiteSpace(relativePath)) return default(T);

      Stream stream = OpenFile(relativePath, validate);
      if (stream == null) return default(T);
      using (var decompressStream = new DeflateStream(stream, CompressionMode.Decompress))
      {
        return Serializer.Deserialize<T>(decompressStream);
      }
    }

    public void Save(Stream stream)
    {
      Contract.Requires(stream != null);

      var element = new XElement(XmlFormatter.DataCollectionString);
      element.Add(new XAttribute(XmlFormatter.NameString, Name));
      element.Add(rootDirectory.ToXml());
      element.Save(stream);
    }
    public void Save(string indexFile)
    {
      using (var stream = File.Create(indexFile))
        Save(stream);
    }
  }
}
