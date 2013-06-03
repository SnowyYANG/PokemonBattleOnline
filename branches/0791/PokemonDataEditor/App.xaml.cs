using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using PokemonDataEditor.ViewModel;
using PokemonDataEditor.View;

namespace PokemonDataEditor
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            MainWindow = new MainWindow() { DataContext = new MainWindowViewModel() };
            MainWindow.Show();
            base.OnStartup(e);
        }
    }
}
