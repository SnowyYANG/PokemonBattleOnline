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
using LightStudio.Tactic.DataModels;
using LightStudio.PokemonBattle.Data;
using LightStudio.PokemonBattle.Game;

namespace LightStudio.PokemonBattle.PBO.Battle
{
  /// <summary>
  /// Interaction logic for BattleReport.xaml
  /// </summary>
  public partial class BattleReport : UserControl
  {
    private class Control : IGameOutwardEvents
    {
      #region Static
      private static TextAlignment GetAlignment(IText text)
      {
        switch (text.Alignment)
        {
          case Alignment.Center:
            return TextAlignment.Center;
          case Alignment.Right:
            return TextAlignment.Right;
          default:
            return TextAlignment.Left;
        }
      }
      private static Brush GetBackground(IText text)
      {
        return Helper.NewBrush(text.Background);
      }
      private static Brush GetForeground(IText text)
      {
        return Helper.NewBrush(text.Foreground);
      }
      private static FontWeight GetFontWeight(IText text)
      {
        return text.IsBold ? FontWeights.Bold : FontWeights.Normal;
      }
      private static FontStyle GetFontStyle(IText text)
      {
        return text.IsItalic ? FontStyles.Italic : FontStyles.Normal;
      }
      private static TextDecorationCollection GetTextDecorations(IText text)
      {
        return text.IsUnderlined ? TextDecorations.Underline : null;
      }
      #endregion

      BattleReport nest;
      Paragraph current;

      public Control(BattleReport battlereport)
      {
        nest = battlereport;
        beginTurn = true;
      }

      public void AddText(string text, Brush foreground)
      {
        if (current == null || current.TextAlignment != TextAlignment.Left)
        {
          current = new Paragraph() { TextAlignment = TextAlignment.Left };
          nest.AddBlock(current);
        }
        current.Inlines.Add(new Run(text) { Foreground = foreground });
      }
      bool beginTurn;
      void IGameOutwardEvents.TurnEnd()
      {
        beginTurn = true;
      }
      void IGameOutwardEvents.GameLogAppend(IText text)
      {
        AddText(text);
        nest.AutoScroll();
        if (beginTurn)
        {
          nest.nowTurn = new LinkedListNode<TextElement>(current.Inlines.Last());
          nest.turnsBookmark.AddLast(nest.nowTurn);
          beginTurn = false;
        }
      }
      private void AddInline(Paragraph paragraph, IText text)
      {
        if (text.Contents == null)
        {
          var str = text.Text;
          paragraph.Inlines.Add(new Run(text.Text)
            {
              Background = GetBackground(text),
              Foreground = GetForeground(text),
              FontSize = nest.report.FontSize + text.FontSize,
              FontWeight = GetFontWeight(text),
              FontStyle = GetFontStyle(text),
              TextDecorations = GetTextDecorations(text)
            });
          using (System.IO.StreamWriter sw = new System.IO.StreamWriter(Messaging.PBOClient.Client.User.Name, true)) sw.Write(text.Text);
        }
        else foreach (IText t in text.Contents) AddText(t);
      }
      private void AddText(IText text)
      {
        if (current == null || GetAlignment(text) != current.TextAlignment)
        {
          if (current != null)
          {
            var run = current.Inlines.LastOrDefault() as Run;
            if (run != null) run.Text = run.Text.TrimEnd();
          }
          current = new Paragraph() { TextAlignment = GetAlignment(text) };
          nest.AddBlock(current);
        }
        AddInline(current, text);
      }
    }
  }
}