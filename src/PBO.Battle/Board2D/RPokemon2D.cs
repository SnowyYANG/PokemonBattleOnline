using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using LightStudio.PokemonBattle.Data;

namespace LightStudio.PokemonBattle.PBO.Battle
{
  class RPokemon2D : Pokemon2DBase
  {
    public RPokemon2D()
      : base(96)
    {
    }
    protected override ImageSource GetImage(PokemonForm form, PokemonGender gender)
    {
      return ImageService.GetPokemonFront(form, gender);
    }
    protected override ImageSource GetSp(string id)
    {
      return ImageService.GetSpFront(id);
    }
  }
}
