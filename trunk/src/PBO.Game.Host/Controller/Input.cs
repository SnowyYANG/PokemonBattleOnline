using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokemonBattleOnline.Game.Host
{
  internal class InputController : ControllerComponent
  {
    public InputController(Controller controller)
      : base(controller)
    {
      _requirements = new InputRequest[2, 2];
    }

    public bool NeedInput
    { get { return !(_requirements[0,0] == null && _requirements[0,1] == null && _requirements[1, 0] == null &&_requirements[1,1] == null); } }
    private InputRequest[,] _requirements;
    public InputRequest[,] InputRequirements
    { get { return _requirements; } }

    public bool CheckInputSucceed(Player player)
    {
      foreach (Tile t in Controller.Tiles)
        if (Controller.GetPlayer(t) == player)
          if (t.Pokemon != null)
          {
            if (t.Pokemon.Action == PokemonAction.WaitingForInput) return false;
          }
          else
          {
            if (t.WillSendOutPokemonIndex < GameSettings.Mode.OnboardPokemonsPerPlayer() && Controller.CanSendOut(t)) return false;
          }
      Controller.Timer.Pause(player);
      _requirements[player.TeamId, player.TeamIndex] = null;
      return true;
    }
    private PmInputRequest NewInputRequest(PokemonProxy pm)
    {
      var pir = new PmInputRequest();
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
              if (pir.Block == null) pir.Block = new string[pm.Moves.Count()];
              if (f.Move == move.Type.Id) pir.Block[i] = f.Key;
              else if (pir.Only == null)
              {
                pir.Only = f.Key;
                pir.OnlyMove = f.Move;
              }
            }
          }
          i++;
        }
        if (struggle)
        {
          pir.Block = null;
          pir.Only = null;
          pir.OnlyMove = Ms.STRUGGLE;
        }
        else pir.CanMega = pm.CanMega;
      }
      {
        pir.CantWithdraw = !pm.CanSelectWithdraw;
      }
      return pir;
    }
    public bool PauseForTurnInput()
    {
      if (!Controller.CanContinue) return false;
      List<PmInputRequest>[, ] pms = new List<PmInputRequest>[2, 2];
      int[,] time = new int[2, 2];
      foreach (var p in Controller.ActingPokemons)
        if (p.Action == PokemonAction.WaitingForInput)
        {
          var player = p.Pokemon.Owner;
          var id = player.TeamId;
          var index = player.TeamIndex;
          if (pms[id, index] == null)
          {
            pms[id, index] = new List<PmInputRequest>();
            time[id, index] = player.SpentTime;
          }
          pms[player.TeamId, player.TeamIndex].Add(NewInputRequest(p));
        }
      if (pms[0, 0] != null) _requirements[0, 0] = new InputRequest() { Pms = pms[0, 0].ToArray(), Time = time[0, 0] };
      if (pms[0, 1] != null) _requirements[0, 1] = new InputRequest() { Pms = pms[0, 1].ToArray(), Time = time[0, 1] };
      if (pms[1, 0] != null) _requirements[1, 0] = new InputRequest() { Pms = pms[1, 0].ToArray(), Time = time[1, 0] };
      if (pms[1, 1] != null) _requirements[1, 1] = new InputRequest() { Pms = pms[1, 1].ToArray(), Time = time[1, 1] };
      return true;
    }
    public bool PauseForEndTurnInput()
    {
      if (!Controller.CanContinue) return false;
      foreach (var t in Controller.Board.Tiles)
        if (Controller.CanSendOut(t))
        {
          var player = Controller.GetPlayer(t);
          if (_requirements[player.TeamId, player.TeamIndex] == null) _requirements[player.TeamId, player.TeamIndex] = new InputRequest() { Time = player.SpentTime };
        }
      return true;
    }
    public bool PauseForSendOutInput(Tile tile)
    {
      if (!Controller.CanContinue) return false;
      if (Controller.CanSendOut(tile))
      {
        var player = Controller.GetPlayer(tile);
        _requirements[player.TeamId, player.TeamIndex] = new InputRequest() { Xs = new int[] { tile.X }, Time = player.SpentTime };
      }
      return true;
    }
    private bool Switch(PokemonProxy withdraw, int sendoutIndex)
    {
      return withdraw.CanInput && withdraw.InputSwitch(sendoutIndex);
    }
    public bool SendOut(Tile tile, int sendoutIndex)
    {
      if (tile.Pokemon == null && Controller.CanSendOut(tile) && Controller.CanSendOut(Controller.GetPlayer(tile).GetPokemon(sendoutIndex)))
      {
        tile.WillSendOutPokemonIndex = sendoutIndex;
        return true;
      }
      else return Switch(tile.Pokemon, sendoutIndex);
    }
    public bool SelectMove(MoveProxy move, Tile target, bool mega)
    {
      return move.Owner.CanInput && move.Owner.SelectMove(move, target, mega);
    }
    public bool Struggle(PokemonProxy pokemon)
    {
      return pokemon.CanInput && pokemon.SelectMove(pokemon.StruggleMove, null, false);
    }
  }
}
