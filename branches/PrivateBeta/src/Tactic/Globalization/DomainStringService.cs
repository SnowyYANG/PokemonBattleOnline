using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace LightStudio.Tactic.Globalization
{
  public class DomainStringService : INotifyPropertyChanged
  {
    // Fields
    private StringService container;
    private LanguagePackProvider packProvider;
    private Dictionary<string, Dictionary<string, string>> stringDictionary;

    // Events
    public event PropertyChangedEventHandler PropertyChanged;

    // Methods
    public DomainStringService(StringService containerValue, string domainName)
    {
      this.container = containerValue;
      this.stringDictionary = new Dictionary<string, Dictionary<string, string>>();
      this.DomainName = domainName;
    }

    public void AddLanguagePack(LanguagePack pack)
    {
      string str = pack.Language.ToLower();
      this.stringDictionary[str] = new Dictionary<string, string>(pack.StringResources);
      if (pack.IsDefault)
      {
        this.DefaultLanguage = str;
        this.OnPropertyChanged(null);
      }
      else if (this.container.Language == str)
      {
        this.OnPropertyChanged(null);
      }
    }

    public string GetString(string key)
    {
      return this.GetString(key, this.container.Language);
    }

    public string GetString(string key, string language)
    {
      if (language != null)
      {
        language = language.ToLower();
      }
      Dictionary<string, string> dictionary = null;
      if ((string.IsNullOrEmpty(language) || !this.stringDictionary.TryGetValue(language, out dictionary)) && (this.packProvider != null))
      {
        LanguagePack pack = this.packProvider(language);
        if (pack != null)
        {
          this.AddLanguagePack(pack);
          this.stringDictionary.TryGetValue(language, out dictionary);
        }
      }
      if ((dictionary == null) && !string.IsNullOrEmpty(this.DefaultLanguage))
      {
        this.stringDictionary.TryGetValue(this.DefaultLanguage, out dictionary);
      }
      string str = null;
      if ((dictionary != null) && dictionary.TryGetValue(key, out str))
      {
        return str;
      }
      if (this.ReturnKeyOnFallback)
      {
        return key;
      }
      return null;
    }

    public void OnLanguageChanged()
    {
      this.OnPropertyChanged(null);
    }

    private void OnPropertyChanged(string propertyName)
    {
      if (PropertyChanged != null)
        this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
    }

    public void SetProvider(LanguagePackProvider provider)
    {
      this.packProvider = provider;
    }

    // Properties
    public string DefaultLanguage { get; set; }

    public string DomainName { get; private set; }

    public string this[string key]
    {
      get
      {
        return this.GetString(key);
      }
    }

    public bool ReturnKeyOnFallback { get; set; }
  }
}
