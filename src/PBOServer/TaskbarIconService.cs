using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LightStudio.PokemonBattle.PBO.Server
{
  static class TaskbarIconService
  {
    private readonly static NotifyIcon NI;

    static TaskbarIconService()
    {
      NI = new NotifyIcon();
      //NI.Icon = new System.Drawing.Icon(AppDomain.CurrentDomain.
      NI.Text = "PBOv0.8 Server";
    }
    
    public static void Show()
    {
      NI.Visible = true;
    }
  }
}
