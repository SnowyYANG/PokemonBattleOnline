using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Data;
using LightStudio.PokemonBattle.Game;

namespace LightStudio.PokemonBattle.Interactive
{
  public class SimMove
  {
    internal readonly Move Move;
    public MoveType Type
    { get { return Move.Type; } }
    public PairValue PP
    { get { return Move.PP; } }
    /// <summary>
    /// CanSelect不代表技能一定能用，但我希望点过再出提示，http://www.smogon.com/dp/articles/move_restrictions#disable
    /// </summary>
    public bool CanBeSelected
    { get; internal set; }

    internal SimMove(Move move)
    {
      this.Move = move;
      CanBeSelected = PP.Value > 0;
    }

    public string Select()
    {
      if (PP.Value < 1) return string.Empty;
      return null;
    }
  }
}
