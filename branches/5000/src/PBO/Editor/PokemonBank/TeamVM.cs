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
    private static object NORMALBG;
    static TeamVM()
    {
      NORMALBG = SBrushes.NewBrush(0xff808080);
    }
    
    public readonly PokemonTeam Model;

    public TeamVM(PokemonTeam model)
    {
      Model = model;
      raw = new PokemonVM[6];
      for (int i = 0; i < 6; ++i) raw[i] = new PokemonVM(this, i);
      _background = NORMALBG;
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

    public bool CanBattle
    {
      get { return Model.CanBattle; }
      set
      {
        if (Model.CanBattle != value)
        {
          Model.CanBattle = value;
          if (value) EditorVM.Current.BattleTeams.Add(Model);
          else EditorVM.Current.BattleTeams.Remove(Model);
          OnPropertyChanged("CanBattle");
        }
      }
    }

    private readonly PokemonVM[] raw;
    public PokemonVM this[int index]
    { get { return raw.ValueOrDefault(index); } }

    public object BorderBrush
    { get { return SBrushes.MagentaM; } }

    private object _background;
    public object Background
    { 
      get { return _background; }
      set
      {
        if (_background != value)
        {
          _background = value;
          OnPropertyChanged("Background");
        }
      }
    }
    
    public object Effect
    { get { return Model.Pokemons.Contains(null) ? R.MagentaShadow : null; } }
  }
}
