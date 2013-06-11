﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace LightStudio.PokemonBattle.PBO.UIElements
{
  public static class Controls
  {
    public static readonly DataTemplate LocalizedText;
    public static readonly Style XButton;
    public static readonly Style CommandMenu;
    public static readonly ItemsPanelTemplate WrapPanel;

    static Controls()
    {
      ResourceDictionary rd = Helper.GetDictionary("Controls", "LocalizedProperty");
      LocalizedText = (DataTemplate)rd["LocalizedText"];
      rd = Helper.GetDictionary("Controls", "XButton");
      XButton = (Style)rd["XButton"];
      rd = Helper.GetDictionary("Controls", "Controls");
      CommandMenu = (Style)rd["CommandMenu"];
      WrapPanel = (ItemsPanelTemplate)rd["WrapPanelTemplate"];
    }
  }
}