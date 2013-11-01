using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonBattleOnline.Game;
using PokemonBattleOnline.PBO.Elements;

namespace PokemonBattleOnline.PBO.Editor
{
  class TeamVM : ObservableObject
  {
    public static TeamVM New;
    
    public readonly PokemonTeam Model;

    public TeamVM(PokemonTeam model)
    {
      Model = model;
      raw = new PokemonVM[6];
      for (int i = 0; i < 6; ++i) raw[i] = new PokemonVM(this, i);
    }

    public string Name
    {
      get { return Model.Name; }
      set
      {
        if (Model.Name != value)
        {
          Model.Name = value;
          OnPropertyChanged("Name");
        }
      }
    }

    private readonly PokemonVM[] raw;
    public PokemonVM this[int index]
    { get { return raw.ValueOrDefault(index); } }

    public object BorderBrush
    { get { return SBrushes.MagentaM; } }

    public object Background
    { get { return SBrushes.NewBrush(0xff808080); } }
    
    public object Effect
    { get { return Model.Pokemons.Contains(null) ? R.MagentaShadow : null; } }
  }
}
