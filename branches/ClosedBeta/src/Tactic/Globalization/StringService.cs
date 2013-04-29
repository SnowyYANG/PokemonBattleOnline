using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace PokemonBattleOnline.Tactic.Globalization
{
  public class StringService
  {
    // Fields
    private string _language;
    private Dictionary<string, DomainStringService> domains = new Dictionary<string, DomainStringService>();

    // Methods
    public DomainStringService GetDomainService(string domainName)
    {
      if (!this.domains.ContainsKey(domainName))
      {
        this.domains[domainName] = new DomainStringService(this, domainName);
      }
      return this.domains[domainName];
    }

    public string Language
    {
      get
      {
        return this._language;
      }
      set
      {
        if (this._language != value)
        {
          this._language = value;
          foreach (DomainStringService service in this.domains.Values)
          {
            service.OnLanguageChanged();
          }
        }
      }
    }
  }
}
