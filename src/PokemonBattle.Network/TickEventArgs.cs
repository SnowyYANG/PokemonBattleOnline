using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightStudio.PokemonBattle.Messaging
{
  public class TickEventArgs : EventArgs
  {
    public bool Canceled
    { get; set; }

    /// <summary>
    /// the amount of remianing thinking time, in seconds
    /// </summary>
    public int RemainingTime
    { get; private set; }

    public TickEventArgs(int remainingTime)
    {
      this.RemainingTime = remainingTime;
    }
  }
}
