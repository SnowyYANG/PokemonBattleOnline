﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows;
using LightStudio.PokemonBattle.Data;
using LightStudio.PokemonBattle.Game;
using LightStudio.PokemonBattle.Interactive;

namespace LightStudio.PokemonBattle.PBO.Battle.VM
{
  class Singles : IControlPanel, IPlayerControllerEvents
  {
    public event PropertyChangedEventHandler PropertyChanged;
    public event Action<string> InputFailed;
    IPlayerController controller;
    int selectedPanel;
    BoardOutward board;
    TeamOutward teamPms, rivalPms;

    internal Singles(Room.IUserController c)
    {
      controller = c.PlayerController;
      controller.AddEventsListener(this);
      board = c.Game.Board;
      teamPms = c.Game.Teams[controller.Player.TeamId];
      rivalPms = c.Game.Teams[1 - controller.Player.TeamId];
      selectedPanel = ControlPanelIndex.INACTIVE;
    }

    public int Time
    { get; private set; }
    public int SelectedPanel
    {
      get { return selectedPanel; }
      set
      {
        if (selectedPanel != value)
        {
          selectedPanel = value;
          OnPropertyChanged("SelectedPanel");
        }
      }
    }
    public Weather Weather
    { get { return board.Weather; } }
    public SimPokemon ControllingPokemon
    { get; private set; }
    public Visibility UndoVisibility
    { get { return Visibility.Collapsed; } }
    public bool IsFightEnabled
    { get { return ControllingPokemon != null && (ControllingPokemon.CanStruggle || ControllingPokemon.CanSelectMove); } }
    public bool IsSwitchEnabled
    { get { return ControllingPokemon != null && ControllingPokemon.CanSwitch; } }
    public TeamOutward TeamPokemonsCount
    { get { return teamPms; } }
    public TeamOutward RivalTeamPokemonsCount
    { get { return rivalPms; } }
    public IEnumerable<Pokemon> Pokemons
    { get { return controller.Player.Pokemons; } }

    protected void OnPropertyChanged(string propertyName)
    {
      if (PropertyChanged != null)
        PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
    }
    public void Pokemon_Click(Pokemon pokemon)
    {
      if (ControllingPokemon.CanSwitch && pokemon.Hp.Value > 0 &&
        pokemon.IndexInOwner >= controller.Game.Settings.Mode.OnboardPokemonsPerPlayer())
        controller.Switch(ControllingPokemon, pokemon);
    }
    public void Move_Click(SimMove move)
    {
      if (ControllingPokemon.CanSelectMove && move.CanBeSelected)
        controller.UseMove(move);
    }
    public void Struggle_Click()
    {
      if (ControllingPokemon.CanStruggle)
        controller.Struggle(ControllingPokemon);
    }
    public void Giveup_Click()
    {
      controller.Quit();
    }

    public TargetPanel TargetPanel
    { get { return null; } }
    public Visibility ThumbnailsVisibility
    { get { return Visibility.Collapsed; } }
    public PokemonOutward[] PokemonsOnBoard
    { get { return null; } }
    void IControlPanel.Undo_Click()
    { }

    void IPlayerControllerEvents.RequireInput()
    {
      ControllingPokemon = controller.Game.ActivePokemons.ValueOrDefault(0);
      if (ControllingPokemon == null) selectedPanel = ControlPanelIndex.POKEMONS; //死亡交换时精灵已经被收回了
      else selectedPanel = ControlPanelIndex.MAIN;
      OnPropertyChanged(null);
    }
    void IPlayerControllerEvents.InputResult(bool suceeded, string message, bool allDone)
    {
      if (allDone)
      {
        SelectedPanel = (int)ControlPanelIndex.INACTIVE;
        ControllingPokemon = null;
        OnPropertyChanged("ControllingPokemon");
      }
      else
        if (InputFailed != null) InputFailed(message);
    }
    void IPlayerControllerEvents.TieRequested()
    {
    }
    void IPlayerControllerEvents.TieRejected()
    {
    }
    void IPlayerControllerEvents.TimeElapsed(int remainingSeconds)
    {
      Time = remainingSeconds;
      OnPropertyChanged("Time");
    }
    void IPlayerControllerEvents.TimeUp() //这个应该是只告诉玩家本人的\
    {
    }
  }
}
