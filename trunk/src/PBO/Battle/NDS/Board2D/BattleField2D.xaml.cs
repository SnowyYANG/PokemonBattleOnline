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
    BoardOutward Board;
    int ObserveTeam;

    public BattleField2D()
    {
      InitializeComponent();
      opm.Back = true;
    }

    internal void Init(BoardOutward board, int observeTeam)
    {
      Board = board;
      ObserveTeam = observeTeam;
      Team.DataContext = board.Teams[observeTeam];
      FoeTeam.DataContext = board.Teams[1 - observeTeam];
      board.AddListener(this);
    }
    void IBoardOutwardEvents.PokemonSentout(int team, int x)
    {
      if (team == ObserveTeam) opm.SendOut(Board[team, x]);
      else rpm.SendOut(Board[team, x]);
    }
  }
}
