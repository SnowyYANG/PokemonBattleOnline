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
  public interface IGameSettings
  {
    GameMode Mode { get; }
    Terrain Terrain { get; }
    double PPUp { get; }
    IRule Rule { get; }
  }
  
  [DataContract(Namespace = Namespaces.DEFAULT)]
  public class GameSettings : IGameSettings, IMessagable
  {
    private bool isLocked;
    private readonly IdGenerator idGen;
    private Queue<int> idQue;
    private List<Rule> rules;
    private IRule combinedRule;
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

    public GameMode Mode
    {
      get { return mode; }
      set { if (!isLocked) mode = value; }
    }
    public Terrain Terrain
    {
      get { return terrain; }
      set { if (!isLocked) terrain = value; }
    }
    public double PPUp
    {
      get { return ppUp; }
      set { if (!isLocked) ppUp = value; }
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
    IRule IGameSettings.Rule
    { get { return combinedRule; } }

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
        combinedRule = new CombinedRule(ruleIds);
      }
    }
    public void AddRule(Rule rule)
    {
      ruleIds.Add(rule.Id);
      rules.Add(rule);
    }
    public void SetIds(int[] ids)
    {
      if (isLocked) return;
      idQue = new Queue<int>(ids);
      Lock();
    }
  }
}
