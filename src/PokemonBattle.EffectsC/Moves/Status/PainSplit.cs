using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightStudio.PokemonBattle.Game.Host.Effects.Moves.Status
{
  class PainSplitEvent : GameEvent
  {
    protected override void Update()
    {
      base.Update();
    }
    public override void Update(SimGame game)
    {
      base.Update(game);
    }
  }
  class PainSplit : StatusMoveE
  {
    static PainSplit()
    {
      EffectsService.Register<PainSplitEvent>();
    }

    public PainSplit(int id)
      : base(id)
    {
    }
  }
}
