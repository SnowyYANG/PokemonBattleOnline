using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Data;

namespace LightStudio.PokemonBattle.PBO.Editor
{
  internal class TeamVM : BTVM
  {
    static TeamVM()
    {
    }

    public TeamVM(PokemonBT model)
      : base(model)
    {
      model.CollectionChanged += (sender, e) =>
        {
          //TODO: I need decompile!!! I need document!!!
          if (e.NewStartingIndex == 0) OnPropertyChanged("Icon");
        };
    }

    PokemonData lastFirst;
    public override object Icon
    {
      get
      {
        var pm = Model.FirstOrDefault();
        return pm == null ? null : ImageService.GetPokemonIcon(pm.Form, pm.Gender);
      }
    }
    public override object BorderBrush
    { get { return PBO.UIElements.Brushes.MagentaM; } }
    public override object Effect
    { get { return Model.CanAdd ? Resources.MagentaShadow : null; } }

    protected override void Remove()
    {
      DataService.UserData.Teams.Remove((PokemonBT)Model);
    }
  }
}
