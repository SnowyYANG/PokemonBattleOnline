using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using LightStudio.PokemonBattle.Game.Host;

namespace LightStudio.PokemonBattle.Game.GameEvents
{
  [DataContract(Namespace = Namespaces.PBO)]
  public class MoveHurt : GameEvent
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
