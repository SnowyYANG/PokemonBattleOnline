using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows;
using LightStudio.PokemonBattle.Game;

namespace LightStudio.PokemonBattle.PBO.Battle.VM
{
#warning
  class Singles : IControlPanel
  {
    public event PropertyChangedEventHandler PropertyChanged;
    public event Action<string> InputFailed;
    IPlayerController controller;
    int selectedPanel;
    BoardOutward board;
    TeamOutward teamPms, rivalPms;

    internal Singles(Messaging.Room.IRoom c)
    {
      controller = c.PlayerController;
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
    public void Pokemon_Click(Pokemon pokemon)
    {
      //if ((ControllingPokemon == null || ControllingPokemon.CanSwitch) && pokemon.Hp.Value > 0 &&
      //  pokemon.IndexInOwner >= controller.Game.Settings.Mode.OnboardPokemonsPerPlayer())
      //  controller.Sendout(0, pokemon);
    }
    public void Fight_Click()
    {
      //if (ControllingPokemon.CanStruggle)
      //  controller.Struggle(0);
    }
    public void Move_Click(SimMove move)
    {
      //if (ControllingPokemon.CanSelectMove && move.CanBeSelected)
      //  controller.UseMove(0, move);
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

    //void IPlayerControllerEvents.RequireInput()
    //{
    //  ControllingPokemon = controller.Game.ActivePokemons.ValueOrDefault(0);
    //  if (ControllingPokemon == null) selectedPanel = ControlPanelIndex.POKEMONS; //死亡交换时精灵已经被收回了
    //  else selectedPanel = ControlPanelIndex.MAIN;
    //  OnPropertyChanged(null);
    //}
    //void IPlayerControllerEvents.InputResult(bool suceeded, string messageKey, bool allDone)
    //{
    //  if (allDone)
    //  {
    //    SelectedPanel = (int)ControlPanelIndex.INACTIVE;
    //    ControllingPokemon = null;
    //    OnPropertyChanged("ControllingPokemon");
    //  }
    //  else if (InputFailed != null)
    //  {
    //    IText message = Game.GameService.Logs[messageKey];
    //    if (message == null) InputFailed(messageKey);
    //    else InputFailed(message.ToString());
    //  }
    //}
    //void IPlayerControllerEvents.TieRequested()
    //{
    //}
    //void IPlayerControllerEvents.TieRejected()
    //{
    //}
    //void IPlayerControllerEvents.TimeElapsed(int remainingSeconds)
    //{
    //  Time = remainingSeconds;
    //  OnPropertyChanged("Time");
    //}
    //void IPlayerControllerEvents.TimeUp() //这个应该是只告诉玩家本人的\
    //{
    //}
  }
}
