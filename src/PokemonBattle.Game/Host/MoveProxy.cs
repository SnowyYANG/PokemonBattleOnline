using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightStudio.PokemonBattle.Game
{
  public class MoveProxy
  {
    public readonly Move Move;
    public readonly PokemonProxy Owner;
    private readonly IMoveE e;
    
    internal MoveProxy(Move move, PokemonProxy owner)
    {
      Move = move;
      Owner = owner;
      e = GameService.GetMove(move.Type.Id);
    }

    public int Id
    { get { return Move.Id; } }
    public int PP
    { 
      get { return Move.PP.Value; }
      set
      {
        if (value < 0) value = 0;
        Move.PP.Value = value;
      }
    }
    public Data.MoveType Type
    { get { return Move.Type; } }
    public int Priority
    { get { return Move.Type.Priority; } }

    /// <summary>
    /// CanSelect不代表技能一定能用，http://www.smogon.com/dp/articles/move_restrictions#disable
    /// </summary>
    public bool CanBeSelected
    { 
      get
      {
        //事件事件事件...
        return PP > 0;
      }
    } //PP>0

    public void Execute()
    {
      Owner.Controller.ReportBuilder.Add(new Interactive.GameEvents.UseMove(this));
      e.Execute(Owner);
    }
  }
}
