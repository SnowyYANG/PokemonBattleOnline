using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.IO;
using LightStudio.Tactic.Messaging;
using LightStudio.Tactic.Globalization;
//using LightStudio.PokemonBattle.Room.Messaging;
using LightStudio.PokemonBattle.Data;
//using LightStudio.PokemonBattle.PBO.Battle.VM;

namespace LightStudio.PokemonBattle.PBO
{
  /// <summary>
  /// Interaction logic for App.xaml
  /// </summary>
  public partial class App : Application
  {
    void InitDataService()
    {
      //MessageResolverFacade.Resolver = new DeflateResolver(new GameMessageResolver()); 
      DataService.Load(System.IO.Path.GetFullPath("Data"), new StringService() { Language = "Chinese" });
      DataService.String.DefaultLanguage = "Chinese";
      DataService.DataString.DefaultLanguage = "Chinese";
      DataService.String.ReturnKeyOnFallback = true;
      DataService.DataString.ReturnKeyOnFallback = true;
      //dynamic data = DataService.Data;
      //data.AddMode(new NamedDynamicObject(1));
      //data.AddRule(new NamedDynamicObject(1));
    }
    
    protected override void OnStartup(StartupEventArgs e)
    {
      base.OnStartup(e);
      InitDataService();
      UIDispatcher.Init(new WpfDispatcher(Application.Current.Dispatcher));
      new MainWindow().Show();
    }
  }
}
