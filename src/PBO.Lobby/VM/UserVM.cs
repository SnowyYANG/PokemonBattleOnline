using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using LightStudio.Tactic.Messaging;
using LightStudio.PokemonBattle.Messaging;
using LightStudio.PokemonBattle.Messaging.Room;
using LightStudio.PokemonBattle.PBO.UIElements;
using User = LightStudio.Tactic.Messaging.User<LightStudio.PokemonBattle.Messaging.UserExtension>;

namespace LightStudio.PokemonBattle.PBO.Lobby
{
  public class UserVM : PBO.UserVM
  {
    public UserVM(User user) : base(user, false)
    {
      if (PBOClient.Client.User.Id != user.Id)
      {
        commands.Add(new MenuCommand("私聊", Chat));
        commands.Add(new MenuCommand("挑战", Challenge));
      }
    }

    void Chat()
    {
      Lobby.Chat.Current.NewChat(this.Model);
    }
    void Challenge()
    {
      new StartBattle(Model, new GameInitSettings(Game.GameMode.Single), false).Show();
    }
  }
}
