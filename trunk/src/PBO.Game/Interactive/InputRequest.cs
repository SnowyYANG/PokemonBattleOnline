using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using PokemonBattleOnline.Game;

namespace PokemonBattleOnline.Game
{
  [Flags]
  public enum InputType
  {
    Struggle = 1,
    UseMove = 2,
    Sendout = 4,
  }
  public class SelectMoveFail
  {
    public readonly string Key;
    public readonly int Move; //知道怎么区分Block和Only了吧...

    public SelectMoveFail(string key, int move)
    {
      Key = key;
      Move = move;
    }
  }
  [DataContract(Namespace = PBOMarks.PBO)]
  public class PmInputRequest
  {
    [DataMember(EmitDefaultValue = false)]
    public int OnlyMove;

    [DataMember(EmitDefaultValue = false)]
    public string Only; //choice/encore

    [DataMember(EmitDefaultValue = false)]
    public string[] Block; //封印挑拨寻衅回复封印残废

    [DataMember(EmitDefaultValue = false)]
    public bool CantWithdraw;

    public override bool Equals(object obj)
    {
      PmInputRequest i = obj as PmInputRequest;
      return
        OnlyMove == i.OnlyMove &&
        Only == i.Only &&
        Block.ArrayEquals(i.Block) &&
        CantWithdraw == i.CantWithdraw;
    }

    #region Client
    private SimGame Game;
    private SimOnboardPokemon Pm;
    private string error;
    private bool showAbility;
    public void Init(SimGame game, SimOnboardPokemon pm)
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
      throw new NotImplementedException();
      //var text = GameService.Logs["subtitle_" + key].Clone(null);
      //text.SetData(Pm.Pokemon.Name, arg1, arg2);
      //error = text.Text;
    }
    public bool Fight()
    {
      return OnlyMove == Ms.STRUGGLE;
    }
    /// <summary>
    /// 不判断PP数及技能是否存在
    /// </summary>
    /// <param name="move"></param>
    /// <returns></returns>
    public bool Move(SimMove move)
    {
      throw new NotImplementedException();
      //if (OnlyMove != 0 && OnlyMove != move.Type.Id) SetErrorMessage(Only, RomData.GetMove(OnlyMove).GetLocalizedName(), Pm.Pokemon.Item == null ? null : Pm.Pokemon.Item.GetLocalizedName());
      //else
      //  if (Block != null)
      //    for (int i = 0; i < Pm.Moves.Length; ++i)
      //      if (move == Pm.Moves[i])
      //      {
      //        if (Block[i] != null) SetErrorMessage(Block[i], move.Type.GetLocalizedName(), null);
      //        break;
      //      }
      //return error == null;
    }
    public void Target(PokemonOutward target = null)
    {
      throw new NotImplementedException();
    }
    /// <summary>
    /// 判断pokemon是否在场和Hp
    /// </summary>
    /// <param name="pokemon"></param>
    /// <returns></returns>
    public bool Pokemon(SimPokemon pokemon)
    {
      throw new NotImplementedException();
      //if (pokemon.Hp.Value == 0)
      //{
      //  error = string.Format(DataService.String["{0} has no strength to fight!"], pokemon.Name);
      //  return false;
      //}
      //if (pokemon.IndexInOwner < Game.Settings.Mode.OnboardPokemonsPerPlayer())
      //{
      //  error = string.Format(DataService.String["{0} is already fighting."], pokemon.Name);
      //  return false;
      //}
      //if (CantWithdraw)
      //{
      //  error = string.Format(DataService.String["Can't withdraw {0}!"], Pm.Pokemon.Name);
      //  return false;
      //}
      //return true;
    }
    #endregion
  }
  
  [DataContract(Namespace = PBOMarks.JSON)]
  public class InputRequest
  {
    [DataMember(EmitDefaultValue = false)]
    public PmInputRequest[] Pms;

    [DataMember(EmitDefaultValue = false)]
    public int[] Xs;

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
    public bool Pokemon(SimPokemon pokemon, int x)
    {
      throw new NotImplementedException();
      //if (pokemon.Hp.Value == 0)
      //{
      //  error = string.Format(DataService.String["{0} has no strength to fight!"], pokemon.Name);
      //  return false;
      //}
      //if (pokemon.IndexInOwner < game.Settings.Mode.OnboardPokemonsPerPlayer())
      //{
      //  error = string.Format(DataService.String["{0} is already fighting."], pokemon.Name);
      //  return false;
      //}
      //input.Sendout(x, pokemon);
      //CheckSendoutFinished();
      //return true;
    }
    public bool Pokemon(SimPokemon pokemon)
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
      throw new NotImplementedException();
    }
    //void TurnLeft();
    //void TurnRight();
    //void MoveToCenter();
    #endregion
  }
}
