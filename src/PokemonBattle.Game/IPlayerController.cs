using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Data;
using LightStudio.PokemonBattle.Interactive;

namespace LightStudio.PokemonBattle.Game
{
  public interface IPlayerControllerEvents
  {
    void RequireInput();
    void InputResult(bool ok, string message, bool allDone);
    void TieRequested();
    void TieRejected();
    void TimeElapsed(int remainingSeconds);
    void TimeUp(); //这个应该是只告诉玩家本人的
  }
  public interface IPlayerController : IDisposable
  {
    Player Player { get; }
    SimGame Game { get; }

    void UseMove(byte x, SimMove move, Tile tile = null);
    void Sendout(byte x, Pokemon sendout);
    void Struggle(byte x);
    //void TurnLeft();
    //void TurnRight();
    //void MoveToCenter(byte x);

    void Quit();
    bool RequestTie();
    bool AcceptTie();
    bool RejectTie();

    void AddEventsListener(IPlayerControllerEvents listner);
  }
}
