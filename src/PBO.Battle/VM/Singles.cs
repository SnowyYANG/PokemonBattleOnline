using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows;
using System.Windows.Threading;
using LightStudio.PokemonBattle.Game;

namespace LightStudio.PokemonBattle.PBO.Battle.VM
{
  class Singles : IControlPanel
  {
    public event PropertyChangedEventHandler PropertyChanged;
    public event Action<string> InputFailed;
    private readonly IPlayerController controller;
    private readonly GameOutward game;
    private readonly DispatcherTimer timer;

    internal Singles(Messaging.Room.IRoom c)
    {
      controller = c.PlayerController;
      game = c.Game;
      timer = new DispatcherTimer() { Interval = TimeSpan.FromSeconds(1) };
      _time = 180;
      _teamPms = c.Game.Teams[controller.Player.TeamId];
      _rivalPms = c.Game.Teams[1 - controller.Player.TeamId];
      _selectedPanel = ControlPanelIndex.INACTIVE;

      controller.RequireInput += RequireInput;
      c.GameEnd += () => timer.Stop();
      timer.Tick += (sender, e) => Time--;
    }

    private int _time;
    public int Time
    { 
      get { return _time; }
      private set
      {
        if (_time != value)
        {
          _time = value;
          OnPropertyChanged("Time");
        }
      }
    }
    private int _selectedPanel;
    public int SelectedPanel
    {
      get { return _selectedPanel; }
      set
      {
        if (_selectedPanel != value)
        {
          _selectedPanel = value;
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
    private TeamOutward _teamPms;
    public TeamOutward TeamPokemonsCount
    { get { return _teamPms; } }
    private TeamOutward _rivalPms;
    public TeamOutward RivalTeamPokemonsCount
    { get { return _rivalPms; } }
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
      if (request.IsSendout)
      {
        if (!request.Pokemon(pokemon, 0)) InputFailed(request.GetErrorMessage());
      }
      else
      {
        if (!request.Pokemon(pokemon))
        {
          request.TryRaiseAbility();
          InputFailed(request.GetErrorMessage());
        }
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

    private void RequireInput(InputRequest request, int spentTime)
    {
      timer.Start();
      this.request = request;
      request.Init(controller.Game);
      request.InputFinished += (i) =>
        {
          SelectedPanel = ControlPanelIndex.INACTIVE;
          controller.Input(i);
          timer.Stop();
        };
      _selectedPanel = request.IsSendout ? ControlPanelIndex.POKEMONS : ControlPanelIndex.MAIN;
      _time = 180 - spentTime;
      OnPropertyChanged(null);
    }
  }
}
