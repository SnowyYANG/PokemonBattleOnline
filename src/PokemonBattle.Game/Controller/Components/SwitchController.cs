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
      //黑眼神状态不能防止任何强制交换的技能或道具效果。 
      return pm.Pokemon.Owner.AlivePms > GameSettings.OnboardPokemonsPerPlayer;
    }
    public bool CanSendout(Tile tile)
    {
      return tile.Pokemon == null && tile.ResponsiblePlayer.AlivePms > GameSettings.OnboardPokemonsPerPlayer;
    }
    public bool CanSendout(Pokemon pm, Tile tile)
    {
      return CanSendout(tile) && pm.Owner == tile.ResponsiblePlayer && pm.Hp.Value > 0 && pm.Owner.GetPokemonIndex(pm) > GameSettings.OnboardPokemonsPerPlayer;
    }

    public bool Withdraw(PokemonProxy pm)
    {
      if (CanWithdraw(pm))
      {
        if (PokemonWithdrawing != null) PokemonWithdrawing();
        pm.Tile.Pokemon = null;
      }
      return false;
    }
    public bool Sendout(Tile tile)
    {
      Player p = tile.ResponsiblePlayer;
      int j = Game.Settings.GetPokemonIndex(tile.X);
      Pokemon pm = p.Pokemons[j];
      if (CanSendout(pm, tile))
      {
        {
          int i = p.GetPokemonIndex(pm);
          Pokemon temp = p.Pokemons[i];
          p.Pokemons[i] = p.Pokemons[j];
          p.Pokemons[j] = temp;
        }
        PokemonProxy pokemonProxy = new PokemonProxy(Controller, pm, tile);
        Board[tile.Team, tile.X].Pokemon = pokemonProxy;
        ReportBuilder.AddSendout(pokemonProxy.Pokemon.Owner, pokemonProxy); 
        return true;
      }
      return false;
    }
  }
}
