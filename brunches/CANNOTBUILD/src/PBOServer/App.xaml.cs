using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

namespace PokemonBattleOnline.PBO.Server
{
  /// <summary>
  /// Interaction logic for App.xaml
  /// </summary>
  public partial class App : Application
  {
    protected override void OnStartup(StartupEventArgs e)
    {
      UIDispatcher.Init(new WpfDispatcher(Application.Current.Dispatcher));
      base.OnStartup(e);
    }
  }
}
