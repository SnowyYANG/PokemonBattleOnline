﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonBattleOnline.Data;
using PokemonBattleOnline.Network.Room;

namespace PokemonBattleOnline.Network.Lobby.Commands
{
  //public class BattleClient : ClientService
  //{
  //  public static event Action<IRoom> EnterSucceed;
  //  public static event Action<string> EnterFailed;

  //  private RoomUserClient user;
    
  //  internal BattleClient(Client client)
  //    : base(client, MessageHeaders.GAME_H2C)
  //  {
  //  }

  //  public bool CanEnterRoom
  //  { get { lock (this) return user == null; } }

  //  protected override void ReadMessage(User sender, byte header, System.IO.BinaryReader reader)
  //  {
  //    if (this.user != null && sender.Id == user.HostId)
  //    {
  //      string h = reader.ReadString();
  //      var message = new TextMessage(h, reader.ReadString());
  //      UserInformation info = message.GetMessageObjectOrNull() as UserInformation;
  //      if (info != null) ((IRoomUser)user).ExecuteInformation(info);
  //    }
  //  }
  //  private void SendCommand(HostCommand command)
  //  {
  //    SendMessage(MessageHeaders.GAME_C2H, command, user.HostId);
  //  }

  //  public void QuitRoom()
  //  {
  //    lock (this)
  //    {
  //      if (user != null) user.Quit();
  //    }
  //  }
  //  public bool JoinGame(int hostId, int teamId, IPokemonData[] pokemons)
  //  {
  //    lock (this)
  //    {
  //      if (user == null)
  //      {
  //        EnterRoom(new PlayerClient(hostId, teamId, pokemons));
  //        return true;
  //      }
  //    }
  //    return false;
  //  }
  //  public bool SpectateGame(int hostId)
  //  {
  //    lock (this)
  //    {
  //      if (user == null)
  //      {
  //        EnterRoom(new SpectatorClient(hostId));
  //        return true;
  //      }
  //    }
  //    return false;
  //  }
  //  private void EnterRoom(RoomUserClient user) //already locked
  //  {
  //    this.user = user;
  //    user.EnterSucceed += user_EnterSucceed;
  //    user.EnterFailed += user_EnterFailed;
  //    user.Quited += user_Quited;
  //    user.SendCommand += SendCommand;
  //    user.EnterRoom();
  //  }

  //  private UserState lastState;
  //  private void user_Quited()
  //  {
  //    lock (this)
  //    {
  //      if (user != null)
  //      {
  //        user.Dispose();
  //        user = null;
  //        Client.ChangeUserState(lastState);
  //      }
  //    }
  //  }
  //  private void user_EnterSucceed()
  //  {
  //    lastState = Client.User.State;
  //    Client.ChangeUserState(user.State);
  //    EnterSucceed(user);
  //  }
  //  private void user_EnterFailed(string message)
  //  {
  //    EnterFailed(message);
  //  }
    
  //  public override void Dispose()
  //  {
  //    lock (this)
  //    {
  //      if (user != null) user.Dispose();
  //    }
  //  }
  //}
}