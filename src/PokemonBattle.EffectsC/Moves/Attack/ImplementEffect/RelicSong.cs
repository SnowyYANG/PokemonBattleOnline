using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokemonBattleOnline.Game.Host.Effects.Moves.Attack
{
  class RelicSong : AttackMoveE
  {
    public RelicSong(int id)
      : base(id)
    {
    }
    protected override void ImplementEffect(DefContext def)
    {
      base.ImplementEffect(def);
      var aer = def.AtkContext.Attacker;
      if (aer.CanChangeForm(648)) aer.ChangeForm(1 - aer.OnboardPokemon.Form.Index);
    }
  }
}
