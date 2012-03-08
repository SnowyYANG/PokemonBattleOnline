using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using LightStudio.PokemonBattle.Data;
using LightStudio.PokemonBattle.Game;
using LightStudio.PokemonBattle.Interactive.GameEvents;

namespace LightStudio.PokemonBattle.Interactive
{
  [KnownType(typeof(BeginTurn))]
  [KnownType(typeof(SendOut))]
  [KnownType(typeof(Withdraw))]
  [DataContract(Namespace = Namespaces.DEFAULT)]
  public class ReportFragment
  {
    [DataMember(EmitDefaultValue = false)]
    public readonly TeamOutward[] Teams;
    [DataMember(EmitDefaultValue = false)]
    public readonly Weather Weather;
    
    [DataMember(EmitDefaultValue = false)]
    private readonly Queue<GameEvent> Events;
    [DataMember(EmitDefaultValue = false)]
    private readonly PokemonOutward[] pokemons; //onBoardOnly

    public bool NeedInput
    { get; set; }

    /// <summary>
    /// 为了节约流量，只在用户第一次进入房间的时候给出teams/pms/weather信息
    /// </summary>
    internal ReportFragment(TeamOutward[] teams, PokemonOutward[] pms, Weather weather)
    {
      Teams = teams;
      pokemons = pms;
      Weather = weather;
      Events = new Queue<GameEvent>();
    }
    /// <summary>
    /// 构建一个non-Leap的战报段
    /// </summary>
    /// <param name="events"></param>
    internal ReportFragment(ReportFragment fragment)
    {
      Events = fragment.Events;
    }

    public PokemonOutward this[int team, int x]
    {
      get
      {
        PokemonOutward value = null;
        foreach (PokemonOutward p in pokemons)
          if (p.Position.Team == team && p.Position.X == x)
          {
            value = p;
            break;
          }
        return value;
      }
    }

    /// <summary>
    /// Host使用
    /// </summary>
    /// <param name="e"></param>
    internal void AddEvent(GameEvent e)
    {
      Events.Enqueue(e);
    }
    private Action<GameEvent> nextEvent;
    private int eventUsers;
    public GameEvent BeginUseEvent()
    {
      eventUsers++;
      return Events.Peek();
    }
    public void EndUseEvent(Action<GameEvent> next)
    {
      nextEvent += next;
      eventUsers--;
      if (eventUsers == 0)
      {
        Events.Dequeue();
        if (nextEvent != null) nextEvent(Events.Peek()); //呼别暴栈
      }
    }
  }
}
