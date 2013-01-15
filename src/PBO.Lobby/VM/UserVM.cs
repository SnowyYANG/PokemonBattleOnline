using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using PokemonBattleOnline.Tactic.Network;
using PokemonBattleOnline.Messaging;
using PokemonBattleOnline.Messaging.Room;
using PokemonBattleOnline.PBO.UIElements;
using User = PokemonBattleOnline.Tactic.Network.User<PokemonBattleOnline.Messaging.UE>;

namespace PokemonBattleOnline.PBO.Lobby
{
  public class UserVM
  {
    public UserVM(User user)
    {
      if (PBOClient.Current.User.Id != user.Id)
      {
        //commands.Add(new MenuCommand("私聊", Chat));
        //commands.Add(new MenuCommand("挑战", Challenge));
      }
    }

    void Chat()
    {
      //Lobby.Chat.Current.NewChat(this.Model);
    }
    void Challenge()
    {
      //new StartBattle(Model, new GameInitSettings(Game.GameMode.Single), false).Show();
    }
  }
}
