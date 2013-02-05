using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonBattleOnline.Data;

namespace PokemonBattleOnline.Game.Host.Effects.Moves.Status
{
  class SleepTalk : StatusMoveE
  {
    private readonly static int[] BLOCK = new int[]
    {
      214,
      117,
      448,
      253,
      383,
      119,
      382,
      264
    };
    
    public SleepTalk(int id)
      : base(id)
    {
    }
    public override void Execute(AtkContext atk)
    {
      var aer = atk.Attacker;
      if (aer.State == PokemonState.SLP)
      {
        var moves = new List<MoveType>();
        foreach (var m in aer.Moves)
          if (!(m.Type.Flags.PrepareOneTurn || BLOCK.Contains(m.Type.Id))) moves.Add(m.Type);
        if (moves.Count != 0)
        {
          atk.StartExecute(moves[aer.Controller.GetRandomInt(0, moves.Count - 1)]);
          return;
        }
      }
      atk.FailAll();
    }
  }
}
