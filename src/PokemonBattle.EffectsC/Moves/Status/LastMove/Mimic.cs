using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace PokemonBattleOnline.Game.Host.Effects.Moves.Status
{
  [DataContract(Namespace = Namespaces.JSON)]
  class MimicEvent : GameEvent
  {
    [DataMember]
    int Pm;
    [DataMember]
    int Move;

    public MimicEvent(PokemonProxy pm, Data.MoveType move)
    {
      Pm = pm.Id;
      Move = move.Id;
    }
    protected override void Update()
    {
      AppendGameLog("Mimic", Pm, Move);
    }
    public override void Update(SimGame game)
    {
      foreach (var pm in game.OnboardPokemons)
        if (pm.Id == Pm && pm.Pokemon.Owner == game.Player) pm.ChangeMove(102, Move);
    }
  }
  class Mimic : StatusMoveE
  {
    static Mimic()
    {
      EffectsService.Register<MimicEvent>();
    }

    public Mimic(int id)
      : base(id)
    {
    }
    protected override void Act(AtkContext atk)
    {
      var aer = atk.Attacker;
      var last = aer.LastMove;
      if (last == null || last == Move) atk.FailAll();
      else
      {
        var move = last;
        aer.ChangeMove(Move, move);
        aer.Controller.ReportBuilder.Add(new MimicEvent(aer, move));
      }
    }
  }
}
