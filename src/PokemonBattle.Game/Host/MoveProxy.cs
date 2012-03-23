using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightStudio.PokemonBattle.Game
{
  public class MoveProxy
  {
    public readonly Move Move;
    internal readonly PokemonProxy Owner;
    protected readonly Controller Controller;
    
    internal MoveProxy(Controller controller, Move move, PokemonProxy owner)
    {
      Controller = controller;
      Move = move;
      Owner = owner;
    }

    public int Id
    { get { return Move.Id; } }
    public int PP
    { get { return Move.PP.Value; } }
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

    //在特效接口处
    //public Position[] GetSelectTarget(Position position)
    //{
    //  if (position == null)
    //    switch (Model.Type.Range)
    //    {
    //    }
    //  return new Position[] { position };
    //}
    public void Act(Tile selectTarget)
    {
    }
  }
}
