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
using PokemonBattleOnline.Game;

namespace PokemonBattleOnline.PBO.Battle
{
  /// <summary>
  /// Interaction logic for BattleField2D.xaml
  /// </summary>
  public partial class BattleField2D : UserControl, IBoardOutwardEvents
  {
    BoardOutward board;
    int observeTeam;

    public BattleField2D()
    {
      InitializeComponent();
    }

    internal void Init(BoardOutward board, int observeTeam)
    {
      this.board = board;
      this.observeTeam = observeTeam;
      board.AddListener(this);
    }
    void IBoardOutwardEvents.PokemonSentout(int team, int x)
    {
      if (team == observeTeam) opm.Sendout(board[team, x]);
      else rpm.Sendout(board[team, x]);
    }
    void IBoardOutwardEvents.WeatherChanged()
    {
    }
  }
}
