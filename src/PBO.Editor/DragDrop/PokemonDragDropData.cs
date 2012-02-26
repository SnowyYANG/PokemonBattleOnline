using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Controls.Primitives;
using LightStudio.PokemonBattle.PBO.UIElements.Interactivity;

namespace LightStudio.PokemonBattle.PBO.Editor
{
    internal class PokemonDragDropData : IDragDropData, INotifyPropertyChanged
    {
        public PokemonViewModel Pokemon
        { get; private set; }

        public int PokemonIndexInFolder
        { get; private set; }

        public DragDropActions AllowedActions
        { get; private set; }

        public IFolderViewModel SourceFolder
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

        public PokemonDragDropData(PokemonViewModel data, DragDropActions allowedActions,
            IFolderViewModel source, int pmIndex)
        {
            this.Pokemon = data;
            this.AllowedActions = allowedActions;
            this.SourceFolder = source;
            this.PokemonIndexInFolder = pmIndex;
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
                return SourceFolder;
            }
        }
    }
}
