using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using LightStudio.Tactic.DataModels;
using LightStudio.PokemonBattle.Data;
using LightStudio.PokemonBattle.Game;

namespace LightStudio.PokemonBattle.Interactive.GameEvents
{
  [DataContract(Namespace = Namespaces.DEFAULT)]
  public class SendOut : GameEvent
  {
    [DataMember]
    public int PlayerId { get; private set; }
    [DataMember]
    public PokemonOutward Pokemon { get; private set; }

    internal SendOut(int playerId, PokemonOutward pokemon)
    {
      PlayerId = playerId;
      Pokemon = pokemon;
    }
    public override IText GetGameLog(GameOutward game)
    {
      IText t = GetGameLog(SENDOUT);
      t.SetData(this);
      return t;
    }
    public override void Update(GameOutward game)
    {
      game.Board[Pokemon.Position.Team, Pokemon.Position.X] = Pokemon;
      game.Board.PokemonSentout(Pokemon.Position.Team, Pokemon.Position.X);
    }
    public override void Update(SimGame game)
    {
      if (Pokemon.Position.Team == game.Team.Id)
      {
        game.Pokemons[Pokemon.Position.X] = new SimPokemon(game.Team.Pokemons[Pokemon.Id], Pokemon);
        game.ActivePokemons.Add(game.Pokemons[Pokemon.Position.X]);
      }
    }
  }

  [DataContract(Namespace = Namespaces.DEFAULT)]
  public class Withdraw : GameEvent
  {
    [DataMember]
    int TeamId { get; private set; }

    [DataMember]
    int X { get; private set; }
    
    internal Withdraw(PokemonProxy pm)
    {
      PmId = pm.Id;
      if (pm.Hp == 0) ;//xxx倒下了
      else if (pm.Action == PokemonAction.Switching) ;//xxx把xxx收了回去
      else ;//xxx回到了xxx身边
    }
    public override IText GetGameLog(GameOutward game)
    {
      return null;
    }
    public override void Update(GameOutward game)
    {
    }
    public override void Update(SimGame game)
    {
    }
  }
}
