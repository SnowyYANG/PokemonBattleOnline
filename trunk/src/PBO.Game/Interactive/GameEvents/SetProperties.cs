﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace PokemonBattleOnline.Game.GameEvents
{
  [DataContract(Name = "est", Namespace = PBOMarks.JSON)]
  public class SetState : GameEvent
  {
    [DataMember(Name = "a")]
    public int Pm;
    [DataMember(Name = "b", EmitDefaultValue = false)]
    public PokemonState State;

    protected override void Update()
    {
      var pm = GetPokemon(Pm);
      pm.State = State;
    }
    public override void Update(SimGame game)
    {
      SimPokemon p = GetPokemon(game, Pm);
      if (p != null) p.State = State;
    }
  }

  [DataContract(Name = "eh", Namespace = PBOMarks.JSON)]
  public class ShowHp : GameEvent
  {
    [DataMember(Name = "a")]
    public int Pm;
    [DataMember(Name = "b", EmitDefaultValue = false)]
    public int Hp;

    protected override void Update()
    {
      var pm = GetPokemon(Pm);
      var h = Hp - pm.Hp.Value;
      pm.Hp.Value = Hp;
      AppendGameLog("Hp", h > 0 ? h.ToString("+0") : h.ToString());
    }
    public override void Update(SimGame game)
    {
      var pm = GetPokemon(game, Pm);
      if (pm != null) pm.SetHp(Hp);
    }
  }
  [DataContract(Namespace = PBOMarks.JSON)]
  public class SetHp : GameEvent
  {
    [DataMember(Name = "a")]
    public int Pm;
    [DataMember(Name = "b", EmitDefaultValue = false)]
    public int Hp;

    protected override void Update()
    {
      var pm = GetPokemon(Pm);
      pm.Hp.Value = Hp;
    }
    public override void Update(SimGame game)
    {
      var pm = GetPokemon(game, Pm);
      if (pm != null) pm.SetHp(Hp);
    }
  }

  [DataContract(Namespace = PBOMarks.JSON)]
  public class SetY : GameEvent
  {
    [DataMember]
    public int Pm;
    [DataMember(EmitDefaultValue = false)]
    public CoordY Y;

    protected override void Update()
    {
      var pm = GetPokemon(Pm);
      pm.ChangePosition(pm.Position.X, Y);
    }
  }

  [DataContract(Namespace = PBOMarks.JSON)]
  public class SetX : GameEvent
  {
    [DataMember]
    public int Pm;
    [DataMember(EmitDefaultValue = false)]
    public int X;

    protected override void Update()
    {
      var pm = GetPokemon(Pm);
      pm.ChangePosition(X, pm.Position.Y);
    }
  }

  [DataContract(Name = "eo", Namespace = PBOMarks.JSON)]
  public class SetOutward : GameEvent
  {
    [DataMember(Name = "a")]
    public int Pm;
    [DataMember(Name = "d", EmitDefaultValue = false)]
    public string Name;
    [DataMember(Name = "e", EmitDefaultValue = false)]
    public int Number;
    [DataMember(Name = "f", EmitDefaultValue = false)]
    public int Form;
    [DataMember(Name = "g", EmitDefaultValue = false)]
    public PokemonGender? Gender;
    [DataMember(Name = "c", EmitDefaultValue = false)]
    public int[] Moves;
    [DataMember(Name = "h", EmitDefaultValue = false)]
    public int Arg;
    [DataMember(Name = "i", EmitDefaultValue = false)]
    public bool Forever; //shaymi，虽然会误判不过不影响

    protected override void Update()
    {
      var pm = GetPokemon(Pm);
      if (Name != null) pm.Name = Name;
      if (Gender != null) pm.Gender = Gender.Value;
      pm.ChangeImage(Number == 0 ? pm.Form.Species.Number : Number, Form);
      //Sleep = 500;
    }
    public override void Update(SimGame game)
    {
      if (Number == 0)
      {
        var pm = GetPokemon(game, Pm);
        if (pm != null) pm.ChangeForm(Form, Forever);
      }
      else if (Moves != null)
      {
        var pm = GetOnboardPokemon(game, Pm);
        if (pm != null && pm.Pokemon.Owner == game.Player) pm.ChangeMoves(Moves);
      }
    }
  }
}
