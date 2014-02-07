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
    { get { return GameHelper.GetHp(Model.Form.Data.Base.Hp, Model.Iv.Hp, Model.Ev.Hp, Model.Lv); } }
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
      return GameHelper.Get5D(stat, Model.Nature, Model.Form.Data.Base.GetStat(stat), Model.Iv.GetStat(stat), Model.Ev.GetStat(stat), Model.Lv);
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
          var m = (LearnedMove)_m;
          EditorVM.Current.EditingPokemon.RemoveMove(m.Move);
        });
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
      get { return Model == null ? null : Model.Form.Species; }
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
      get { return Model == null ? null : Model.Form; }
      set
      {
        if (Model.Form != value && value != null)
        {
          var oldNumber = Model.Form.Species.Number;
          var oldData = Model.Form.Data;
          Model.Form = value;
          if (oldNumber != value.Species.Number || oldNumber == 413 || oldNumber == 479 || oldNumber == 646 || oldNumber == 678) RefreshLearnset();
          if (oldData != Model.Form.Data) Stats.RefreshAll();
          RefreshImage();
          OnPropertyChanged();
        }
      }
    }
    public PokemonGender Gender
    {
      get { return Model == null ? 0 : Model.Gender; }
      set
      {
        if (Model.Gender != value)
        {
          var form = PokemonForm;
          Model.Gender = value;
          if (form != PokemonForm)
          {
            OnPropertyChanged("PokemonForm");
            if (form.Data != PokemonForm.Data)
            {
              Stats.RefreshAll();
              if (PokemonForm.Species.Number == 678) RefreshLearnset();
            }
          }
          OnPropertyChanged("Gender");
          RefreshImage();
        }
      }
    }
    public int HeldItem
    {
      get { return Model == null ? 0 : Model.Item; }
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
    { get { return Model == null ? 0 : 510 - Model.Ev.Sum(); } }
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
    { get { return _learnset == null ? null : _learnset.Values; } }
    private void RefreshLearnset()
    {
      _learnset = new Dictionary<int, LearnVM>();
      GetLearnset(PokemonSpecies.Number, PokemonForm.Index);
      //var dataView = CollectionViewSource.GetDefaultView(Learnset);
      //dataView.SortDescriptions.Add(new SortDescription("IsSelected", ListSortDirection.Descending));
      OnPropertyChanged("Learnset");
    }
    private void GetLearnset(int number, int form)
    {
      if (!(number == 413 || number == 479 || number == 646 || number == 678)) form = 0;
      GetGenericLearnset(number, form);
      if (number == 235)
      {
        for (var m = 1; m <= RomData.Moves.Count(); ++m)
          if (m != Ms.STRUGGLE && m != Ms.CHATTER && m != 603 && m != 606 && m != 607) GetLearnVM(m).AddMethod(LearnCategory.Other);
      }
      else
      {
        foreach (var sp in LearnList.SP.Get(number, form)) GetLearnVM(sp).AddMethod(LearnCategory.Other);
        if (number == 492) GetGenericLearnset(492, 1 - form);
        else
        {
          number = RomData.GetPreEvolution(number);
          GetGenericLearnset(number, 0);
          number = RomData.GetPreEvolution(number);
          GetGenericLearnset(number, 0);
        }
      }
    }
    private void GetGenericLearnset(int number, int form)
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
    { get { return Model == null ? 0 : GameHelper.HiddenPower(Model.Iv); } }
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

    public void AddMove(MoveType move)
    {
      foreach(var l in Learnset)
        if (l.Move == move && Model.AddMove(move))
        {
          RefreshOptionalVisibility();
          l.IsLearned = true;
          break;
        }
    }
    public void RemoveMove(MoveType m)
    {
      if (Model.RemoveMove(m))
      {
        RefreshOptionalVisibility();
        _learnset[m.Id].IsLearned = false;
        if (m.Id == Ms.SECRET_SWORD && PokemonSpecies.Number == 647) RefreshImage();
      }
    }

    public void Close()
    {
      Origin = null;
    }
  }
}
