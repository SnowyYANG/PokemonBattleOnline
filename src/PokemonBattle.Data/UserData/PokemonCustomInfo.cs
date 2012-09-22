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
  [DataContract(Namespace = Namespaces.LIGHT)]
  public class PokemonCustomInfo : ICloneable, INotifyPropertyChanged
  {
    private string DefaultName
    { get { return DataService.GetPokemonType(PokemonTypeId).GetLocalizedName(); } }

    [DataMember(EmitDefaultValue = false)]
    private string _name;
    public string Name
    {
      get
      {
        if (string.IsNullOrWhiteSpace(_name)) return DefaultName;
        return _name;
      }
      set
      {
        if (_name != value)
        {
          if (value.Length > UserDataRules.NameLength)
            value = value.Substring(0, UserDataRules.NameLength);
          _name = value;
          OnPropertyChanged("Name");
        }
      }
    }

    [DataMember]
    private int _pokemonTypeId;
    public int PokemonTypeId
    { 
      get { return _pokemonTypeId; }
      private set
      {
        if (_pokemonTypeId != value && value > 0 && value <= 664) ChangeType(value);
      }
    }

    private byte _lv;
    [DataMember]
    public byte Lv
    {
      get { return _lv; }
      set
      {
        if (_lv != value)
        {
          if (PokemonValidator.ValidateLv(value))
            _lv = value;
          OnPropertyChanged("Lv");
        }
      }
    }

    private PokemonGender _gender;
    [DataMember(EmitDefaultValue = false)]
    public PokemonGender Gender
    {
      get { return _gender; }
      set
      {
        if (_gender != value)
        {
          _gender = value;
          OnPropertyChanged("Gender");
        }
      }
    }

    private PokemonNature _nature;
    [DataMember(EmitDefaultValue = false)]
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

    private int _abilityIndex;
    [DataMember(EmitDefaultValue = false)]
    public int AbilityIndex
    {
      get { return _abilityIndex; }
      set
      {
        if (_abilityIndex != value && value != -1)
        {
          _abilityIndex = value;
          OnPropertyChanged("AbilityIndex");
        }
      }
    }

    private int? _itemId;
    [DataMember(EmitDefaultValue = false)]
    public int? ItemId
    {
      get { return _itemId; }
      set
      {
        if (_itemId != value)
        {
          _itemId = value;
          OnPropertyChanged("ItemId");
        }
      }
    }
    
    private byte _happiness;
    [DataMember(EmitDefaultValue = false)]
    public byte Happiness
    { 
      get { return _happiness; }
      set
      {
        if (_happiness != value)
        {
          _happiness = value;
          OnPropertyChanged("Happiness");
        }
      }
    }

    #region Iv
    [DataMember(EmitDefaultValue = false)]
    private byte _hpIv;
    public byte HpIv
    {
      get
      {
        return (byte)(31 - _hpIv);
      }
      set
      {
        if (HpIv != value)
        {
          if (PokemonValidator.ValidateIv(value))
            _hpIv = (byte)(31 - value);
          OnPropertyChanged("HpIv");
        }
      }
    }

    [DataMember(EmitDefaultValue = false)]
    private byte _atkIv;
    public byte AtkIv
    {
      get
      {
        return (byte)(31 - _atkIv);
      }
      set
      {
        if (_atkIv != value)
        {
          if (PokemonValidator.ValidateIv(value))
            _atkIv = (byte)(31 - value);
          OnPropertyChanged("AtkIv");
        }
      }
    }

    [DataMember(EmitDefaultValue = false)]
    private byte _defIv;
    public byte DefIv
    {
      get
      {
        return (byte)(31 - _defIv);
      }
      set
      {
        if (_defIv != value)
        {
          if (PokemonValidator.ValidateIv(value))
            _defIv = (byte)(31 - value);
          OnPropertyChanged("DefIv");
        }
      }
    }

    [DataMember(EmitDefaultValue = false)]
    private byte _speedIv;
    public byte SpeedIv
    {
      get
      {
        return (byte)(31 - _speedIv);
      }
      set
      {
        if (_speedIv != value)
        {
          if (PokemonValidator.ValidateIv(value))
            _speedIv = (byte)(31 - value);
          OnPropertyChanged("SpeedIv");
        }
      }
    }

    [DataMember(EmitDefaultValue = false)]
    private byte _spAtkIv;
    public byte SpAtkIv
    {
      get
      {
        return (byte)(31 - _spAtkIv);
      }
      set
      {
        if (_spAtkIv != value)
        {
          if (PokemonValidator.ValidateIv(value))
            _spAtkIv = (byte)(31 - value);
          OnPropertyChanged("SpAtkIv");
        }
      }
    }

    [DataMember(EmitDefaultValue = false)]
    private byte _spDefIv;
    public byte SpDefIv
    {
      get
      {
        return (byte)(31 - _spDefIv);
      }
      set
      {
        if (_spDefIv != value)
        {
          if (PokemonValidator.ValidateIv(value))
            _spDefIv = (byte)(31 - value);
          OnPropertyChanged("SpDefIv");
        }
      }
    }
    #endregion

    #region Ev
    private byte _hpEv;
    [DataMember(EmitDefaultValue = false)]
    public byte HpEv
    {
      get
      {
        return _hpEv;
      }
      set
      {
        if (_hpEv != value)
        {
          SetValueIfTrue(ref _hpEv, value, () => PokemonValidator.ValidateEv(this));
          OnPropertyChanged("HpEv");
        }
      }
    }

    private byte _atkEv;
    [DataMember(EmitDefaultValue = false)]
    public byte AtkEv
    {
      get
      {
        return _atkEv;
      }
      set
      {
        if (_atkEv != value)
        {
          SetValueIfTrue(ref _atkEv, value, () => PokemonValidator.ValidateEv(this));
          OnPropertyChanged("AtkEv");
        }
      }
    }

    private byte _defEv;
    [DataMember(EmitDefaultValue = false)]
    public byte DefEv
    {
      get
      {
        return _defEv;
      }
      set
      {
        if (_defEv != value)
        {
          SetValueIfTrue(ref _defEv, value, () => PokemonValidator.ValidateEv(this));
          OnPropertyChanged("DefEv");
        }
      }
    }

    private byte _speedEv;
    [DataMember(EmitDefaultValue = false)]
    public byte SpeedEv
    {
      get
      {
        return _speedEv;
      }
      set
      {
        if (_speedEv != value)
        {
          SetValueIfTrue(ref _speedEv, value, () => PokemonValidator.ValidateEv(this));
          OnPropertyChanged("SpeedEv");
        }
      }
    }

    private byte _spAtkEv;
    [DataMember(EmitDefaultValue = false)]
    public byte SpAtkEv
    {
      get
      {
        return _spAtkEv;
      }
      set
      {
        if (_spAtkEv != value)
        {
          SetValueIfTrue(ref _spAtkEv, value, () => PokemonValidator.ValidateEv(this));
          OnPropertyChanged("SpAtkEv");
        }
      }
    }

    private byte _spDefEv;
    [DataMember(EmitDefaultValue = false)]
    public byte SpDefEv
    {
      get
      {
        return _spDefEv;
      }
      set
      {
        if (_spDefEv != value)
        {
          SetValueIfTrue(ref _spDefEv, value, () => PokemonValidator.ValidateEv(this));
          OnPropertyChanged("SpDefEv");
        }
      }
    }
    #endregion

    [DataMember]
    private ObservableCollection<int> moveIds;
    /// <summary>
    /// 绑定可以使用索引器
    /// </summary>
    public IEnumerable<int> MoveIds
    { get { return moveIds; } }

    public PokemonCustomInfo(int typeId)
    {
      Lv = 100;
      moveIds = new ObservableCollection<int>();
      HpIv = AtkIv = DefIv = SpeedIv = SpAtkIv = SpDefIv = 31;
      HpEv = AtkEv = DefEv = SpeedEv = SpAtkEv = SpDefEv = 0;
      ChangeType(typeId);
    }

    public void ChangeType(int typeId)
    {
      if (PokemonTypeId != typeId)
        ChangeType(DataService.GetPokemonType(typeId));
    }
    public void ChangeType(PokemonType type)
    {
      if (PokemonTypeId != type.Id)
      {
        _pokemonTypeId = type.Id;
        _abilityIndex = 0;
        _gender = type.GetAvailableGenders().First();
        moveIds.Clear();
        OnPropertyChanged();
      }
    }

    public bool AddMove(int moveId)
    {
      if (moveIds.Count < 4 && !moveIds.Contains(moveId))
      {
        moveIds.Add(moveId);
        OnPropertyChanged("MoveIds");//对绑定没什么意义，主要是手动订阅
        return true;
      }
      return false;
    }
    public void RemoveMove(int moveId)
    {
      moveIds.Remove(moveId);
      OnPropertyChanged("MoveIds");//对绑定无意义，主要是手动订阅
    }

    public bool ValueEquals(PokemonCustomInfo pm)
    {
      if (moveIds.Count != pm.moveIds.Count)
        return false;
      for (int i = 0; i < moveIds.Count; i++)
      {
        if (moveIds[i] != pm.moveIds[i])
          return false;
      }
      return
          Name == pm.Name &&
          PokemonTypeId == pm.PokemonTypeId &&
          Lv == pm.Lv &&
          Gender == pm.Gender &&
          Nature == pm.Nature &&
          AbilityIndex == pm.AbilityIndex &&
          ItemId == pm.ItemId &&
          HpIv == pm.HpIv &&
          AtkIv == pm.AtkIv &&
          DefIv == pm.DefIv &&
          SpeedIv == pm.SpeedIv &&
          SpAtkIv == pm.SpAtkIv &&
          SpDefIv == pm.SpDefIv &&
          HpEv == pm.HpEv &&
          AtkEv == pm.AtkEv &&
          DefEv == pm.DefEv &&
          SpeedEv == pm.SpeedEv &&
          SpAtkEv == pm.SpAtkEv &&
          SpDefEv == pm.SpDefEv;
    }

    #region ICloneable
    public PokemonCustomInfo Clone()
    {
      var clone = MemberwiseClone() as PokemonCustomInfo;
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

    private static void SetValueIfTrue<T>(ref T target, T value, Func<bool> predicate) where T : struct
    {
      T copy = target;
      target = value;
      if (!predicate()) target = copy;
    }
  }
}
