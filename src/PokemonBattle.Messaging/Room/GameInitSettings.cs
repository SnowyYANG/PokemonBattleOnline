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
    private readonly IdGenerator idGen;
    private Queue<int> idQue;

    /// <summary>
    /// HostOnly
    /// </summary>
    /// <param name="mode"></param>
    /// <param name="ppUp"></param>
    /// <param name="terrain"></param>
    public GameInitSettings(GameMode mode, Terrain terrain = Terrain.Path, bool sleepRule = true)
    {
      idGen = new IdGenerator();
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

    internal int NextId()
    {
      if (idGen != null) return idGen.NextId();
      return idQue.Dequeue();
    }
    public void Lock()
    {
      lock (this)
      {
        isLocked = true;
      }
    }
    internal void SetIds(int[] ids)
    {
      if (isLocked) return;
      idQue = new Queue<int>(ids);
      Lock();
    }
  }
}
