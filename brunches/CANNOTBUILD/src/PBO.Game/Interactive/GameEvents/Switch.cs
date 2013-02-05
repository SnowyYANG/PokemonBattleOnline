using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using PokemonBattleOnline.Data;
using PokemonBattleOnline.Game.Host;

namespace PokemonBattleOnline.Game.GameEvents
{
  [DataContract(Name = "es2", Namespace = PBOMarks.JSON)]
  internal class GameStartSendOut : GameEvent
  {
    [DataMember(Name = "a")]
    PokemonOutward[] Pms;

    internal GameStartSendOut(IEnumerable<PokemonProxy> pms)
    {
      Pms = pms.Select((p) => p.GetOutward()).ToArray();
    }

    protected override void Update()
    {
      foreach (PokemonOutward p in Pms)
      {
        p.Init(Game);
        Game.Board[p.Position.Team, p.Position.X] = p;
        Game.Board.PokemonSentout(Game, p.Position.Team, p.Position.X);
      }
      AppendGameLog("SendOut" + Pms.Length, Pms.Select((p) => p.Id).ToArray());
      foreach (PokemonOutward p in Pms)
        if (p.Chatter != null) AppendGameLog("Chatter", p.Id);
    }
    public override void Update(SimGame game)
    {
      if (game.Pokemons.ContainsKey(Pms[0].Id))
        foreach (PokemonOutward p in Pms)
        {
          game.OnboardPokemons[p.Position.X] = new SimOnboardPokemon(game.Pokemons[p.Id], p);
          if (p.Owner.Id == game.Player.Id) game.Player.SwitchPokemon(game.Settings.Mode.GetPokemonIndex(p.Position.X), game.Player.GetPokemonIndex(p.Id));
        }
    }
  }

  [DataContract(Name = "es", Namespace = PBOMarks.JSON)]
  internal class SendOut : GameEvent
  {
    [DataMember(Name = "a")]
    PokemonOutward Pm;
    [DataMember(Name = "b")]
    int FormerIndex;
    [DataMember(Name = "c", EmitDefaultValue = false)]
    string Log;

    internal SendOut(PokemonProxy pm, int formerIndex, string log)
    {
      Log = log == "Sendout1" ? null : log;
      Pm = pm.GetOutward();
      FormerIndex = formerIndex;
    }

    protected override void Update()
    {
      Pm.Init(Game);
      Game.Board[Pm.Position.Team, Pm.Position.X] = Pm;
      Game.Board.PokemonSentout(Game, Pm.Position.Team, Pm.Position.X);
      Game.Teams[Pm.Position.Team].SwitchPokemon(FormerIndex, Game.Settings.Mode.GetPokemonIndex(Pm.Position.X));
      AppendGameLog(Log ?? "SendOut1", Pm.Id);
      if (Pm.Chatter != null) AppendGameLog("Chatter", Pm.Id);
    }
    public override void Update(SimGame game)
    {
      if (Pm.Position.Team == game.Player.Team)
      {
        game.OnboardPokemons[Pm.Position.X] = new SimOnboardPokemon(game.Pokemons[Pm.Id], Pm);
        if (Pm.Owner.Id == game.Player.Id) game.Player.SwitchPokemon(game.Settings.Mode.GetPokemonIndex(Pm.Position.X), FormerIndex);
      }
    }
  }

  [DataContract(Name = "ew", Namespace = PBOMarks.JSON)]
  internal class Withdraw : GameEvent
  {
    [DataMember(Name = "a", EmitDefaultValue = false)]
    int Pm;

    [DataMember(Name = "b", EmitDefaultValue = false)]
    int Ab;

    public Withdraw(PokemonProxy pm)
    {
      Pm = pm.Id;
      Ab = pm.Ability.Id;
      if (Ab != Host.Sp.Abilities.REGENERATOR && Ab != Host.Sp.Abilities.NATURAL_CURE) Ab = 0;
    }

    int team, x;
    protected override void Update()
    {
      var pm = GetPokemon(Pm);
      team = pm.Position.Team;
      x = pm.Position.X;
      if (pm.Hp.Value == 0) pm.Faint();
      else
      {
        pm.Withdraw();
        if (Ab == Host.Sp.Abilities.NATURAL_CURE) pm.State = PokemonState.Normal;//for TeamOutward
      }
      Game.Board[team, x] = null;
    }
    public override void Update(SimGame game)
    {
      if (team == game.Player.Team)
      {
        var pm = game.OnboardPokemons[x].Pokemon;
        game.OnboardPokemons[x] = null;
        pm.ResetForm();
        if (pm.Hp.Value == 0) pm.State = PokemonState.Faint;
        else
        {
          if (Ab == Host.Sp.Abilities.REGENERATOR) pm.SetHp(pm.Hp.Value + pm.Hp.Origin / 3);
          else if (Ab == Host.Sp.Abilities.NATURAL_CURE) pm.State = PokemonState.Normal;
        }
      }
    }
  }
}
