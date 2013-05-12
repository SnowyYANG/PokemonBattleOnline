using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightStudio.PokemonBattle.Game.Host.Effects.Moves
{
  class BatonPass : StatusMoveE
  {
    public BatonPass(int id)
      : base(id)
    {
    }

    protected override void Act(AtkContext atk)
    {
      var aer = atk.Attacker;
      var t = aer.Tile;
      var o = aer.OnboardPokemon;
      if (aer.Controller.Withdraw(aer, "SelfWithdraw", false))
      {
        t.SetCondition("BatonPass", o);
        aer.Controller.PauseForSendoutInput(t);
      }
      else atk.FailAll();
    }
  }
}
