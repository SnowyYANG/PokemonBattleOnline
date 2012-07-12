using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using LightStudio.PokemonBattle.Data;
using LightStudio.PokemonBattle.Game;

namespace LightStudio.PokemonBattle.Interactive.GameEvents
{
  [DataContract(Namespace = Namespaces.DEFAULT)]
  internal class StateChange : GameEvent
  {
    [DataMember]
    int Id;
    [DataMember(EmitDefaultValue = false)]
    PokemonState State;
    
    public StateChange(PokemonProxy pm)
    {
      Id = pm.Id;
      State = pm.Pokemon.State;
    }

    PokemonState oldState;
    PokemonOutward pm;
    public override void Update(GameOutward game)
    {
      pm = game.GetPokemon(Id);
      oldState = pm.State;
      pm.State = State;
    }
    public override void Update(SimGame game)
    {
      Pokemon p = game.Team.Pokemons.ValueOrDefault(Id);
      if (p != null) p.State = State;
    }
    public override IText GetGameLog()
    {
      string key = null;
      if (State == PokemonState.Normal)
        switch (oldState)
        {
          case PokemonState.BadlyPoisoned:
          case PokemonState.Poisoned:
            key = "DePosioned";
            //免疫
            break;
          case PokemonState.Burned:
          case PokemonState.Frozen:
          case PokemonState.Paralyzed:
          case PokemonState.Sleeping:
            key = "De" + oldState.ToString();
            break;
        }
      else key = "En" + State.ToString();
      IText t = GetGameLog(key);
      t.SetData(pm);
      return t;
    }
  }

  [DataContract(Namespace = Namespaces.DEFAULT)]
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

    public bool SetHurt(IEnumerable<DefContext> defs)
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

    PokemonOutward[] pms;
    List<PokemonOutward> sh;
    List<PokemonOutward> wh;
    List<PokemonOutward> ct;
    private static string Convert(IList<PokemonOutward> pms)
    {
      return GameService.Logs.ConvertMultiObjects((p) => p.Name, pms);
    }
    public override void Update(GameOutward game)
    {
      pms = new PokemonOutward[Pms.Length];
      sh = new List<PokemonOutward>();
      wh = new List<PokemonOutward>();
      ct = new List<PokemonOutward>();
      for (int i = 0; i < Pms.Length; ++i)
      {
        PokemonOutward p = game.GetPokemon(Pms[i]);
        pms[i] = p;
        p.Hurt(Damages[i]);
        if (SH != null && SH.Contains(Pms[i])) sh.Add(p);
        if (WH != null && WH.Contains(Pms[i])) wh.Add(p);
        if (CT != null && CT.Contains(Pms[i])) ct.Add(p);
      }
    }
    public override void Update(SimGame game)
    {
      for (int i = 0; i < pms.Length; ++i)
      {
        var pm = game.Team.Pokemons.ValueOrDefault(Pms[i]);
        if (pm != null) pm.SetHp(pm.Hp.Value - Damages[i]);
      }
    }
    public override IText GetGameLog()
    {
      List<IText> ts = new List<IText>();
      for(int i = 0; i < pms.Length; ++i)
      {
        IText t = GetGameLog("Hurt") as IText;
        t.SetData(pms[i]);
        ts.Add(t);
        t = GetGameLog("Hp") as IText;
        t.SetData(-Damages[i]);
        ts.Add(t);
      }
      if (SH != null)
      {
        IText t = GetGameLog("SuperHurt"); t.SetData(Convert(sh));
        ts.Add(t);
      }
      if (WH != null)
      {
        IText t = GetGameLog("WeakHurt"); t.SetData(Convert(wh));
        ts.Add(t);
      }
      if (CT != null)
      {
        IText t = GetGameLog("CT"); t.SetData(Convert(ct));
        ts.Add(t);
      }
      return new LogText(ts.ToArray());
    }
  }

  [DataContract(Namespace = Namespaces.DEFAULT)]
  public class HpChange : GameEvent
  {
    [DataMember]
    protected int Pm;
    [DataMember(EmitDefaultValue = false)]
    public int Hp;
    [DataMember]
    protected string Key;
    [DataMember(EmitDefaultValue = false)]
    protected bool ResetY;

    public HpChange(PokemonProxy pm, string logKey, bool resetCoordY = false)
    {
      Pm = pm.Id;
      Hp = pm.Hp;
      Key = logKey;
      ResetY = resetCoordY;
    }

    protected int oldHp;
    protected PokemonOutward pm;
    public override void Update(GameOutward game)
    {
      pm = game.GetPokemon(Pm);
      oldHp = pm.Hp.Value;
      pm.Hp.Value = Hp;
      if (ResetY) pm.ChangePosition(pm.Position.X, CoordY.Plate);
    }
    public override void Update(SimGame game)
    {
      var pm = game.Team.Pokemons.ValueOrDefault(Pm);
      if (pm != null) pm.SetHp(Hp);
    }
    public override IText GetGameLog()
    {
      IText t1 = GetGameLog(Key);
      t1.SetData(pm); //虽然第二个参数可能用不着，但传过去无妨
      IText h = GetGameLog("Hp");
      h.SetData(Hp - oldHp);
      return new LogText(t1, h);
    }
  }

  [DataContract(Namespace = Namespaces.DEFAULT)]
  public class PositionChange : PmEvent
  {
    public static PositionChange Reset(string gameLogKey, PokemonProxy pm, params string[] args)
    {
      return new PositionChange(gameLogKey, pm, args);
    }
    public static PositionChange Leap(string gameLogKey, PokemonProxy pm, CoordY y)
    {
      return new PositionChange(gameLogKey, pm) { Y = y };
    }
    
    [DataMember(EmitDefaultValue = false)]
    CoordY Y;
    
    private PositionChange(string gameLogKey, PokemonProxy pm, params string[] args)
      : base(gameLogKey, pm, args)
    {
    }
    
    public override void Update(GameOutward game)
    {
      base.Update(game);
      pm.ChangePosition(pm.Position.X, Y);
    }
  }

  [DataContract(Namespace = Namespaces.DEFAULT)]
  public class UseItem : GameEvent
  {
    [DataMember]
    string Key;
    [DataMember]
    int Pm;
    [DataMember(EmitDefaultValue = false)]
    int Item;

    public UseItem(string logKey, PokemonProxy pm, Item i)
    {
      Key = logKey;
      Pm = pm.Id;
      if (i != null) Item = i.Id;
    }
    private PokemonOutward pm;
    private Item i;
    public override void Update(GameOutward game)
    {
      pm = game.GetPokemon(Pm);
      if (Key == "PowerHerb") pm.ChangePosition(pm.Position.X, CoordY.Plate);
      if (Item > 0) i = DataService.GetItem(Item);
    }
    public override IText GetGameLog()
    {
      IText t = GetGameLog(Key);
      t.SetData(pm, i);
      return t;
    }
    public override void Update(SimGame game)
    {
      var pm = game.Team.Pokemons.ValueOrDefault(Pm);
      if (pm != null && pm.Item.Type != ItemType.Normal)
        pm.Item = null;
    }
  }

  [DataContract(Namespace = Namespaces.DEFAULT)]
  public class OutwardChange : GameEvent
  {
    public static OutwardChange FormOnly(string logKey, PokemonProxy pm, string arg = null)
    {
      return new OutwardChange(logKey, pm.GetOutward(), false);
    }
    public static OutwardChange All(string logKey, PokemonProxy pm, string arg = null)
    {
      return new OutwardChange(logKey, pm.GetOutward(), true);
    }

    [DataMember]
    string Log;
    [DataMember]
    int Pm;
    [DataMember(EmitDefaultValue = false)]
    string Name;
    [DataMember]
    int Image;
    [DataMember(EmitDefaultValue = false)]
    PokemonGender? Gender;
    [DataMember(EmitDefaultValue = false)]
    string Arg;

    private OutwardChange(string log, PokemonOutward pm, bool allProperty)
    {
      Log = log;
      Pm = pm.Id;
      Image = pm.ImageId;
      if (allProperty)
      {
        Name = pm.Name;
        Gender = pm.Gender;
      }
    }
    PokemonOutward pm;
    public override void Update(GameOutward game)
    {
      pm = game.GetPokemon(Pm);
      if (Name != null) pm.Name = Name;
      if (Gender != null) pm.Gender = Gender.Value;
      pm.ChangeImageId(Image);
    }
    public override IText GetGameLog()
    {
      IText t = GetGameLog(Log);
      t.SetData(pm, Arg);
      return t;
    }
  }
}

