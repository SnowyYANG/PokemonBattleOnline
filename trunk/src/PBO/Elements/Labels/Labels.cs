﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace PokemonBattleOnline.PBO.Elements
{
  public static class Labels
  {
    public static readonly DataTemplate BattleType;
    public static readonly DataTemplate Ability;
    public static readonly DataTemplate Item;
    public static readonly DataTemplate MoveCategory;
    public static readonly DataTemplate PokemonState;
    public static readonly DataTemplate Gender;
    public static readonly DataTemplate GenderShowNone;
    public static readonly DataTemplate MultipliedValue;

    static Labels()
    {
      BattleType = GetDataTemplate("BattleType");
      Ability = GetDataTemplate("Ability");
      Item = GetDataTemplate("Item");
      MoveCategory = GetDataTemplate("MoveCategory");
      PokemonState = GetDataTemplate("PokemonState");
      MultipliedValue = GetDataTemplate("MultipliedValue");
      ResourceDictionary rd = GetDictionary("Gender");
      Gender = rd["GenderLabel"] as DataTemplate;
      GenderShowNone = rd["GenderLabelShowNone"] as DataTemplate;
    }

    static ResourceDictionary GetDictionary(string filename)
    {
      return Helper.GetDictionary("Elements/Labels", filename);
    }
    static DataTemplate GetDataTemplate(string filename)
    {
      return GetDictionary(filename)[filename + "Label"] as DataTemplate;
    }
  }
}
