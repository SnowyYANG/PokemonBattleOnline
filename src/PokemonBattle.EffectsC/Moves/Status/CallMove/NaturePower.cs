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
    public override void InitAtkContext(AtkContext atk)
    {
      switch (atk.Controller.GameSettings.Terrain)
      {
        case Terrain.Path:
          atk.Move = Data.GameDataService.GetMove(89);//earthquake
          break;
      }
      atk.Controller.ReportBuilder.Add("NaturePower", atk.Move);
    }
  }
}
