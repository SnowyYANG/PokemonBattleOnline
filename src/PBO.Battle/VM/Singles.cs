using System;
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
      selectedPanel = (int)ControlPanelIndex.INACTIVE;
      //controller.Board.MyTeam.PokemonOnBoardChanged += (sender, e) =>
      //  {
      //    if (e.NewPokemon != null) pokemonsInBall.Remove(e.NewPokemon);
      //    if (e.OldPokemon != null) pokemonsInBall.Add(e.OldPokemon);
      //  };
      //pokemonsInBall = new List<IPokemon>(controller.Player.Pokemons.Where(p => p.PositionOnBoard == null));//如果中途加载的话有可能存在正在战斗的精灵
      //this.controller_IsActiveChanged(null, null);
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
    { get { return controller.Game.ActivePokemons.FirstOrDefault(); } }
    public Visibility UndoVisibility
    { get { return Visibility.Collapsed; } }
    public bool IsFightEnabled
    { get { return ControllingPokemon != null && (ControllingPokemon.CanStruggle || ControllingPokemon.CanUseMove); } }
    public bool IsSwitchEnabled
    { get { return ControllingPokemon != null && ControllingPokemon.CanSwitch; } }
    public TeamOutward TeamPokemonsCount
    { get { return teamPms; } }
    public TeamOutward RivalTeamPokemonsCount
    { get { return rivalPms; } }
    public Pokemon[] Pokemons
    { get { return controller.Player.Pokemons; } }

    protected void OnPropertyChanged(string propertyName)
    {
      if (PropertyChanged != null)
        PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
    }
    public void Pokemon_Click(Pokemon pokemon)
    {
      MessageBox.Show(pokemon.Id.ToString());
      return;
      if (ControllingPokemon.CanSwitch && pokemon.Hp.Value > 0 && controller.Player.GetPokemonIndex(pokemon) > controller.Game.Settings.OnboardPokemonsPerPlayer )
        controller.Switch(ControllingPokemon, pokemon);
    }
    public void Move_Click(Move move)
    {
      MessageBox.Show(move.Id.ToString());
      return;
      if (ControllingPokemon.CanUseMove && move.CanSelect)
        controller.UseMove(move);
    }
    public void Struggle_Click()
    {
      if (ControllingPokemon != null)
        MessageBox.Show(ControllingPokemon.Id.ToString());
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
      if (ControllingPokemon.Hp == 0) selectedPanel = ControlPanelIndex.POKEMONS;
      else selectedPanel = ControlPanelIndex.MAIN;
      OnPropertyChanged(null);
    }
    void IPlayerControllerEvents.InputSucceeded()
    {
      SelectedPanel = (int)ControlPanelIndex.INACTIVE;
      OnPropertyChanged("ControllingPokemon");
    }
    void IPlayerControllerEvents.InputFailed()
    {
      System.Windows.MessageBox.Show("Debug.InputFailed");
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
