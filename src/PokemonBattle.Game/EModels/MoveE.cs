using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Data;

namespace LightStudio.PokemonBattle.Game
{
  public interface IMoveE
  {
    void Prepare();
    void Pre_Act();
    void Act();
  }

  public abstract class MoveE : IMoveE
  {
    protected readonly MoveType MoveType;

    protected MoveE(MoveType moveType)
    {
      MoveType = moveType;
    }

    public void Prepare() { } //追击new
    public void Pre_Act() { } //气合拳new
    public void Act()
    {
      if (CanExecute()) Execute();
    }

    protected PokemonProxy[] GetTargets(Tile selectTarget)
    {
      return null;
    }
    public bool CanExecute() //梦话new
    {
      throw new NotImplementedException();
    }
    public void Execute()
    {
    }
    protected bool CanImplement()
    {
      double type = TypeBoost();
      return type != 0;
    }
    protected virtual double TypeBoost()
    {
      switch (MoveType.Class)
      {
        case MoveInnerClass.AddState:
        case MoveInnerClass.Attack:
        case MoveInnerClass.AttackAndAbsorb:
        case MoveInnerClass.AttackWithSelfLv7DChange:
        case MoveInnerClass.AttackWithState:
        case MoveInnerClass.AttackWithTargetLv7DChange:
        case MoveInnerClass.OHKO:
          return 0;
      }
      return 1;
    }
  }

  public class StatusMoveE : MoveE
  {
    public StatusMoveE(MoveType move)
      : base(move)
    {
    }
    
    public void Act() //1、2、3、5、A、B、C、D
    {
    }
    protected virtual void Override_Act() //D要override
    {
    }
    protected override double TypeBoost() //嗅觉改写
    {
      if (MoveType.Class == MoveInnerClass.AddState) ;
      return 1;
    }
  }

  public class AttackMoveE : MoveE
  {
    public AttackMoveE(MoveType move)
      : base(move)
    {
    }
    
    public void Act()
    {
    }
    protected void PostEffect()
    {
    }
  }
}
