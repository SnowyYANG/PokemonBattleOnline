using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using LightStudio.Tactic.DataModels;
using LightStudio.PokemonBattle.Data;

namespace LightStudio.PokemonBattle.Game
{
  /// <summary>
  /// 在场pm数据副本，不一定正在对战，比如“转盘”
  /// </summary>
  internal class OnboardPokemon : ConditionalObject
  {
    private static double LvToCoeff(int lv)
    {
      double denominator = 2, numerator = 2;
      if (lv > 0) numerator += lv;
      else denominator -= lv;
      return numerator / denominator;
    }

    #region Data
    public BattleType Type1;
    public BattleType Type2;
    public PokemonGender Gender;
    public Ability Ability;
    public readonly SixD Base; //百变怪变成会围攻
    public readonly SixD Iv; //模仿觉醒力
    public readonly SixD Ev;
    public readonly SixD Static; //包含性格修正，不包含等级修正
    public readonly SixD Lv5D;
    public int AccuracyLv;
    public int AvoidanceLv;
    #endregion

    public readonly Position Position;
    public bool IsActive { get; internal set; } //这个是外界设置吧
    public bool CanUseMove { get; internal set; }
    public bool CanStruggle { get; internal set; }
    public bool CanSwitch { get; internal set; }

    internal OnboardPokemon(Pokemon pokemon, int x)
    {
      Type1 = pokemon.PokemonType.Type1;
      Type2 = pokemon.PokemonType.Type2;
      Gender = pokemon.Gender;
      Ability = pokemon.Ability;
      Base = new SixD(pokemon.Base);
      Iv = new SixD(pokemon.Iv);
      Ev = new SixD(pokemon.Ev);
      Static = new SixD(pokemon.Static);
      Lv5D = new SixD();

      Position = new Position(pokemon.TeamId, x);
    }
    //#region HpChange
    ///// <summary>
    ///// 到这一步特性道具什么的无视了，要查特效（比如坚硬）提前查
    ///// </summary>
    ///// <param name="damage"></param>
    //public void Hurt(int damage)
    //{
    //  if (damage < pokemon.Hp.Value)
    //  {
    //    pokemon.Hp.Value -= damage;
    //  }
    //  else
    //  {
    //    pokemon.Hp.Value = 0;
    //  }
    //}
    //public void HurtTo(int hp)
    //{
    //  if (hp > 0 && hp <= Hp) //就为了闪烁效果变成<=
    //  {
    //    pokemon.Hp.Value = hp;
    //  }
    //}
    //#endregion
  }
}
