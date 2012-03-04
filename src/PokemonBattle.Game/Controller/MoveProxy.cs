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

    public bool CanBeSelected
    { get { throw new NotImplementedException(); } } //PP>0
    public bool CanBeExecuted
    { get { throw new NotImplementedException(); } }

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
