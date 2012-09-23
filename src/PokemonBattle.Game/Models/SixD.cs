using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StatType = LightStudio.PokemonBattle.Data.StatType;

namespace LightStudio.PokemonBattle.Game
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
  public class ReadOnly6D : I6D
  {
    public int Hp { get; private set; }
    public int Atk { get; private set; }
    public int Def { get; private set; }
    public int SpAtk { get; private set; }
    public int SpDef { get; private set; }
    public int Speed { get; private set; }

    public ReadOnly6D()
    {
    }
    public ReadOnly6D(int h, int a, int d, int sa, int sd, int s)
    {
      Hp = h;
      Atk = a;
      Def = d;
      SpAtk = sa;
      SpDef = sd;
      Speed = s;
    }
    public ReadOnly6D(SixD values)
    {
      Hp = values.Hp;
      Atk = values.Atk;
      Def = values.Def;
      SpAtk = values.SpAtk;
      SpDef = values.SpDef;
      Speed = values.Speed;
    }
    public ReadOnly6D(ReadOnly6D values)
    {
      Hp = values.Hp;
      Atk = values.Atk;
      Def = values.Def;
      SpAtk = values.SpAtk;
      SpDef = values.SpDef;
      Speed = values.Speed;
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
  }
  
  /// <summary>
  /// 这东西存的是引用，所以不要在pm间直接传来传去，不用struct是为了性能及避免隐藏的错误
  /// </summary>
  public class SixD : I6D
  {
    public int Hp;
    public int Atk;
    public int Def;
    public int SpAtk;
    public int SpDef;
    public int Speed;

    public SixD()
    {
    }
    public SixD(int h, int a, int d, int sa, int sd, int s)
    {
      Hp = h;
      Atk = a;
      Def = d;
      SpAtk = sa;
      SpDef = sd;
      Speed = s;
    }
    public SixD(SixD values)
    {
      Set6D(values);
    }
    public SixD(ReadOnly6D values)
    {
      Hp = values.Hp;
      Atk = values.Atk;
      Def = values.Def;
      SpAtk = values.SpAtk;
      SpDef = values.SpDef;
      Speed = values.Speed;
    }

    int I6D.Hp
    { get { return Hp; } }
    int I6D.Atk
    { get { return Atk; } }
    int I6D.Def
    { get { return Def; } }
    int I6D.SpAtk
    { get { return SpAtk; } }
    int I6D.SpDef
    { get { return SpDef; } }
    int I6D.Speed
    { get { return Speed; } }

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

    public void Set6D(SixD values)
    {
      Hp = values.Hp;
      Set5D(values);
    }
    /// <summary>
    /// all but Hp
    /// </summary>
    public void Set5D(SixD values)
    {
      Set5D(values.Atk, values.Def, values.SpAtk, values.SpDef, values.Speed);
    }
    public void Set5D(int a, int d, int sa, int sd, int s)
    {
      Atk = a;
      Def = d;
      SpAtk = sa;
      SpDef = sd;
      Speed = s;
    }
  }
}
