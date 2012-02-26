using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace LightStudio.Tactic.Globalization
{
  public interface IDomainStringService : INotifyPropertyChanged
  {
    // Methods
    void AddLanguagePack(LanguagePack pack);
    string GetFormattedString(string key, params object[] args);
    string GetFormattedString(string key, string language, params object[] args);
    string GetString(string key);
    string GetString(string key, string language);
    void SetProvider(LanguagePackProvider provider);

    // Properties
    string DefaultLanguage { get; set; }
    string DomainName { get; }
    string this[string key] { get; }
    bool ReturnKeyOnFallback { get; set; }
  }
}
