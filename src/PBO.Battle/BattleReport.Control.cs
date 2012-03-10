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
using LightStudio.PokemonBattle.Interactive;

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
      private static Thickness GetBorderThickness(IText text)
      {
        return text.IsUnderlined ? new Thickness(0, 0, 0, 1) : new Thickness(0);
      }
      private static Block GetBlock(IText text)
      {
        Paragraph p = new Paragraph()
        {
          TextAlignment = GetAlignment(text),
          Background = GetBackground(text),
          Foreground = GetForeground(text),
          FontSize = text.FontSize,
          FontWeight = GetFontWeight(text),
          FontStyle = GetFontStyle(text),
          BorderThickness = GetBorderThickness(text),
          BorderBrush = GetForeground(text)
        };
        if (text.Contents == null)
        {
          p.Inlines.Add(new Run(text.Text));
        }
        else
        {
          System.Diagnostics.Debugger.Break();
          //foreach (IText t in text.Contents)
          //  p.Inlines.Add(GetBlock(t));
        }
        return p;
      }
      #endregion

      BattleReport nest;

      public Control(BattleReport battlereport)
      {
        this.nest = battlereport;
      }

      void IGameOutwardEvents.EventOccurred(GameEvent e)
      {
        IText text = e.GetGameLog();
        Block b = GetBlock(text);
        text.ClearData();
        nest.AddBlock(b);
        if (e is Interactive.GameEvents.BeginTurn)
        {
          nest.nowTurn = new LinkedListNode<Block>(b);
          nest.turnsBookmark.AddLast(nest.nowTurn);
        }
      }
    }
  }
}