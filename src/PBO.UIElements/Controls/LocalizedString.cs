using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows;
using LightStudio.Tactic.Globalization;

namespace LightStudio.PokemonBattle.PBO.UIElements
{
  public class LocalizedString : INotifyPropertyChanged, IWeakEventListener
  {
    // Fields
    private string _key;
    private IDomainStringService service;

    // Events
    public event PropertyChangedEventHandler PropertyChanged;

    // Methods
    public LocalizedString(IDomainStringService stringService)
    {
      //__ContractsRuntime.Requires(stringService != null, null, "stringService != null");
      this.PropertyChanged = delegate(object param0, PropertyChangedEventArgs param1)
      {
      };
      this.service = stringService;
      PropertyChangedEventManager.AddListener(this.service, this, string.Empty);
    }

    public LocalizedString(IDomainStringService stringService, string key)
      : this(stringService)
    {
      this._key = key;
    }

    private void OnPropertyChanged(string propertyName)
    {
      this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
    }

    bool IWeakEventListener.ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
    {
      if (managerType == typeof(PropertyChangedEventManager))
      {
        this.OnPropertyChanged("Value");
        return true;
      }
      return false;
    }

    public override string ToString()
    {
      string str = this.Value;
      //__ContractsRuntime.Ensures(str != null, null, "Contract.Result<string>() != null");
      return str;
    }

    // Properties
    public string Key
    {
      get
      {
        return this._key;
      }
      set
      {
        if (this._key != value)
        {
          this._key = value;
          this.OnPropertyChanged(null);
        }
      }
    }

    public string Value
    {
      get
      {
        if (this.Key != null)
        {
          return this.service[this.Key];
        }
        return null;
      }
    }
  }
  public static class IDomainStringServiceExtensions
  {
    // Methods
    public static LocalizedString GetLocalizedString(this IDomainStringService service, string key)
    {
      return new LocalizedString(service, key);
    }
  }
}
