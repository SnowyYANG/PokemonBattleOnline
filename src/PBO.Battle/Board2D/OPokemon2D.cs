using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using LightStudio.PokemonBattle.Data;

namespace LightStudio.PokemonBattle.PBO.Battle
{
  class OPokemon2D : Pokemon2DBase
  {
    public OPokemon2D()
      : base(192)
    {
    }
    protected override ImageSource GetMale(int id)
    {
      return DataService.Image.GetPokemonMaleBack(id);
    }
    protected override ImageSource GetFemale(int id)
    {
      return DataService.Image.GetPokemonFemaleBack(id);
    }
    protected override ImageSource GetSp(string id)
    {
      return DataService.Image.GetSpBack(id);
    }
  }
}
