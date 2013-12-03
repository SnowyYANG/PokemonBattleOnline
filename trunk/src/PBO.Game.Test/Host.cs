﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonBattleOnline.Game;
using PokemonBattleOnline.Game.Host;

namespace PokemonBattleOnline.Test
{
  class Host
  {
    public event Action GameEnd;
    public readonly GameSettings Settings;
    private readonly GameContext Game;
    private readonly InitingGame Init;
    private readonly Dictionary<int, TestClient> Clients;

    public Host()
    {
      Settings = new GameSettings();
      Clients = new Dictionary<int, TestClient>(2);
      //Game = GameFactory.CreateGame(Settings, Settings.NextId);
      //Game.ReportUpdated += InformReportUpdate;
    }

    public TestClient AddPlayer(IPokemonData[] pokemons)
    {
      //int tid = Clients.Count;
      //int pid = tid + 1;
      //if (Game.SetPlayer(tid, pid, pokemons))
      //{
      //  Player p = Game.GetPlayer(pid);
      //  int[] ids = new int[p.Pokemons.Count()];
      //  {
      //    int i = -1;
      //    foreach (SimPokemon pm in p.Pokemons) ids[++i] = pm.Id;
      //  }
      //  var c = new PlayerClient(this, pid, tid, pokemons, pid == 1, ids);
      //  if (pid == 1) c.GameEnd += GameEnd;
      //  Clients.Add(pid, c);
      //  return c;
      //}
      //return null;
      throw new NotImplementedException();
    }
    public bool StartGame()
    {
      //Game.Start();
      throw new NotImplementedException();
    }

    private void InformReportUpdate(ReportFragment fragment, IDictionary<int, InputRequest> requirements)
    {
      if (requirements != null)
      {
        foreach (var pair in requirements)
          Clients[pair.Key].InformRequireInput(pair.Value);
      }
      foreach (var c in Clients.Values) c.InformReportUpdate(fragment);
    }
    public bool Input(TestClient client, ActionInput input)
    {
      lock (this)
      {
        if (Game.InputAction(client.TeamId, 0, input))
        {
          Game.TryContinue();
          return true;
        }
        return false;
      }
    }
  }
}
