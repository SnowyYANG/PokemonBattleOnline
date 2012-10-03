﻿using System;
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
  public class PokemonCustomInfo : ICloneable, INotifyPropertyChanged
  {
    [DataMember]
    private short number;
    [DataMember(EmitDefaultValue = false)]
    private byte forme;
    [DataMember]
    private ObservableCollection<int> moveIds;

    public PokemonCustomInfo(int number, int forme)
    {
      moveIds = new ObservableCollection<int>();
      Ev = new Observable6D();
      Forme = DataService.GetPokemon(number, forme);
    }

    #region properties
    [DataMember(EmitDefaultValue = false)]
    private string _name;
    public string Name
    {
      get { return _name ?? Forme.Type.GetLocalizedName(); }
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

    private PokemonForme _forme;
    public PokemonForme Forme
    { 
      get
      {
        if (_forme == null) _forme = DataService.GetPokemon(number, forme);
        return _forme;
      }
      set
      {
        if (Forme != value && value != null)
        {
          _forme = value;
          number = _forme.Type.Number;
          forme = (byte)_forme.Index;
          _abilityIndex = 0;
          _gender = Forme.Type.GetAvailableGenders().First();
          moveIds.Clear();
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
        if (_gender != value)
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
    private int _abilityIndex;
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

    public Ability Ability
    { get { return Forme.Data.GetAbility(AbilityIndex); } }

    private Observable6D _iv;
    public Observable6D Iv
    {
      get
      {
        if (_iv == null) _iv = new Observable6D(31, 31, 31, 31, 31, 31);
        return _iv;
      }
      private set { _iv = value; }
    }
    
    [DataMember(EmitDefaultValue = false)]
    private short _itemId;
    public int ItemId
    {
      get { return _itemId; }
      set
      {
        if (_itemId != value)
        {
          _itemId = (short)value;
          OnPropertyChanged("ItemId");
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
    public Observable6D Ev
    { get; private set; }
    
    public IEnumerable<int> MoveIds
    { get { return moveIds; } }

    #region datacontract only
    [DataMember(EmitDefaultValue = false)]
    private ReadOnly6D _Iv
    {
      get { return new ReadOnly6D(31 - Iv.Hp, 31 - Iv.Atk, 31 - Iv.SpAtk, 31 - Iv.Def, 31 - Iv.SpDef, 31 - Iv.Speed); }
      set { Iv = new Observable6D(31 - value.Hp, 31 - value.Atk, 31 - value.Def, 31 - value.SpAtk, 31 - value.SpDef, 31 - value.Speed); }
    }
    #endregion
    #endregion

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
  }
}
