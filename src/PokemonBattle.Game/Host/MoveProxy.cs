using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Game.GameEvents;

namespace LightStudio.PokemonBattle.Game.Host
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
      e = EffectsService.GetMove(move.Type.Id);
    }

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
    public bool CanBeSelected
    { get { return PP > 0 && IfSelected() == null; } }
    
    /// <summary>
    /// CanSelect不代表技能一定能用，http://www.smogon.com/dp/articles/move_restrictions#disable
    /// </summary>
    /// <returns>key, null if no problem</returns>
    internal SelectMoveFail IfSelected()
    { 
      //事件事件事件...
      //return PP > 0; //这是不真实的，PP>0才不告诉客户端呢你自己猜去吧！
      return null;
    }

    public void Execute()
    {
      Owner.Controller.ReportBuilder.Add(new UseMove(Owner, Type));
      e.Execute(Owner);
    }

    internal bool CanExecute()
    {
      if (Type.Id != Sp.Moves.STRUGGLE)
      {
        if (PP < 1)
        {
          Owner.Controller.ReportBuilder.Add("NoPP");
          return false;
        }
        PP--;
      }
      return true;
    }
  }
}
