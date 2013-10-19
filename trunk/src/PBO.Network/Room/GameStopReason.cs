﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using PokemonBattleOnline.Game;

namespace PokemonBattleOnline.Network
{
  public enum GameStopReason
  {
    PlayerGiveUp,
    PlayerDisconnect,
    InvalidInput
  }
}