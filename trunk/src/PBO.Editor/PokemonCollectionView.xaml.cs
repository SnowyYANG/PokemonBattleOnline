using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PokemonBattleOnline.PBO.Editor
{
    /// <summary>
    /// Interaction logic for PokemonCollectionView.xaml
    /// </summary>
    public partial class PokemonCollectionView : ItemsControl
    {
        public PokemonCollectionView()
        {
          InitializeComponent();
          Background = PBO.UIElements.Brushes.GetGridTileBrush(8, PBO.Helper.NewBrush(0x33808080));//opacity=0.2
        }
    }
}
