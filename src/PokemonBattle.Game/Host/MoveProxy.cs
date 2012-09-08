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
        var pp = (PairValue)Move.PP;
        if (value < 0) pp.Value = 0;
        else if (value > Move.PP.Origin) pp.Value = Move.PP.Origin;
        else pp.Value = value;
      }
    }
    public bool HasUsed
    { get; internal set; }

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
      var um = new UseMove(Owner, Type);
      Owner.Controller.ReportBuilder.Add(um);
      e.Execute(Owner, um);
      HasUsed = true;
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
