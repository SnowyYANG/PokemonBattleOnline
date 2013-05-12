using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Markup;
using System.Globalization;
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
    private void Init()
    {
      DataService.Load(new StringService() { Language = "Chinese" });
      DataService.String.DefaultLanguage = "Chinese";
      DataService.DataString.DefaultLanguage = "Chinese";
      DataService.String.ReturnKeyOnFallback = true;
      DataService.DataString.ReturnKeyOnFallback = true;
      
      GameDataService.Load("..\\res\\Data");
      Data.TempLearnSet.Load("..\\res\\Data\\learnset\\temp.xml");
      DataService.LoadUserData();

      Game.Host.Effects.EffectsRegister.Register();
      Tactic.Scripting.ExecuteAll("..\\src\\PokemonBattle.EffectsP");
    }
    
    protected override void OnStartup(StartupEventArgs e)
    {
      Init();
      
      FrameworkElement.LanguageProperty.OverrideMetadata(typeof(FrameworkElement), new FrameworkPropertyMetadata(XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));
      var font = new FontFamily("Microsoft YaHei");
      TextBlock.FontFamilyProperty.OverrideMetadata(typeof(TextBlock), new FrameworkPropertyMetadata(font));
      TextElement.FontFamilyProperty.OverrideMetadata(typeof(TextElement), new FrameworkPropertyMetadata(font));
      UIDispatcher.Init(new WpfDispatcher(Application.Current.Dispatcher));
      new MainWindow().Show();
      base.OnStartup(e);
    }
    protected override void OnExit(ExitEventArgs e)
    {
      base.OnExit(e);
      Messaging.PBOClient.Dispose();
      GameDataService.Unload();
    }
  }
}
