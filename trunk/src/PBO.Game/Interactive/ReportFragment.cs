using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using PokemonBattleOnline.Game.GameEvents;

namespace PokemonBattleOnline.Game
{
  [KnownType("KnownEvents")]
  [DataContract(Namespace = PBOMarks.JSON)]
  public class ReportFragment
  {
    static Type[] knownGameEvents;
    static IEnumerable<Type> KnownEvents()
    {
      if (knownGameEvents == null) knownGameEvents = typeof(GameEvent).SubClasses().ToArray();
      return knownGameEvents;
    }

    [DataMember(Name = "b", EmitDefaultValue = false)]
    private int _turnNumber;
    public int TurnNumber
    { 
      get { return _turnNumber - 1; }
      private set { _turnNumber = value + 1; }
    }

    [DataMember(Name = "c_", EmitDefaultValue = false)]
    public readonly TeamOutward[] Teams;
    [DataMember(Name = "e", EmitDefaultValue = false)]
    public readonly Weather Weather;
    
    /// <summary>
    /// 为了节约流量，只在用户第一次进入房间的时候给出teams/pms/weather信息
    /// </summary>
    public ReportFragment(int turnNumber, TeamOutward[] teams, PokemonOutward[] pms, Weather weather)
    {
      TurnNumber = turnNumber;
      Teams = teams;
      pokemons = pms;
      Weather = weather;
      _events = new List<GameEvent>();
    }
    protected ReportFragment(ReportFragment fragment)
    {
      _turnNumber = fragment._turnNumber;
      Teams = fragment.Teams;
      Weather = fragment.Weather;
      pokemons = fragment.pokemons;
      _events = fragment._events;
    }
    private ReportFragment()
    {
    }

    [DataMember(Name = "d_", EmitDefaultValue = false)]
    private readonly PokemonOutward[] pokemons; //onBoardOnly
    public PokemonOutward this[int team, int x]
    {
      get
      {
        PokemonOutward value = null;
        if (pokemons != null)
          foreach (PokemonOutward p in pokemons)
            if (p.Position.Team == team && p.Position.X == x)
            {
              value = p;
              break;
            }
        return value;
      }
    }

    private List<GameEvent> _events;
    [DataMember(Name = "a_", EmitDefaultValue = false)]
    private GameEvent[] e_
    {
      get { return _events.ToArray(); }
      set { _events = new List<GameEvent>(value); }
    }
    public IEnumerable<GameEvent> Events
    { get { return _events; } }

    /// <summary>
    /// Host使用
    /// </summary>
    /// <param name="e"></param>
    public void AddEvent(GameEvent e)
    {
      _events.Add(e);
    }
    public ReportFragment NonLeap()
    {
      return new ReportFragment() { _events = this._events };
    }
  }
}
