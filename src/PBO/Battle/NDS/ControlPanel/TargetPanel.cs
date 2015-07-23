using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using PokemonBattleOnline.Game;

namespace PokemonBattleOnline.PBO.Battle
{
    class TargetPanel : ObservableObject
    {
        internal void SetTargetPanel(Move move, MoveRange range)
        {
            switch(range)
            {

            }
            OnPropertyChanged();
        }

        bool CanSelect;
        public KeyValuePair<PokemonOutward, bool> PO1;
        public KeyValuePair<PokemonOutward, bool> PO0;
        public KeyValuePair<SimPokemon, bool> P0;
        public KeyValuePair<SimPokemon, bool> P1;
        public Visibility C01
        { get { return !CanSelect && P0.Value && P1.Value ? Visibility.Visible : Visibility.Collapsed; } }
        public Visibility C0O1
        { get { return !CanSelect && P0.Value && PO1.Value ? Visibility.Visible : Visibility.Collapsed; } }
        public Visibility C1O0
        { get { return !CanSelect && P1.Value && PO0.Value ? Visibility.Visible : Visibility.Collapsed; } }
        public Visibility CO1O0
        { get { return !CanSelect && PO1.Value && PO0.Value ? Visibility.Visible : Visibility.Collapsed; } }
    }
}
