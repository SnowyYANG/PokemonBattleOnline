using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace LightStudio
{
  [DataContract(Namespace = Namespaces.DEFAULT)]
  public class PairValue : INotifyPropertyChanged
  {
    public event PropertyChangedEventHandler PropertyChanged;
    [DataMember]
    int origin;
    [DataMember(EmitDefaultValue = false)]
    int value;
    [DataMember(EmitDefaultValue = false)]
    double normalizedOrigin;

    public PairValue(int origin, int value, double normalizedorigin = 0)
    {
      this.origin = origin;
      normalizedOrigin = normalizedorigin;
      this.value = value;
    }
    public PairValue(int origin)
      : this(origin, origin)
    {
    }

    public int Origin
    { get { return origin; } }
    public int Value
    { 
      get { return value; }
      set
      {
        if (this.value != value)
        {
          this.value = value;
          if (PropertyChanged != null)
            PropertyChanged(this, new PropertyChangedEventArgs(null));
        }
      }
    }
    public double Percentage
    { 
      get
      {
        if (origin == 0) return 0;
        return (double)value / (double)origin; 
      }
    }
    public double NormalizedValue
    { 
      get
      {
        if (origin == 0) return 0;
        return (normalizedOrigin * value) / origin;
      }
    }
    public bool IsIncreased
    { get { return value > origin; } }
    public bool IsDecreased
    { get { return value < origin; } }
    public bool IsChanged
    { get { return value != origin; } }

    public override string ToString()
    {
      return value.ToString();
    }
  }
}
