using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using PokemonBattleOnline.Game;
using PokemonBattleOnline.PBO.Elements;

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
    public int Atk
    { get { return GetStat(StatType.Atk); } }
    public int Def
    { get { return GetStat(StatType.Def); } }
    public int SpAtk
    { get { return GetStat(StatType.SpAtk); } }
    public int SpDef
    { get { return GetStat(StatType.SpDef); } }
    public int Speed
    { get { return GetStat(StatType.Speed); } }
    private int GetStat(StatType stat)
    {
      return PokemonStatHelper.Get5D(stat, Model.Nature, Model.Form.Data.Base.GetStat(stat), Model.Iv.GetStat(stat), Model.Ev.GetStat(stat), Model.Lv);
    }

    public void RefreshAll()
    {
      OnPropertyChanged();
    }
  }
  internal class PokemonEditorVM : ObservableObject
  {
    public static readonly ICommand PPUpChangeCommand;

    static PokemonEditorVM()
    {
      PPUpChangeCommand = new SimpleCommand((_m) =>
        {
          var m = (LearnedMove)_m;
          m.PPUp = m.PPUp == 3 ? 0 : m.PPUp + 1;
        });
    }

    private static bool HasRandomMove(IEnumerable<LearnedMove> moves)
    {
      return moves.Any((m) => m.Move.Id == Ms.METRONOME || m.Move.Id == Ms.TRANSFORM || m.Move.Id == Ms.ASSIST || m.Move.Id == Ms.ME_FIRST || m.Move.Id == Ms.COPYCAT);
    }

    public PokemonEditorVM()
    {
    }

    private PokemonVM _origin;
    public PokemonVM Origin
    {
      get { return _origin; }
      set
      {
        if (_origin != value)
        {
          PokemonData m;
          if (_origin != null)
          {
            _origin.OnIsEditingChanged();
            m = _origin.Model;
          }
          else m = null;
          _origin = value;
          if (_origin != null)
          {
            _origin.OnIsEditingChanged();
            if (_origin.Model == null)
            {
              //var number = Config.Current.PokemonNumber;
              //var sp = RomData.GetPokemon(number);
              //if (sp == null) number = 1;
              _origin.Model = new PokemonData(Helper.Random.Next(1, 719), 0);
            }
            if (m != _origin.Model)
            {
              m = _origin.Model;
              _stats = new PokemonEditor6D(m);
              m.Iv.PropertyChanged += (sender, e) => OnPropertyChanged("HiddenPowerType");
              m.Ev.PropertyChanged += (sender, e) => RefreshRemainingEv();
              RefreshImage();
              RefreshLearnset();
              RefreshOptionalVisibility();
              RefreshRemainingEv();
              OnPropertyChanged();
              return;
            }
          }
          OnPropertyChanged("Origin");
        }
      }
    }

    public PokemonData Model
    { get { return Origin == null ? null : Origin.Model; } }
    public PokemonSpecies PokemonSpecies
    {
      get { return Model.Form.Species; }
      set
      {
        if (Model.Form.Species != value)
        {
          PokemonForm = value.GetForm(0);
          //Config.Current.PokemonNumber = PokemonForm.Species.Number;
        }
      }
    }
    public PokemonForm PokemonForm
    {
      get { return Model.Form; }
      set
      {
        if (Model.Form != value && value != null)
        {
          var oldNumber = Model.Form.Species.Number;
          var oldData = Model.Form.Data;
          Model.Form = value;
          if (oldNumber != value.Species.Number || oldNumber == 413 || oldNumber == 479 || oldNumber == 646) RefreshLearnset();
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
          var form = PokemonForm;
          Model.Gender = value;
          if (form != PokemonForm)
          {
            OnPropertyChanged("PokemonForm");
            if (form.Data != PokemonForm.Data) Stats.RefreshAll();
          }
          OnPropertyChanged("Gender");
          RefreshImage();
        }
      }
    }
    public int HeldItem
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
          OnPropertyChanged("HeldItem");
        }
      }
    }

    private PokemonEditor6D _stats;
    public PokemonEditor6D Stats
    { get { return _stats; } }
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
      _learnset = new Dictionary<int, LearnVM>();
      var number = PokemonSpecies.Number;
      var form = PokemonForm.Index;
      GetLearnset(number, form);
      number = RomData.GetPreEvolution(number);
      GetLearnset(number, 0);
      number = RomData.GetPreEvolution(number);
      GetLearnset(number, 0);
      var dataView = CollectionViewSource.GetDefaultView(Learnset);
      dataView.SortDescriptions.Add(new SortDescription("IsSelected", ListSortDirection.Descending));
      OnPropertyChanged("Learnset");
    }
    private void GetLearnset(int number, int form)
    {
      if (number != 0)
      {
        foreach (var gen in LearnList.Index)
          foreach (var game in gen.Games)
          {
            var l = game.Lv.Get(number, form);
            if (l != null)
            {
              foreach (var pair in l) GetLearnVM(pair.Key).AddMethod(LearnCategory.Lv);
              if (game.Tutor != null) foreach (var tutor in game.Tutor.Get(number, form)) GetLearnVM(tutor).AddMethod(LearnCategory.Tutor);
              if (game.TM != null) foreach (var tm in game.TM.Get(number, form)) GetLearnVM(tm).AddMethod(LearnCategory.Machine);
              if (game.HM != null) foreach (var hm in game.HM.Get(number, form)) GetLearnVM(hm).AddMethod(LearnCategory.Machine);
              if (game.Egg != null) foreach (var egg in game.Egg.Get(number)) GetLearnVM(egg).AddMethod(LearnCategory.Egg);
            }
          }
        if (number == 235)
        {
          for (var m = 1; m <= RomData.Moves.Count(); ++m)
            if (m != Ms.STRUGGLE && m != 603 && m != 606 && m != 607 && m != 614 && m != 615) GetLearnVM(m).AddMethod(LearnCategory.Other);
        }
        else foreach (var sp in LearnList.SP.Get(number, form)) GetLearnVM(sp).AddMethod(LearnCategory.Other);
      }
    }
    private LearnVM GetLearnVM(int move)
    {
      LearnVM vm;
      if (!_learnset.TryGetValue(move, out vm))
      {
        vm = new LearnVM(this, move);
        _learnset.Add(move, vm);
      }
      return vm;
    }

    private BitmapImage _image;
    public BitmapImage Image
    { get { return _image; } }
    private void RefreshImage()
    {
      var value = ImageService.GetPokemonFront(Model.Form, Model.Gender, false);
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
          var value = Model.HasMove(Ms.RETURN) || Model.HasMove(Ms.FRUSTRATION) ? Visibility.Visible : Visibility.Collapsed;
          if (_happinessVisibility != value)
          {
            _happinessVisibility = value;
            OnPropertyChanged("HappinessVisibility");
          }
        }
        {
          var value = Model.HasMove(Ms.HIDDEN_POWER) ? Visibility.Visible : Visibility.Collapsed;
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
      if (m.Id == Ms.SECRET_SWORD && PokemonSpecies.Number == 647) RefreshImage();
    }

    public void Close()
    {
      Origin = null;
    }
  }
}
