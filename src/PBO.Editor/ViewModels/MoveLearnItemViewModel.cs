using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using LightStudio.PokemonBattle.Data;
using LightStudio.Tactic.Globalization;
using LightStudio.PokemonBattle.PBO.UIElements;

namespace LightStudio.PokemonBattle.PBO.Editor
{
    internal class MoveLearnItemViewModel : INotifyPropertyChanged
    {
        public MoveLearnItem Model
        { get; private set; }

        public PokemonCustomInfo Pokemon
        { get; private set; }

        public MoveType MoveType
        { get; private set; }

        public LocalizedString Name
        { get; private set; }

        private bool _isSelected;
        public bool IsSelected
        {
            get
            {
                return _isSelected;
            }
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    OnIsSelectedChanged();
                    OnPropertyChanged("IsSelected");
                }
            }
        }

        public MoveLearnItemViewModel(PokemonCustomInfo pm, MoveLearnItem model)
        {
            this.Pokemon = pm;
            this.Model = model;
            this.MoveType = DataService.GetMove(Model.MoveId);
            this._isSelected = Pokemon.MoveIds.Contains(Model.MoveId);
            this.Name = DataService.DataString.GetLocalizedString(MoveType.Name);
        }

        private void OnIsSelectedChanged()
        {
            if (!IsSelected)
            {
                Pokemon.RemoveMove(MoveType.Id);
            }
            else
            {
                if (!Pokemon.AddMove(MoveType.Id))
                    _isSelected = false;
            }
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
        
    }
}
