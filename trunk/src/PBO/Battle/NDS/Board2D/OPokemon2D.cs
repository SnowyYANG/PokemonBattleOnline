using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;
using PokemonBattleOnline.Game;

namespace PokemonBattleOnline.PBO.Battle
{
  class OPokemon2D : Pokemon2DBase
  {
    public OPokemon2D()
      : base(2)
    {
    }
    protected override BitmapImage GetImage(PokemonForm form, PokemonGender gender, bool shiny)
    {
      return ImageService.GetPokemonBack(form, gender, shiny);
    }
    protected override BitmapImage GetSp(string id)
    {
      return ImageService.GetSpBack(id);
    }
  }
}
