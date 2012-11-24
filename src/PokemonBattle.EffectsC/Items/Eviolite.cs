using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightStudio.PokemonBattle.Game.Host.Effects.Items
{
  class Eviolite : ItemE
  {
    public Eviolite(int id)
      : base(id)
    {
    }
    public override Modifier DModifier(DefContext def)
    {
      var es = Data.GameDataService.GetEvolutions(def.Defender.Pokemon.Form.Type.Number);
      return (Modifier)(es.Any() ? 0x1800 : 0x1000);
    }
  }
}
