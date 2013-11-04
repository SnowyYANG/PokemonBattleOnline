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
using System.Threading;
using System.IO;
using PokemonBattleOnline.Game;
using PokemonBattleOnline.PBO.Elements;

namespace PokemonBattleOnline.PBO.Battle
{
  /// <summary>
  /// Interaction logic for BattleReport.xaml
  /// </summary>
  public partial class BattleReport : UserControl
  {
    private class Control : IGameOutwardEvents
    {
      private readonly BattleReport Nest;
      public readonly BattleReportImplement RealTime;
      private readonly BattleReportImplement Final;
      private readonly BattleReportImplement TextReport;
      private readonly StringBuilder Text;

      public Control(BattleReport battlereport)
      {
        Nest = battlereport;
        beginTurn = true;
        RealTime = new DocumentBattleReport(Nest.RealTime, true, false);
        Final = new DocumentBattleReport(Nest.Final, false, false);
        Text = new StringBuilder();
        TextReport = new StringBattleReport(Text);
      }

      bool beginTurn;
      void IGameOutwardEvents.TurnEnd()
      {
        beginTurn = true;
      }
      void IGameOutwardEvents.GameLogAppend(LogText text)
      {
        TextReport.AddText(text);
        UIDispatcher.Invoke(() =>
          {
            RealTime.AddText(text);
            Final.AddText(text);
            Nest.AutoScroll();
            if (beginTurn)
            {
              //Nest.nowTurn = new LinkedListNode<TextElement>(currentRealTime.Inlines.Last());
              //Nest.turnsBookmark.AddLast(Nest.nowTurn);
              beginTurn = false;
            }
          });
      }
      public void GameEnd()
      {
        Nest.reportViewer.Document = Nest.Final;
      }

      public void Save(string title, string player)
      {
        try
        {
          var path = "..\\MyPBO\\Logs\\" + player;
          if (!Directory.Exists(path)) Directory.CreateDirectory(path);
          using (StreamWriter sw = new System.IO.StreamWriter(path + string.Format("\\[{0}] {1}", DateTime.Now.ToString("yyyy-MM-dd-HHmm"), title) + ".txt", false, Encoding.Unicode))
            sw.Write(Text);
        }
        catch
        {
          ShowMessageBox.SaveLogFail();
        }
      }
    }

    private class DocumentBattleReport : BattleReportImplement
    {
      private static TextAlignment GetAlignment(Alignment alignment)
      {
        switch (alignment)
        {
          case Alignment.Center:
            return TextAlignment.Center;
          case Alignment.Right:
            return TextAlignment.Right;
          default:
            return TextAlignment.Left;
        }
      }

      private readonly FlowDocument Document;
      private readonly bool Battle, Final;

      public DocumentBattleReport(FlowDocument doc, bool battle, bool final)
      {
        Document = doc;
        Battle = battle;
        Final = final;
      }

      private Paragraph current;
      private uint lastBg = 0;
      private Alignment lastAlignment = Alignment.Left;
      public override void AddText(string text, uint foreground, Alignment alignment, uint background, double size, bool bold, bool italic, bool underline)
      {
        if (current == null || alignment != lastAlignment || lastBg != background)
        {
          lastAlignment = alignment;
          lastBg = background;
          if (current != null)
          {
            var run = current.Inlines.LastOrDefault() as Run;
            if (run != null) run.Text = run.Text.TrimEnd();
          }
          current = new Paragraph() { TextAlignment = GetAlignment(alignment), Background = SBrushes.NewBrush(background) };
          Document.Blocks.Add(current);
        }
        current.Inlines.Add(new Run(text)
        {
          Foreground = SBrushes.NewBrush(foreground),
          FontSize = BattleReport.DEFAULT_FONTSIZE + size,
          FontWeight = bold ? FontWeights.Bold : FontWeights.Normal,
          FontStyle = italic ? FontStyles.Italic : FontStyles.Normal,
          TextDecorations = underline ? TextDecorations.Underline : null
        });
      }
      protected override bool Visible(LogText text)
      {
        return !(text.HiddenInBattle && Battle || text.HiddenAfterBattle && Final);
      }
    }

    internal void Save(string title, string player)
    {
      controller.Save(title, player);
    }
  }
}