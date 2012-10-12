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
  internal class EditingPokemonVM : INotifyPropertyChanged
  {
    private static bool HasRandomMove(IEnumerable<int> moves)
    {
      const int METRONOME = 118, TRANSFORM = 144, ASSIST = 274, ME_FIRST = 382, COPYCAT = 383;
      moves.FirstOrDefault();
      return moves.Any((i) => i == METRONOME || i == TRANSFORM || i == ASSIST || i == ME_FIRST || i == COPYCAT);
    }

    private readonly PropertyChangedEventHandler statChanged;
    private bool changed;

    public EditingPokemonVM(PokemonData pm)
    {
      statChanged = (sender, e) => OnPropertyChanged(e);
      Origin = pm;
      Model = pm.Clone();
      this.Learnset = Enumerable.Empty<LearnItemVM>();
      CollectionViewSource.GetDefaultView(Learnset).Culture = CultureInfo.CurrentUICulture;
    }
    
    #region 6D
    public Visibility RemainingEvVisibility
    { get { return RemainingEv > 2 ? Visibility.Visible : Visibility.Collapsed; } }
    public int RemainingEv
    {
      get
      {
        var ev = Model.Ev;
        return 510 - ev.Hp - ev.Atk - ev.Def - ev.SpAtk - ev.SpDef - ev.Speed;
      }
    }
    public int Hp
    { get { return PokemonStatHelper.GetHp(PokemonForm.Data.Base.Hp, Model.Iv.Hp, Model.Ev.Hp, Model.Lv); } }
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
          PokemonStatHelper.Get5D(stat, PokemonNature.Serious, PokemonForm.Data.Base.GetStat(stat), Model.Iv.GetStat(stat), Model.Ev.GetStat(stat), Model.Lv),
          PokemonStatHelper.Get5D(stat, Model.Nature, PokemonForm.Data.Base.GetStat(stat), Model.Iv.GetStat(stat), Model.Ev.GetStat(stat), Model.Lv), 0);
    }
    #endregion

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
          _model.Iv.PropertyChanged += statChanged;
          _model.Ev.PropertyChanged += statChanged;
          UpdateLearnset();
          OnPropertyChanged();
        }
      }
    }
    public PokemonForm PokemonForm
    {
      get { return Model.Form; }
      set
      {
        if (Model.Form != value)
        {
          Model.Form = value;
          UpdateLearnset();
          Model.Ev.SetStat(StatType.All, 0);
          OnPropertyChanged();
        }
      }
    }
    public ImageSource Image
    { get { return ImageService.GetPokemonFront(PokemonForm, Model.Gender); } }
    public IEnumerable<LearnItemVM> Learnset
    { get; private set; }

    public Visibility HiddenPowerVisibility
    { get { return Model.MoveIds.Contains(237) || HasRandomMove(Model.MoveIds) ? Visibility.Visible : Visibility.Collapsed; } }
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
    public Visibility HappinessVisibility
    { get { return Model.MoveIds.Contains(216) || Model.MoveIds.Contains(218) || HasRandomMove(Model.MoveIds) ? Visibility.Visible : Visibility.Collapsed; } }
    
    private static IEnumerable<LearnItemVM> temp_moves;
    private void UpdateLearnset()
    {
      //if (temp_moves == null) temp_moves = Data.GameDataService.Moves.Select((m) => new LearnItemVM(new MoveLearnItem(m.Id));
      //Learnset = temp_moves;
      //OnPropertyChanged("Learnset");
    }

    public MessageBoxResult ChangedConfirm()
    {
      return changed ? UIElements.ShowMessageBox.PokemonUnsaved() : MessageBoxResult.None;
    }
    public void Save()
    {
      if (Origin != null)
      {
        var index = Origin.Container.IndexOf(Origin);
        Origin = _model.Clone();
        Origin.Container[index] = Origin;
        changed = false;
      }
    }
    public void ResetToLastSaved()
    {
      Model = Origin.Clone();
      UpdateLearnset();
      OnPropertyChanged();
      changed = false;
    }

    #region INotifyPropertyChanged
    public event PropertyChangedEventHandler PropertyChanged;
    private void OnPropertyChanged(PropertyChangedEventArgs e)
    {
      changed = true;
      if (PropertyChanged != null) PropertyChanged(this, e);
    }
    private void OnPropertyChanged(string propertyName)
    {
      OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
    }
    private void OnPropertyChanged()
    {
      OnPropertyChanged(new PropertyChangedEventArgs(null));
    }
    #endregion
  }
}
