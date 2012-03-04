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
    private static double LvToCoeff(int lv)
    {
      double denominator = 2, numerator = 2;
      if (lv > 0) numerator += lv;
      else denominator -= lv;
      return numerator / denominator;
    }

    private readonly Pokemon pokemon;
    private readonly PokemonOutward Outward; //幻影
    public readonly Player Owner;

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
      this.pokemon = pokemon;
      Owner = pokemon.Owner;

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

      //幻影new完后覆盖属性
      Outward = new PokemonOutward(this, pokemon.Hp);
      Outward.Name = pokemon.Name;
      Outward.Gender = Gender;
      Outward.ImageId = pokemon.PokemonType.Id;
    }

    #region Properties
    public int Id
    { get { return pokemon.Id; } }
    public int StruggleId
    { get { return pokemon.StruggleId; } }
    public int SwitchId
    { get { return pokemon.SwitchId; } }
    public string Name
    { get { return pokemon.Name; } }
    public int Lv
    { get { return pokemon.Lv; } }
    public int MaxHp
    { get { return pokemon.Hp.Origin; } }
    public int Hp
    {
      get { return pokemon.Hp.Value; }
      set { pokemon.Hp.Value = value; }
    }
    public PokemonNature Nature
    { get { return pokemon.Nature; } }
    public PokemonState State
    {
      get { return pokemon.State; }
      set { pokemon.State = value; }
    }
    public Item Item
    {
      get { return pokemon.Item; }
      set { pokemon.Item = value; }
    }
    #endregion

    public PokemonOutward GetOutward()
    {
      return Outward;
    }

    #region HpChange
    /// <summary>
    /// 到这一步特性道具什么的无视了，要查特效（比如坚硬）提前查
    /// </summary>
    /// <param name="damage"></param>
    public void Hurt(int damage)
    {
      if (damage < pokemon.Hp.Value)
      {
        pokemon.Hp.Value -= damage;
        Outward.Hurt();
      }
      else
      {
        pokemon.Hp.Value = 0;
        Outward.Faint();
      }
    }
    public void HurtTo(int hp)
    {
      if (hp > 0 && hp <= Hp) //就为了闪烁效果变成<=
      {
        pokemon.Hp.Value = hp;
        Outward.Hurt();
      }
    }
    #endregion
  }
}
