using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Data;

namespace LightStudio.PokemonBattle.Game.Host
{
  public class Condition
  {
    public PokemonProxy By;
    public PokemonProxy Pm;
    public MoveProxy MoveProxy;
    public MoveType Move;
    public Tile Tile;
    public AtkContext Atk;
    public int Turn;
    public int Damage;
    public int Int;
    public bool Bool;
  }
}
