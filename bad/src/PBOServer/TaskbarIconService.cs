using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;
using System.Windows.Forms;

namespace PokemonBattleOnline.PBO.Server
{
  static class TaskbarIconService
  {
    private readonly static NotifyIcon NI;
    private static System.Windows.Window Window;

    static TaskbarIconService()
    {
      NI = new NotifyIcon();
      NI.Icon = new System.Drawing.Icon(typeof(TaskbarIconService), "server.ico");
      NI.Text = "PBOv0.8 Server";
      NI.Click += (sender, e) => Window.Show();
      NI.ContextMenu = new ContextMenu(new MenuItem[]
        {
          new MenuItem("退出", (sender, e) =>
            {
              if (System.Windows.MessageBox.Show("真的要退出吗？", "PBO Server", System.Windows.MessageBoxButton.YesNo) == System.Windows.MessageBoxResult.Yes)
              {
                NI.Visible = false;
                NI.Dispose();
                System.Windows.Application.Current.Shutdown();
              }
            })
        });
    }

    public static void Init(System.Windows.Window window)
    {
      Window = window;
      NI.Visible = true;
    }
  }
}
