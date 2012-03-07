using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightStudio.PokemonBattle.Game
{
  public class MoveProxy : Tactic.DataModels.GameElementProxy<Controller, Move>
  {
    internal PokemonProxy Owner;
    
    internal MoveProxy(Controller controller, Move move, PokemonProxy owner)
      : base(controller, move)
    {
      Owner = owner;
    }

    public int Id
    { get { return Model.Id; } }
    public int PP
    { get { return Model.PP.Value; } }
    public int Priority
    { get { return Model.Type.Priority; } }
    public Tile SelectedTarget
    { get; internal set; }

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
    public bool CanBeExecuted
    { 
      get
      { 
        
        return PP > 0;
      }
    }

    //在特效接口处
    //public Position[] GetSelectTarget(Position position)
    //{
    //  if (position == null)
    //    switch (Model.Type.Range)
    //    {
    //    }
    //  return new Position[] { position };
    //}
  }
}
