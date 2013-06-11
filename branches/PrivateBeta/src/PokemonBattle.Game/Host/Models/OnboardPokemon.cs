﻿using System;
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
    private readonly Pokemon Pokemon;
    public int X;
    public CoordY CoordY;
    public PokemonForm Form;
    public PokemonGender Gender;
    public int Ability; //特性交换用，不可为0，未必是有效的特性
    public readonly Simple6D FiveD; //力量交换，包含性格修正，不包含等级修正
    private readonly Simple6D lv5D;

    internal OnboardPokemon(Pokemon pokemon, int x)
    {
      Pokemon = pokemon;
      FiveD = new Simple6D();
      ChangeForm(pokemon.Form);
      Gender = pokemon.Gender;
      lv5D = new Simple6D();
      X = x; //CoordY 默认值
    }

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

    public void ChangeLv7D(StatType stat, int change)
    {
      switch (stat)
      {
        case StatType.Accuracy:
          AccuracyLv += change;
          break;
        case StatType.Evasion:
          EvasionLv += change;
          break;
        default:
          var value = lv5D.GetStat(stat);
          value += change;
          if (value > 6) value = 6;
          else if (value < -6) value = -6;
          lv5D.SetStat(stat, value);
          break;
      }
    }
    public void SetLv7D(StatType stat, int lv)
    {
      if (stat == StatType.Accuracy) AccuracyLv = lv;
      else if (stat == StatType.Evasion) EvasionLv = lv;
      else
      {
        if (lv > 6) lv = 6;
        else if (lv < -6) lv = -6;
        lv5D.SetStat(stat, lv);
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
      return 
        type == BattleType.Flying || type == BattleType.Normal ?
        Type1 == type || Type2 == type : _type1 == type || _type2 == type;
    }
    
    private int Get5D(StatType type)
    {
      return PokemonStatHelper.Get5D(type, Pokemon.Nature, Form.Data.Base.GetStat(type), (byte)Pokemon.Iv.GetStat(type), (byte)Pokemon.Ev.GetStat(type), (byte)Pokemon.Lv);
    }
    public void ChangeForm(PokemonForm form)
    {
      Form = form;
      _weight = form.Type.Weight;
      _type1 = form.Type1;
      _type2 = form.Type2;
      Ability = form.Data.GetAbility(Pokemon.AbilityIndex).Id;
      FiveD.Atk = Get5D(StatType.Atk);
      FiveD.SpAtk = Get5D(StatType.SpAtk);
      FiveD.Speed = Get5D(StatType.Speed);
      var d = Get5D(StatType.Def);
      var sd = Get5D(StatType.SpDef);
      if (HasCondition("WonderRoom"))
      {
        FiveD.Def = sd;
        FiveD.SpDef = d;
      }
      else
      {
        FiveD.Def = d;
        FiveD.SpDef = sd;
      }
    }
    public void Transform(OnboardPokemon op)
    {
      //形态（包括种族值）、能力值、能力等级、属性、特性、技能等变为与对方怪兽一样。
      Form = op.Form;
      FiveD.Set5D(op.FiveD);
      lv5D.Set5D(op.lv5D);
      Gender = op.Gender;
      _accuracyLv = op._accuracyLv;
      _evasionLv = op._evasionLv;
      _type1 = op._type1;//无视羽休
      _type2 = op._type2;
      Ability = op.Ability;
      _weight = op.Weight;
    }
  }
}