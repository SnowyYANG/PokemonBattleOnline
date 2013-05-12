using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Data;

namespace LightStudio.PokemonBattle.Game
{
  public interface IPlayerController : IDisposable
  {
    event Action<InputRequest, int> RequireInput;
    
    SimPlayer Player { get; }
    SimGame Game { get; }

    void Input(ActionInput input);
    void Quit();
    bool RequestTie();
    bool AcceptTie();
    bool RejectTie();
  }
}
