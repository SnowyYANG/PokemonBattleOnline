using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace LightStudio.PokemonBattle.PBO
{
  public abstract class ViewModelBase : INotifyPropertyChanged
  {
    private static readonly PropertyChangedEventArgs ALL = new PropertyChangedEventArgs(null);
    
    public event PropertyChangedEventHandler PropertyChanged;

    protected ViewModelBase()
    {
    }

    protected void OnPropertyChanged(PropertyChangedEventArgs e)
    {
      if (PropertyChanged != null) PropertyChanged(this, e);
    }
    protected void OnPropertyChanged(string propertyName)
    {
      OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
    }
    protected void OnPropertyChanged()
    {
      OnPropertyChanged(ALL);
    }
  }
}
