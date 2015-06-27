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
        SimPokemon[] PokemonsOwner;
        internal void SetTargetPanel(Move move, MoveRange range)
        {
            OnPropertyChanged();
        }

        public KeyValuePair<PokemonOutward, bool> PF1;
        public KeyValuePair<PokemonOutward, bool> PF0;
        public KeyValuePair<SimPokemon, bool> PO0;
        public KeyValuePair<SimPokemon, bool> PO1;
        public Visibility O0O1
        { get { return PO0.Value && PO1.Value ? Visibility.Visible : Visibility.Collapsed; } }
        public Visibility O0F1
        { get { return PO0.Value && PF1.Value ? Visibility.Visible : Visibility.Collapsed; } }
        public Visibility O1F0
        { get { return PO1.Value && PF0.Value ? Visibility.Visible : Visibility.Collapsed; } }
        public Visibility F1F0
        { get { return PF1.Value && PF0.Value ? Visibility.Visible : Visibility.Collapsed; } }
    }
}
