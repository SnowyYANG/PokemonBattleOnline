using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.IO;

namespace LightStudio.Tactic.Globalization
{
  public delegate LanguagePack LanguagePackProvider(string language);
  public static class LanguagePackProviderFactory
  {
    // Methods
    private static LanguagePackProvider CaptureExceptionWrapper(LanguagePackProvider func, bool captureException)
    {
      if (!captureException)
      {
        return func;
      }
      return delegate(string lang)
      {
        try
        {
          return func(lang);
        }
        catch
        {
          return null;
        }
      };
    }

    public static LanguagePackProvider GetProviderFromFile(Func<string, string> getPathFunc)
    {
      return GetProviderFromFile(getPathFunc, false);
    }

    public static LanguagePackProvider GetProviderFromFile(Func<string, string> getPathFunc, bool captureException)
    {
      return CaptureExceptionWrapper(delegate(string lang)
      {
        string path = getPathFunc(lang);
        if (!File.Exists(path))
        {
          return null;
        }
        return LanguagePack.CreateFromXml(XElement.Load(path));
      }, captureException);
    }

    public static LanguagePackProvider GetProviderFromXml(Func<string, XElement> getXmlFunc)
    {
      return GetProviderFromXml(getXmlFunc, false);
    }

    public static LanguagePackProvider GetProviderFromXml(Func<string, XElement> getXmlFunc, bool captureException)
    {
      return CaptureExceptionWrapper(lang => LanguagePack.CreateFromXml(getXmlFunc(lang)), captureException);
    }
  }
  public class LanguagePack
  {
    // Fields
    private const string DEFAULT = "default";
    private const string ITEM = "item";
    private const string KEY = "key";
    private const string LANGUAGE = "language";
    private const string ROOT = "pack";
    private const string VALUE = "value";

    private static string ReadAttribute(XElement xml, string name)
    {
      XAttribute attribute = xml.Attribute(name);
      if (attribute != null)
      {
        return attribute.Value;
      }
      XElement element = xml.Element(name);
      if (element != null)
      {
        return element.Value;
      }
      return null;
    }
    public static LanguagePack CreateFromStream(Stream stream)
    {
      return CreateFromXml(XElement.Load(stream));
    }
    public static LanguagePack CreateFromXml(XElement root)
    {
      if (root == null)
      {
        throw new ArgumentNullException("root");
      }
      if (root.Name != "pack")
      {
        return null;
      }
      LanguagePack pack = new LanguagePack
      {
        Language = root.Attribute("language").Value
      };
      XAttribute attribute = root.Attribute("default");
      bool result = false;
      if (attribute != null)
      {
        bool.TryParse(attribute.Value, out result);
      }
      pack.IsDefault = result;
      var enumerable = from item in root.Elements("item") select new { Key = ReadAttribute(item, "key"), Value = ReadAttribute(item, "value") };
      try
      {
        foreach (var type in enumerable)
        {
          if (!string.IsNullOrEmpty(type.Key))
          {
            pack.StringResources.Add(type.Key, type.Value);
          }
        }
      }
      catch (Exception)
      {
        throw;
      }
      return pack;
    }

    // Methods
    public LanguagePack()
    {
      this.StringResources = new Dictionary<string, string>();
    }

    // Properties
    public bool IsDefault { get; set; }
    public string Language { get; set; }
    public IDictionary<string, string> StringResources { get; private set; }
  }
}
