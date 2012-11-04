﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using LightStudio.PokemonBattle.Data;
using LightStudio.PokemonBattle.Game.Host;

namespace LightStudio.PokemonBattle.Game.GameEvents
{
  [DataContract(Namespace = Namespaces.PBO)]
  public class StateChange : GameEvent
  {
    [DataMember]
    int Pm;
    [DataMember(EmitDefaultValue = false)]
    PokemonState State;
    [DataMember(EmitDefaultValue = false)]
    string Log;
    [DataMember(EmitDefaultValue = false)]
    int Arg1;
    [DataMember(EmitDefaultValue = false)]
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
      Pokemon p = GetPokemon(game, Pm);
      if (p != null)
      {
        p.ClientChangePokemonStateWithNotify(State);
        if (Item) p.Item = null;
      }
    }
  }

  [DataContract(Namespace = Namespaces.PBO)]
  public class MoveHurts : GameEvent
  {
    [DataMember(EmitDefaultValue = false)]
    protected int[] Pms;
    [DataMember(EmitDefaultValue = false)]
    protected int[] Damages;
    [DataMember(EmitDefaultValue = false)]
    protected int[] SH; //效果拔群
    [DataMember(EmitDefaultValue = false)]
    protected int[] WH; //没有什么效果
    [DataMember(EmitDefaultValue = false)]
    protected int[] CT;

    internal bool SetHurt(IEnumerable<DefContext> defs) //auto delay
    {
      List<int> pms = new List<int>();
      List<int> damages = new List<int>();
      List<int> sh = new List<int>();
      List<int> wh = new List<int>();
      List<int> ct = new List<int>();
      foreach(DefContext d in defs)
        if (!d.HitSubstitute)
        {
          int id = d.Defender.Id;
          pms.Add(id);
          damages.Add(d.Damage);
          if (d.EffectRevise > 0) sh.Add(id);
          else if (d.EffectRevise < 0) wh.Add(id);
          if (d.IsCt) ct.Add(id);
        }
      if (pms.Count > 0) Pms = pms.ToArray();
      else return false;
      if (damages.Count > 0) Damages = damages.ToArray();
      if (sh.Count > 0) SH = sh.ToArray();
      if (wh.Count > 0) WH = wh.ToArray();
      if (ct.Count > 0) CT = ct.ToArray();
      return true;
    }

    protected override void Update()
    {
      int max = 0;
      for (int i = 0; i < Pms.Length; ++i)
      {
        PokemonOutward p = GetPokemon(Pms[i]);
        p.Hurt(Damages[i]);
        AppendGameLog("Hurt", Pms[i]); AppendGameLog("Hp", -Damages[i]);
        if (Damages[i] > max) max = Damages[i];
      }
      Sleep = 17 * max + 2000;
      if (SH != null) AppendGameLog("SuperHurt" + SH.Length, SH);
      if (WH != null) AppendGameLog("WeakHurt" + WH.Length, WH);
      if (CT != null) AppendGameLog("CT" + CT.Length, CT);
    }
    public override void Update(SimGame game)
    {
      for (int i = 0; i < Pms.Length; ++i)
      {
        var pm = GetPokemon(game, Pms[i]);
        if (pm != null) pm.SetHp(pm.Hp.Value - Damages[i]);
      }
    }
  }

  [DataContract(Namespace = Namespaces.PBO)]
  public class HpChange : GameEvent
  {
    [DataMember]
    protected int Pm;
    [DataMember(EmitDefaultValue = false)]
    public int Hp;
    [DataMember(EmitDefaultValue = false)]
    public bool ResetY;
    [DataMember(EmitDefaultValue = false)]
    public bool ConsumeItem;
    [DataMember(EmitDefaultValue = false)]
    protected string Key;
    [DataMember(EmitDefaultValue = false)]
    protected int Arg1;
    [DataMember(EmitDefaultValue = false)]
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
        AppendGameLog("Hp", Hp - oldHp);
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

  [DataContract(Namespace = Namespaces.PBO)]
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

  [DataContract(Namespace = Namespaces.PBO)]
  public class RemoveItem : GameEvent
  {
    [DataMember]
    string Key;
    [DataMember]
    int Pm;
    [DataMember(EmitDefaultValue = false)]
    object Arg1;
    [DataMember(EmitDefaultValue = false)]
    object Arg2;

    public RemoveItem(string logKey, PokemonProxy pm, object arg1 = null, object arg2 = null)
    {
      Key = logKey;
      Pm = pm.Id;
      Arg1 = arg1 is int ? (object)arg1 : arg1.ToString();
      Arg2 = arg2 is int ? (object)arg2 : arg2.ToString();
    }

    protected override void Update()
    {
      var pm = GetPokemon(Pm);
      if (Key == "PowerHerb") pm.ChangePosition(pm.Position.X, CoordY.Plate);
      if (Key != null) AppendGameLog(Key, Pm, Arg1, Arg2);
    }
    public override void Update(SimGame game)
    {
      var pm = GetPokemon(game, Pm);
      if (pm != null) pm.Item = null;
    }
  }

  [DataContract(Namespace = Namespaces.PBO)]
  public class OutwardChange : GameEvent
  {
    public static OutwardChange FormOnly(string logKey, PokemonProxy pm, string arg = null)
    {
      return new OutwardChange(logKey, pm.GetOutward(), false) { Arg = arg };
    }
    public static OutwardChange All(string logKey, PokemonProxy pm, string arg = null)
    {
      return new OutwardChange(logKey, pm.GetOutward(), true) { Arg = arg };
    }

    [DataMember]
    string Log;
    [DataMember]
    int Pm;
    [DataMember(EmitDefaultValue = false)]
    string Name;
    [DataMember(EmitDefaultValue = false)]
    int Number;
    [DataMember(EmitDefaultValue = false)]
    int Form;
    [DataMember(EmitDefaultValue = false)]
    PokemonGender? Gender;
    [DataMember(EmitDefaultValue = false)]
    string Arg;

    private OutwardChange(string log, PokemonOutward pm, bool allProperty)
    {
      Log = log;
      Pm = pm.Id;
      Number = pm.Form.Type.Number;
      Form = pm.Form.Index;
      if (allProperty)
      {
        Name = pm.Name;
        Gender = pm.Gender;
      }
    }
    protected override void Update()
    {
      {
        var pm = GetPokemon(Pm);
        if (Name != null) pm.Name = Name;
        if (Gender != null) pm.Gender = Gender.Value;
        pm.ChangeForm(Number, Form);
      }
      AppendGameLog(Log, Pm, Arg);
    }
  }
}

