using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.IO;
using System.Windows;
using System.Windows.Controls;
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
      FrameworkElement.LanguageProperty.OverrideMetadata(typeof(FrameworkElement), new FrameworkPropertyMetadata(XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));
      var font = new FontFamily("Microsoft YaHei");
      TextBlock.FontFamilyProperty.OverrideMetadata(typeof(TextBlock), new FrameworkPropertyMetadata(font));
      ScrollViewer.FontFamilyProperty.OverrideMetadata(typeof(ScrollViewer), new FrameworkPropertyMetadata(font));
      UIDispatcher.Init(new WpfDispatcher(Application.Current.Dispatcher));
      new MainWindow().Show();
    }
    protected override void OnExit(ExitEventArgs e)
    {
      base.OnExit(e);
      Messaging.PBOClient.Dispose();
      GameDataService.Unload();
    }
  }
}
