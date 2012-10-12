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
      GameDataService.Load("Data");
#if DEBUG
      DataService.Load(new StringService() { Language = "Chinese" });
      DataService.String.DefaultLanguage = "Chinese";
      DataService.DataString.DefaultLanguage = "Chinese";
#else
      throw new NotImplementedException();
#endif
      DataService.String.ReturnKeyOnFallback = true;
      DataService.DataString.ReturnKeyOnFallback = true;
      Game.Host.Effects.EffectsRegister.Register();
#if DEBUG
      Tactic.Scripting.ExecuteAll("..\\src\\PokemonBattle.EffectsP");
#else
      throw new NotImplementedException();
#endif
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
