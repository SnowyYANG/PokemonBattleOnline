using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokemonBattleOnline.Game.Host
{
  internal class InputController : ControllerComponent
  {
    private Dictionary<int, InputRequest> requirements;
    private bool singleSendout;

    public InputController(Controller controller)
      : base(controller)
    {
      requirements = new Dictionary<int, InputRequest>();
    }

    public bool NeedInput
    { get { return requirements.Count > 0; } }
    public IDictionary<int, InputRequest> InputRequirements
    { get { return requirements; } }

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
            if (t.WillSendoutPokemonIndex < GameSettings.Mode.OnboardPokemonsPerPlayer() && Controller.CanSendout(t)) return false;
          }
      Controller.Timer.Pause(player.Id);
      requirements.Remove(player.Id);
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
      }
      {
        pir.CantWithdraw = !pm.CanSelectWithdraw;
      }
      return pir;
    }
    public bool PauseForTurnInput()
    {
      if (requirements.Any()) return false;
      var groups = from p in Controller.ActingPokemons
                   where p.Action == PokemonAction.WaitingForInput
                   group p by p.Pokemon.Owner.Id into playerPms
                   select playerPms;
      foreach (var g in groups) requirements.Add(g.Key, new InputRequest() { Pms = g.Select((p) => NewInputRequest(p)).ToArray(), Time = Controller.Timer.GetState(g.Key) });
      return true;
    }
    public bool PauseForEndTurnInput()
    {
      if (requirements.Any()) return false;
      var groups = from t in Controller.Board.Tiles
                   where Controller.CanSendout(t)
                   group t by Controller.GetPlayer(t).Id into playerTiles
                   select playerTiles;
      foreach (var g in groups) requirements[g.Key] = new InputRequest() { Time = Controller.Timer.GetState(g.Key) };
      singleSendout = false;
      return true;
    }
    public bool PauseForSendoutInput(Tile tile)
    {
      if (requirements.Any()) return false;
      if (Controller.CanSendout(tile))
      {
        var player = Controller.GetPlayer(tile).Id;
        requirements.Add(player, new InputRequest() { Xs = new int[] { tile.X }, Time = Controller.Timer.GetState(player) });
      }
      singleSendout = true;
      return true;
    }
    private bool Switch(PokemonProxy withdraw, int sendoutIndex)
    {
      return withdraw.CanInput && withdraw.InputSwitch(sendoutIndex);
    }
    public bool Sendout(Tile tile, int sendoutIndex)
    {
      if (tile.Pokemon == null && Controller.CanSendout(tile) && Controller.CanSendout(Controller.GetPlayer(tile).GetPokemon(sendoutIndex)))
      {
        tile.WillSendoutPokemonIndex = sendoutIndex;
        return true;
      }
      else return Switch(tile.Pokemon, sendoutIndex);
    }
    public bool SelectMove(MoveProxy move, Tile target)
    {
      return move.Owner.CanInput && move.Owner.SelectMove(move, target);
    }
    public bool Struggle(PokemonProxy pokemon)
    {
      return pokemon.CanInput && pokemon.SelectMove(pokemon.StruggleMove, null);
    }
  }
}
