using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using PokemonBattleOnline.PBO.UIElements;
using PokemonBattleOnline.Network;

namespace PokemonBattleOnline.PBO
{
  /// <summary>
  /// Lobby和Battle都需要用这个 所以...
  /// </summary>
  public class UserVM : INotifyPropertyChanged
  {
    static readonly Brush[] ChatColors;
    static UserVM()
    {
      ChatColors = new Brush[8];
      ChatColors[0] = Helper.NewBrush(0xff0f253f);
      ChatColors[1] = Helper.NewBrush(0xff632523);
      ChatColors[2] = Helper.NewBrush(0xff4f6228);
      ChatColors[3] = Helper.NewBrush(0xff3f3151);
      ChatColors[4] = Helper.NewBrush(0xff215867);
      ChatColors[5] = Helper.NewBrush(0xff974807);
      ChatColors[6] = Helper.NewBrush(0xff272727);
      ChatColors[7] = Helper.NewBrush(0xff002060);
    }
    public static Brush GetChatBrush(string userName)
    {
      return ChatColors[userName[0] & 7];
    }
    
    public event PropertyChangedEventHandler PropertyChanged;
    protected readonly ObservableCollection<MenuCommand> commands;
    protected readonly User Model;

    public UserVM(User model)
    {
      Model = model;
      commands = new ObservableCollection<MenuCommand>();
      Commands = new ReadOnlyObservableCollection<MenuCommand>(commands);
    }

    public int Id
    { get { return Model.Id; } }
    public string Name
    { get { return Model.Name; } }
    public int Avatar
    { get { return Model.Avatar; } }
    public UserState State
    { get { return Model.State; } }
    public ReadOnlyObservableCollection<MenuCommand> Commands
    { get; private set; }

    void OnPropertyChanged(string propertyname)
    {
      UIDispatcher.Invoke(() =>
        {
          if (PropertyChanged != null)
            PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
        });
    }
    public void RefreshProperties()
    {
      OnPropertyChanged("State");
    }
    public override string ToString()
    {
      return Name;
    }
  }
}
