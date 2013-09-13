using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Data;
using System.Windows.Input;
using System.ComponentModel;
using System.Globalization;
using System.Collections.Specialized;
using PokemonBattleOnline.Game;
using PokemonBattleOnline.PBO;
using PokemonBattleOnline.PBO.UIElements;

namespace PokemonBattleOnline.PBO.Editor
{
    internal class PokemonEditor6D : ObservableObject
    {
        private readonly PokemonData Model;

        private readonly PropertyChangedEventHandler statChanged;
        public PokemonEditor6D(PokemonData model)
        {
            Model = model;
            statChanged = (sender, e) => OnPropertyChanged(e);
            model.Iv.PropertyChanged += statChanged;
            model.Ev.PropertyChanged += statChanged;
            model.PropertyChanged += (sender, e) =>
              {
                  if (e.PropertyName == "Lv" || e.PropertyName == "Nature") RefreshAll();
              };
        }

        public int Hp
        { get { return PokemonStatHelper.GetHp(Model.Form.Data.Base.Hp, Model.Iv.Hp, Model.Ev.Hp, Model.Lv); } }
        public PairValue Atk
        { get { return GetStat(StatType.Atk); } }
        public PairValue Def
        { get { return GetStat(StatType.Def); } }
        public PairValue SpAtk
        { get { return GetStat(StatType.SpAtk); } }
        public PairValue SpDef
        { get { return GetStat(StatType.SpDef); } }
        public PairValue Speed
        { get { return GetStat(StatType.Speed); } }
        private PairValue GetStat(StatType stat)
        {
            return new PairValue(
                PokemonStatHelper.Get5D(stat, PokemonNature.Serious, Model.Form.Data.Base.GetStat(stat), Model.Iv.GetStat(stat), Model.Ev.GetStat(stat), Model.Lv),
                PokemonStatHelper.Get5D(stat, Model.Nature, Model.Form.Data.Base.GetStat(stat), Model.Iv.GetStat(stat), Model.Ev.GetStat(stat), Model.Lv));
        }

        public void RefreshAll()
        {
            OnPropertyChanged();
        }
    }
    internal class PokemonEditorVM : ObservableObject
    {
        public static readonly ICommand RemoveMoveCommand;
        public static readonly ICommand PPUpChangeCommand;

        static PokemonEditorVM()
        {
            RemoveMoveCommand = new SimpleCommand((_m) =>
              {
                  var l = EditorVM.Current.EditingPokemon._learnset.ValueOrDefault(((LearnedMove)_m).Move.Id);
                  if (l != null) l.IsSelected = false;
              });
            PPUpChangeCommand = new SimpleCommand((_m) =>
              {
                  var m = (LearnedMove)_m;
                  m.PPUp = m.PPUp == 3 ? 0 : m.PPUp + 1;
              });
        }

        private static bool HasRandomMove(IEnumerable<LearnedMove> moves)
        {
            const int METRONOME = 118, TRANSFORM = 144, ASSIST = 274, ME_FIRST = 382, COPYCAT = 383;
            moves.FirstOrDefault();
            return moves.Any((m) => m.Move.Id == METRONOME || m.Move.Id == TRANSFORM || m.Move.Id == ASSIST || m.Move.Id == ME_FIRST || m.Move.Id == COPYCAT);
        }

        public PokemonEditorVM(PokemonData pm)
        {
            Origin = pm;
            Model = pm.Clone();
        }

        public PokemonData Origin
        { get; private set; }
        private PokemonData _model;
        public PokemonData Model
        {
            get { return _model; }
            set
            {
                if (_model != value)
                {
                    _model = value;
                    _stats = new PokemonEditor6D(_model);
                    _model.Iv.PropertyChanged += (sender, e) => OnPropertyChanged("HiddenPowerType");
                    _model.Ev.PropertyChanged += (sender, e) => RefreshRemainingEv();
                    RefreshImage();
                    RefreshLearnset();
                    RefreshOptionalVisibility();
                    RefreshRemainingEv();
                    OnPropertyChanged();
                }
            }
        }
        public PokemonType PokemonType
        {
            get { return Model.Form.Type; }
            set { if (Model.Form.Type != value) PokemonForm = value.GetForm(0); }
        }
        public PokemonForm PokemonForm
        {
            get { return Model.Form; }
            set
            {
                if (Model.Form != value && value != null)
                {
                    var oldNumber = Model.Form.Type.Number;
                    var oldData = Model.Form.Data;
                    Model.Form = value;
                    if (oldNumber != value.Type.Number || oldNumber == 413 || oldNumber == 479 || oldNumber == 646) RefreshLearnset();
                    if (oldData != Model.Form.Data) Stats.RefreshAll();
                    RefreshImage();
                    OnPropertyChanged();
                }
            }
        }
        public PokemonGender Gender
        {
            get { return Model.Gender; }
            set
            {
                if (Model.Gender != value)
                {
                    Model.Gender = value;
                    RefreshImage();
                }
            }
        }
        public Item HeldItem
        {
            get { return Model.Item; }
            set
            {
                if (Model.Item != value)
                {
                    var form = PokemonForm;
                    Model.Item = value;
                    if (form != PokemonForm)
                    {
                        RefreshImage();
                        OnPropertyChanged("PokemonForm");
                        if (form.Data != PokemonForm.Data) Stats.RefreshAll();
                    }
                }
            }
        }

        private PokemonEditor6D _stats;
        public PokemonEditor6D Stats
        {
            get { return _stats; }
            private set
            {
                if (_stats != value)
                {
                    _stats = value;
                    OnPropertyChanged("Stats");
                }
            }
        }
        public int RemainingEv
        {
            get
            {
                var ev = Model.Ev;
                return 510 - ev.Sum();
            }
        }
        private Visibility _remainingEvVisibility;
        public Visibility RemainingEvVisibility
        { get { return _remainingEvVisibility; } }
        private void RefreshRemainingEv()
        {
            OnPropertyChanged("RemainingEv");
            var value = RemainingEv > 2 ? Visibility.Visible : Visibility.Collapsed;
            if (_remainingEvVisibility != value)
            {
                _remainingEvVisibility = value;
                OnPropertyChanged("RemainingEvVisibility");
            }
        }

        private Dictionary<int, LearnVM> _learnset;
        public IEnumerable<LearnVM> Learnset
        { get { return _learnset.Values; } }
        private void RefreshLearnset()
        {
            _learnset = Data.TempLearnSet.GetMoves(PokemonType.Number).ToDictionary(m => m, (m) => new LearnVM(this, Data.GameDataService.GetMove(m)));
            OnPropertyChanged("Learnset");
        }

        private ImageSource _image;
        public ImageSource Image
        { get { return _image; } }
        private void RefreshImage()
        {
            var value = GameDataService.GetPokemonFront(Model.Form, Model.Gender, false);
            if (_image != value)
            {
                _image = value;
                OnPropertyChanged("Image");
            }
        }

        private Visibility _hiddenPowerVisibility;
        public Visibility HiddenPowerVisibility
        { get { return _hiddenPowerVisibility; } }
        public BattleType HiddenPowerType
        {
            get
            {
                int pI;
                var iv = Model.Iv;
                pI = iv.Hp & 1;
                pI |= (iv.Atk & 1) << 1;
                pI |= (iv.Def & 1) << 2;
                pI |= (iv.Speed & 1) << 3;
                pI |= (iv.SpAtk & 1) << 4;
                pI |= (iv.SpDef & 1) << 5;
                return (BattleType)(pI * 15 / 63 + 2);
            }
        }
        private Visibility _happinessVisibility;
        public Visibility HappinessVisibility
        { get { return _happinessVisibility; } }
        private void RefreshOptionalVisibility()
        {
            if (HasRandomMove(Model.Moves))
            {
                if (_happinessVisibility != Visibility.Visible)
                {
                    _happinessVisibility = Visibility.Visible;
                    OnPropertyChanged("HappinessVisibility");
                }
                if (_hiddenPowerVisibility != Visibility.Visible)
                {
                    _hiddenPowerVisibility = Visibility.Visible;
                    OnPropertyChanged("HiddenPowerVisibility");
                }
            }
            else
            {

                {
                    var value = Model.HasMove(216) || Model.HasMove(218) ? Visibility.Visible : Visibility.Collapsed;
                    if (_happinessVisibility != value)
                    {
                        _happinessVisibility = value;
                        OnPropertyChanged("HappinessVisibility");
                    }
                }
                {
                    var value = Model.HasMove(237) ? Visibility.Visible : Visibility.Collapsed;
                    if (_hiddenPowerVisibility != value)
                    {
                        _hiddenPowerVisibility = value;
                        OnPropertyChanged("HiddenPowerVisibility");
                    }
                }
            }
        }

        public bool AddMove(MoveType m)
        {
            if (Model.AddMove(m))
            {
                RefreshOptionalVisibility();
                return true;
            }
            else return false;
        }
        public void RemoveMove(MoveType m)
        {
            Model.RemoveMove(m);
            RefreshOptionalVisibility();
            if (m.Id == 548 && PokemonType.Number == 647) RefreshImage();
        }

        public MessageBoxResult ChangedConfirm()
        {
            return !(Origin.Name == Model.Name && Origin.ValueEquals(Model)) ? UIElements.ShowMessageBox.PokemonUnsaved() : MessageBoxResult.None;
        }
        public void Save()
        {
            if (Origin != null)
            {
                var container = Origin.Container;
                var index = container.IndexOf(Origin);
                Origin = _model.Clone();
                container[index] = Origin;
                DataService.UserData.Save();
            }
        }
        public void ResetToLastSaved()
        {
            if (!(Origin.Name == Model.Name && Origin.ValueEquals(Model)) && ShowMessageBox.PokemonResetToLastSaved())
            {
                Model = Origin.Clone();
                RefreshLearnset();
                OnPropertyChanged();
            }
        }
    }
}
