using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows;
using LightStudio.PokemonBattle.Game;

namespace LightStudio.PokemonBattle.PBO.Battle.VM
{
  class Singles : IControlPanel
  {
    public event PropertyChangedEventHandler PropertyChanged;
    public event Action<string> InputFailed;
    IPlayerController controller;
    int selectedPanel;
    private readonly GameOutward game;
    TeamOutward teamPms, rivalPms;

    internal Singles(Messaging.Room.IRoom c)
    {
      controller = c.PlayerController;
      game = c.Game;
      teamPms = c.Game.Teams[controller.Player.TeamId];
      rivalPms = c.Game.Teams[1 - controller.Player.TeamId];
      selectedPanel = ControlPanelIndex.INACTIVE;
      controller.RequireInput += RequireInput;
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
    { get { return game.Board.Weather; } }
    public SimPokemon ControllingPokemon
    { get { return controller.Game.OnboardPokemons.FirstOrDefault(); } }
    public Visibility UndoVisibility
    { get { return Visibility.Collapsed; } }
    public bool IsFightEnabled
    { get { return ControllingPokemon != null; } }
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
    private InputRequest request;
    public void Pokemon_Click(Pokemon pokemon)
    {
      if (pokemon.Hp.Value == 0 || pokemon == Pokemons.First()) return;
      if (!request.Pokemon(pokemon))
      {
        request.TryRaiseAbility();
        InputFailed(request.GetErrorMessage());
      }
    }
    public void Fight_Click()
    {
      if (!request.Fight()) SelectedPanel = ControlPanelIndex.FIGHT;
    }
    public void Move_Click(SimMove move)
    {
      if (move.PP.Value == 0) return;
      if (!request.Move(move)) InputFailed(request.GetErrorMessage());
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

    private void RequireInput(InputRequest request)
    {
      this.request = request;
      request.Init(controller.Game);
      request.InputFinished += (i) =>
        {
          SelectedPanel = ControlPanelIndex.INACTIVE;
          controller.Input(i);
        };
      selectedPanel = request.IsSendout ? ControlPanelIndex.POKEMONS : ControlPanelIndex.MAIN;
      OnPropertyChanged(null);
    }
  }
}
