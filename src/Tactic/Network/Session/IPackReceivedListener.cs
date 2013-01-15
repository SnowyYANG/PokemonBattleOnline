using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokemonBattleOnline.Tactic.Network
{
  public interface IPackReceivedListener
  {
    void OnPackReceived(byte[] pack);
  }
  internal class NullPackReceivedListener : IPackReceivedListener
  {
    public static readonly IPackReceivedListener I = new NullPackReceivedListener();
    
    private NullPackReceivedListener()
    {
    }

    void IPackReceivedListener.OnPackReceived(byte[] pack)
    {
    }
  }
}
