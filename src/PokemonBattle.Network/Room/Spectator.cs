﻿using System;
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
    protected override void InformPmAdditional(Game.PokemonAdditionalInfo pminfo)
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
    protected override void InformInputResult(bool ok, string message, bool allDone)
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
