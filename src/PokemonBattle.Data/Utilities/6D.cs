using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace LightStudio.PokemonBattle.Data
{
  public interface I6D
  {
    int Hp { get; }
    int Atk { get; }
    int Def { get; }
    int SpAtk { get; }
    int SpDef { get; }
    int Speed { get; }
    int GetStat(StatType type);
  }

  [DataContract(Namespace = Namespaces.PBO)]
  public struct ReadOnly6D : I6D
  {
    public ReadOnly6D(int h, int a, int d, int sa, int sd, int s)
    {
      _hp = h;
      _atk = a;
      _def = d;
      _spAtk = sa;
      _spDef = sd;
      _speed = s;
    }
    public ReadOnly6D(I6D values)
    {
      _hp = values.Hp;
      _atk = values.Atk;
      _def = values.Def;
      _spAtk = values.SpAtk;
      _spDef = values.SpDef;
      _speed = values.Speed;
    }

    [DataMember(EmitDefaultValue = false)]
    private readonly int _hp;
    public int Hp
    { get { return _hp; } }
    [DataMember(EmitDefaultValue = false)]
    private readonly int _atk;
    public int Atk
    { get { return _atk; } }
    [DataMember(EmitDefaultValue = false)]
    private readonly int _def;
    public int Def
    { get { return _def; } }
    [DataMember(EmitDefaultValue = false)]
    private readonly int _spAtk;
    public int SpAtk
    { get { return _spAtk; } }
    [DataMember(EmitDefaultValue = false)]
    private readonly int _spDef;
    public int SpDef
    { get { return _spDef; } }
    [DataMember(EmitDefaultValue = false)]
    private readonly int _speed;
    public int Speed
    { get { return _speed; } }

    public int GetStat(StatType type)
    {
      int value;
      switch (type)
      {
        case StatType.Hp:
          value = Hp;
          break;
        case StatType.Atk:
          value = Atk;
          break;
        case StatType.Def:
          value = Def;
          break;
        case StatType.SpAtk:
          value = SpAtk;
          break;
        case StatType.SpDef:
          value = SpDef;
          break;
        case StatType.Speed:
          value = Speed;
          break;
        default:
          value = 0;
          break;
      }
      return value;
    }
  }

  [DataContract(Namespace = Namespaces.PBO)]
  public class Observable6D : I6D
  {
    private static readonly PropertyChangedEventArgs HP = new PropertyChangedEventArgs("Hp");
    private static readonly PropertyChangedEventArgs ATK = new PropertyChangedEventArgs("Atk");
    private static readonly PropertyChangedEventArgs DEF = new PropertyChangedEventArgs("Def");
    private static readonly PropertyChangedEventArgs SPATK = new PropertyChangedEventArgs("SpAtk");
    private static readonly PropertyChangedEventArgs SPDEF = new PropertyChangedEventArgs("SpDef");
    private static readonly PropertyChangedEventArgs SPEED = new PropertyChangedEventArgs("Speed");
    
    public event PropertyChangedEventHandler PropertyChanged;

    public Observable6D(I6D obj)
      : this(obj.Hp, obj.Atk, obj.Def, obj.SpAtk, obj.SpDef, obj.Speed)
    {
    }
    public Observable6D(int h = 0, int a = 0, int d = 0, int sa = 0, int sd = 0, int s = 0)
    {
      _hp = h;
      _atk = a;
      _def = d;
      _spAtk = sa;
      _spDef = sd;
      _speed = s;
    }

    [DataMember(EmitDefaultValue = false)]
    private int _hp;
    public int Hp
    {
      get { return _hp; }
      set
      {
        if (_hp != value)
        {
          _hp = value;
          SendPropertyChanged(HP);
        }
      }
    }
    [DataMember(EmitDefaultValue = false)]
    private int _atk;
    public int Atk
    {
      get { return _atk; }
      set
      {
        if (_atk != value)
        {
          _atk = value;
          SendPropertyChanged(ATK);
        }
      }
    }
    [DataMember(EmitDefaultValue = false)]
    private int _def;
    public int Def
    {
      get { return _def; }
      set
      {
        if (_def != value)
        {
          _def = value;
          SendPropertyChanged(DEF);
        }
      }
    }
    [DataMember(EmitDefaultValue = false)]
    private int _spAtk;
    public int SpAtk
    {
      get { return _spAtk; }
      set
      {
        if (_spAtk != value)
        {
          _spAtk = value;
          SendPropertyChanged(SPATK);
        }
      }
    }
    [DataMember(EmitDefaultValue = false)]
    private int _spDef;
    public int SpDef
    {
      get { return _spDef; }
      set
      {
        if (_spDef != value)
        {
          _spDef = value;
          SendPropertyChanged(SPDEF);
        }
      }
    }
    [DataMember(EmitDefaultValue = false)]
    private int _speed;
    public int Speed
    {
      get { return _speed; }
      set
      {
        if (_speed != value)
        {
          _speed = value;
          SendPropertyChanged(SPEED);
        }
      }
    }

    private void SendPropertyChanged(PropertyChangedEventArgs e)
    {
      if (PropertyChanged != null) PropertyChanged(this, e);
    }
    public int GetStat(StatType type)
    {
      int value;
      switch (type)
      {
        case StatType.Hp:
          value = Hp;
          break;
        case StatType.Atk:
          value = Atk;
          break;
        case StatType.Def:
          value = Def;
          break;
        case StatType.SpAtk:
          value = SpAtk;
          break;
        case StatType.SpDef:
          value = SpDef;
          break;
        case StatType.Speed:
          value = Speed;
          break;
        default:
          value = 0;
          break;
      }
      return value;
    }
    public void SetStat(StatType type, int value)
    {
      switch (type)
      {
        case StatType.Hp:
          Hp = value;
          break;
        case StatType.Atk:
          Atk = value;
          break;
        case StatType.Def:
          Def = value;
          break;
        case StatType.SpAtk:
          SpAtk = value;
          break;
        case StatType.SpDef:
          SpDef = value;
          break;
        case StatType.Speed:
          Speed = value;
          break;
      }
    }
  }
}
