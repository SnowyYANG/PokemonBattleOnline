using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;
using LightStudio.PokemonBattle.Data;

namespace LightStudio.PokemonBattle.PBO.Battle
{
  class OPokemon2D : Pokemon2DBase
  {
    public OPokemon2D()
      : base(2)
    {
    }
    protected override BitmapImage GetImage(PokemonForm form, PokemonGender gender, bool shiny)
    {
      return GameDataService.GetPokemonBack(form, gender, shiny);
    }
    protected override BitmapImage GetSp(string id)
    {
      return GameDataService.GetSpBack(id);
    }
  }
}
