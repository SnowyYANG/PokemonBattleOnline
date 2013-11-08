using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonBattleOnline.Game.GameEvents;

namespace PokemonBattleOnline.Game.Host
{
  internal class MoveProxy
  {
    public readonly Move Move;
    public readonly PokemonProxy Owner;
    
    internal MoveProxy(Move move, PokemonProxy owner)
    {
      Move = move;
      Owner = owner;
    }
    internal MoveProxy(MoveType move, PokemonProxy owner)
    {
      Move = new Move(move, 5);
      Owner = owner;
    }

    public int PP
    { 
      get { return Move.PP.Value; }
      set
      {
        var pp = (PairValue)Move.PP;
        if (value < 0) value = 0;
        else if (value > pp.Origin) value = pp.Origin;
        if (pp.Value != value)
        {
          pp.Value = value;
          Owner.Controller.ReportBuilder.SetPP(this);
        }
      }
    }
    public bool HasUsed
    { get; internal set; }

    public MoveType Type
    { get { return Move.Type; } }
    public bool CanBeSelected
    { get { return PP > 0 && IfSelected() == null; } }
    
    /// <summary>
    /// CanSelect不代表技能一定能用，http://www.smogon.com/dp/articles/move_restrictions#disable
    /// </summary>
    /// <returns>key, null if no problem</returns>
    internal SelectMoveFail IfSelected()
    {
      if (Type.Id != Ms.STRUGGLE)
      {
        var op = Owner.OnboardPokemon;
        //专爱
        if (ITs.ChoiceItem(Owner.Item))
        {
          var o = op.GetCondition<MoveType>("ChoiceItem");
          if (o != null && o != Type) return new SelectMoveFail("ChoiceItem", o.Id);
        }
        //寻衅
        if (op.HasCondition("Torment") && Owner.LastMove == Type) return new SelectMoveFail("Torment", Owner.AtkContext.MoveProxy.Type.Id);
        //鼓掌
        {
          var o = op.GetCondition("Encore");
          if (o != null && o.Move != Type) return new SelectMoveFail("Encore", Owner.AtkContext.MoveProxy.Type.Id);
        }
        //封印
        foreach (var pm in Owner.Controller.Board[1 - Owner.Pokemon.TeamId].GetPokemons(Owner.OnboardPokemon.X - 1, Owner.OnboardPokemon.X + 1))
          if (pm.OnboardPokemon.HasCondition("Imprison"))
            foreach (var m in pm.Moves)
              if (m.Type == Type) return new SelectMoveFail("Imprison", Type.Id);
        //残废
        {
          var o = op.GetCondition("Disable");
          if (o != null && o.Move == Type) return new SelectMoveFail("Disable", Type.Id);
        }
        //重力
        if (Type.Flags.UnavailableWithGravity && Owner.Controller.Board.HasCondition("Gravity")) return new SelectMoveFail("GravityCantUseMove", Type.Id);
        //回复封印
        if (Type.Flags.IsHeal && op.HasCondition("HealBlock")) return new SelectMoveFail("HealBlockCantUseMove", Type.Id);
        //挑拨
        if (Type.Category == MoveCategory.Status && op.HasCondition("Taunt")) return new SelectMoveFail("Taunt", Type.Id);
      }
      return null;
    }

    internal void Execute()
    {
      if (ITs.ChoiceItem(Owner.Item)) Owner.OnboardPokemon.AddCondition("ChoiceItem", Type);
      Owner.AddReportPm("UseMove", Type.Id);
      Owner.BuildAtkContext(this);
      Owner.AtkContext.StartExecute(Type, Owner.SelectedTarget, null);
      HasUsed = true;
    }

    internal bool CanExecute()
    {
      if (Type.Id != Ms.STRUGGLE)
      {
        if (PP == 0)
        {
          Owner.Controller.ReportBuilder.ShowLog("NoPP");
          return false;
        }
        PP--;
      }
      return true;
    }
  }
}
