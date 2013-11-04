﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;
using System.Windows.Forms;
using Visibility = System.Windows.Visibility;

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
      NI.Click += (sender, e) => Window.Visibility = Window.Visibility == Visibility.Collapsed ? Visibility.Visible : Visibility.Collapsed;
      NI.ContextMenu = new ContextMenu(new MenuItem[] { new MenuItem("退出", (sender, e) => Window.Close()) });
    }

    public static void Init(System.Windows.Window window)
    {
      Window = window;
      NI.Visible = true;
    }
    public static void Close()
    {
      NI.Visible = false;
      NI.Dispose();
    }
  }
}