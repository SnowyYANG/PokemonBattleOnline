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
    protected readonly TurnBuilder TurnBuilder;
    protected readonly GameSettings GameSettings;
    protected readonly GameContext Game;
    protected readonly Board Board;

    protected ControllerComponent(Controller controller)
    {
      Controller = controller;
      TurnBuilder = controller.TurnBuilder;
      GameSettings = controller.Game.Settings;
      Game = controller.Game;
      Board = controller.Board;
    }
  }
}
