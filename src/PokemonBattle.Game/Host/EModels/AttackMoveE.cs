using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Data;
using LightStudio.PokemonBattle.Interactive;
using LightStudio.PokemonBattle.Interactive.GameEvents;

namespace LightStudio.PokemonBattle.Game
{
  public class AttackMoveE : MoveE
  {
    public AttackMoveE(MoveType move)
      : base(move)
    {
    }

    public override void Execute(PokemonProxy pm)
    {
      AtkContext atk = new AtkContext(pm);
      BuildTargets(atk);
      if (atk.Targets.Count() == 0) return;
      
      //发动属性宝石
      atk.Attacker.Item.ImplementMove(atk);
      //生成攻击次数
      if (Move.MinTimes != Move.MaxTimes) atk.Times = TIMES25[atk.Controller.GetRandomInt(0, 7)];
      else atk.Times = Move.MinTimes;


    }
    protected void PostEffect()
    {
    }
  }

}