﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Game.GameEvents;
using LightStudio.PokemonBattle.Data;

namespace LightStudio.PokemonBattle.Game.Host
{
  public class ReportBuilder
  {
    private readonly Controller Controller;
    private readonly DateTime Begin;
    private ReportFragment lastLeapFragment;
    private ReportFragment lastFragment;
    private ReportFragment current;

    internal ReportBuilder(Controller controller)
    {
      Controller = controller;
      TurnNumber = -1;
      Begin = DateTime.Now;
    }

    public int TurnNumber
    { get; private set; }

    internal void NewFragment()
    {
      if (current != null)
      {
        lastLeapFragment = current;
        if (lastFragment == null) lastFragment = lastLeapFragment;
        else lastFragment = new ReportFragment(lastLeapFragment);
      }

      TeamOutward[] t = new TeamOutward[Controller.Board.TeamCount];
      for (int i = 0; i < t.Length; i++) t[i] = Controller.Game.Teams[i].GetOutward();
      List<PokemonOutward> pms = new List<PokemonOutward>();
      {
        foreach (PokemonProxy p in Controller.ActingPokemons) pms.Add(p.GetOutward());
        current = new ReportFragment(Controller.TurnNumber, t, pms.ToArray(), Controller.Board.Weather);
        var s = (int)((DateTime.Now - Begin).TotalSeconds);
        if (s != 0) Add(new TimeTick(s));
      }
    }
    internal ReportFragment GetFragment()
    {
      return lastFragment;
    }
    internal ReportFragment GetLeapFragment()
    {
      return lastLeapFragment;
    }
    internal void NewTurn()
    {
      ++TurnNumber;
      Add(new BeginTurn());
    }

    public void Add(GameEvent e)
    {
      current.AddEvent(e);
    }
    public void Add(string key, object arg0 = null, object arg1 = null, object arg2 = null)
    {
      Add(new SimpleEvent(key, arg0, arg1, arg2));
    }
    public void AddHorizontalLine()
    {
      if (!(current.LastEvent is HorizontalLine)) current.AddEvent(new HorizontalLine());
    }
  }
}
