﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using LightStudio.PokemonBattle.Data;
using LightStudio.PokemonBattle.Game;
using LightStudio.PokemonBattle.Interactive.GameEvents;

namespace LightStudio.PokemonBattle.Interactive
{
  [KnownType("KnownEvents")]
  [DataContract(Namespace = Namespaces.DEFAULT)]
  public class ReportFragment
  {
    static HashSet<Type> knownGameEvents = new HashSet<Type>() { typeof(AbilityEvent), typeof(BeginTurn), typeof(DeIllusion), typeof(HpChange), typeof(StateChange), typeof(MoveHurts), typeof(MultiPmEvent), typeof(PmEvent), typeof(SimpleEvent), typeof(Substitute), typeof(SendOut), typeof(UseMove), typeof(Withdraw) };
    static IEnumerable<Type> KnownEvents()
    {
      return knownGameEvents;
    }
    public static void AddEventType<T>() where T : GameEvent
    {
      knownGameEvents.Add(typeof(T));
    }
    
    [DataMember(EmitDefaultValue = false)]
    public readonly TeamOutward[] Teams;
    [DataMember(EmitDefaultValue = false)]
    public readonly Weather Weather;
    
    [DataMember(EmitDefaultValue = false)]
    private readonly Queue<GameEvent> events;
    [DataMember(EmitDefaultValue = false)]
    private readonly PokemonOutward[] pokemons; //onBoardOnly

    /// <summary>
    /// 为了节约流量，只在用户第一次进入房间的时候给出teams/pms/weather信息
    /// </summary>
    internal ReportFragment(TeamOutward[] teams, PokemonOutward[] pms, Weather weather)
    {
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

    /// <summary>
    /// Host使用
    /// </summary>
    /// <param name="e"></param>
    internal void AddEvent(GameEvent e)
    {
      events.Enqueue(e);
    }
  }
}
