using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Media;
using System.Windows.Threading;
using LightStudio.Tactic.Messaging.Lobby;
using LightStudio.PokemonBattle.Data;
using LightStudio.PokemonBattle.Game;
using LightStudio.PokemonBattle.Messaging;
using LightStudio.PokemonBattle.Room;
using LightStudio.PokemonBattle.PBO.UIElements;
using SoundPlayer = System.Media.SoundPlayer;

namespace LightStudio.PokemonBattle.PBO.Lobby
{
  class StartBattleVM : INotifyPropertyChanged
  {
    static SoundPlayer sound;
    static StartBattleVM()
    {
      sound = new SoundPlayer("Resources\\challenged.wav");
      try
      {
        sound.LoadAsync();
      }
      catch { }
    }
    static void PlaySound()
    {
      if (sound.IsLoadCompleted) sound.Play();
    }
    
    readonly bool isPassive;
    
    public event PropertyChangedEventHandler PropertyChanged;
    internal event Action Processed;
    PokemonLobbyClient client;
    IPokemonFolder chosenTeam;
    DispatcherTimer timer;
    bool isWaiting;

    public StartBattleVM(PokemonLobbyClient client, User rival, GameSettings settings, bool isPassive)
    {
      this.client = client;
      Rival = rival;
      this.isPassive = isPassive;
      RivalAvatar = AvatarVM.GetAvatar(rival.Avatar);
      Teams = Helper.DataMainInstance.PokemonData.Teams.Folders;
      chosenTeam = Teams.FirstOrDefault();
      GameSettings = settings;
      if (isPassive)
      {
        OkCommand = new MenuCommand("接受", Accept);
        CancelCommand = new MenuCommand("拒绝", Refuse);
        client.ChallengeCanceled += OnProcessed;
        PlaySound();
      }
      else
      {
        OkCommand = new MenuCommand("挑战", Challenge);
        CancelCommand = new MenuCommand("取消", Cancel);
        client.ChallengeAccepted += OnProcessed;
        client.ChallengeRefused += OnProcessed;
      }
      client.EnterSucceed += OnProcessed;
      OkCommand.IsEnabled = ChosenTeam != null;
      timer = new DispatcherTimer() { Interval = TimeSpan.FromSeconds(10) };
      timer.Tick += (sender, e) => CancelCommand.IsEnabled = true;
    }

    public User Rival { get; private set; }
    public ImageSource RivalAvatar { get; private set; }
    public MenuCommand OkCommand { get; private set; }
    public MenuCommand CancelCommand { get; private set; }
    public GameSettings GameSettings { get; private set; }
    public ReadOnlyObservableCollection<IPokemonFolder> Teams { get; private set; }
    public IPokemonFolder ChosenTeam
    {
      get { return chosenTeam; }
      set
      {
        if (chosenTeam != value)
        {
          chosenTeam = value;
          if (PropertyChanged != null)
            PropertyChanged(this, new PropertyChangedEventArgs("ChosenTeam"));
          OkCommand.IsEnabled = chosenTeam != null;
        }
      }
    }

    void OnProcessed(IUserController u = null)
    {
      if (isPassive) client.ChallengeCanceled -= OnProcessed;
      else
      {
        client.ChallengeAccepted -= OnProcessed;
        client.ChallengeRefused -= OnProcessed;
      }
      client.EnterSucceed -= OnProcessed;
      if (CancelCommand.IsEnabled) CancelCommand.Execute(null); //auto refuse others
      if (Processed != null) UIDispatcher.Invoke(Processed);
    }
    void OnProcessed(User user)
    {
      if (user.Id == Rival.Id)
      {
        OkCommand.IsEnabled = CancelCommand.IsEnabled = false;
        OnProcessed();
      }
    }
    void Accept()
    {
      CancelCommand.IsEnabled = false;
      lock (ChosenTeam.Pokemons)
      {
        if (!client.AcceptChallenge(Rival.Id, ChosenTeam.Pokemons.ToArray()))
          OnProcessed();
      }
    }
    void Refuse()
    {
      OkCommand.IsEnabled = CancelCommand.IsEnabled = false;
      client.RefuseChallenge(Rival.Id);
      OnProcessed();
    }
    void Challenge()
    {
      lock (ChosenTeam.Pokemons)
      {
        if (!client.Challenge(Rival.Id, ChosenTeam.Pokemons.ToArray(), GameSettings)) return;
        isWaiting = true;
        OkCommand.IsEnabled = CancelCommand.IsEnabled = false;
      }
      timer.Start();
    }
    void Cancel()
    {
      OkCommand.IsEnabled = CancelCommand.IsEnabled = false;
      if (isWaiting) client.CancelChallenge(Rival.Id);
      OnProcessed();
    }
  }
}
