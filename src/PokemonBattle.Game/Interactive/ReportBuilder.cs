﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Game;
using LightStudio.PokemonBattle.Data;
using LightStudio.PokemonBattle.Interactive.GameEvents;

namespace LightStudio.PokemonBattle.Interactive
{
  public class ReportBuilder
  {
    private readonly Controller Controller;
    private ReportFragment lastLeapFragment;
    private ReportFragment lastFragment;
    private ReportFragment current;

    internal ReportBuilder(Controller controller)
    {
      Controller = controller;
      TurnNumber = -1;
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
        foreach (PokemonProxy p in Controller.OnboardPokemons) pms.Add(p.GetOutward());
        current = new ReportFragment(t, pms.ToArray(), Controller.Board.Weather);
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
      Add(new BeginTurn(++TurnNumber));
    }

    public void Add(GameEvent e)
    {
      current.AddEvent(e);
    }
    public void Add(string key, PokemonProxy pm, params string[] data)
    {
      Add(new PmEvent(key, pm, data));
    }
    public void Add(string key, params string[] data)
    {
      Add(new SimpleEvent(key, data));
    }
  }
}
