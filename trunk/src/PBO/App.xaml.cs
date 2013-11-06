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
using PokemonBattleOnline.Game;
using PokemonBattleOnline.Network;

namespace PokemonBattleOnline.PBO
{
  /// <summary>
  /// Interaction logic for App.xaml
  /// </summary>
  public partial class App : Application
  {
    private void Init()
    {
      using (var pack = new ZipData("..\\res\\rom.zip"))
      {
        RomData.Load(pack, "/rom.xml");
        LearnList.Load(pack, "/learnset");
      }
      ImageService.Load("..\\res\\image.zip");
      GameString.Load("..\\res\\string", "zh", "en");
      UserData.Load("..\\MyPBO\\user.dat");
      Config.Load("..\\MyPBO\\config.xml");
    }
    
    protected override void OnStartup(StartupEventArgs e)
    {
      Init();
      
      FrameworkElement.LanguageProperty.OverrideMetadata(typeof(FrameworkElement), new FrameworkPropertyMetadata(XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));
      var font = new FontFamily("Microsoft YaHei");
      TextBlock.FontFamilyProperty.OverrideMetadata(typeof(TextBlock), new FrameworkPropertyMetadata(font));
      TextElement.FontFamilyProperty.OverrideMetadata(typeof(TextElement), new FrameworkPropertyMetadata(font));
      UIDispatcher.Init(Application.Current.Dispatcher);
      new MainWindow().Show();
      base.OnStartup(e);
    }
  }
}
