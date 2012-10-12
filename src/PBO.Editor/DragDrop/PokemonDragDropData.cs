using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Controls.Primitives;
using LightStudio.PokemonBattle.Data;
using LightStudio.PokemonBattle.PBO.UIElements.Interactivity;

namespace LightStudio.PokemonBattle.PBO.Editor
{
    internal class PokemonDragDropData : IDragDropData, INotifyPropertyChanged
    {
        public PokemonData Pokemon
        { get; private set; }

        public int IndexInContainer
        { get; private set; }

        public DragDropActions AllowedActions
        { get; private set; }

        public PokemonCollection Source
        { get; private set; }

        private DragDropActions _actions = DragDropActions.None;
        public DragDropActions Actions
        {
            get
            {
                return _actions;
            }
            set
            {
                value = value & AllowedActions;
                if (_actions != value)
                {
                    _actions = value;
                    OnPropertyChanged("Actions");
                }
            }
        }

        public PokemonDragDropData(PokemonData data, DragDropActions allowedActions, PokemonCollection source, int pmIndex)
        {
            this.Pokemon = data;
            this.AllowedActions = allowedActions;
            this.Source = source;
            this.IndexInContainer = pmIndex;
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        object IDragDropData.Data
        {
            get
            {
                return Pokemon;
            }
        }

        object IDragDropData.Source
        {
            get
            {
                return Source;
            }
        }
    }
}
