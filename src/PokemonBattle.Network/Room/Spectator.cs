using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightStudio.PokemonBattle.Room
{
  internal class Spectator : User
  {
    public Spectator(int hostId)
      : base(hostId)
    {
    }

    public override UserRole Role
    { get { return UserRole.Spectator; } }

    #region Errors
    protected override void InformPmAdditional(Interactive.PokemonAdditionalInfo pminfo)
    {
      error("收到非法的消息，数据包损毁或房间主机程序被修改");
    }
    protected override void InformRequestTie()
    {
      error("收到非法的消息，数据包损毁或房间主机程序被修改");
    }
    protected override void InformTieRejected()
    {
      error("收到非法的消息，数据包损毁或房间主机程序被修改");
    }
    protected override void InformRequireInput()
    {
      error("收到非法的消息，数据包损毁或房间主机程序被修改");
    }
    protected override void InformInputFail()
    {
      error("收到非法的消息，数据包损毁或房间主机程序被修改");
    }
    protected override void InformInputSucceed()
    {
      error("收到非法的消息，数据包损毁或房间主机程序被修改");
    }
    #endregion

    public void SpectateGame()
    {
      sendCommand(new SpectateGameCommand());
    }
  }
}
