using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.Tactic.Messaging;
using LightStudio.PokemonBattle.Messaging.Room;
using LightStudio.PokemonBattle.Data;

namespace LightStudio.PokemonBattle.Messaging
{
  //internal class SingleClient : DisposableObject, IBattleClient
  //{
  //  public const byte GAME_MESSAGE = 3;
  //  public static SingleClient Player(IClient sender, int hostId, PokemonCustomInfo[] pokemons)
  //  {
  //    return new SingleClient(sender, hostId, new Player(hostId, 1, pokemons));
  //  }
  //  public static SingleClient Spectator(IClient sender, int hostId)
  //  {
  //    return new SingleClient(sender, hostId, new Spectator(hostId));
  //  }

  //  private readonly IClient sender;
  //  private readonly UserBase User;

  //  private SingleClient(IClient sender, int hostId, UserBase user)
  //  {
  //    this.sender = sender;
  //    HostId = hostId;
  //    User = user;
  //    User.SendCommand += SendCommand;
  //  }

  //  event Action<IRoom> IBattleClient.EnterSucceed
  //  {
  //    add { User.EnterSucceed += value; }
  //    remove { User.EnterSucceed -= value; }
  //  }
  //  public int HostId
  //  { get; private set; }

  //  public void Start()
  //  {
  //    User.EnterRoom();
  //  }

  //  protected override void DisposeManagedResources()
  //  {
  //    base.DisposeManagedResources();
  //    User.Dispose();
  //  }

  //  #region Handle Message
  //  private void SendCommand(IHostCommand command)
  //  {
  //    var message = command.ToMessage();
  //    sender.SendMessage(GAME_MESSAGE, writer =>
  //    {
  //      writer.Write(message.Header);
  //      writer.Write(message.Content);
  //    }, HostId);
  //  }
  //  public void OnReceived(IMessage message)
  //  {
  //    IUserInformation info = message.GetMessageObjectOrNull() as IUserInformation;
  //    if (info != null) ((IUser)User).ExecuteInformation(info);
  //  }
  //  #endregion

  //}
}
