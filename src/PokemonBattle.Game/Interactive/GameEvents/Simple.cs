using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using LightStudio.PokemonBattle.Game.Host;

namespace LightStudio.PokemonBattle.Game.GameEvents
{
  [DataContract(Namespace = Namespaces.DEFAULT)]
  public class SimpleEvent : GameEvent
  {
    [DataMember]
    string Key;

    [DataMember(EmitDefaultValue = false)]
    string Arg;

    [DataMember(EmitDefaultValue = false)]
    string[] Args;

    public SimpleEvent(string gameLogKey, string[] args)
    {
      Key = gameLogKey;
      if (args.Length == 1) Arg = args[0];
      else if (args.Length > 1) Args = args;
    }
    public override IText GetGameLog()
    {
      var t = GetGameLog(Key);
      if (Arg != null) t.SetData(Arg);
      else if (Args != null) t.SetData(Args);
      return t;
    }
  }
  
  [DataContract(Namespace = Namespaces.DEFAULT)]
  public class PmEvent: GameEvent
  {
    [DataMember]
    string Key;

    [DataMember(EmitDefaultValue = false)]
    protected int Pm;

    [DataMember(EmitDefaultValue = false)]
    string Arg;

    [DataMember(EmitDefaultValue = false)]
    string[] Args;

    public PmEvent(string gameLogKey, PokemonProxy pm, params string[] args)
    {
      Key = gameLogKey;
      Pm = pm.Id;
      if (args.Length == 1) Arg = args[0];
      else if (args.Length > 1) Args = args;
    }

    protected PokemonOutward pm;
    public override void Update(GameOutward game)
    {
      pm = game.GetPokemon(Pm);
    }
    public override IText GetGameLog()
    {
      IText t = GetGameLog(Key);
      if (t != null)
      {
        if (Arg != null) t.SetData(pm, Arg);
        else if (Args != null) t.SetData(pm, Args);
        else t.SetData(pm);
      }
      return t;
    }
  }

  [DataContract(Namespace = Namespaces.DEFAULT)]
  public class MultiPmEvent : GameEvent
  {
    [DataMember]
    string Key;

    [DataMember]
    int[] Pms;

    public MultiPmEvent(string key, IList<PokemonProxy> pms)
    {
      Key = key;
      Pms = pms.Select((p) => p.Id).ToArray();
    }
    public MultiPmEvent(string key, IList<DefContext> pms)
    {
      Key = key;
      Pms = pms.Select((p) => p.Defender.Id).ToArray();
    }

    PokemonOutward[] pms;
    public override void Update(GameOutward game)
    {
      pms = new PokemonOutward[Pms.Length];
      for (int i = 0; i < Pms.Length; ++i)
        pms[i] = game.GetPokemon(Pms[i]);
    }
    public override IText GetGameLog()
    {
      IText t = GetGameLog(Key);
      t.SetData(GameService.Logs.ConvertMultiObjects((p) => p.Name, pms));
      return t;
    }
  }
}
