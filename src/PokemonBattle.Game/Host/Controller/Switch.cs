using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Interactive.GameEvents;

namespace LightStudio.PokemonBattle.Game
{
  class SwitchController : ControllerComponent
  {
    public SwitchController(Controller controller)
      : base(controller)
    {
    }

    public bool CanWithdraw(PokemonProxy pm)
    {
      //黑眼神状态不能防止任何强制交换的技能或道具效果，阻止的是CanSelectSwitch
      return pm.Hp == 0 || pm.Pokemon.Owner.PmsAlive > GameSettings.Mode.OnboardPokemonsPerPlayer();
    }
    public bool CanSendout(Tile tile)
    {
      Player p = Controller.GetPlayer(tile);
      return tile.Pokemon == null &&
        (p.PmsAlive > GameSettings.Mode.OnboardPokemonsPerPlayer() ||
        (p.PmsAlive == GameSettings.Mode.OnboardPokemonsPerPlayer() && p.GetPokemon(GameSettings.Mode.GetPokemonIndex(tile.X)).Hp.Value == 0));
    }
    public bool CanSendout(Pokemon pokemon)
    {
      return pokemon != null && pokemon.Hp.Value > 0 && pokemon.IndexInOwner >= GameSettings.Mode.OnboardPokemonsPerPlayer();
    }

    public bool Withdraw(PokemonProxy pm, bool canPursuit)
    {
      if (CanWithdraw(pm))
      {
        if (canPursuit) Sp.Moves.Pursuit(pm);
        pm.Tile.Pokemon = null;
        pm.Tile = null;
        Controller.OnboardPokemons.Remove(pm);
        ReportBuilder.Add(new Withdraw(pm));
        Sp.Abilities.Withdrawn(pm);
        return true;
      }
      return false;
    }
    public bool Sendout(Tile tile, bool debut)
    {
      Player p = Controller.GetPlayer(tile);
      int origin = Game.Settings.Mode.GetPokemonIndex(tile.X);
      int sendout = tile.WillSendoutPokemonIndex;
      if ((ReportBuilder.TurnNumber == 0 && origin == sendout) || (CanSendout(tile) && CanSendout(p.GetPokemon(sendout))))
      {
        //交换必须在构建实例之后，交换怪兽会导致队伍的排序改变，幻影特性以交换时的队伍顺序决定。
        PokemonProxy pm = new PokemonProxy(Controller, p.GetPokemon(sendout), tile);
        pm.Tile = tile;
        p.SwitchPokemon(origin, sendout);
        tile.Pokemon = pm;
        tile.WillSendoutPokemonIndex = Tile.NOPM_INDEX;
        Controller.OnboardPokemons.Add(pm);
        ReportBuilder.Add(new SendOut(pm));
        if (debut) pm.Debut();
        return true;
      }
      return false;
    }
  }
}
