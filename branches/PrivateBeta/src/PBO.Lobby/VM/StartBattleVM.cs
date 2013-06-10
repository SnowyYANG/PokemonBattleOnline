using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Media;
using System.Windows.Threading;
using LightStudio.Tactic.Messaging;
using LightStudio.PokemonBattle.Data;
using LightStudio.PokemonBattle.Game;
using LightStudio.PokemonBattle.Messaging;
using LightStudio.PokemonBattle.Messaging.Room;
using LightStudio.PokemonBattle.PBO.UIElements;
using SoundPlayer = System.Media.SoundPlayer;

namespace LightStudio.PokemonBattle.PBO.Lobby
{
  class StartBattleVM : INotifyPropertyChanged
  {
    static SoundPlayer sound;
    static StartBattleVM()
    {
      sound = new SoundPlayer("..\\res\\challenged.wav");
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
    private readonly ChallengeManager challenge;
    DispatcherTimer timer;
    bool isWaiting;

    public StartBattleVM(User rival, GameInitSettings settings, bool isPassive)
    {
      challenge = PBOClient.Challenge; //thread safe?
      Rival = rival;
      this.isPassive = isPassive;
      RivalAvatar = Converters.IdAvatar.GetAvatar(rival.Avatar);
      Teams = DataService.UserData.Teams;
      _chosenTeam = Teams.FirstOrDefault();
      GameSettings = settings;
      if (isPassive)
      {
        OkCommand = new MenuCommand("接受", Accept);
        CancelCommand = new MenuCommand("拒绝", Refuse);
        challenge.ChallengeCanceled += OnProcessed;
        PlaySound();
      }
      else
      {
        OkCommand = new MenuCommand("挑战", Challenge);
        CancelCommand = new MenuCommand("取消", Cancel);
        challenge.ChallengeAccepted += OnProcessed;
        challenge.ChallengeRefused += OnProcessed;
      }
      BattleClient.EnterSucceed += OnProcessed;
      OkCommand.IsEnabled = ChosenTeam != null;
      timer = new DispatcherTimer() { Interval = TimeSpan.FromSeconds(10) };
      timer.Tick += (sender, e) => CancelCommand.IsEnabled = true;
    }

    public User Rival { get; private set; }
    public ImageSource RivalAvatar { get; private set; }
    public MenuCommand OkCommand { get; private set; }
    public MenuCommand CancelCommand { get; private set; }
    public GameInitSettings GameSettings { get; private set; }
    public IEnumerable<IEnumerable<IPokemonData>> Teams { get; private set; }
    
    private IEnumerable<IPokemonData> _chosenTeam;
    public IEnumerable<IPokemonData> ChosenTeam
    {
      get { return _chosenTeam; }
      set
      {
        if (_chosenTeam != value)
        {
          _chosenTeam = value;
          if (PropertyChanged != null)
            PropertyChanged(this, new PropertyChangedEventArgs("ChosenTeam"));
          OkCommand.IsEnabled = _chosenTeam != null;
        }
      }
    }

    void OnProcessed(IRoom u = null)
    {
      if (isPassive) challenge.ChallengeCanceled -= OnProcessed;
      else
      {
        challenge.ChallengeAccepted -= OnProcessed;
        challenge.ChallengeRefused -= OnProcessed;
      }
      BattleClient.EnterSucceed -= OnProcessed;
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
      lock (ChosenTeam)
      {
        if (challenge.AcceptChallenge(Rival.Id, ChosenTeam.ToArray()))
          OnProcessed();
      }
    }
    void Refuse()
    {
      OkCommand.IsEnabled = CancelCommand.IsEnabled = false;
      challenge.RefuseChallenge(Rival.Id);
      OnProcessed();
    }
    void Challenge()
    {
      lock (ChosenTeam)
      {
        if (!challenge.Challenge(Rival.Id, ChosenTeam.ToArray(), GameSettings)) return;
        isWaiting = true;
        OkCommand.IsEnabled = CancelCommand.IsEnabled = false;
      }
      timer.Start();
    }
    void Cancel()
    {
      OkCommand.IsEnabled = CancelCommand.IsEnabled = false;
      if (isWaiting) challenge.CancelChallenge(Rival.Id);
      OnProcessed();
    }
  }
}
