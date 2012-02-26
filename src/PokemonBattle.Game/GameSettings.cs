using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using LightStudio.Tactic.Messaging;
using LightStudio.PokemonBattle.Data;

namespace LightStudio.PokemonBattle.Game
{
  [DataContract(Namespace = Namespaces.DEFAULT)]
  public class GameSettings : IMessagable
  {
    public static GameSettings ReadFromMessage(BinaryReader reader)
    {
      GameSettings s = new GameSettings((GameMode)reader.ReadByte());
      s.PPUp = reader.ReadDouble();
      int n = reader.ReadInt32();
      while (n-- > 0)
        s.AddRule(GameService.GetRule(reader.ReadInt32()));
      s.Lock();
      return s;
    }

    private readonly Tactic.DataModels.IdGenerator idGen;
    private Queue<int> idQue;
    [DataMember]
    private readonly List<Rule> rules;
    [DataMember]
    private GameMode mode;
    [DataMember]
    private Terrain terrain;
    [DataMember]
    private double ppUp;

    /// <summary>
    /// HostOnly
    /// </summary>
    /// <param name="mode"></param>
    /// <param name="ppUp"></param>
    /// <param name="terrain"></param>
    public GameSettings(GameMode mode, double ppUp = 1.6, Terrain terrain = Terrain.Path)
    {
      idGen = new Tactic.DataModels.IdGenerator();
      rules = new List<Rule>();
      this.mode = mode;
      this.ppUp = ppUp;
      this.terrain = terrain;
    }
    
    public bool IsLocked
    { get; private set; }
    public GameMode Mode
    {
      get { return mode; }
      set { if (!IsLocked) mode = value; }
    }
    public int XBound
    { 
      get
      {
        switch (Mode)
        {
          case GameMode.Single:
            return 1;
          default:
            System.Diagnostics.Debugger.Break();
            return 0;
        }
      }
    }
    public int TeamCount
    { get { return 2; } }
    public int PlayersPerTeam
    {
      get
      {
        switch (Mode)
        {
          case GameMode.Single:
            return 1;
          default:
            System.Diagnostics.Debugger.Break();
            return 0;
        }
      }
    }
    public Terrain Terrain
    {
      get { return terrain; }
      set { if (!IsLocked) terrain = value; }
    }
    public double PPUp
    {
      get { return ppUp; }
      set { if (!IsLocked) ppUp = value; }
    }
    public IEnumerable<Rule> Rules
    { get { return rules; } }

    public void Lock()
    {
      IsLocked = true;
    }
    public void AddRule(Rule rule)
    {
      rules.Add(rule);
    }
    public void SetIds(int[] ids)
    {
      idQue = new Queue<int>(ids);
    }
    public int NextId()
    {
      if (idGen != null) return idGen.NextId();
      return idQue.Dequeue();
    }

    public void WriteToMessage(BinaryWriter writer)
    {
      writer.Write((byte)Mode);
      writer.Write(PPUp);
      writer.Write(Rules.Count());
      foreach (Rule r in Rules)
        writer.Write(r.Id);
    }
  }
}
