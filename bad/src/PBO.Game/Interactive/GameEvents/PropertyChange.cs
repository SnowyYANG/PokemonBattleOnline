using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using PokemonBattleOnline.Data;
using PokemonBattleOnline.Game.Host;

namespace PokemonBattleOnline.Game.GameEvents
{
  [DataContract(Name = "est", Namespace = PBOMarks.JSON)]
  public class StateChange : GameEvent
  {
    [DataMember(Name = "a")]
    int Pm;
    [DataMember(Name = "b", EmitDefaultValue = false)]
    PokemonState State;
    [DataMember(Name = "c", EmitDefaultValue = false)]
    string Log;
    [DataMember(Name = "d", EmitDefaultValue = false)]
    int Arg1;
    [DataMember(Name = "e", EmitDefaultValue = false)]
    public bool Item;
    
    public StateChange(PokemonProxy pm, string log = null, int arg1 = 0)
    {
      Pm = pm.Id;
      State = pm.Pokemon.State;
      Log = log;
      Arg1 = arg1;
    }

    protected override void Update()
    {
      var pm = GetPokemon(Pm);
      string key;
      if (Log == null)
      {
        if (State == PokemonState.Normal)
        {
          switch (pm.State)
          {
            case PokemonState.BadlyPSN:
            case PokemonState.PSN:
              key = "DePSN";
              break;
            default:
              key = "De" + pm.State.ToString();
              break;
          }
          if (Item) key = "Item" + key;
        }
        else key = "En" + State.ToString();
      }
      else key = Log;
      pm.State = State;
      AppendGameLog(key, Pm, Arg1);
    }
    public override void Update(SimGame game)
    {
      SimPokemon p = GetPokemon(game, Pm);
      if (p != null)
      {
        p.State = State;
        if (Item) p.Item = null;
      }
    }
  }

  [DataContract(Name = "eh", Namespace = PBOMarks.JSON)]
  public class HpChange : GameEvent
  {
    [DataMember(Name = "a")]
    protected int Pm;
    [DataMember(Name = "b", EmitDefaultValue = false)]
    public int Hp;
    [DataMember(Name = "d", EmitDefaultValue = false)]
    public bool ResetY;
    [DataMember(Name = "e", EmitDefaultValue = false)]
    public bool ConsumeItem;
    [DataMember(Name = "c", EmitDefaultValue = false)]
    protected string Key;
    [DataMember(Name = "f", EmitDefaultValue = false)]
    protected int Arg1;
    [DataMember(Name = "g", EmitDefaultValue = false)]
    protected int Arg2;

    public HpChange(PokemonProxy pm, string logKey, int arg1 = 0, int arg2 = 0)
    {
      Pm = pm.Id;
      Hp = pm.Hp;
      Key = logKey;
      Arg1 = arg1;
      Arg2 = arg2;
    }

    protected override void Update()
    {
      var pm = GetPokemon(Pm);
      var oldHp = pm.Hp.Value;
      pm.Hp.Value = Hp;
      if (ResetY) pm.ChangePosition(pm.Position.X, CoordY.Plate);
      if (Key != null)
      {
        AppendGameLog(Key, Pm, Arg1, Arg2);
        var h = Hp - oldHp;
        Sleep = Math.Abs(h) * 17 + 1000;
        AppendGameLog("Hp", h > 0 ? h.ToString("+0") : h.ToString());
      }
    }
    public override void Update(SimGame game)
    {
      var pm = GetPokemon(game, Pm);
      if (pm != null)
      {
        pm.SetHp(Hp);
        if (ConsumeItem) pm.Item = null;
      }
    }
  }

  [DataContract(Namespace = PBOMarks.JSON)]
  public class PositionChange : SimpleEvent
  {
    public static PositionChange Reset(string gameLogKey, PokemonProxy pm, object arg1 = null, object arg2 = null)
    {
      return new PositionChange(gameLogKey, pm, arg1, arg2);
    }
    public static PositionChange Leap(string gameLogKey, PokemonProxy pm, CoordY y)
    {
      return new PositionChange(gameLogKey, pm, null, null) { Y = y };
    }

    [DataMember(EmitDefaultValue = false)]
    CoordY Y;

    private PositionChange(string gameLogKey, PokemonProxy pm, object arg1 = null, object arg2 = null)
      : base(gameLogKey, pm, arg1, arg2)
    {
    }

    private int Pm
    { get { return I0; } }

    protected override void Update()
    {
      var pm = GetPokemon(Pm);
      pm.ChangePosition(pm.Position.X, Y);
      base.Update();
    }
  }

  [DataContract(Namespace = PBOMarks.JSON)]
  public class RemoveItem : GameEvent
  {
    [DataMember(EmitDefaultValue = false)]
    string Key;
    [DataMember]
    int Pm;
    [DataMember(EmitDefaultValue = false)]
    int I1;
    [DataMember(EmitDefaultValue = false)]
    int I2;
    [DataMember(EmitDefaultValue = false)]
    string S1;
    [DataMember(EmitDefaultValue = false)]
    string S2;

    public RemoveItem(string logKey, PokemonProxy pm, ValueType arg1 = null, ValueType arg2 = null)
    {
      Key = logKey;
      Pm = pm.Id;
      if (arg1 is int) I1 = (int)arg1;
      else if (arg1 != null) S1 = arg1.ToString();
      if (arg2 is int) I2 = (int)arg2;
      else if (arg2 != null) S2 = arg2.ToString();
    }

    protected override void Update()
    {
      var pm = GetPokemon(Pm);
      if (Key == "PowerHerb") pm.ChangePosition(pm.Position.X, CoordY.Plate);
      if (Key == null) Sleep = 0;
      else AppendGameLog(Key, Pm, (object)S1 ?? I1, (object)S2 ?? I2);
    }
    public override void Update(SimGame game)
    {
      var pm = GetPokemon(game, Pm);
      if (pm != null) pm.Item = null;
    }
  }

  [DataContract(Name = "eo", Namespace = PBOMarks.JSON)]
  public class OutwardChange : GameEvent
  {
    public static OutwardChange Transform(PokemonProxy pm, PokemonProxy target)
    {
      var o = pm.GetOutward();
      return new OutwardChange("Transform", o.Id, target.Id) { Number = o.Form.Type.Number, Form = o.Form.Index, Moves = pm.Moves.Select((m) => m.Type.Id).ToArray() };
    }
    public static OutwardChange DeIllusion(string log, PokemonProxy pm, int arg = 0)
    {
      var o = pm.GetOutward();
      return new OutwardChange(log, o.Id, arg) { Number = o.Form.Type.Number, Form = o.Form.Index, Name = o.Name, Gender = o.Gender, Arg = arg };
    }
    public static OutwardChange ChangeForm(PokemonProxy pm)
    {
      return new OutwardChange(null, pm.Id, 0) { Form = pm.OnboardPokemon.Form.Index, Forever = pm.Pokemon.Form.Type.Number == 492 && pm.OnboardPokemon.Form == pm.Pokemon.Form };
    }

    [DataMember(Name = "b", EmitDefaultValue = false)]
    string Log;
    [DataMember(Name = "a")]
    int Pm;
    [DataMember(Name = "d", EmitDefaultValue = false)]
    string Name;
    [DataMember(Name = "e", EmitDefaultValue = false)]
    int Number;
    [DataMember(Name = "f", EmitDefaultValue = false)]
    int Form;
    [DataMember(Name = "g", EmitDefaultValue = false)]
    PokemonGender? Gender;
    [DataMember(Name = "c", EmitDefaultValue = false)]
    int[] Moves;
    [DataMember(Name = "h", EmitDefaultValue = false)]
    int Arg;
    [DataMember(Name = "i", EmitDefaultValue = false)]
    bool Forever; //shaymi，虽然会误判不过不影响

    private OutwardChange(string log, int pm, int arg)
    {
      Log = log;
      Pm = pm;
      Arg = arg;
    }

    protected override void Update()
    {
      {
        var pm = GetPokemon(Pm);
        if (Name != null) pm.Name = Name;
        if (Gender != null) pm.Gender = Gender.Value;
        pm.ChangeImage(Number == 0 ? pm.Form.Type.Number : Number, Form);
        if (Number == 0 && pm.Form.Type.Number == 555) Log = Form == 0 ? "DeZenMode" : "EnZenMode";
      }
      AppendGameLog(Log ?? "FormChange", Pm, Arg);
      Sleep = 500;
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
        var pm = game.OnboardPokemons.FirstOrDefault((p) => p.Id == Pm);
        if (pm != null && pm.Pokemon.Owner == game.Player) pm.ChangeMoves(Moves);
      }
    }
  }
}

