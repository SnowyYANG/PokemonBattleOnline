using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Data;

namespace LightStudio.PokemonBattle.Game
{
  public class StatusMoveE : MoveE
  {
    public StatusMoveE(MoveType move)
      : base(move)
    {
    }

    public void Act() //1、2、3、5、A、B、C、D
    {
    }
    protected virtual void Override_Act() //D要override
    {
    }
  }
}