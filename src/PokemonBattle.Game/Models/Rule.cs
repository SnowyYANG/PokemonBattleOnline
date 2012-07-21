using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using LightStudio.Tactic.DataModels;
using LightStudio.PokemonBattle.Data;

namespace LightStudio.PokemonBattle.Game
{
  public class Rule : GameElement
  {
    public Rule(int id, string name, string description) : base(id)
    {
      Name = name;
      Description = description;
    }
    public new string Description
    { get; private set; }

  }
}
