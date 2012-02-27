using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightStudio.PokemonBattle.Game
{
  class SwitchController : ControllerComponent
  {
    public event Action PokemonWithdrawing;
    public event Action PokemonSendout;
    private readonly int[,] lastPokemonIds;

    public SwitchController(Controller controller)
      : base(controller)
    {
      lastPokemonIds = new int[GameSettings.TeamCount, GameSettings.XBound];
    }

    public bool CanWithdraw(PokemonProxy pm)
    {
      //黑眼神状态不能防止任何强制交换的技能或道具效果。 
      return pm != null && pm.Pokemon.Owner.AlivePms > GameSettings.OnboardPokemonsPerPlayer;
    }
    public bool CanSendout(Pokemon pm, Position position)
    {
      return pm.Hp.Value > 0 && pm.Owner.GetPokemonIndex(pm.Id) > GameSettings.OnboardPokemonsPerPlayer;
    }

    public bool Withdraw(PokemonProxy pm)
    {
      if (CanWithdraw(pm))
      {
        if (PokemonWithdrawing != null) PokemonWithdrawing();
        Board[pm.Position.Team, pm.Position.X] = null;
      }
      return false;
    }
    public bool Sendout(Pokemon pm, Position position)
    {
      if (CanSendout(pm, position))
      {
        {
          Player p = pm.Owner;
          int i = p.GetPokemonIndex(pm.Id);
          int j = p.GetPokemonIndex(lastPokemonIds[position.Team, position.X]);
          Pokemon temp = p.Pokemons[i];
          p.Pokemons[i] = p.Pokemons[j];
          p.Pokemons[j] = temp;
        }
        PokemonProxy pokemonProxy = new PokemonProxy(Controller, pm, position);
        Board[position.Team, position.X] = pokemonProxy;
        lastPokemonIds[position.Team, position.X] = pokemonProxy.Id;
        TurnBuilder.AddSendout(pokemonProxy.Pokemon.Owner, pokemonProxy); 
        return true;
      }
      return false;
    }
  }
}
