﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using LightStudio.PokemonBattle.Game.Host;
using LightStudio.PokemonBattle.Data;

namespace LightStudio.PokemonBattle.Game
{
  [DataContract(Namespace = Namespaces.PBO)]
  public class PmInputRequest
  {
    internal static PmInputRequest Origin(Pokemon pm)
    {
      throw new NotImplementedException();
    }

    [DataMember(EmitDefaultValue = false)]
    int OnlyMove;

    [DataMember(EmitDefaultValue = false)]
    string Only; //choice/encore

    [DataMember(EmitDefaultValue = false)]
    string[] Block; //封印挑拨寻衅回复封印残废

    [DataMember(EmitDefaultValue = false)]
    bool CantWithdraw;

    [DataMember(EmitDefaultValue = false)]
    int CW_pm; //如果因为对方特性，但目前没试出显示特性的实例，逃跑似乎会显示特性交换不显示

    [DataMember(EmitDefaultValue = false)]
    int CW_a;

    internal PmInputRequest(PokemonProxy pm)
    {
      {
        int i = 0;
        bool struggle = true;
        foreach (var move in pm.Moves)
        {
          if (move.PP != 0)
          {
            var f = move.IfSelected();
            if (f == null) struggle = false;
            else
            {
              if (Block == null) Block = new string[pm.Moves.Count()];
              if (f.Move == move.Type.Id) Block[i] = f.Key;
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
          Only = null;
          OnlyMove = Host.Sp.Moves.STRUGGLE;
        }
      }
      {
        CantWithdraw = !pm.CanSelectWithdraw;
      }
    }
    public override bool Equals(object obj)
    {
      PmInputRequest i = obj as PmInputRequest;
      return
        OnlyMove == i.OnlyMove &&
        Only == i.Only &&
        Block.ArrayEquals(i.Block) &&
        CantWithdraw == i.CantWithdraw &&
        CW_pm == i.CW_pm &&
        CW_a == i.CW_a;
    }

    #region Client
    private SimGame Game;
    private SimPokemon Pm;
    private string error;
    private bool showAbility;
    public void Init(SimGame game, SimPokemon pm)
    {
      Game = game;
      Pm = pm;
    }
    public string GetErrorMessage()
    {
      string r = error;
      error = null;
      return r;
    }
    private void SetErrorMessage(string key, string arg1, string arg2)
    {
      var text = GameService.Logs["subtitle_" + key].Clone(Game.Outward);
      text.SetData(Pm.Pokemon.Name, arg1, arg2);
      error = text.Text;
    }
    /// <summary>
    /// 只在Rom数据里见过，游戏里没找到需要发动特性的地方
    /// </summary>
    public void TryRaiseAbility()
    {
      if (showAbility)
      {
        showAbility = false;
        Game.Outward.Board.ShowAbility(Game.Outward.GetPokemon(CW_pm), GameDataService.GetAbility(CW_a));
      }
    }
    public bool Fight()
    {
      return OnlyMove == PokemonBattle.Game.Host.Sp.Moves.STRUGGLE;
    }
    /// <summary>
    /// 不判断PP数及技能是否存在
    /// </summary>
    /// <param name="move"></param>
    /// <returns></returns>
    public bool Move(SimMove move)
    {
      if (OnlyMove != 0 && OnlyMove != move.Type.Id) SetErrorMessage(Only, GameDataService.GetMove(OnlyMove).GetLocalizedName(), Pm.Pokemon.Item == null ? null : Pm.Pokemon.Item.GetLocalizedName());
      else
        if (Block != null)
          for (int i = 0; i < Pm.Moves.Length; ++i)
            if (move == Pm.Moves[i])
            {
              if (Block[i] != null) SetErrorMessage(Block[i], move.Type.GetLocalizedName(), null);
              break;
            }
      return error == null;
    }
    public void Target(PokemonOutward target = null)
    {
#if DEBUG
      throw new NotImplementedException();
#endif
    }
    /// <summary>
    /// 判断pokemon是否在场和Hp
    /// </summary>
    /// <param name="pokemon"></param>
    /// <returns></returns>
    public bool Pokemon(Pokemon pokemon)
    {
      if (pokemon.Hp.Value == 0)
      {
        error = string.Format(DataService.String["{0} has no strength to fight!"], pokemon.Name);
        return false;
      }
      if (pokemon.IndexInOwner < Game.Settings.Mode.OnboardPokemonsPerPlayer())
      {
        error = string.Format(DataService.String["{0} is already fighting."], pokemon.Name);
        return false;
      }
      if (CantWithdraw)
      {
        error = string.Format(DataService.String["Can't withdraw {0}!"], Pm.Pokemon.Name);
        if (CW_a != 0) showAbility = true;
        return false;
      }
      return true;
    }
    #endregion
  }
  
  [DataContract(Namespace = Namespaces.PBO)]
  public class InputRequest
  {
    internal static InputRequest Origin()
    {
      throw new NotImplementedException();
    }

    [DataMember(EmitDefaultValue = false)]
#if DEBUG
    public
#endif
    PmInputRequest[] Pms;

    [DataMember(EmitDefaultValue = false)]
#if DEBUG
    public
#endif
    int[] Xs;

    internal InputRequest(IEnumerable<PokemonProxy> pokemons = null)
    {
      if (pokemons != null)
        Pms = pokemons.Select((p) => new PmInputRequest(p)).ToArray();
    }
    internal InputRequest(params Tile[] tiles)
    {
      Xs = tiles.Select((t)=>t.X).ToArray();
    }

    public override bool Equals(object obj)
    {
      InputRequest ri = obj as InputRequest;
      return ri != null && Pms.ArrayEquals(ri.Pms);
    }

    #region PlayerClient
    public event Action<ActionInput> InputFinished;
    private ActionInput input;
    private SimGame game;
    private string error;

    public bool IsSendout
    { get { return Pms == null; } }
    public int CurrentX
    { get; private set; }

    private void NextPm()
    {
      while (CurrentX < Pms.Length)
        if (Pms[CurrentX++] != null) break;
      if (CurrentX == Pms.Length) InputFinished(input);
    }
    private void CheckSendoutFinished()
    {
#if MULTI
      if (Tiles.Length == 0)
      {
      }
      else
#endif
      {
        InputFinished(input);
      }
    }
    public void Init(SimGame game)
    {
      this.game = game;
      CurrentX = -1;
      if (Pms != null)
        for (int i = 0; i < Pms.Length; ++i)
          if (Pms[i] != null)
          {
            Pms[i].Init(game, game.OnboardPokemons[i]);
            if (CurrentX == -1) CurrentX = i;
          }
      input = new ActionInput(game.Settings.Mode.XBound());
    }
    public string GetErrorMessage()
    {
      return error;
    }
    public void TryRaiseAbility()
    {
      Pms[CurrentX].TryRaiseAbility();
    }
    public bool Fight()
    {
      if (Pms[CurrentX].Fight())
      {
        input.Struggle(CurrentX);
        NextPm();
        return true;
      }
      error = Pms[CurrentX].GetErrorMessage();
      return false;
    }
    public bool Move(SimMove move)
    {
      if (Pms[CurrentX].Move(move))
      {
        if (!game.Settings.Mode.NeedTarget())
        {
          input.UseMove(CurrentX, move);
          NextPm();
        }
        return true;
      }
      error = Pms[CurrentX].GetErrorMessage();
      return false;
    }
    public void Target(PokemonOutward target = null)
    {
      Pms[CurrentX].Target(target);
    }
    public bool Pokemon(Pokemon pokemon, int x)
    {
      if (pokemon.Hp.Value == 0)
      {
        error = string.Format(DataService.String["{0} has no strength to fight!"], pokemon.Name);
        return false;
      }
      if (pokemon.IndexInOwner < game.Settings.Mode.OnboardPokemonsPerPlayer())
      {
        error = string.Format(DataService.String["{0} is already fighting."], pokemon.Name);
        return false;
      }
      input.Sendout(x, pokemon);
      CheckSendoutFinished();
      return true;
    }
    public bool Pokemon(Pokemon pokemon)
    {
      if (Pms[CurrentX].Pokemon(pokemon))
      {
        input.Switch(CurrentX, pokemon);
        NextPm();
        return true;
      }
      error = Pms[CurrentX].GetErrorMessage();
      return false;
    }
    public void Undo()
    {
#if DEBUG
      throw new NotImplementedException();
#endif
    }
    //void TurnLeft();
    //void TurnRight();
    //void MoveToCenter();
    #endregion
  }
}
