using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonBattleOnline.Game.GameEvents;
using PokemonBattleOnline.Game.Host.Triggers;

namespace PokemonBattleOnline.Game.Host
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
        STs.Withdrawing(pm, canPursuit);
        if (pm.Tile != null)
        {
          ReportBuilder.Withdraw(pm);
          var ability = pm.Ability;
          pm.Action = PokemonAction.InBall;
          pm.Tile.Pokemon = null;
          pm.OnboardPokemon = pm.NullOnboardPokemon;
          Controller.ActingPokemons.Remove(pm);
          ATs.Withdrawn(pm, ability);
          return true;
        }
      }
      return false;
    }
    private PokemonProxy SendoutImplement(Tile tile)
    {
      var pm = Controller.GetPokemon(Controller.GetPlayer(tile).GetPokemon(tile.WillSendoutPokemonIndex));
      pm.Action = PokemonAction.Debuting;
      tile.Pokemon = pm;
      tile.WillSendoutPokemonIndex = Tile.NOPM_INDEX;
      pm.OnboardPokemon = new OnboardPokemon(pm.Pokemon, tile.X);
      STs.SendingOut(pm);
      Controller.ActingPokemons.Insert(0, pm);
      return pm;
    }
    public void GameStartSendout(IEnumerable<Tile> tiles)
    {
      var pms = tiles.Select((t) => SendoutImplement(t)).ToArray();
      ReportBuilder.ShowLog("Sendout" + pms.Length, pms.ValueOrDefault(0), pms.ValueOrDefault(1), pms.ValueOrDefault(2));
    }
    public bool Sendout(Tile tile, bool debut, string log)
    {
      Player p = Controller.GetPlayer(tile);
      int origin = Game.Settings.Mode.GetPokemonIndex(tile.X);
      int sendout = tile.WillSendoutPokemonIndex;
      if (CanSendout(tile) && CanSendout(p.GetPokemon(sendout)))
      {
        var pm = SendoutImplement(tile);
        p.SwitchPokemon(origin, sendout);
        ReportBuilder.SendOut(pm, sendout);
        pm.AddReportPm(log);
        ATs.Trace(pm);
        if (debut)
        {
          ATs.AttachUnnerve(Controller);
          pm.Debut();
          if (pm.Hp != 0) ATs.AttachWeatherObserver(pm);
          ATs.WeatherChanged(Controller);
        }
        return true;
      }
      return false;
    }
  }
}
