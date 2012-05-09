using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Interactive;

namespace LightStudio.PokemonBattle.Game
{
  internal abstract class ControllerComponent
  {
    protected readonly Controller Controller;

    protected ControllerComponent(Controller controller)
    {
      Controller = controller;
    }

    protected ReportBuilder ReportBuilder
    { get { return Controller.ReportBuilder; } }
    protected IGameSettings GameSettings
    { get { return Controller.Game.Settings; } }
    protected GameContext Game
    { get { return Controller.Game; } }
    protected Board Board
    { get { return Controller.Game.Board; } }
  }
}
