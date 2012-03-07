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
  public class OnboardPokemon : ConditionalObject
  {
    #region Data
    public readonly Position Position;
    public BattleType Type1;
    public BattleType Type2;
    public PokemonGender Gender;
    public Ability Ability;
    public readonly SixD Base; //百变怪变成会围攻
    public readonly SixD Iv; //模仿觉醒力
    public readonly SixD Ev;
    public readonly SixD Static; //力量交换，包含性格修正，不包含等级修正
    private readonly SixD lv5D;
    private int accuracyLv;
    private int evasionLv;
    #endregion

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
      lv5D = new SixD();

      Position = new Position(pokemon.TeamId, x);
    }

    public double Get5D(StatType stat)
    {
      double coeff;
      {
        int lv = lv5D.GetStat(stat);
        double denominator = 2, numerator = 2;
        if (lv > 0) numerator += lv;
        else denominator -= lv;
        coeff = numerator / denominator;
      }
      return Static.GetStat(stat) * coeff;
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
