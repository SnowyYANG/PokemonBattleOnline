using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Controls.Primitives;
using PokemonBattleOnline.Data;
using PokemonBattleOnline.PBO.UIElements.Interactivity;

namespace PokemonBattleOnline.PBO.Editor
{
  internal class PokemonDragDropData : ObservableObject, IDragDropData
  {
    public PokemonData Pokemon
    { get; private set; }

    public int IndexInContainer
    { get; private set; }

    public DragDropActions AllowedActions
    { get; private set; }

    public PokemonCollection Source
    { get; private set; }

    private DragDropActions _actions = DragDropActions.None;
    public DragDropActions Actions
    {
      get { return _actions; }
      set
      {
        value = value & AllowedActions;
        if (_actions != value)
        {
          _actions = value;
          OnPropertyChanged("Actions");
        }
      }
    }

    public PokemonDragDropData(PokemonData data, DragDropActions allowedActions, PokemonCollection source, int pmIndex)
    {
      this.Pokemon = data;
      this.AllowedActions = allowedActions;
      this.Source = source;
      this.IndexInContainer = pmIndex;
    }

    object IDragDropData.Data
    { get { return Pokemon; } }

    object IDragDropData.Source
    { get { return Source; } }
  }
}
