﻿using System;
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
using PokemonBattleOnline.Game;
using PokemonBattleOnline.Network;
using PokemonBattleOnline.PBO.Elements;

namespace PokemonBattleOnline.PBO.Battle
{
  /// <summary>
  /// Interaction logic for BattleReport.xaml
  /// </summary>
  public partial class BattleReport : UserControl
  {
    internal const double DEFAULT_FONTSIZE = 15;
    
    internal FlowDocument RealTime, Final;
    private StringBuilder Text;
    private LinkedList<TextElement> turnsBookmark;
    private Control controller;
    
    public BattleReport()
    {
      InitializeComponent();
      Reset();
    }

    public void Init(GameOutward game)
    {
      game.AddListner(controller);
      game.GameEnd += () => reportViewer.Document = Final;
    }

    internal void Reset()
    {
      turnsBookmark = new LinkedList<TextElement>();
      RealTime = new FlowDocument();
      Final = new FlowDocument();
      Text = new StringBuilder();
      reportViewer.Document = RealTime;
      controller = new Control(this);
    }

    private LinkedListNode<TextElement> nowTurn;
    private void previousTurn_Click(object sender, RoutedEventArgs e)
    {
      if (nowTurn.Previous != null)
      {
        nowTurn = nowTurn.Previous;
        nowTurn.Value.BringIntoView();
      }
    }
    
    private ScrollViewer scroll;
    private void AutoScroll()
    {
      if (scroll == null)
        scroll = reportViewer.Template.FindName("PART_ContentHost", reportViewer) as ScrollViewer;
      if (scroll.ScrollableHeight - scroll.VerticalOffset < 5) scroll.ScrollToBottom();
    }
    public void AddLogText(string text)
    {
      controller.RealTime.AddText(text, Brushes.OrangeRed);
      AutoScroll();
    }
    public void AddChatText(string chat, User user)
    {
      controller.RealTime.AddText((user.Name + "：" + chat).LineBreak(), Cartes.GetChatBrush(user.Name));
      AutoScroll();
    }

    internal void Save(string title, string player)
    {
      controller.Save(title, player);
    }
  }
}
