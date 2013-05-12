using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightStudio.PokemonBattle.Game.Host.Effects.Moves.Status
{
  class NaturePower : StatusMoveE
  {
    public NaturePower(int id)
      : base(id)
    {
    }
    public override void Execute(AtkContext atk)
    {
      Data.MoveType m;
      switch (atk.Controller.GameSettings.Terrain)
      {
        case Terrain.Path:
          m = Data.GameDataService.GetMove(89);//earthquake
          break;
        default:
          atk.Controller.ReportBuilder.Add("error");
          return;
      }
      atk.StartExecute(m, null, "NaturePower");
    }
  }
}
