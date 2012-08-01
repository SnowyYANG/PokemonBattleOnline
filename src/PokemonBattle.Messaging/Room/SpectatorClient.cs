using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightStudio.PokemonBattle.Messaging.Room
{
  internal class SpectatorClient : RoomUserClient
  {
    public SpectatorClient(int hostId)
      : base(hostId)
    {
    }

    public override Tactic.Messaging.UserState State
    { get { return Tactic.Messaging.UserState.Watching; } }
    public override Game.IPlayerController PlayerController
    { get { return null; } }

    #region Errors
    protected override void InformRequireInput(Game.RequireInput info)
    {
      if (info != null) error("收到非法的消息，数据包损毁或房间主机程序被修改");
    }
    protected override void InformRequestTie()
    {
      error("收到非法的消息，数据包损毁或房间主机程序被修改");
    }
    protected override void InformTieRejected()
    {
      error("收到非法的消息，数据包损毁或房间主机程序被修改");
    }
    #endregion

    public override void EnterRoom()
    {
      sendCommand(new SpectateGameCommand());
    }
  }
}
