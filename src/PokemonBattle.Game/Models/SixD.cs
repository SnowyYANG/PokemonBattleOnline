using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StatType = LightStudio.PokemonBattle.Data.StatType;

namespace LightStudio.PokemonBattle.Game
{
  public class ReadOnly6D
  {
    public readonly int Hp;
    public readonly int Atk;
    public readonly int Def;
    public readonly int SpAtk;
    public readonly int SpDef;
    public readonly int Speed;

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
  public class SixD
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
      Atk = values.Atk;
      Def = values.Def;
      SpAtk = values.SpAtk;
      SpDef = values.SpDef;
      Speed = values.Speed;
    }
  }
}
