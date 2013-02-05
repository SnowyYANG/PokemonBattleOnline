﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using PokemonBattleOnline.Data;
using PokemonBattleOnline.Game.GameEvents;

namespace PokemonBattleOnline.Game
{
  [KnownType("KnownEvents")]
  [DataContract(Namespace = PBOMarks.JSON)]
  public class ReportFragment
  {
    static HashSet<Type> knownGameEvents = new HashSet<Type>() { typeof(AbilityEvent), typeof(BeginTurn), typeof(EndTurn), typeof(GameStartSendOut), typeof(GetItem), typeof(HLLD), typeof(HorizontalLine), typeof(HpChange), typeof(MoveHurt), typeof(OutwardChange), typeof(PPChange), typeof(PositionChange), typeof(SelectMoveFail), typeof(SendOut), typeof(SimpleEvent), typeof(StateChange), typeof(Substitute), typeof(RemoveItem), typeof(UseMove), typeof(WeatherChange), typeof(Withdraw) };
    static IEnumerable<Type> KnownEvents()
    {
      return knownGameEvents;
    }
    /// <summary>
    /// For effects, use in register
    /// </summary>
    internal static void AddEventType(Type type)
    {
      knownGameEvents.Add(type);
    }

    [DataMember(Name = "e", EmitDefaultValue = false)]
    private int _turnNumber;
    public int TurnNumber
    { 
      get { return _turnNumber - 1; }
      private set { _turnNumber = value + 1; }
    }

    [DataMember(Name = "c", EmitDefaultValue = false)]
    public readonly TeamOutward[] Teams;
    [DataMember(Name = "d", EmitDefaultValue = false)]
    public readonly Weather Weather;

    [DataMember(Name = "a0", EmitDefaultValue = false)] //json deserialization bug
    private GameEvent[] _events
    {
      get { return events.ToArray(); }
      set { events = new Queue<GameEvent>(value); }
    }
    private Queue<GameEvent> events;
    [DataMember(Name = "b", EmitDefaultValue = false)]
    private readonly PokemonOutward[] pokemons; //onBoardOnly

    /// <summary>
    /// 为了节约流量，只在用户第一次进入房间的时候给出teams/pms/weather信息
    /// </summary>
    public ReportFragment(int turnNumber, TeamOutward[] teams, PokemonOutward[] pms, Weather weather)
    {
      TurnNumber = turnNumber;
      Teams = teams;
      pokemons = pms;
      Weather = weather;
      events = new Queue<GameEvent>();
    }
    /// <summary>
    /// 构建一个non-Leap的战报段
    /// </summary>
    /// <param name="events"></param>
    internal ReportFragment(ReportFragment fragment)
    {
      events = fragment.events;
    }

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
    public IEnumerable<GameEvent> Events
    { get { return events; } }
    public GameEvent LastEvent
    { get; private set; }

    /// <summary>
    /// Host使用
    /// </summary>
    /// <param name="e"></param>
    public void AddEvent(GameEvent e)
    {
      LastEvent = e;
      events.Enqueue(e);
    }
  }
}
