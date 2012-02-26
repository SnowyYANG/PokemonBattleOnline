using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Data;
using LightStudio.PokemonBattle.Interactive;

namespace LightStudio.PokemonBattle.Game
{
  /// <summary>
  /// 这必须是单线程的...
  /// </summary>
  internal class GameContext : IGame
  {
    private static void GameLoop(GameContext game)
    {
      ActionInput[] actions = new ActionInput[game.Settings.PlayersPerTeam * game.Settings.TeamCount];

    }

    public event Action<int, int> GameEnd;
    public event Action<Turn> Turn;
    public event Action<Player> RequireInput;

    public readonly Board Board;
    public readonly Team[] Teams;

    public GameContext(GameSettings settings)
    {
      Settings = settings;
      Teams = new Team[settings.TeamCount];
      for (int i = 0; i < settings.TeamCount; i++)
        Teams[i] = new Team(i, settings);
      Board = new Board(settings);
    }

    public GameSettings Settings
    { get; private set; }

    private void CheckForNextTurn()//考虑调整到IController
    {
      
      RequireInput(Teams[0].Players[0]);
      RequireInput(Teams[1].Players[0]);
    }
    private void OnGameEnd()
    {
      if (GameEnd != null)
        GameEnd(Teams[0].Pokemons.Values.Count((pm) => pm.Hp.Value > 0), Teams[1].Pokemons.Values.Count((pm) => pm.Hp.Value > 0));
    }
    public Pokemon GetPokemon(int id)
    {
      foreach (Team t in Teams)
        foreach (Pokemon pm in t.Pokemons.Values)
          if (pm.Id == id) return pm;
      return null;
    }
    public Player GetPlayer(int id)
    {
      foreach (Team t in Teams)
        foreach (Player p in t.Players)
          if (p.Id == id) return p;
      return null;
    }

    #region IGame Only
    bool IGame.Prepared
    {
      get
      {
        foreach (Team t in Teams)
          if (!t.Prepared) return false;
        return true;
      }
    }
    bool IGame.SetPlayer(int teamId, int userId, PokemonCustomInfo[] pokemons)
    {
      //TODO: Verify
      return Teams[teamId].AddPlayer(userId, pokemons);
    }
    bool IGame.Start() //考虑让IController实现
    {
      if (((IGame)this).Prepared)
      {
        switch (Settings.Mode)
        {
          case GameMode.Single:
            turnBuilder.NewTurn();
            Board[0, 0] = new OnboardPokemon(Teams[0].Players[0].Pokemons[0], 0);
            turnBuilder.AddSendout(Teams[0].Players[0].Id, Board[0, 0].Id);
            Board[1, 0] = new OnboardPokemon(Teams[1].Players[0].Pokemons[0], 0);
            turnBuilder.AddSendout(Teams[1].Players[0].Id, Board[1, 0].Id);
            Turn(turnBuilder.GetLeapTurn()); //第0回合已经结束了
            CheckForNextTurn();
            break;
          default:
            return false;
        }
        return true;
      }
      return false;
    }
    bool IGame.InputAction(int playerId, ActionInput action)
    {
      return ActionInput.InputAction(this, GetPlayer(playerId), action);
    }
    Turn IGame.GetLastLeapTurn() // for spectator
    {
      return turnBuilder.GetLeapTurn(); //is null possible?
    }
    #endregion
  }
}
