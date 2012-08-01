using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using LightStudio.Tactic.Messaging;
using LightStudio.PokemonBattle.Messaging;
using User = LightStudio.Tactic.Messaging.User<LightStudio.PokemonBattle.Messaging.RoomInfo>;

namespace LightStudio.PokemonBattle.PBO
{
  public class UserVM : INotifyPropertyChanged
  {
    //static readonly Brush[] ChatColors;
    //static UserVM()
    //{
    //  ChatColors = new Brush[8];
    //  ChatColors[0] = Helper.NewBrush(0xff0f253f);
    //  ChatColors[1] = Helper.NewBrush(0xff632523);
    //  ChatColors[2] = Helper.NewBrush(0xff4f6228);
    //  ChatColors[3] = Helper.NewBrush(0xff3f3151);
    //  ChatColors[4] = Helper.NewBrush(0xff215867);
    //  ChatColors[5] = Helper.NewBrush(0xff974807);
    //  ChatColors[6] = Helper.NewBrush(0xff272727);
    //  ChatColors[7] = Helper.NewBrush(0xff002060);
    //}
    //public static Brush GetChatBrush(string userName)
    //{
    //  return ChatColors[userName[0] & 7];
    //}
    
    public event PropertyChangedEventHandler PropertyChanged;
    //protected readonly ObservableCollection<MenuCommand> commands;
    protected readonly User Model;
    private readonly AvatarVM avatar;

    public UserVM(User user, bool innerAvatarOnly)
    {
      Model = user;
      avatar = new AvatarVM(user.Avatar, innerAvatarOnly);
      //commands = new ObservableCollection<MenuCommand>();
      //Commands = new ReadOnlyObservableCollection<MenuCommand>(commands);
    }

    public int Id
    { get { return Model.Id; } }
    public string Name
    { get { return Model.Name; } }
    public AvatarVM Avatar
    { get { return avatar; } }
    public UserState State
    { get { return Model.State; } }
    public string Sign
    { get { return Model.Sign; } }
    //public ReadOnlyObservableCollection<MenuCommand> Commands { get; private set; }

    void OnPropertyChanged(string propertyname)
    {
      WpfDispatcher.Invoke(() =>
        {
          if (PropertyChanged != null)
            PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
        });
    }
    public void RefreshProperties()
    {
      OnPropertyChanged("State");
      OnPropertyChanged("Sign");
    }
    public override string ToString()
    {
      return Model.Name;
    }
  }
}
