using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Diagnostics.Contracts;
using LightStudio.PokemonBattle.Data;

namespace LightStudio.PokemonBattle.Data
{
  [DataContract(Namespace = Namespaces.PBO)]
  public class PokemonData : ICloneable, IPokemonData, INotifyPropertyChanged
  {
    private const int WORMADAM = 413;
    private const int ROTOM = 479;
    private const int GIRATINA = 487;
    private const int ARCEUS = 493;
    private const int DEERLING = 585;
    private const int SAWSBUCK = 586;
    private const int GENESECT = 649;
    private const int KYUREM = 646;
    private const int KELDEO = 647;
    private const int SECRET_SWORD = 548;
    private const int GRISEOUS_ORB = 1;
    private const int PLATE_MINID = 75;
    private const int PLATE_MAXID = 90;
    private const int DRIVE_MINID = 98;
    private const int DRIVE_MAXID = 101;
    private static bool CanChangeIv(I6D sender, int oldValue, int newValue)
    {
      return 0 <= newValue && newValue < 32;
    }
    private static bool CanChangeEv(I6D sender, int oldValue, int newValue)
    {
      return 0 <= newValue && newValue <= 255 && sender.Hp + sender.Atk + sender.Def + sender.SpAtk + sender.SpDef + sender.Speed + newValue - oldValue <= 510;
    }

    [DataMember]
    private short number;
    [DataMember(EmitDefaultValue = false)]
    private byte form;
    [DataMember]
    private ObservableCollection<int> moveIds;

    public PokemonData(int number, int form)
    {
      moveIds = new ObservableCollection<int>();
      _ev = new Observable6D();
      Form = GameDataService.GetPokemon(number, form);
    }

    #region properties
    [DataMember(EmitDefaultValue = false)]
    private string _name;
    public string Name
    {
      get { return _name ?? Form.Type.GetLocalizedName(); }
      set
      {
        if (string.IsNullOrWhiteSpace(value)) value = null;
        else value = value.Trim();
        if (_name != value && PokemonValidator.ValidateName(value))
        {
          _name = value;
          OnPropertyChanged("Name");
        }
      }
    }

    private static readonly int[] CAN_CHOOSE_FORM = { 201, 386, 412, 413, 422, 423, 479, 492, 641, 642, 645, 646 };
    public bool CanChooseForm
    { get { return CAN_CHOOSE_FORM.Contains(number) || number == KELDEO && MoveIds.Contains(SECRET_SWORD); } }

    private PokemonForm _form;
    public PokemonForm Form
    { 
      get
      {
        if (_form == null)
        {
          CheckSpForm();
          _form = GameDataService.GetPokemon(number, form);
        }
        return _form;
      }
      set
      {
        if (!(number == value.Type.Number && form == value.Index))
        {
          _form = null;
          var oldNumber = number;
          var oldForm = form;
          
          number = value.Type.Number;
          form = (byte)value.Index;
          CheckSpForm();
          
          if (oldNumber != number)
          {
            moveIds.Clear();
            _gender = value.Type.Genders.First();
            _ev.SetStat(StatType.All, 0);
          }
          else if (number == 413 || number == 479 || number == 646) moveIds.Clear();
          _abilityIndex = 0;
          
          OnPropertyChanged();
        }
      }
    }

    [DataMember(EmitDefaultValue = false)]
    private byte _lv;
    public int Lv
    {
      get { return 100 - _lv; }
      set
      {
        if (_lv != value)
        {
          if (PokemonValidator.ValidateLv(value))
            _lv = (byte)(100 - value);
          OnPropertyChanged("Lv");
        }
      }
    }

    [DataMember(EmitDefaultValue = false)]
    private PokemonGender _gender;
    public PokemonGender Gender
    {
      get { return _gender; }
      set
      {
        if (_gender != value && Form.Type.Genders.Contains(value))
        {
          _gender = value;
          OnPropertyChanged("Gender");
        }
      }
    }

    [DataMember(EmitDefaultValue = false)]
    private PokemonNature _nature;
    public PokemonNature Nature
    {
      get { return _nature; }
      set
      {
        if (_nature != value)
        {
          _nature = value;
          OnPropertyChanged("Nature");
        }
      }
    }

    [DataMember(EmitDefaultValue = false)]
    private byte _abilityIndex;
    int IPokemonData.AbilityIndex
    { get { return _abilityIndex; } }
    public Ability Ability
    { 
      get { return Form.Data.GetAbility(_abilityIndex); }
      set
      {
        if (Ability != value)
        {
          _abilityIndex = 0;
          if (Form.Data.Abilities[1] == value) _abilityIndex = 1;
          else if (Form.Data.Abilities[2] == value) _abilityIndex = 2;
          OnPropertyChanged("Ability");
        }
      }
    }

    private Observable6D _iv;
    public Observable6D Iv
    {
      get
      {
        if (_iv == null)
        {
          _iv = new Observable6D(31, 31, 31, 31, 31, 31);
          _iv.CanChange6D += CanChangeIv;
        }
        return _iv;
      }
    }
    I6D IPokemonData.Iv
    { get { return Iv; } }
    [DataMember(EmitDefaultValue = false)]
    private ReadOnly6D _Iv
    {
      get { return new ReadOnly6D(31 - Iv.Hp, 31 - Iv.Atk, 31 - Iv.SpAtk, 31 - Iv.Def, 31 - Iv.SpDef, 31 - Iv.Speed); }
      set
      { 
        _iv = new Observable6D(31 - value.Hp, 31 - value.Atk, 31 - value.Def, 31 - value.SpAtk, 31 - value.SpDef, 31 - value.Speed);
        _iv.CanChange6D += CanChangeIv;
      }
    }
    
    [DataMember(EmitDefaultValue = false)]
    private short _itemId;
    int IPokemonData.ItemId
    { get { return _itemId; } }
    public Item Item
    { 
      get { return GameDataService.GetItem(_itemId); }
      set
      { 
        if (Item != value)
        {
          _itemId = (short)(value == null ? 0 : value.Id);
          if (CheckSpForm()) OnPropertyChanged();
          else OnPropertyChanged("HeldItem");
        }
      }
    }

    [DataMember(EmitDefaultValue = false)]
    private byte _happiness;
    public int Happiness
    { 
      get { return 255 - _happiness; }
      set
      {
        if (_happiness != value)
        {
          _happiness = (byte)(255 - value);
          OnPropertyChanged("Happiness");
        }
      }
    }

    [DataMember]
    private Observable6D _ev;
    I6D IPokemonData.Ev
    { get { return _ev; } }
    bool got;
    public Observable6D Ev
    { 
      get
      {
        if (!got)
        {
          got = true;
          _ev.CanChange6D += CanChangeEv;
        }
        return _ev;
      }
    }

    public IEnumerable<int> MoveIds
    { get { return moveIds; } }
    #endregion

    private PokemonCollection _container;
    public PokemonCollection Container
    {
      get { return _container; }
      internal set
      {
        if (_container != value)
        {
          _container = value;
          OnPropertyChanged("Container");
        }
      }
    }

    private bool CheckSpForm()
    {
      switch (number)
      {
        case DEERLING:
        case SAWSBUCK:
          form = (byte)((DateTime.Now.Month - 1) & 3);
          break;
        case ARCEUS:
          form = (byte)(PLATE_MINID <= _itemId && _itemId <= PLATE_MAXID ? _itemId - DRIVE_MINID + 1 : 0);
          break;
        case GIRATINA:
          form = (byte)(_itemId == GRISEOUS_ORB ? 1 : 0);
          break;
        case GENESECT:
          form = (byte)(DRIVE_MINID <= _itemId && _itemId <= DRIVE_MAXID ? _itemId - DRIVE_MINID + 1 : 0);
          break;
        case KELDEO:
          if (!moveIds.Contains(SECRET_SWORD)) form = 0;
          break;
      }
      if (_form != null && _form.Index != form) _form = null;
      return _form == null;
    }

    public bool AddMove(int moveId)
    {
      if (moveIds.Count < 4 && !moveIds.Contains(moveId))
      {
        moveIds.Add(moveId);
        if (number == KELDEO && moveId == SECRET_SWORD) OnPropertyChanged("CanChooseForm");
        return true;
      }
      return false;
    }
    public void RemoveMove(int moveId)
    {
      if (moveIds.Remove(moveId))
      {
        if (CheckSpForm()) OnPropertyChanged();
        if (number == KELDEO && moveId == SECRET_SWORD) OnPropertyChanged("CanChooseForm");
      }
    }

    #region ICloneable
    public PokemonData Clone()
    {
      var clone = MemberwiseClone() as PokemonData;
      clone.PropertyChanged = null;
      clone._iv = new Observable6D(Iv);
      clone._ev = new Observable6D(Ev);
      clone.got = false;
      clone.moveIds = new ObservableCollection<int>(MoveIds);
      return clone;
    }
    object ICloneable.Clone()
    {
      return Clone();
    }
    #endregion

    #region INotifyPropertyChanged
    public event PropertyChangedEventHandler PropertyChanged;
    private void OnPropertyChanged(string propertyName = null)
    {
      if (PropertyChanged != null)
        PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
    }
    #endregion
  }
}
