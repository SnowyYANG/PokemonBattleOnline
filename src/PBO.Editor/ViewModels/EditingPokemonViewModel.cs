using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Data;
using System.ComponentModel;
using System.Globalization;
using System.Collections.Specialized;
using LightStudio.PokemonBattle.Data;
using LightStudio.PokemonBattle.PBO;
using LightStudio.PokemonBattle.PBO.UIElements;

namespace LightStudio.PokemonBattle.PBO.Editor
{
  internal class EditingPokemonViewModel : INotifyPropertyChanged
  {
    public EditingPokemonViewModel()
    {
      this.Learnset = new ObservableCollection<MoveLearnItemViewModel>();
      CollectionViewSource.GetDefaultView(Learnset).Culture = CultureInfo.CurrentUICulture;
      this.ResetEvCommand = new MenuCommand("ClearEv", ClearEv);
    }

    #region 6D
    PairValue GetStat(StatType stattype, int typebase, byte iv, byte ev)
    {
      return new PairValue(
          PokemonStatHelper.GetStat(stattype, PokemonNature.Serious, typebase, iv, ev, Model.Lv),
          PokemonStatHelper.GetStat(stattype, Model.Nature, typebase, iv, ev, Model.Lv), 0);
    }
    /// <summary>
    /// 判断的是4的倍数，所以剩余1点或2点将被忽略
    /// </summary>
    public bool HasRemainingEv
    { get { return RemainingEv > 2; } }
    public int RemainingEv
    {
      get
      {
        return ExecIfModelLoaded(() =>
            510 - Model.HpEv - Model.AtkEv - Model.DefEv
                - Model.SpeedEv - Model.SpAtkEv - Model.SpDefEv);
      }
    }
    public int Hp
    {
      get
      {
        return ExecIfModelLoaded(() =>
            PokemonStatHelper.GetStat(StatType.Hp, Model.Nature, PokemonType.BaseHp,
            Model.HpIv, Model.HpEv, Model.Lv));
      }
    }
    public int HiddenPowerPower
    {
      get
      {
        return ExecIfModelLoaded(() =>
             PokemonStatHelper.GetHiddenPowerPower(Model.HpIv, Model.AtkIv, Model.DefIv,
             Model.SpeedIv, Model.SpAtkIv, Model.SpDefIv));
      }
    }
    public PairValue Atk
    {
      get
      {
        return ExecIfModelLoaded(() =>
            GetStat(StatType.Atk, PokemonType.BaseAtk, Model.AtkIv, Model.AtkEv));
      }
    }
    public PairValue Def
    {
      get
      {
        return ExecIfModelLoaded(() =>
            GetStat(StatType.Def, PokemonType.BaseDef, Model.DefIv, Model.DefEv));
      }
    }
    public PairValue Speed
    {
      get
      {
        return ExecIfModelLoaded(() =>
            GetStat(StatType.Speed, PokemonType.BaseSpeed, Model.SpeedIv, Model.SpeedEv));
      }
    }
    public PairValue SpAtk
    {
      get
      {
        return ExecIfModelLoaded(() =>
            GetStat(StatType.SpAtk, PokemonType.BaseSpAtk, Model.SpAtkIv, Model.SpAtkEv));
      }
    }
    public PairValue SpDef
    {
      get
      {
        return ExecIfModelLoaded(() =>
            GetStat(StatType.SpDef, PokemonType.BaseSpDef, Model.SpDefIv, Model.SpDefEv));
      }
    }
    #endregion

    #region Properties
    public bool IsChanged
    { get { return Model != null && PokemonViewModel != null && !Model.ValueEquals(PokemonViewModel.Model); } }
    private PokemonType _pokemonType;
    public PokemonCustomInfo Model { get; private set; }
    private PokemonViewModel _pokemonViewModel;
    public PokemonViewModel PokemonViewModel
    {
      get
      {
        return _pokemonViewModel;
      }
      set
      {
        if (_pokemonViewModel != value)
        {
          MessageBoxResult r = ChangedConfirm();
          if (r == MessageBoxResult.Yes) Save();
          else if (r == MessageBoxResult.Cancel) return;
          if (Model != null)
          {
            Model.PropertyChanged -= Model_PropertyChanged;
          }
          _pokemonViewModel = value;
          if (value != null)
          {
            Model = value.Model.Clone();
            _pokemonType = DataService.GetPokemonType(Model.PokemonTypeId);
            UpdateLearnset();
            Model.PropertyChanged += Model_PropertyChanged;
          }
          else Model = null;
          OnPropertyChanged("PokemonType");
          OnPropertyChanged(null);
        }
      }
    }

    public PokemonType PokemonType
    {
      get { return _pokemonType; }
      set
      {
        if (_pokemonType != value)
        {
          _pokemonType = value;
          Model.ChangeType(_pokemonType);
          UpdateLearnset();
          ClearEv();
          OnPropertyChanged("PokemonType");
        }
      }
    }
    public MenuCommand ResetEvCommand
    { get; private set; }
    public ObservableCollection<MoveLearnItemViewModel> Learnset
    { get; private set; }
    #endregion

    internal void Save()
    {
      PokemonViewModel.Model = this.Model.Clone();
      Editor.CurrentEditor.Model.Save();
      OnPropertyChanged("IsChanged");
    }
    internal void ResetToLastSaved()
    {
      if (PokemonViewModel != null)
      {
        Model = PokemonViewModel.Model.Clone();
        OnPropertyChanged();
      }
    }
    internal MessageBoxResult ChangedConfirm()
    {
      if (IsChanged) return UIElements.ShowMessageBox.PokemonUnsaved();
      else return MessageBoxResult.None;
    }

    private void UpdateLearnset()
    {
      Learnset.Clear();
      foreach (MoveLearnItem move in PokemonType.Learnset)
        Learnset.Add(new MoveLearnItemViewModel(Model, move));
    }
    private void ClearEv()
    {
      Model.HpEv = 0;
      Model.AtkEv = 0;
      Model.DefEv = 0;
      Model.SpeedEv = 0;
      Model.SpAtkEv = 0;
      Model.SpDefEv = 0;
    }
    public Visibility HasHiddenPower
    { get { return Model!=null? (Model.MoveIds.Contains(237)? Visibility.Visible : Visibility.Hidden) : Visibility.Hidden; } }

    void Model_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      OnPropertyChanged();
    }
    #region INotifyPropertyChanged
    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged(string propertyName)
    {
      if (PropertyChanged != null)
        PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
    }
    protected void OnPropertyChanged()
    {
      OnPropertyChanged(null);
    }
    #endregion

    private T ExecIfModelLoaded<T>(Func<T> func)
    {
      if (PokemonType != null && Model != null)
        return func();
      return default(T);
    }
  }
}
