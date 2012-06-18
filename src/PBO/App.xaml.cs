using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.IO;
using LightStudio.Tactic.Messaging;
using LightStudio.Tactic.Globalization;
using LightStudio.PokemonBattle.Data;

namespace LightStudio.PokemonBattle.PBO
{
  /// <summary>
  /// Interaction logic for App.xaml
  /// </summary>
  public partial class App : Application
  {
    void InitGameService()
    {
      DataService.Load(System.IO.Path.GetFullPath("Data"), new StringService() { Language = "Chinese" });
      DataService.String.DefaultLanguage = "Chinese";
      DataService.DataString.DefaultLanguage = "Chinese";
      DataService.String.ReturnKeyOnFallback = true;
      DataService.DataString.ReturnKeyOnFallback = true;
      Effects.EffectsRegister.Register();
      Tactic.Scripting.ExecuteAll("dll\\scripts");
    }
    
    protected override void OnStartup(StartupEventArgs e)
    {
      base.OnStartup(e);
      InitGameService();
      UIDispatcher.Init(new WpfDispatcher(Application.Current.Dispatcher));
      new MainWindow().Show();
    }
  }
}
