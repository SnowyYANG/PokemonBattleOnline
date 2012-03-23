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

    private readonly IdGenerator idGen;
    private Queue<int> idQue;
    private List<Rule> rules;
    [DataMember]
    private List<int> ruleIds;
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
      idGen = new IdGenerator();
      rules = new List<Rule>();
      ruleIds = new List<int>();
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
    public IEnumerable<Rule> ChosenRules
    { 
      get
      {
        if (rules == null)
        {
          rules = new List<Rule>();
          foreach (int i in ruleIds) rules.Add(GameService.GetRule(i));
        }
        return rules;
      }
    }
    public IRule Rule
    { get; private set; }

    public void Lock()
    {
      IsLocked = true;
      Rule = new CombinedRule(ruleIds);
    }
    public void AddRule(Rule rule)
    {
      ruleIds.Add(rule.Id);
      rules.Add(rule);
    }
    public void SetIds(int[] ids)
    {
      if (IsLocked) return;
      idQue = new Queue<int>(ids);
      Lock();
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
      writer.Write(ChosenRules.Count());
      foreach (Rule r in ChosenRules)
        writer.Write(r.Id);
    }
  }
}
