using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonBattleOnline.Game;

namespace PokemonBattleOnline.PBO.Editor
{
  internal class TeamVM : BTVM
  {
    public TeamVM(PokemonBT model)
      : base(model)
    {
      model.CollectionChanged += (sender, e) =>
        {
          if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Reset || e.NewStartingIndex == 0 || e.OldStartingIndex == 0)
            OnPropertyChanged("Icon");
        };
    }

    public override object Icon
    {
      get
      {
        var pm = Model.FirstOrDefault();
        return pm == null ? null : GameDataService.GetPokemonIcon(pm.Form, pm.Gender);
      }
    }
    public override System.Windows.Thickness IconMargin
    { get { return new System.Windows.Thickness(); } }
    public override object BorderBrush
    { get { return PBO.UIElements.Brushes.MagentaM; } }
    public override object Effect
    { get { return Model.CanAdd ? Resources.MagentaShadow : null; } }

    protected override void Remove()
    {
      base.Remove();
      DataService.UserData.Teams.Remove((PokemonBT)Model);
    }
  }
}
