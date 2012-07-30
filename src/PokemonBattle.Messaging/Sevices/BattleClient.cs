using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.Tactic.Messaging;
using LightStudio.PokemonBattle.Messaging.Room;
using LightStudio.PokemonBattle.Data;
using IUser = LightStudio.Tactic.Messaging.IUser<LightStudio.PokemonBattle.Messaging.RoomInfo>;

namespace LightStudio.PokemonBattle.Messaging
{
  internal class BattleClient : ClientService
  {
    public event Action<IRoom> EnterSucceed = delegate { };
    public event Action<string> EnterFailed = delegate { };

    private RoomUserBase user;
    
    public BattleClient(Client client)
      : base(client, MessageHeaders.GAME_H2C)
    {
      Client.ChangeUserState(user.State);
    }

    public bool CanEnterRoom
    { get { lock (this) return user == null; } }

    protected override void ReadMessage(IUser sender, byte header, System.IO.BinaryReader reader)
    {
      if (this.user != null && sender.Id == user.HostId)
      {
        string h = reader.ReadString();
        var message = new TextMessage(h, reader.ReadString());
        IUserInformation info = message.GetMessageObjectOrNull() as IUserInformation;
        if (info != null) ((IRoomUser)user).ExecuteInformation(info);
      }
    }
    private void SendCommand(IHostCommand command)
    {
      var message = command.ToMessage();
      SendMessage(MessageHeaders.GAME_C2H, writer =>
      {
        writer.Write(message.Header);
        writer.Write(message.Content);
      }, user.HostId);
    }

    public void QuitRoom()
    {
      lock (this)
      {
        if (user != null) user.Quit();
      }
    }
    public bool JoinGame(int hostId, int teamId, PokemonCustomInfo[] pokemons)
    {
      lock (this)
      {
        if (user == null)
        {
          EnterRoom(new PlayerClient(hostId, teamId, pokemons));
          return true;
        }
      }
      return false;
    }
    public bool SpectateGame(int hostId)
    {
      lock (this)
      {
        if (user == null)
        {
          EnterRoom(new SpectatorClient(hostId));
          return true;
        }
      }
      return false;
    }
    private void EnterRoom(RoomUserBase user) //already locked
    {
      this.user = user;
      user.EnterSucceed += user_EnterSucceed;
      user.EnterFailed += user_EnterFailed;
      user.Quited += user_Quited;
      user.EnterRoom();
    }

    private void user_Quited()
    {
      lock (this)
      {
        if (user != null)
        {
          user.Dispose();
          user = null;
        }
      }
    }
    private void user_EnterSucceed()
    {
      EnterSucceed(user);
    }
    private void user_EnterFailed(string message)
    {
      EnterFailed(message);
    }
  }
}
