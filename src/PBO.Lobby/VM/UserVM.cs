using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using LightStudio.Tactic.Messaging.Lobby;
using LightStudio.PokemonBattle.Messaging;
using LightStudio.PokemonBattle.PBO.UIElements;

namespace LightStudio.PokemonBattle.PBO.Lobby
{
  public class UserVM : PBO.UserVM
  {
    PokemonLobbyClient client;

    public UserVM(PokemonLobbyClient client, User user) : base(user, false)
    {
      this.client = client;
      if (client.User.Id != user.Id)
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
      new StartBattle(client, Model, new Game.GameSettings(Game.GameMode.Single), false).Show();
    }
  }
}
