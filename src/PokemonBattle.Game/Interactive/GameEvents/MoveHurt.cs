using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using PokemonBattleOnline.Game.Host;

namespace PokemonBattleOnline.Game.GameEvents
{
  [DataContract(Name = "ed", Namespace = Namespaces.JSON)]
  public class MoveHurt : GameEvent
  {
    [DataMember(Name = "a", EmitDefaultValue = false)]
    protected int[] Pms;
    [DataMember(Name = "b", EmitDefaultValue = false)]
    protected int[] Damages;
    [DataMember(Name = "e", EmitDefaultValue = false)]
    protected int[] CT;
    [DataMember(Name = "c", EmitDefaultValue = false)]
    protected int[] SH; //效果拔群
    [DataMember(Name = "d", EmitDefaultValue = false)]
    protected int[] WH; //没有什么效果

    internal bool SetHurt(IEnumerable<DefContext> defs, bool effects) //auto delay
    {
      List<int> pms = new List<int>();
      List<int> damages = new List<int>();
      List<int> sh = new List<int>();
      List<int> wh = new List<int>();
      List<int> ct = new List<int>();
      foreach (DefContext d in defs)
        if (!d.HitSubstitute)
        {
          int id = d.Defender.Id;
          pms.Add(id);
          damages.Add(d.Damage);
          if (d.EffectRevise > 0) sh.Add(id);
          else if (d.EffectRevise < 0) wh.Add(id);
          if (d.IsCt) ct.Add(id);
        }
      if (pms.Any()) Pms = pms.ToArray();
      else return false;
      if (damages.Any()) Damages = damages.ToArray();
      if (effects)
      {
        if (sh.Any()) SH = sh.ToArray();
        if (wh.Any()) WH = wh.ToArray();
      }
      if (ct.Any()) CT = ct.ToArray();
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
      Sleep = 17 * max + 1000;
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
}
