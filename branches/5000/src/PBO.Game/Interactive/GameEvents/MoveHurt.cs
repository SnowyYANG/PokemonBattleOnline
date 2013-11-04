using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace PokemonBattleOnline.Game.GameEvents
{
  /// <summary>
  /// because damage can be zero, and also the flash animation, this cannot be replaced by several ShowHp
  /// </summary>
  [DataContract(Name = "ed", Namespace = PBOMarks.JSON)]
  public class MoveHurt : GameEvent
  {
    [DataMember(Name = "a", EmitDefaultValue = false)]
    public int[] Pms;
    [DataMember(Name = "b", EmitDefaultValue = false)]
    public int[] Damages;
    [DataMember(Name = "e", EmitDefaultValue = false)]
    public int[] CT;
    [DataMember(Name = "c", EmitDefaultValue = false)]
    public int[] SH; //效果拔群
    [DataMember(Name = "d", EmitDefaultValue = false)]
    public int[] WH; //没有什么效果

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
      //Sleep = 17 * max + 1000;
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
