using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightStudio.PokemonBattle.Game.Host
{
  internal class InputController : ControllerComponent
  {
    private Dictionary<int, RequireInput> inputRequirements;

    public InputController(Controller controller)
      : base(controller)
    {
      inputRequirements = new Dictionary<int, RequireInput>();
    }

    public bool NeedInput
    { get { return inputRequirements.Count > 0; } }
    public IEnumerable<KeyValuePair<int, RequireInput>> InputRequirements
    { get { return inputRequirements; } }

    //private bool CheckInputSucceed(Player player)
    //{
    //  foreach (Tile t in Controller.Tiles)
    //    if (Controller.GetPlayer(t) == player)
    //      if (t.Pokemon != null)
    //      {
    //        if (t.Pokemon.Action == PokemonAction.WaitingForInput) return false;
    //      }
    //      else
    //      {
    //        if (t.WillSendoutPokemonIndex < GameSettings.Mode.OnboardPokemonsPerPlayer() && Controller.CanSendout(t)) return false;
    //      }
    //  players.Remove(player.Id);
    //  return true;
    //}
    public void PauseForTurnInput()
    {
      inputRequirements.Clear();
      var groups = from p in Controller.OnboardPokemons
                   where p.Action == PokemonAction.WaitingForInput
                   group p by p.Pokemon.Owner.Id into playerPms
                   select playerPms;
      foreach (var g in groups) inputRequirements.Add(g.Key, new RequireInput(g));
    }
    public void PauseForEndTurnInput()
    {
      inputRequirements.Clear();
      foreach (Tile t in Controller.Tiles) inputRequirements[Controller.GetPlayer(t).Id] = null;
    }
    public void PauseForSendoutInput(Tile tile)
    {
      inputRequirements.Clear();
      if (Controller.CanSendout(tile)) inputRequirements.Add(Controller.GetPlayer(tile).Id, null);
    }
    private bool Switch(PokemonProxy withdraw, int sendoutIndex)
    {
      throw new NotImplementedException();
      //if (withdraw.CanInput)
      //{
      //  string m = withdraw.InputSwitch(sendoutIndex);
      //  if (m == null) return CheckInputSucceed(withdraw.Pokemon.Owner);
      //  return false;
      //}
      //return false;
    }
    public bool Sendout(Tile tile, int sendoutIndex)
    {
      throw new NotImplementedException();
      //if (tile.Pokemon == null)
      //{
      //  if (!Controller.CanSendout(tile)) return InputResult.Fail("非空场地或已经没有可以送出的精灵了");
      //  if (!Controller.CanSendout(Controller.GetPlayer(tile).GetPokemon(sendoutIndex))) return InputResult.Fail("这只精灵无法被送出");
      //  tile.WillSendoutPokemonIndex = sendoutIndex;
      //  return CheckInputSucceed(Controller.GetPlayer(tile));
      //}
      //else return Switch(tile.Pokemon, sendoutIndex);
    }
    public bool SelectMove(MoveProxy move, Tile target)
    {
      throw new NotImplementedException();
      //if (move.Owner.CanInput)
      //{
      //  string m = move.Owner.SelectMove(move, target);
      //  if (m == null) return CheckInputSucceed(move.Owner.Pokemon.Owner);
      //  return InputResult.Fail(m == string.Empty ? null : m);
      //}
      //return InputResult.Fail("当前精灵没有等待训练师的命令");
    }
    public bool Struggle(PokemonProxy pokemon)
    {
      throw new NotImplementedException();
      //if (pokemon.CanInput)
      //{
      //  string m = pokemon.SelectMove(pokemon.StruggleMove, null);
      //  if (m == null) return CheckInputSucceed(pokemon.Pokemon.Owner);
      //  return InputResult.Fail(m == string.Empty ? null : m);
      //}
      //return InputResult.Fail("当前精灵没有等待训练师的命令");
    }
  }
}
