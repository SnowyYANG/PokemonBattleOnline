using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace PokemonBattleOnline.Game.GameEvents
{
  [DataContract(Name = "es", Namespace = PBOMarks.JSON)]
  public class SendOut : GameEvent
  {
    [DataMember(Name = "a",EmitDefaultValue = false)]
    public PokemonOutward Pm;
    [DataMember(Name = "b")]
    public int FormerIndex;

    protected override void Update()
    {
      Pm.Init(Game);
      Game.Board[Pm.Position.Team, Pm.Position.X] = Pm;
      Game.Board.OnPokemonSentOut(Pm.Position.Team, Pm.Position.X);
      Game.Board.Teams[Pm.Position.Team].SwitchPokemon(FormerIndex, Game.Settings.Mode.GetPokemonIndex(Pm.Position.X));
    }
    public override void Update(SimGame game)
    {
      if (Pm.Position.Team == game.Player.Team)
      {
        game.OnboardPokemons[Pm.Position.X] = new SimOnboardPokemon(game.Pokemons[Pm.Id], Pm);
        if (Pm.Position.Team == game.Player.Team && Pm.TeamIndex == game.Player.TeamIndex) game.Player.SwitchPokemon(game.Settings.Mode.GetPokemonIndex(Pm.Position.X), FormerIndex);
      }
    }
  }

  [DataContract(Name = "ew", Namespace = PBOMarks.JSON)]
  public class Withdraw : GameEvent
  {
    [DataMember(Name = "a", EmitDefaultValue = false)]
    public int Pm;

    int team, x;
    protected override void Update()
    {
      var pm = GetPokemon(Pm);
      team = pm.Position.Team;
      x = pm.Position.X;
      if (pm.Hp.Value == 0) pm.Faint();
      else pm.Withdraw();
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
      }
    }
  }
}
