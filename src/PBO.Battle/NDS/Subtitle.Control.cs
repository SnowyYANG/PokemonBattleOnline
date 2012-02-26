using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Data;
using LightStudio.PokemonBattle.Game;
using LightStudio.PokemonBattle.Interactive;
using LightStudio.PokemonBattle.PBO.Battle.VM;

namespace LightStudio.PokemonBattle.PBO.Battle
{
  /// <summary>
  /// Interaction logic for Subtitle.xaml
  /// </summary>
  public partial class Subtitle : System.Windows.Controls.Border
  {
    private class Control
    {
      Subtitle nest;
      Dictionary<string, object> dictionary;
      //BlockTranslator translator;

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
              nest.SetText("等待对方玩家出招...");
              break;
            case ControlPanelIndex.STOP:
              nest.SetText("真的要中止战斗么？");
              break;
            default:
              try
              {
                if (cp.ControllingPokemon.Hp > 0)
                  nest.SetText(cp.ControllingPokemon.Name + "要做什么？");
              }
              catch { }
              break;
          }
        else if (e.PropertyName == "ControllingPokemon" && (cp.SelectedPanel == ControlPanelIndex.MAIN || cp.SelectedPanel == ControlPanelIndex.FIGHT))
          if (cp.ControllingPokemon != null && cp.ControllingPokemon.Hp > 0)
            nest.SetTextForcibly(cp.ControllingPokemon.Name + "要做什么？");
      }
      public void EventFinished()
      {
        nest.timer.Stop();
        //gameEventControl.Interrupt();
      }

      //void IUIInnerControl.Init(Game game)
      //{
        //gameEventControl = game.GameEventControl;
        //dictionary = gameEventControl.Dictionary;
        //translator = gameEventControl.Translator;
        //game.ControlPanel.PropertyChanged += ControlPanel_PropertyChanged;
      //}
      //void IUIInnerControl.EventOccurred(GameEvent e)
      //{
        //gameEventDictionary["e"] = e.GameEvent;
        //string expression = DataService.GameLog.GetGameLogExpression(e.GameEvent.Type);
        //if (expression != null)
        //{
        //  //ISegment[] s = SegmentReader.ReadSegments(expression, gameEventDictionary);
        //  //SetTextForcibly(s[0].ToString());
        //}
        //switch (e.Type)
        //{
        //  case EventType.BeginTurn:
        //    nest.SetTextForcibly("主机响应中...");
        //    break;
        //  case EventType.HpChange:
        //    break;
        //}
      //}
    }
  }
}
