using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightStudio.PokemonBattle.Game
{
  class SwitchController : ControllerComponent
  {
    public event Action PokemonWithdrawing;

    public SwitchController(Controller controller)
      : base(controller)
    {
    }

    public bool CanWithdraw(PokemonProxy pm)
    {
      //黑眼神状态不能防止任何强制交换的技能或道具效果，阻止的是CanSelectSwitch
      return pm.Hp == 0 || pm.Pokemon.Owner.AlivePms > GameSettings.Mode.OnboardPokemonsPerPlayer();
    }
    public bool CanSendout(Tile tile)
    {
      return tile.Pokemon == null && Controller.GetPlayer(tile).AlivePms > GameSettings.Mode.OnboardPokemonsPerPlayer();
    }
    public bool CanSendout(Tile tile, int sendoutIndex)
    {
      if (CanSendout(tile))
      {
        Pokemon pm = Controller.GetPlayer(tile).GetPokemon(sendoutIndex);
        return pm != null && pm.Hp.Value > 0 && sendoutIndex >= GameSettings.Mode.OnboardPokemonsPerPlayer();
      }
      return false;
    }

    public bool Withdraw(PokemonProxy pm)
    {
      if (CanWithdraw(pm))
      {
        if (PokemonWithdrawing != null) PokemonWithdrawing();
        pm.Tile.Pokemon = null;
        Controller.OnboardPokemons.Remove(pm);
        ReportBuilder.AddWithdraw(pm);
      }
      return false;
    }
    public bool Sendout(Tile tile)
    {
      Player p = Controller.GetPlayer(tile);
      int origin = Game.Settings.Mode.GetPokemonIndex(tile.X);
      int sendout = tile.WillSendoutPokemonIndex;
      if (CanSendout(tile, sendout))
      {
        p.SwitchPokemon(origin, sendout);
        PokemonProxy pm = new PokemonProxy(Controller, p.GetPokemon(sendout), tile);
        tile.Pokemon = pm;
        tile.WillSendoutPokemonIndex = Tile.NOPM_INDEX;
        Controller.OnboardPokemons.Add(pm);
        ReportBuilder.AddSendout(pm); 
        return true;
      }
      return false;
    }
  }
}
