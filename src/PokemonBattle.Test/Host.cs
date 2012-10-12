using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.Tactic;
using LightStudio.Tactic.Serialization;
using LightStudio.PokemonBattle.Data;
using LightStudio.PokemonBattle.Game;
using LightStudio.PokemonBattle.Game.Host;

namespace LightStudio.PokemonBattle.Test
{
  class Host
  {
    public event Action GameEnd;
    public readonly GameSettings Settings;
    private readonly IGame Game;
    private readonly Dictionary<int, PlayerClient> Clients;

    public Host()
    {
      Settings = new GameSettings();
      Clients = new Dictionary<int, PlayerClient>(2);
      Game = GameFactory.CreateGame(Settings, Settings.NextId);
      Game.ReportUpdated += InformReportUpdate;
    }

    public ITestClient AddPlayer(IPokemonData[] pokemons)
    {
      int tid = Clients.Count;
      int pid = tid + 1;
      if (Game.SetPlayer(tid, pid, pokemons))
      {
        Player p = Game.GetPlayer(pid);
        int[] ids = new int[p.Pokemons.Count()];
        {
          int i = -1;
          foreach (Pokemon pm in p.Pokemons) ids[++i] = pm.Id;
        }
        var c = new PlayerClient(this, pid, tid, pokemons, pid == 1, ids);
        if (pid == 1) c.GameEnd += GameEnd;
        Clients.Add(pid, c);
        return c;
      }
      return null;
    }
    public bool StartGame()
    {
      return Game.Start();
    }

    private void InformReportUpdate(ReportFragment fragment, IDictionary<int, InputRequest> requirements)
    {
      if (requirements != null)
      {
        foreach (var pair in requirements)
          Clients[pair.Key].InformRequireInput(Serializer.DeserializeFromString<InputRequest>(Serializer.SerializeToString(pair.Value)));
      }
      foreach (var c in Clients.Values) c.InformReportUpdate(fragment);
    }
    public bool Input(PlayerClient client, ActionInput input)
    {
      lock (this)
      {
        if (Game.InputAction(client.PlayerId, input))
        {
          Game.TryContinue();
          return true;
        }
        return false;
      }
    }
  }
}
