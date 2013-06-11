﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonBattleOnline.Data;

namespace PokemonBattleOnline.Game.Host.Effects.Moves.Status
{
  class Metronome : StatusMoveE
  {
    #region
    private static readonly int[] BLACK_LIST = new int[]
    {
      68,
      102,
      118,
      119,
      144,
      165,
      166,
      168,
      173,
      182,
      194,
      197,
      203,
      214,
      243,
      264,
      266,
      267,
      270,
      271,
      274,
      289,
      343,
      364,
      382,
      383,
      415,
      448,
      469,
      476,
      495,
      501,
      511,
      516,
    };
    #endregion

    public Metronome(int id)
      : base(id)
    {
    }
    
    public override void Execute(AtkContext atk)
    {
    LOOP:
      var m = GameDataService.GetMove(atk.Controller.GetRandomInt(1, GameDataService.Moves.Count()));
      if (BLACK_LIST.Contains(m.Id)) goto LOOP;
      atk.StartExecute(m, null, "Metronome");
    }
  }
}