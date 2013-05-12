using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightStudio.PokemonBattle.Game.Host
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
      requirements.Remove(player.Id);
      return true;
    }
    public bool PauseForTurnInput()
    {
      if (requirements.Any()) return false;
      var groups = from p in Controller.ActingPokemons
                   where p.Action == PokemonAction.WaitingForInput
                   group p by p.Pokemon.Owner.Id into playerPms
                   select playerPms;
      foreach (var g in groups) requirements.Add(g.Key, new InputRequest(g));
      return true;
    }
    public bool PauseForEndTurnInput()
    {
      if (requirements.Any()) return false;
      var groups = from t in Controller.Game.Board.Tiles
                   where Controller.CanSendout(t)
                   group t by Controller.GetPlayer(t).Id into playerTiles
                   select playerTiles;
      foreach (var g in groups) requirements[g.Key] = new InputRequest();
      singleSendout = false;
      return true;
    }
    public bool PauseForSendoutInput(Tile tile)
    {
      if (requirements.Any()) return false;
      if (Controller.CanSendout(tile)) requirements.Add(Controller.GetPlayer(tile).Id, new InputRequest(tile));
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
        if (singleSendout)
        {
          Controller.Sendout(tile, true);
          ReportBuilder.AddHorizontalLine();
        }
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
