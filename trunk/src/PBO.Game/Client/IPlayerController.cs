using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonBattleOnline.Data;

namespace PokemonBattleOnline.Game
{
  public interface IPlayerController : IDisposable
  {
    event Action<InputRequest, int> RequireInput;
    
    SimPlayer Player { get; }
    SimGame Game { get; }

    void Input(ActionInput input);
    void GiveUp();
    /// <summary>
    /// 拔网线，增加一次断线率
    /// </summary>
    void PullCable();
  }
}
