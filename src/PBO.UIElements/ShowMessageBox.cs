using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using DataService = LightStudio.PokemonBattle.Data.DataService;

namespace LightStudio.PokemonBattle.PBO.UIElements
{
  public static class ShowMessageBox
  {
    private const string PBO = "PBO";
    private static MessageBoxResult Show(string message, MessageBoxButton button)
    {
      return MessageBox.Show(message, PBO, button);
    }

    #region Lobby
    public static MessageBoxResult ExitLobby()
    {
      return Show("退出大厅？", MessageBoxButton.YesNo);
    }
    #endregion

    #region Battle
    public static MessageBoxResult ClosingInBattle(Window window)
    {
      //虽然我的目的是不干扰其他窗体，不过如果UI真的只有一个线程，会有效果么
      return MessageBox.Show(window, "现在退出将输掉这场对战，确定退出？", PBO, MessageBoxButton.YesNo, MessageBoxImage.None, MessageBoxResult.No);
    }
    public static MessageBoxResult GiveUpBattle(Window window)
    {
      return MessageBox.Show(window, "这将输掉这场对战，确定放弃？", PBO, MessageBoxButton.YesNo);
    }
    #endregion

    #region Editor
    public static MessageBoxResult PokemonUnsaved()
    {
      return Show("当前精灵的改动尚未保存，是否保存？", MessageBoxButton.YesNoCancel);
    }
    public static void FolderExportFail()
    {
      MessageBox.Show(DataService.String["FolderExportFail"]);
    }
    public static void FolderImportFail()
    {
      MessageBox.Show(DataService.String["FolderImportFail"]);
    }
    #endregion
  }
}
