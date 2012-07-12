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
    protected override ImageSource GetMale(int id)
    {
      return DataService.Image.GetPokemonMaleFront(id);
    }
    protected override ImageSource GetFemale(int id)
    {
      return DataService.Image.GetPokemonFemaleFront(id);
    }
    protected override ImageSource GetSp(string id)
    {
      return DataService.Image.GetSpFront(id);
    }
  }
}
