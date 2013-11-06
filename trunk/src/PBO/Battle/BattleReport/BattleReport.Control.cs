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
      public readonly DocumentBattleReport RealTime;
      private readonly DocumentBattleReport Final;
      private readonly StringBuilder TextReport;

      public Control(BattleReport battlereport)
      {
        Nest = battlereport;
        beginTurn = true;
        RealTime = new DocumentBattleReport(Nest.RealTime);
        Final = new DocumentBattleReport(Nest.Final);
        TextReport = new StringBuilder();
      }

      bool beginTurn;
      void IGameOutwardEvents.TurnEnd()
      {
        beginTurn = true;
      }
      void IGameOutwardEvents.GameLogAppend(string text, LogStyle style)
      {
        if (!style.HasFlag(LogStyle.HiddenAfterBattle))
        {
          if (style.HasFlag(LogStyle.NoBr)) TextReport.Append(text);
          else TextReport.AppendLine(text);
        }
        UIDispatcher.Invoke(() =>
          {
            var align = style.HasFlag(LogStyle.Center) ? TextAlignment.Center : style.HasFlag(LogStyle.EndTurn) ? TextAlignment.Right : TextAlignment.Left;
            var color = SBrushes.GetBrush(style);
            var bold = style.HasFlag(LogStyle.Bold);
            if (!style.HasFlag(LogStyle.NoBr)) text = text.LineBreak();
            if (!style.HasFlag(LogStyle.HiddenInBattle)) RealTime.AddText(text, color, align, bold);
            if (!style.HasFlag(LogStyle.HiddenAfterBattle)) Final.AddText(text, color, align, bold);
            Nest.AutoScroll();
            if (beginTurn)
            {
              //Nest.nowTurn = new LinkedListNode<TextElement>(currentRealTime.Inlines.Last());
              //Nest.turnsBookmark.AddLast(Nest.nowTurn);
              beginTurn = false;
            }
          });
      }

      public void Save(string title, string player)
      {
        try
        {
          var path = "..\\MyPBO\\Logs\\" + player;
          if (!Directory.Exists(path)) Directory.CreateDirectory(path);
          using (StreamWriter sw = new System.IO.StreamWriter(path + string.Format("\\[{0}] {1}", DateTime.Now.ToString("yyyy-MM-dd-HHmm"), title) + ".txt", false, Encoding.Unicode))
            sw.Write(TextReport);
        }
        catch
        {
          ShowMessageBox.SaveLogFail();
        }
      }
    }
  }
}