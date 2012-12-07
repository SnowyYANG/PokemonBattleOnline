using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using LightStudio.Tactic.Messaging;
using LightStudio.PokemonBattle.Game;

namespace LightStudio.PokemonBattle.Messaging.Room
{
  [DataContract(Namespace = Namespaces.PBO)]
  public class GameInitSettings : IGameSettings, IMessagable
  {
    private bool isLocked;

    /// <summary>
    /// HostOnly
    /// </summary>
    /// <param name="mode"></param>
    /// <param name="terrain"></param>
    public GameInitSettings(GameMode mode, Terrain terrain = Terrain.Path, bool sleepRule = true)
    {
      Mode = mode;
      Terrain = terrain;
      SleepRule = sleepRule;
    }

    [DataMember(EmitDefaultValue = false)]
    private GameMode _mode;
    public GameMode Mode
    {
      get { return _mode; }
      set { if (!isLocked) _mode = value; }
    }
    [DataMember(EmitDefaultValue = false)]
    private Terrain _terrain;
    public Terrain Terrain
    {
      get { return _terrain; }
      set { if (!isLocked) _terrain = value; }
    }
    [DataMember(EmitDefaultValue = false)]
    private bool _noSR;
    public bool SleepRule
    {
      get { return !_noSR; }
      set { if (!isLocked) _noSR = !value; }
    }

    public void Lock()
    {
      lock (this)
      {
        isLocked = true;
      }
    }
  }
}
