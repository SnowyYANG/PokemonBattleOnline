using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace PokemonBattleOnline
{
  public interface IPairValue : INotifyPropertyChanged
  {
    int Origin { get; }
    int Value { get; }
    double Percentage { get; }
    #region for binding
    bool IsIncreased { get; }
    bool IsDecreased { get; }
    bool IsChanged { get; }
    #endregion
  }

  [DataContract(Name = "pv", Namespace = Namespaces.PBO)]
  public class PairValue : ObservableObject, IPairValue
  {
    public PairValue(int origin, int value)
    {
      this._origin = origin;
      this._value = value;
    }
    public PairValue(int origin)
      : this(origin, origin)
    {
    }

    [DataMember(Name = "o", EmitDefaultValue = false)]
    int _origin;
    public int Origin
    { get { return _origin; } }
    [DataMember(Name = "v", EmitDefaultValue = false)]
    int _value;
    public int Value
    { 
      get { return _value; }
      set
      {
        if (_value != value)
        {
          _value = value;
          OnPropertyChanged();
        }
      }
    }
    public double Percentage
    { get { return _origin == 0 ? 0 : _value / (double)_origin; } }
    public bool IsIncreased
    { get { return _value > _origin; } }
    public bool IsDecreased
    { get { return _value < _origin; } }
    public bool IsChanged
    { get { return _value != _origin; } }

    public override string ToString()
    {
      return _value.ToString();
    }
  }
}
