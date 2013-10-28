using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;
using PokemonBattleOnline.Game;

namespace PokemonBattleOnline.PBO.Battle
{
  class RPokemon2D : Pokemon2DBase
  {
    public RPokemon2D()
      : base(1)
    {
    }
    protected override BitmapImage GetImage(PokemonForm form, PokemonGender gender, bool shiny)
    {
      return ImageService.GetPokemonFront(form, gender, shiny);
    }
    protected override BitmapImage GetSp(string id)
    {
      return ImageService.GetSpFront(id);
    }
  }
}
