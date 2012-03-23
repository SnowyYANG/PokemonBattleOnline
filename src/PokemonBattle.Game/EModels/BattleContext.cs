using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Data;

namespace LightStudio.PokemonBattle.Game
{
  public class AtkContext
  {
    public readonly Controller Controller;
    public readonly PokemonProxy Attacker;
    public readonly MoveType MoveType;
    public IEnumerable<DefContext> Targets
    { get; private set; }
    public int AtkValue; //攻击力 × （攻击方等级 × 2 ÷ 5 + 2）
    public BattleType Type;
    public int Times;
    
    public AtkContext(Controller c, PokemonProxy pm, MoveType move)
    {
      Controller = c;
      Attacker = pm;
      MoveType = move;
    }
    public void SetTargets(IEnumerable<DefContext> targets)
    {
      Targets = targets;
    }
  }
  public class DefContext
  {
    public readonly AtkContext AtkContext;
    public readonly PokemonProxy Defender;
    public double BattleTypeBoost;
    public int DefValue; // 防御力 ÷ 50 
    
    public DefContext(AtkContext a, PokemonProxy pm)
    {
      AtkContext = a;
      Defender = pm;
    }
  }
}
