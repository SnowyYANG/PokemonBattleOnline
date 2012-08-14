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
using LightStudio.PokemonBattle.Game;

namespace LightStudio.PokemonBattle.PBO.Battle
{
  /// <summary>
  /// Interaction logic for BattleReport.xaml
  /// </summary>
  public partial class BattleReport : UserControl
  {
    ScrollViewer scroll;
    LinkedList<TextElement> turnsBookmark;
    LinkedListNode<TextElement> nowTurn;
    Control controller;
    
    public BattleReport()
    {
      InitializeComponent();
      turnsBookmark = new LinkedList<TextElement>();
      this.controller = new Control(this);
    }

    public void Init(GameOutward game)
    {
      game.AddListner(controller);
    }

    private void previousTurn_Click(object sender, RoutedEventArgs e)
    {
      if (nowTurn.Previous != null)
      {
        nowTurn = nowTurn.Previous;
        nowTurn.Value.BringIntoView();
      }
    }
    private void AddBlock(Block block)
    {
      if (scroll == null)
        scroll = reportViewer.Template.FindName("PART_ContentHost", reportViewer) as ScrollViewer;
      if (scroll.ScrollableHeight - scroll.ExtentHeight < 5)
      {
        report.Blocks.Add(block);
        scroll.ScrollToEnd();
      }
      else
        report.Blocks.Add(block);
    }
    public void AddLogText(string text)
    {
      controller.AddText(text, Brushes.OrangeRed);
    }
  }
}
