using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Data;
using LightStudio.PokemonBattle.Interactive;
using LightStudio.PokemonBattle.Interactive.GameEvents;

namespace LightStudio.PokemonBattle.Game
{
  public class AttackMoveE : MoveE
  {
    public AttackMoveE(MoveType move)
      : base(move)
    {
    }

    public void Act()
    {
    }
    protected void PostEffect()
    {
    }
  }

}