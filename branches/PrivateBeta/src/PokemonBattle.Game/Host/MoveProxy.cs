using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Game.GameEvents;
using LightStudio.PokemonBattle.Game.Host.Sp;
using LightStudio.PokemonBattle.Data;

namespace LightStudio.PokemonBattle.Game.Host
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
      Move = new Move(move);
      Owner = owner;
    }

    public int PP
    { 
      get { return Move.PP.Value; }
      set
      {
        var pp = (PairValue)Move.PP;
        if (value < 0) pp.Value = 0;
        else if (value > pp.Origin) pp.Value = pp.Origin;
        else pp.Value = value;
      }
    }
    public bool HasUsed
    { get; internal set; }

    public MoveType Type
    { get { return Move.Type; } }
    public int Priority
    { get { return Move.Type.Priority; } }
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
        if (Is.ChoiceItem(Owner.Item))
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
        foreach (var pm in Owner.Controller.Board[1 - Owner.Pokemon.TeamId].Pokemons)
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
      if (Is.ChoiceItem(Owner.Item)) Owner.OnboardPokemon.AddCondition("ChoiceItem", Type);

      UseMove um = null;
      int pp = 0;
      if (Type.Id == Ms.STRUGGLE) Owner.AddReportPm("UseMove", Ms.STRUGGLE);
      else
      {
        um = new UseMove(Owner, Type);
        Owner.Controller.ReportBuilder.Add(um);
        pp = PP;
      }
      
      Owner.BuildAtkContext(this);
      Owner.AtkContext.StartExecute(Type, Owner.SelectedTarget, null);
      
      if (um != null) um.PP = pp - PP;
      HasUsed = true;
    }

    internal bool CanExecute()
    {
      if (Type.Id != Ms.STRUGGLE)
      {
        if (PP == 0)
        {
          Owner.Controller.ReportBuilder.Add("NoPP");
          return false;
        }
        PP--;
      }
      return true;
    }
  }
}
