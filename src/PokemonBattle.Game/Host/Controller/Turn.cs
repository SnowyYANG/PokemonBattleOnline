﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Game.Host.Sp;

namespace LightStudio.PokemonBattle.Game.Host
{
  internal class TurnController : ControllerComponent
  {
    private readonly Comparer comparer;
    private readonly Tile[] tiles;
    private byte current;

    public TurnController(Controller controller)
      : base(controller)
    {
      comparer = new Comparer(Game.Board);
      Tiles = tiles = Board.Tiles.ToArray(); //this is a copy
      ActingPokemons = new List<PokemonProxy>();
    }

    public List<PokemonProxy> ActingPokemons
    { get; private set; }
    public IEnumerable<Tile> Tiles
    { get; private set; }
    public IEnumerable<PokemonProxy> Pokemons
    { get { return Tiles.Where((t) => t.Pokemon != null).Select((t) => t.Pokemon); } }

    private void SortActingPokemons()
    {
      for (int i = 0; i < ActingPokemons.Count - 1; i++)
      {
        int j;
        j = Controller.GetRandomInt(i, ActingPokemons.Count - 1);
        PokemonProxy temp = ActingPokemons[i];
        ActingPokemons[i] = ActingPokemons[j];
        ActingPokemons[j] = temp;
      }
      foreach (var p in Board.Pokemons) p.ItemSpeedValue = p.Item.CompareValue(p);
      ActingPokemons = ActingPokemons.OrderBy((pm) => pm, comparer).ToList();
    }
    private void SortTiles()
    {
      for (int i = 0; i < tiles.Length - 1; i++)
      {
        int j;
        j = Controller.GetRandomInt(i, tiles.Length - 1);
        Tile temp = tiles[i];
        tiles[i] = tiles[j];
        tiles[j] = temp;
      }
      Tiles = tiles.OrderBy((pm) => pm, comparer).ToArray();
    }

    public void StartGameLoop()
    {
      if (!Controller.CanContinue) return;
    LOOP:
      switch (current)
      {
        case 0:
          BeginTurn();
          break;
        case 1:
          Prepare();
          break;
        case 2:
          Switch();
          CheckFocusPunch();//暂定，实际上不在这
          break;
        case 3:
          Move();
          break;
        case 4:
          EndTurnEffects();
          break;
        case 5:
          EndTurnCheckForInput();
          break;
        case 6:
          EndTurnSendout();
          break;
        case 7:
          NextTurn();
          break;
        default:
          current = 0;
          goto case 0;
      }
      if (Controller.CanContinue)
      {
        current++;
        goto LOOP;
      }
    }

    private void BeginTurn()
    {
      bool needInput = false;
      foreach (PokemonProxy p in Pokemons)
        needInput |= p.CheckNeedInput();
      if (needInput) Controller.PauseForTurnInput();
    }
    private void Prepare()
    {
      ReportBuilder.NewTurn();
      SortTiles();
      SortActingPokemons();
      foreach (PokemonProxy p in ActingPokemons) p.AttachBehaviors();
    }
    private void Switch()
    {
    LOOP:
      PokemonProxy p = ActingPokemons.FirstOrDefault((pm) => pm.Action == PokemonAction.WillSwitch);
      if (p != null)
      {
        p.Switch();
        ReportBuilder.AddHorizontalLine();
        goto LOOP;
      }
    }
    private void CheckFocusPunch()
    {
      foreach (PokemonProxy p in ActingPokemons)
        if (p.Action == PokemonAction.MoveAttached && Sp.Moves.FocusPunch(p.SelectedMove)) p.AddReportPm("EnFocusPunch");
    }
    private void Move()
    {
    LOOP:
      PokemonProxy p = ActingPokemons.FirstOrDefault((pm) => pm.CanMove);
      if (p != null)
      {
        p.Move();
        if (Controller.CanContinue)
        {
          ReportBuilder.AddHorizontalLine();
          goto LOOP;
        }
      }
    }
    private void EndTurnEffects()
    {
      if (Controller.TurnNumber != 0)
      {
        SortTiles();
        EffectsService.EndTurn.Execute(Controller);
        ReportBuilder.AddHorizontalLine();
      }
    }
    private void EndTurnCheckForInput()
    {
      if (Controller.TurnNumber != 0)
      {
        current++;
        foreach (Tile t in Tiles)
          if (t.Pokemon == null && Controller.CanSendout(t))
          {
            Controller.PauseForEndTurnInput();
            return;
          }
      }
    }
    private void EndTurnSendout()
    {
      var sendouts = new bool[2, 3]; //max size
      foreach(Tile t in Tiles)
        if (t.WillSendoutPokemonIndex != Tile.NOPM_INDEX)
        {
          sendouts[t.Team, t.X] = true;
          Controller.Sendout(t, false);
        }
      SortTiles();
      Abilities.AttachUnnerve(Controller);
      foreach (Tile t in Tiles)
        if (sendouts[t.Team, t.X]) t.Pokemon.Debut();
      foreach (Tile t in Tiles)
        if (sendouts[t.Team, t.X]) Abilities.AttachWeatherObserver(t.Pokemon);
      Abilities.WeatherChanged(Controller);
      if (ReportBuilder.TurnNumber != 0) current -= 2;
    }
    private void NextTurn()
    {
      Sp.Abilities.SlowStart(Controller);
      Board.ClearTurnCondition();
      foreach (var f in Board.Fields) f.ClearTurnCondition();
      foreach (var t in Tiles)
      {
        if (t.Pokemon != null)
        {
          t.Pokemon.OnboardPokemon.ClearTurnCondition();
          if (t.Pokemon.AtkContext != null) t.Pokemon.AtkContext.ClearTurnCondition();
        }
        t.ClearTurnCondition();
      }
      ReportBuilder.Add(new GameEvents.EndTurn());
    }
  }
}
