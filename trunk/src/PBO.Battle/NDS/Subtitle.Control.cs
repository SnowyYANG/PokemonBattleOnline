using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonBattleOnline.Game;
using PokemonBattleOnline.Game;
using PokemonBattleOnline.PBO.Battle.VM;

namespace PokemonBattleOnline.PBO.Battle
{
  public partial class Subtitle : System.Windows.Controls.Border
  {
    private class Control
    {
      Subtitle nest;

      public Control(Subtitle subtitle)
      {
        nest = subtitle;
      }

      public void ControlPanel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
      {
        IControlPanel cp = sender as IControlPanel;
        if (e.PropertyName == null || e.PropertyName == "SelectedPanel")
          switch (cp.SelectedPanel)
          {
            case ControlPanelIndex.POKEMONS:
              nest.SetText("要让哪只精灵出场？");
              break;
            case ControlPanelIndex.INACTIVE:
              nest.SetText("通信待机中...");
              break;
            case ControlPanelIndex.STOP:
              nest.SetText("真的要中止战斗么？");
              break;
            default:
              try
              {
                if (cp.ControllingPokemon.Pokemon.Hp.Value > 0)
                  nest.SetText(cp.ControllingPokemon.Pokemon.Name + "要做什么？");
              }
              catch { }
              break;
          }
        else if (e.PropertyName == "ControllingPokemon" && (cp.SelectedPanel == ControlPanelIndex.MAIN || cp.SelectedPanel == ControlPanelIndex.FIGHT))
          if (cp.ControllingPokemon != null && cp.ControllingPokemon.Pokemon.Hp.Value > 0)
            nest.SetTextForcibly(cp.ControllingPokemon.Pokemon.Name + "要做什么？");
      }
      public void ControlPanel_InputFailed(string message)
      {
        if (message != null) nest.SetTextForcibly(message);
        else System.Windows.MessageBox.Show("InputFailed");
      }

      public void EventFinished()
      {
        nest.timer.Stop();
      }
    }
  }
}
