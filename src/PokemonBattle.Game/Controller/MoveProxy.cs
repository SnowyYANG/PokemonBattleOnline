using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightStudio.PokemonBattle.Game
{
  internal class MoveProxy : Tactic.DataModels.GameElementProxy<IController, Move>, IMoveProxy
  {
    internal PokemonProxy Owner;
    
    public MoveProxy(IController controller, Move move, PokemonProxy owner)
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
