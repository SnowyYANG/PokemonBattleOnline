using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PokemonBattleOnline.PBO.Server
{
  static class TaskbarIconService
  {
    [System.Runtime.InteropServices.DllImport("user32.dll")]
    private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

    private readonly static NotifyIcon NI;
    private readonly static IntPtr hWnd;

    static TaskbarIconService()
    {
      NI = new NotifyIcon();
      NI.Icon = new System.Drawing.Icon(typeof(TaskbarIconService), "server.ico");
      NI.Text = "PBOv0.8 Server";
      NI.ContextMenu = new ContextMenu(new MenuItem[] { new MenuItem("退出", Quit_Click) });
      hWnd = System.Diagnostics.Process.GetCurrentProcess().MainWindowHandle;
    }


    private static int sw;
    private static void NI_Click(object sender, EventArgs e)
    {
      ShowWindow(hWnd, sw); // 0 = SW_HIDE
      sw = 1 - sw;
    }
    private static void Quit_Click(object sender, EventArgs e)
    {
      //Hide the window
      ShowWindow(hWnd, sw); // 0 = SW_HIDE
    }

    public static void Init()
    {
      if (hWnd != IntPtr.Zero) NI.Click += NI_Click;
      NI.Visible = true;
      Application.Run();
    }

    public static void Close()
    {
      NI.Visible = false;
      NI.Dispose();
    }
  }
}
