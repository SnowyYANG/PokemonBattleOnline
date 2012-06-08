using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightStudio.PokemonBattle.Game.Sp.Conditions
{
  public class Disable : PmCondition
  {
    private readonly int MoveId;
    
    public Disable(PokemonProxy pm, int moveId)
      : base("Disable", pm, 4)
    {
    }
    
    public override bool CanExecute()
    {
      //When a Pokémon uses the move Disable, it locks the last move executed by the target.
      //This lock prevents both the selection and execution of the move and remains in effect for 4-7 rounds or until the target leaves the field.
      //If the last action taken by the target was not an executed move, Disable fails.
      //If the targeted move has no PP left, Disable fails.
      //Only one move can be Disabled per Pokémon at any given time.
      if (MoveId == pm.SelectedMove.Type.Id)
      {
        pm.Controller.ReportBuilder.Add(new Interactive.GameEvents.ToPlate("Disable", pm));
        return false;
      }
      return true;
    }
    public override void EndTurn()
    {
      if (--count <= 0) Remove();
    }
  }
}
