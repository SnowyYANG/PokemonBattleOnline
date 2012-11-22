using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Game.GameEvents;
using LightStudio.PokemonBattle.Game.Host.Sp;

namespace LightStudio.PokemonBattle.Game.Host
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
      return pm.Tile != null && (pm.Hp == 0 || pm.Pokemon.Owner.PmsAlive > GameSettings.Mode.OnboardPokemonsPerPlayer());
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

    public bool Withdraw(PokemonProxy pm, string log, bool canPursuit)
    {
      if (CanWithdraw(pm))
      {
        if (log != null) pm.AddReportPm(log);
        Triggers.Withdrawing(pm, canPursuit);
        if (pm.Tile != null)
        {
          ReportBuilder.Add(new GameEvents.Withdraw(pm));
          var ability = pm.Ability.Id;
          pm.Action = PokemonAction.InBall;
          pm.Tile.Pokemon = null;
          pm.OnboardPokemon = pm.NullOnboardPokemon;
          Controller.OnboardPokemons.Remove(pm);
          Abilities.Withdrawn(pm, ability);
          return true;
        }
      }
      return false;
    }
    public bool Sendout(Tile tile, bool debut, string log = null)
    {
      Player p = Controller.GetPlayer(tile);
      int origin = Game.Settings.Mode.GetPokemonIndex(tile.X);
      int sendout = tile.WillSendoutPokemonIndex;
      if ((ReportBuilder.TurnNumber == 0 && origin == sendout) || (CanSendout(tile) && CanSendout(p.GetPokemon(sendout))))
      {
        var pm = Controller.GetPokemon(p.GetPokemon(sendout));
        pm.Action = PokemonAction.Debuting;
        tile.Pokemon = pm;
        tile.WillSendoutPokemonIndex = Tile.NOPM_INDEX;
        pm.OnboardPokemon = new OnboardPokemon(pm.Pokemon, tile.X);
        Triggers.SendingOut(pm);
        Controller.OnboardPokemons.Insert(0, pm);
        p.SwitchPokemon(origin, sendout);
        ReportBuilder.Add(new SendOut(log, pm));
        Abilities.Trace(pm);
        if (debut)
        {
          Abilities.AttachUnnerve(Controller);
          pm.Debut();
          Abilities.AttachWeatherObserver(pm);
        }
        return true;
      }
      return false;
    }
  }
}
