using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using LightStudio.PokemonBattle.Game.Host;
using LightStudio.PokemonBattle.Data;

namespace LightStudio.PokemonBattle.Game
{
  [DataContract(Namespace = Namespaces.LIGHT)]
  public class RequirePmInput
  {
    internal static RequirePmInput Origin(Pokemon pm)
    {
      throw new NotImplementedException();
    }
    
    [DataMember(EmitDefaultValue = false)]
    int[] Moves;

    [DataMember(EmitDefaultValue = false)]
    int OnlyMove;

    [DataMember(EmitDefaultValue = false)]
    string Only; //choice/encore

    [DataMember(EmitDefaultValue = false)]
    string[] Block; //封印挑拨寻衅回复封印残废

    [DataMember(EmitDefaultValue = false)]
    string CantWithdraw;

    [DataMember(EmitDefaultValue = false)]
    int CW_pm; //如果因为对方特性

    [DataMember(EmitDefaultValue = false)]
    int CW_a;

    internal RequirePmInput(PokemonProxy pm)
    {
      {
        int i = 0;
        bool struggle = true;
        Moves = new int[pm.Moves.Count()];
        foreach (var move in pm.Moves)
        {
          Moves[i] = move.Type.Id;
          if (move.PP != 0)
          {
            var f = move.IfSelected();
            if (f == null) struggle = false; //PP不为0且IfSelect为null肯定是可以选择了
            else
            {
              if (f.Move == Moves[i]) Block[i] = f.Key;
              else if (Only == null)
              {
                Only = f.Key;
                OnlyMove = f.Move;
              }
            }
          }
          i++;
        }
        if (struggle)
        {
          Block = null;
          Only = null; //就是说如果有鼓掌效果但又没PP的话，用IfSelect可能还是会返回一个Only的Fail
          OnlyMove = Host.Sp.Moves.STRUGGLE;
        }
      }
      {
        //CantWithdraw
        //CW_pm;
        //CW_a;
      }
    }

    public override bool Equals(object obj)
    {
      RequirePmInput i = obj as RequirePmInput;
      return
        Moves.ArrayEquals(i.Moves) &&
        OnlyMove == i.OnlyMove &&
        Only == i.Only &&
        Block.ArrayEquals(i.Block) &&
        CantWithdraw == i.CantWithdraw &&
        CW_pm == i.CW_pm &&
        CW_a == i.CW_a;
    }
  }
  
  [DataContract(Namespace = Namespaces.LIGHT)]
  public class RequireInput
  {
    internal static RequireInput Origin()
    {
      throw new NotImplementedException();
    }

    [DataMember(EmitDefaultValue = false)]
    RequirePmInput[] Pms;

    internal RequireInput(IEnumerable<PokemonProxy> pokemons)
    {
      Pms = pokemons.Select((p) => new RequirePmInput(p)).ToArray();
    }
    internal RequireInput()
    {
    }

    internal bool Input(Controller Controller, ActionInput action)
    {
      throw new NotImplementedException();
    }

    public override bool Equals(object obj)
    {
      RequireInput ri = obj as RequireInput;
      return ri != null && Pms.ArrayEquals(ri.Pms);
    }

    #region PlayerClient
    public event Action<ActionInput> InputFinished;
    public event Action<int> NextPm; //int是X

    public string GetErrorMessage(GameOutward game)
    {
      throw new NotImplementedException();
    }
    public void RaiseAbility(GameOutward game)
    {
    }

    public void Fight()
    {
    }
    public void Move()
    {
    }
    public void Target()
    {
    }
    public void Pokemon()
    {
    }
    public void Undo()
    {
    }
    //void TurnLeft();
    //void TurnRight();
    //void MoveToCenter();
    #endregion
  }
}
