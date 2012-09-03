using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using LightStudio.PokemonBattle.Data;

namespace LightStudio.PokemonBattle.Game.Host
{
  /// <summary>
  /// 在场pm数据副本，不一定正在对战，比如“转盘”
  /// </summary>
  public class OnboardPokemon : ConditionalObject
  {
    public static int Get5D(int val, int lv)
    {
      int denominator = 2, numerator = 2;
      if (lv > 0) numerator += lv;
      else denominator -= lv;
      return val * numerator / denominator;
    }
    
    #region Data
    public int X;
    public CoordY CoordY;
    public PokemonType PokemonType;
    private BattleType _type1;
    public BattleType Type1
    {
      get { return _type1 == BattleType.Flying && HasCondition("Roost") ? BattleType.Normal : _type1; }
      set
      {
        _type1 = value;
        RemoveCondition("Roost");
      }
    }
    private BattleType _type2;
    public BattleType Type2
    {
      get { return _type2 == BattleType.Flying && HasCondition("Roost") ? BattleType.Invalid : _type2; }
      set
      {
        _type2 = value;
        RemoveCondition("Roost");
      }
    }
    public PokemonGender Gender;
    public int Ability; //特性交换用，不可为0，未必是有效的特性
    public readonly SixD Static; //力量交换，包含性格修正，不包含等级修正
    private readonly SixD lv5D;
    public I6D Lv5D
    { get { return lv5D; } }
    private int _accuracyLv;
    public int AccuracyLv
    { 
      get { return _accuracyLv; }
      set
      {
        if (value < -6) value = -6;
        if (value > 6) value = 6;
        _accuracyLv = value;
      }
    }
    private int _evasionLv;
    public int EvasionLv
    { 
      get { return _evasionLv; }
      set
      {
        if (value < -6) value = -6;
        if (value > 6) value = 6;
        _evasionLv = value;
      }
    }
    private double _weight;
    public double Weight
    {
      get { return _weight; }
      set
      {
        if (value < 0.1) _weight = 0.1;
        else _weight = value;
      }
    }
    #endregion

    internal OnboardPokemon(Pokemon pokemon, int x)
    {
      PokemonType = pokemon.PokemonType;
      _type1 = pokemon.PokemonType.Type1;
      _type2 = pokemon.PokemonType.Type2;
      Gender = pokemon.Gender;
      Ability = pokemon.Ability.Id;
      Static = new SixD(pokemon.Static);
      _weight = pokemon.PokemonType.Weight;
      lv5D = new SixD();

      X = x; //CoordY 默认值
    }

    private void ChangeLv7D(ref int lv, int change)
    {
      lv += change;
      if (lv > 6) lv = 6;
      else if (lv < -6) lv = -6;
    }
    public void ChangeLv7D(StatType stat, int change)
    {
      switch (stat)
      {
        case StatType.Accuracy:
          ChangeLv7D(ref _accuracyLv, change);
          break;
        case StatType.Evasion:
          ChangeLv7D(ref _evasionLv, change);
          break;
        case StatType.Atk:
          ChangeLv7D(ref lv5D.Atk, change);
          break;
        case StatType.Def:
          ChangeLv7D(ref lv5D.Def, change);
          break;
        case StatType.SpAtk:
          ChangeLv7D(ref lv5D.SpAtk, change);
          break;
        case StatType.SpDef:
          ChangeLv7D(ref lv5D.SpDef, change);
          break;
        case StatType.Speed:
          ChangeLv7D(ref lv5D.Speed, change);
          break;
#if DEBUG
        default:
          System.Diagnostics.Debugger.Break();
          break;
#endif
      }
    }
    private int FilterLv7D(int? lv, int formerLv)
    {
      if (lv.HasValue)
      {
        int l = lv.Value;
        if (l > 6) l = 6;
        else if (l < -6) l = -6;
        return l;
      }
      return formerLv;
    }
    public void SetLv7D(int? a, int? d, int? sa, int? sd, int? s, int? accuracy, int? evasion)
    {
      if (accuracy != null) AccuracyLv = accuracy.Value;
      if (evasion != null) EvasionLv = evasion.Value;
      lv5D.Set5D(
        FilterLv7D(a, lv5D.Atk),
        FilterLv7D(d, lv5D.Def),
        FilterLv7D(sa, lv5D.SpAtk),
        FilterLv7D(sd, lv5D.SpDef),
        FilterLv7D(s, lv5D.Speed));
    }
    public bool HasType(BattleType type)
    {
      return Type1 == type || Type2 == type;
    }
  }
}
