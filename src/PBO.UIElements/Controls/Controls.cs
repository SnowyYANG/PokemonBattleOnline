using System;
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
    public static readonly DataTemplate LocalizedDataText;
    public static readonly Style XButton;
    public static readonly Style CommandMenu;
    public static readonly ItemsPanelTemplate WrapPanel;

    static Controls()
    {
      ResourceDictionary rd = Helper.GetDictionary("Controls", "LocalizedResource");
      LocalizedText = (DataTemplate)rd["LocalizedText"];
      LocalizedDataText = (DataTemplate)rd["LocalizedDataText"];
      rd = Helper.GetDictionary("Controls", "XButton");
      XButton = (Style)rd["XButton"];
      rd = Helper.GetDictionary("Controls", "Controls");
      CommandMenu = (Style)rd["CommandMenu"];
      WrapPanel = (ItemsPanelTemplate)rd["WrapPanelTemplate"];
    }
  }
}
