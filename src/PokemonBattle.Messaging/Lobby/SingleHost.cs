using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Messaging.Room;
using LightStudio.PokemonBattle.Data;

namespace LightStudio.PokemonBattle.Messaging
{
  //internal class SingleHost : DisposableObject, IBattleClient, IBattleHost
  //{
  //  public const byte GAME_MESSAGE = 3;
  //  event Action<IRoom> IBattleClient.EnterSucceed
  //  {
  //    add { user.EnterSucceed += value; }
  //    remove { user.EnterSucceed -= value; }
  //  }
  //  private readonly Host host;
  //  private readonly Player user;

  //  public SingleHost(int adminId, GameInitSettings settings, PokemonCustomInfo[] pokemons)
  //  {
  //    host = new Host(settings, true);
  //    host.SendInformation += (info, ids) => SendMessage(info.ToMessage(), ids);
  //    user = new Player(adminId, 0, pokemons);
  //    user.SendCommand += (command) => SendMessage(command.ToMessage(), adminId);
  //    user.Quited += () => host.CloseRoom();
  //  }

  //  int IBattleClient.HostId
  //  { get { return sender.User.Id; } }

  //  public IRoom UserController
  //  { get { return user; } }
  //  public Host AdminController
  //  { get { return host; } }

  //  public void Start()
  //  {
  //    //useless, for host_PropertyChanged has already done this
  //    host.StartGame();
  //  }

  //  protected override void DisposeManagedResources()
  //  {
  //    base.DisposeManagedResources();
  //    host.Dispose();
  //    user.Dispose();
  //  }

  //  #region Handle Message
  //  public void OnReceived(int senderId, IMessage message)
  //  {
  //    IMessagable obj = message.GetMessageObjectOrNull();
  //    if (obj != null)
  //      if (obj is IHostCommand) ((IHost)host).ExecuteCommand((IHostCommand)obj, senderId);
  //      else if (obj is IUserInformation) ((IUser)user).ExecuteInformation((IUserInformation)obj);
  //  }
  //  public void OnReceived(IMessage message)
  //  {
  //    IUserInformation info = message.GetMessageObjectOrNull() as IUserInformation;
  //    if (info != null) ((IUser)user).ExecuteInformation(info);
  //  }

  //  private void SendMessage(IMessage message, params int[] receivers)
  //  {
  //    sender.SendMessage(GAME_MESSAGE, writer =>
  //    {
  //      writer.Write(message.Header);
  //      writer.Write(message.Content);
  //    }, receivers);
  //  }

  //  #endregion
  //}
}
