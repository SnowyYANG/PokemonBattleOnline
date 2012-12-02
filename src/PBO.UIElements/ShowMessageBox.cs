using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using LightStudio.PokemonBattle.Data;

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
    public static MessageBoxResult CantCloseMainWindow(Window window)
    {
      return MessageBox.Show(window, "您正在对战或观战，无法退出，请关闭所有对战/观战窗口后再退出主程序。", PBO);
    }
    public static MessageBoxResult ClosingInBattle(Window window)
    {
      //虽然我的目的是不干扰其他窗体，不过如果UI真的只有一个线程，会有效果么
      return MessageBox.Show(window, "现在退出将输掉这场对战，确定退出？", PBO, MessageBoxButton.YesNo, MessageBoxImage.None, MessageBoxResult.No);
    }
    public static MessageBoxResult GiveUpBattle(Window window)
    {
      return MessageBox.Show(window, "这将输掉这场对战，确定放弃？", PBO, MessageBoxButton.YesNo);
    }
    public static MessageBoxResult SaveLogFail()
    {
      return MessageBox.Show("存储战报失败！");
    }
    #endregion

    #region Editor
    public static MessageBoxResult PokemonUnsaved()
    {
      return Show("当前精灵的改动尚未保存，是否保存？", MessageBoxButton.YesNoCancel);
    }
    public static bool PokemonResetToLastSaved()
    {
      return Show("当前精灵已修改，确定放弃改动？", MessageBoxButton.YesNo) == MessageBoxResult.Yes;
    }
    public static void FolderExportFail(string reason)
    {
      MessageBox.Show(DataService.String["Export fail!"] + "\n" + reason);
    }
    public static void FolderImportFail(string reason)
    {
      MessageBox.Show(DataService.String["Import fail!"] + "\n" + reason);
    }
    #endregion
  }
}
