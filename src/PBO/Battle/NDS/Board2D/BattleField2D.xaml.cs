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
    public partial class BattleField2D : UserControl
    {
        BoardOutward Board;
        int ObserveTeam;

        public BattleField2D()
        {
            InitializeComponent();
            PO0.Back = PO1.Back = true;
        }

        internal void Init(BoardOutward board, int observeTeam)
        {
            Board = board;
            ObserveTeam = observeTeam;
            Team.DataContext = board.Teams[observeTeam];
            FoeTeam.DataContext = board.Teams[1 - observeTeam];
            board.PokemonSentOut += OnPokemonSentOut;
        }
        void OnPokemonSentOut(int team, int x)
        {
            var c = team == ObserveTeam ? (x == 0 ? PO0 : PO1) : x == 0 ? PF0 : PF1;
            c.SendOut(Board[team, x]);
        }
    }
}
