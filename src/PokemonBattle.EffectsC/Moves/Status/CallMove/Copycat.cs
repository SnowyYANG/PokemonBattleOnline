using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightStudio.PokemonBattle.Game.Host.Effects.Moves.Status
{
  class Copycat : StatusMoveE
  {
    #region BLOCK
    private static readonly int[] BLOCK = new int[]
    {
      182,
      197,
      383,
      102,
      166,
      271,
      415,
      509,
      525,
      194,
      364,
      264,
      165
    };
    #endregion

    public Copycat(int id)
      : base(id)
    {
    }

    public override void Execute(AtkContext atk)
    {
      var o = atk.Controller.Board.GetCondition("LastMove");
      if (o == null || BLOCK.Contains(o.Move.Id)) atk.FailAll();
      else atk.StartExecute(o.Move);
    }
  }
}
